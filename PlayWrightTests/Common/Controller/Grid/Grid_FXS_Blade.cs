using LogService;
using Microsoft.Playwright;
using PlaywrightTests.Common.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaywrightTests.Common.Controller.Grid
{
    public class Grid_FXS_Blade : GridBase, IGrid
    {
        private ILocator Grid_FXS_Blade_Locator = null;
        public Grid_FXS_Blade(IPage? page, string? frameName, EnumHelper.Language language, ILocator? parentLocator = null, string noDataText = null) : base(page, frameName, language, parentLocator, noDataText: noDataText)
        {
        }
        public Grid_FXS_Blade(IPage? page, string? frameName, string dataContenttitle, EnumHelper.Language language, ILocator? parentLocator = null, string noDataText = null) : base(page, frameName, language, parentLocator, noDataText: noDataText)
        {
            Grid_FXS_Blade_Locator = GetGridBladeLocatorByContentTitleAsync(dataContenttitle).Result;
        }

        public Task ClickColumnNameToSortASCAsync(string ColumnName)
        {
            throw new NotImplementedException();
        }

        public Task ClickColumnNameToSortDESCAsync(string ColumnName)
        {
            throw new NotImplementedException();
        }

        public async Task ClickRowHeaderToShowDetailAsync(string policyName)
        {
            await ControlHelper.ClickByGridCellAndHasTextAsync(this.CurrentIPage, policyName, 0);
        }
        public Task ClickSpecialColumnToShowDetailAsync(string policyName, string ColumnName)
        { throw new NotImplementedException(); }
        public Task RemoveRowByRowHeaderAsync(string RowHeader)
        {
            throw new NotImplementedException();
        }
        public Task DeleteRowByRowHeaderAsync(string RowHeader)
        {
            throw new NotImplementedException();
        }
        public async Task<List<(string? displayName, string? dataItemKey)>> GetDisplayedRowHeadersAsync()
        {
            List<(string? displayName, string? dataItemKey)> result = new List<(string? displayName, string? dataItemKey)>();
            var rowHeaderLocators = await ControlHelper.GetLocatorsByRoleAndHasTextAsync(Grid_FXS_Blade_Locator, AriaRole.Columnheader, "");
            foreach (ILocator header in rowHeaderLocators)
            {
                string? ClassList = await header.GetAttributeAsync("class");
                string dataItemKey = ClassList.Split(" ").Where(c => c.Contains("fxc-gc-columnheader_")).First().Replace("columnheader", "cell-content");
                var displayNameLocator = await ControlHelper.GetLocatorByClassAsync(header, "fxc-gc-columnheader-content fxc-gc-text");
                string? displayName = await displayNameLocator.TextContentAsync();
                result.Add((displayName, dataItemKey));
            }
            return result;
        }

        public async Task<DataTable> GetGridAllDataAsync()
        {
            DataTable dt = new DataTable();
            if (!await WaitGridDataToLoadAsync())
            {
                LogHelper.Error("No data in grid");
                return dt;
            }
            List<string> TempCacheRecordRowContent = new List<string>();
            var rowHeaders = await GetDisplayedRowHeadersAsync();
            //Get first row data            
            try
            {
                // init DataTable column name               
                foreach (var rowHeader in rowHeaders)
                {
                    var valueLocator = await ElementHelper.GetByLocatorAsync(Grid_FXS_Blade_Locator, $"[id='{rowHeader.dataItemKey}']");
                    var valueList = await valueLocator.AllInnerTextsAsync();
                    dt.Columns.Add(rowHeader.displayName, DataTypeHelper.IdentityDataType(valueList.FirstOrDefault()));
                }
            }
            catch
            {
                return dt;
            }
            int retryCount = 0;

            //Get all row data
            while (true)
            {
                var allRowData = await ControlHelper.GetLocatorsByRoleAndHasTextAsync(Grid_FXS_Blade_Locator, AriaRole.Row, "");
                if (!TempCacheRecordRowContent.Contains(await allRowData.Last().InnerTextAsync()))
                {
                    retryCount = 0;
                    try
                    {
                        LogHelper.Info("Start record grid content");
                        foreach (var rowData in allRowData)
                        {
                            var currentRowInnerText = await rowData.InnerTextAsync();
                            LogHelper.Info(currentRowInnerText);
                            if (!TempCacheRecordRowContent.Contains(currentRowInnerText))
                            {
                                TempCacheRecordRowContent.Add(currentRowInnerText);
                                DataRow dr = dt.NewRow();
                                foreach (var rowHeader in rowHeaders)
                                {
                                    var valueLocator = await ElementHelper.GetByLocatorAsync(Grid_FXS_Blade_Locator, $"[id='{rowHeader.dataItemKey}']");
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
                            await ClickNextPageButtonAsync();
                        }
                        catch { }
                        retryCount++;
                    }
                }
            }
            return dt;
        }
        public async Task<List<string>> GetColumnsDataByColumnNameAsync(string columnName)
        {
            var dt = await GetGridAllDataAsync();
            return BaseGetColumnsDataByColumnName(dt, columnName);
        }
        public async Task<string> GetGridContextTextAsync()
        {
            return await Grid_FXS_Blade_Locator.InnerTextAsync();
        }
        public async Task ClickContextMenuAsync(int n = 0)
        {
            var contextMenu = await ControlHelper.GetByRoleAndAriaLabelAsync(Grid_FXS_Blade_Locator, AriaRole.Button, "Context menu", n);
            LogHelper.Info("Click \"...\" ");
            await contextMenu.ClickAsync();
        }
        #region Private Methods
        private async Task ClickPreviousButtonAsync()
        {
            await ControlHelper.ClickByClassAndHasTextAsync(Grid_FXS_Blade_Locator, "fxs-button fxt-button fxs-inner-solid-border", "< Previous", 0);
        }
        private async Task ClickNextPageButtonAsync()
        {
            await ControlHelper.ClickByClassAndHasTextAsync(Grid_FXS_Blade_Locator, "fxs-button fxt-button fxs-inner-solid-border", "Next >", 0);
        }
        private async Task<ILocator> GetGridBladeLocatorByContentTitleAsync(string data_contenttitle)
        {
            var locator = await ElementHelper.GetByLocatorAsync(CurrentIPage, $"[class*='fxs-blade-content-wrapper'][data-contenttitle='{data_contenttitle}']");
            return locator;
        }

        public Task ClearGridAllDataAsync(string columnName, string keyWord)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> WaitGridDataToLoadAsync()
        {
            //throw new NotImplementedException();
            ILocator noData = await ControlHelper.GetLocatorByTextAsync(CurrentIPage, NoDataText, CurrentIFrameName, IsStrongWait: false);
            ILocator DataList = await ControlHelper.GetLocatorByClassAsync(CurrentIPage, "fxc-gcflink-link", 0, CurrentIFrameName, waitUntilElementExist: false);

            LogHelper.Info("Wait grid data to load...");
            return await ControlHelper.CheckDataExitInListPageAsync(DataList, noData);
        }
        #endregion
    }
}
