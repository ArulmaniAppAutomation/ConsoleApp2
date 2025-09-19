using LogService;
using LogService.Extension;
using Microsoft.Playwright;
using PlaywrightTests.Common.Controller.ConfirmDialog;
using PlaywrightTests.Common.Controller.ContextMenu;
using PlaywrightTests.Common.Helper;
using System.Data;

namespace PlaywrightTests.Common.Controller.Grid
{
    public class Grid_MS_DetailsList : GridBase, IGrid
    {
        private ILocator? MS_DetailsList_Locator { get { return GetMS_DetailsList_Locator(); } }
        private IContextMenu contextMenu;
        private IConfirmDialog confirmDialog;
        public Grid_MS_DetailsList(IPage page, string? frameName, EnumHelper.Language language, ILocator? parentLocator = null, string noDataText = null) : base(page, frameName, language, parentLocator, noDataText)
        {
            contextMenu = new ContextMenu_MS_ContextualMenu_Callout(page, frameName, language);
            confirmDialog = new ConfirmDialog_MS_Dialog_Main(page, frameName, language);
        }
        /// <summary>
        /// Get all displayed row headers
        /// </summary>
        /// <returns></returns>
        public async Task<List<(string? displayName, string? dataItemKey)>> GetDisplayedRowHeadersAsync()
        {
            List<(string? displayName, string? dataItemKey)> result = new List<(string? displayName, string? dataItemKey)>();
            var rowHeaderLocators = await ControlHelper.GetLocatorsByRoleAndHasTextAsync(MS_DetailsList_Locator, AriaRole.Columnheader, "");
            foreach (ILocator header in rowHeaderLocators)
            {
                string? dataItemKey = await header.GetAttributeAsync("data-item-key");
                if (string.IsNullOrEmpty(dataItemKey))
                {
                    continue;
                }
                var displayNameLocator = await ControlHelper.GetLocatorByClassAsync(header, "ms-DetailsHeader-cellName");
                string? displayName = await displayNameLocator.TextContentAsync();
                result.Add((displayName, dataItemKey));
            }
            return result;
        }
        public Task RemoveRowByRowHeaderAsync(string RowHeader)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Delete row by row header
        /// </summary>
        /// <param name="RowHeader"></param>
        /// <returns></returns>
        public async Task DeleteRowByRowHeaderAsync(string RowHeader)
        {
            var RowLocator = await ControlHelper.GetLocatorByClassAndHasTextAsync(MS_DetailsList_Locator, "ms-DetailsRow-fields", RowHeader, 0);
            //Click contextMenu
            await ControlHelper.ClickByClassWithAriaLableAsync(RowLocator, "ms-Button ms-Button--icon ms-Button--hasMenu", "Click to open context menu", 0);
            await contextMenu.ClickDeleteContextMenuItemAsync();
            var deleteButton = await confirmDialog.GetDialogButtonLocatorByNameAsync("Delete");
            var buttonCount = await deleteButton.CountAsync();
            if (buttonCount > 0)
            {
                await confirmDialog.ClickDialogDeleteButtonAsync();
            }
            else
            {
                var yesButton = await confirmDialog.GetDialogButtonLocatorByNameAsync("Yes");
                buttonCount = await yesButton.CountAsync();
                if (buttonCount > 0)
                {
                    await confirmDialog.ClickDialogYesButtonAsync();
                }
                else
                {
                    await confirmDialog.ClickDialogOKButtonAsync();
                }
            }
        }
        /// <summary>
        /// Click row header to show detail
        /// </summary>
        /// <param name="RowHeader"></param>
        /// <returns></returns>
        public async Task ClickRowHeaderToShowDetailAsync(string policyName)
        {
            var RowHeaderLocator = await ControlHelper.GetLocatorByRoleAndHasTextAsync(MS_DetailsList_Locator, AriaRole.Rowheader, policyName);
            await ControlHelper.ClickByClassAndHasTextAsync(RowHeaderLocator, "ms-Link", policyName, 0);
        }
        public async Task ClickSpecialColumnToShowDetailAsync(string policyName, string ColumnName)
        {
            var RowLocator = await ControlHelper.GetLocatorByClassAndHasTextAsync(MS_DetailsList_Locator, "ms-List-cell", policyName, 0);
            var rowHeaders = await GetDisplayedRowHeadersAsync();
            var rowHeader = rowHeaders.Where(t => t.displayName == ColumnName).FirstOrDefault();
            var valueLocator = await ControlHelper.GetLocatorByClassAndDataAutomationKeyAsync(RowLocator, "ms-DetailsRow-cell", rowHeader.dataItemKey, 0, isNeedSleep: false);
            await ControlHelper.ClickByClassAsync(valueLocator, "ms-Link", 0);
        }
        /// <summary>
        /// Click column name to sort ASC
        /// </summary>
        /// <param name="ColumnName"></param>
        /// <returns></returns>
        public async Task ClickColumnNameToSortASCAsync(string ColumnName)
        {
            await SortByColumnNameAsync("ASC", ColumnName);
        }
        /// <summary>
        /// Click column name to sort DESC
        /// </summary>
        /// <param name="ColumnName"></param>
        /// <returns></returns>
        public async Task ClickColumnNameToSortDESCAsync(string ColumnName)
        {
            await SortByColumnNameAsync("DESC", ColumnName);
        }

        public async Task<DataTable> GetGridAllDataAsync()
        {
            DataTable dt = new DataTable();
            //base.Sleep(5);
            if (!await WaitGridDataToLoadAsync())
            {
                LogHelper.Error("No data in grid");
                return dt;
            }
            List<string> TempCacheRecordRowContent = new List<string>();
            var rowHeaders = await GetDisplayedRowHeadersAsync();
            //Get first row data
            var defaultSortColumn = "";
            var defaultSortType = "";
            var defaultSortColumnDataItemKey = "";
            try
            {
                foreach (var rowHeader in rowHeaders)
                {
                    try
                    {
                        var rowHeaderLocator = await ControlHelper.GetByLocatorAsync(MS_DetailsList_Locator, $"[class*='ms-DetailsHeader-cell'][data-item-key='{rowHeader.dataItemKey}']", 0);
                        var ColumnSortValue = await rowHeaderLocator.GetAttributeAsync("aria-sort");
                        if (!string.IsNullOrEmpty(ColumnSortValue) && ColumnSortValue.ToLower() != "none")
                        {
                            defaultSortColumn = rowHeader.displayName;
                            defaultSortType = ColumnSortValue;
                            defaultSortColumnDataItemKey = rowHeader.dataItemKey;
                            break;
                        }
                    }
                    catch
                    {
                        LogHelper.Info($"This column : {rowHeader.displayName} is not sort by default.");
                    }
                }
                var firstRowData = (await ControlHelper.GetLocatorByClassAsync(MS_DetailsList_Locator, "ms-List-cell")).First;
                // init DataTable column name               
                foreach (var rowHeader in rowHeaders)
                {
                    var valueLocator = await ControlHelper.GetLocatorByClassAndDataAutomationKeyAsync(firstRowData, "ms-DetailsRow-cell", rowHeader.dataItemKey, 0);
                    var valueList = await valueLocator.AllInnerTextsAsync();
                    if (valueList == null || valueList.Count() <= 0 || string.IsNullOrEmpty(valueList.FirstOrDefault()))
                    {
                        await SortByColumnNameAsync("DESC", rowHeader.displayName, rowHeader.dataItemKey);
                        firstRowData = (await ControlHelper.GetLocatorByClassAsync(MS_DetailsList_Locator, "ms-List-cell")).First;
                        valueLocator = await ControlHelper.GetLocatorByClassAndDataAutomationKeyAsync(firstRowData, "ms-DetailsRow-cell", rowHeader.dataItemKey, 0);
                        valueList = await valueLocator.AllInnerTextsAsync();
                    }
                    dt.Columns.Add(rowHeader.displayName, DataTypeHelper.IdentityDataType(valueList.FirstOrDefault()));
                }
                if (!string.IsNullOrEmpty(defaultSortColumn))
                {
                    await SortByColumnNameAsync(defaultSortType, defaultSortColumn, defaultSortColumnDataItemKey);
                }
            }
            catch (Exception ex)
            {
                return dt;
            }
            int retryCount = 0;
            List<int> DataListIndex = new();
            //Get all row data
            while (true)
            {
                var allRowData = await (await ControlHelper.GetAllLocatorsByClassAsync(MS_DetailsList_Locator, "ms-List-cell")).AllAsync();
                if (!TempCacheRecordRowContent.Contains(await allRowData.Last().InnerTextAsync()))
                {
                    retryCount = 0;
                    try
                    {
                        LogHelper.Info("Start record grid content");
                        foreach (var rowData in allRowData)
                        {
                            var currentRowInnerText = await rowData.InnerTextAsync();
                            //LogHelper.Info(currentRowInnerText);
                            if (!TempCacheRecordRowContent.Contains(currentRowInnerText))
                            {
                                DataListIndex.Add(int.Parse(await rowData.GetAttributeAsync("data-list-index")));
                                TempCacheRecordRowContent.Add(currentRowInnerText);
                                DataRow dr = dt.NewRow();
                                foreach (var rowHeader in rowHeaders)
                                {
                                    var valueLocator = await ControlHelper.GetLocatorByClassAndDataAutomationKeyAsync(rowData, "ms-DetailsRow-cell", rowHeader.dataItemKey, 0, isNeedSleep: false);                                    
                                    var valueList = await valueLocator.AllInnerTextsAsync();
                                    if (dt.Columns[rowHeader.displayName].DataType == typeof(DateTime))
                                    {
                                        var value = valueList.FirstOrDefault();
                                        if (!string.IsNullOrEmpty(value))
                                        {
                                            dr[rowHeader.displayName] = value;
                                        }
                                    }
                                    else
                                    {
                                        dr[rowHeader.displayName] = valueList.FirstOrDefault();
                                    }
                                }
                                dt.Rows.Add(dr);
                            }
                        }
                        LogHelper.Info("End record grid content");
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error(ex.Message + ex.StackTrace);
                    }
                }
                else
                {
                    if (retryCount > 1)
                    {
                        break;
                    }
                    else
                    {
                        try
                        {
                            LogHelper.Info("Scroll to bottom");
                            var scrollLocator = await ControlHelper.GetByAttributeDataIsScrollableAsync(this.CurrentIPage, "true", 0, iFrameName: this.CurrentIFrameName);
                            await ControlHelper.ScrollToBottomAsync(scrollLocator);
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Info($"Scroll to bottom Exception:{ex.Message}");
                        }
                        retryCount++;
                    }
                }
            }
            return dt;
        }
        public async Task<string> GetGridContextTextAsync()
        {
            return await MS_DetailsList_Locator.InnerTextAsync();
        }
        public async Task<List<string>> GetColumnsDataByColumnNameAsync(string columnName)
        {
            var dt = await GetGridAllDataAsync();
            return BaseGetColumnsDataByColumnName(dt, columnName);
        }
        public async Task ClickContextMenuAsync(int nth = 0)
        {
            var contextMenu = ControlHelper.GetByClassAsync(MS_DetailsList_Locator, "ms-Button ms-Button--icon").Result.currentLocator;
            var contextMenuCount = await contextMenu.CountAsync();
            await ControlHelper.WaitElementEnabledAsync(contextMenu.Nth(nth));
            if (!await contextMenu.Nth(nth).IsEnabledAsync())
            {
                LogHelper.Error("\"...\" failed to load...");
                throw new CustomLogException(CustomExceptionPrefix.CodeError_Element_Load_Failed + "\"...\"");
            }
            LogHelper.Info("Click \"...\" ");
            await contextMenu.Nth(nth).ClickAsync();
            var expandStatus = await contextMenu.Nth(nth).GetAttributeAsync("aria-expanded");
            if (expandStatus == "false")
            {
                LogHelper.Info("Click \"...\" again");
                await contextMenu.Nth(nth).ClickAsync();
            }
        }
        #region
        private ILocator GetMS_DetailsList_Locator()
        {
            return ControlHelper.GetLocatorByClassAsync(CurrentIPage, "ms-DetailsList--Compact", 0, iFrameName: CurrentIFrameName).Result;
        }

        private async Task SortByColumnNameAsync(string SortType, string ColumnName, string DataItemKey = null)
        {
            if (string.IsNullOrEmpty(DataItemKey))
            {
                var rowHeaderList = await GetDisplayedRowHeadersAsync();
                var rowHeader = rowHeaderList.Where(t => t.displayName == ColumnName).FirstOrDefault();
                DataItemKey = rowHeader.dataItemKey;
            }


            var RowHeaderLocator = await ControlHelper.GetByLocatorAsync(MS_DetailsList_Locator, $"[class*='ms-DetailsHeader-cell'][data-item-key='{DataItemKey}']", 0);
            await ControlHelper.ClickByClassAndHasTextAsync(RowHeaderLocator, "ms-DetailsHeader-cellName", ColumnName, 0);

            var ActualSortType = await RowHeaderLocator.GetAttributeAsync("aria-sort");
            if (ActualSortType != null)
            {
                if (!ActualSortType.ToUpper().Contains(SortType.ToUpper()))
                {
                    await ControlHelper.ClickByClassAndHasTextAsync(RowHeaderLocator, "ms-DetailsHeader-cellName", ColumnName, 0);
                }
            }
            else
            {
                throw new CustomLogException(CustomExceptionPrefix.CodeError_GetElementAttribute_Failed + $":Attribute:aria-sort");
            }
            int retry = 0;
            while (retry < 3)
            {
                try
                {
                    await ControlHelper.GetByLocatorAsync(MS_DetailsList_Locator, "[aria-busy='false']", 0);
                    break;
                }
                catch
                {
                    retry++;
                }
            }
        }

        public async Task ClearGridAllDataAsync(string columnName, string keyWord = null)
        {
            var gridData = await GetGridAllDataAsync();
            if (gridData.Rows.Count > 0)
            {
                foreach (DataRow dr in gridData.Rows)
                {
                    try
                    {
                        string RowHeader = dr[columnName].ToString();
                        if (!string.IsNullOrEmpty(keyWord) && !RowHeader.Contains(keyWord))
                        {
                            continue;
                        }
                        await DeleteRowByRowHeaderAsync(RowHeader);
                    }
                    catch { }
                }
            }
        }

        public async Task<bool> WaitGridDataToLoadAsync()
        {
            ILocator noData = await ControlHelper.GetLocatorByTextAsync(CurrentIPage, NoDataText, CurrentIFrameName, IsStrongWait: false);
            ILocator DataList = await ControlHelper.GetLocatorByClassAsync(CurrentIPage, "ms-DetailsRow-fields fields-", 0, CurrentIFrameName, waitUntilElementExist: false);

            LogHelper.Info("Wait grid data to load...");
            return await ControlHelper.CheckDataExitInListPageAsync(DataList, noData);
        }
        #endregion
    }
}
