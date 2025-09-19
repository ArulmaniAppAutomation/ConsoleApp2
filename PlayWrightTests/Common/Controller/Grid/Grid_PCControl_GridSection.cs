using LogService;
using Microsoft.Playwright;
using PlaywrightTests.Common.Controller.ConfirmDialog;
using PlaywrightTests.Common.Helper;
using System.Data;

namespace PlaywrightTests.Common.Controller.Grid
{
    public class Grid_PCControl_GridSection : GridBase, IGrid
    {
        private ILocator? Grid_Locator { get { return GetGridLocatorAsync().Result; } }
        private IConfirmDialog confirmDialog;
        public Grid_PCControl_GridSection(IPage? page, string? frameName, EnumHelper.Language language, ILocator? parentLocator = null, string noDataText = null) : base(page, frameName, language, parentLocator, noDataText: noDataText)
        {
            confirmDialog = new ConfirmDialog_FXS_Balloon_Dialog(this.CurrentIPage, this.CurrentIFrameName, this.CurrentLanguage);
        }

        public Task ClickColumnNameToSortASCAsync(string ColumnName)
        {
            throw new NotImplementedException();
        }

        public Task ClickColumnNameToSortDESCAsync(string ColumnName)
        {
            throw new NotImplementedException();
        }

        public Task ClickContextMenuAsync(int nth = 0)
        {
            throw new NotImplementedException();
        }

        public async Task ClickRowHeaderToShowDetailAsync(string RowHeader)
        {
            await ControlHelper.ClickByGridCellAndHasTextAsync(this.CurrentIPage, RowHeader, 0);
        }
        public async Task ClickSpecialColumnToShowDetailAsync(string RowHeader, string ColumnName)
        {
            var rowLocator = await ControlHelper.GetByRoleAndHasTextAsync(Grid_Locator, AriaRole.Row, RowHeader, 0);
            var rowHeaders = await GetDisplayedRowHeadersAsync();
            var index = rowHeaders.IndexOf(rowHeaders.Where(t => t.displayName == ColumnName).FirstOrDefault());
            var cellLocator = await ElementHelper.GetByLocatorAsync(rowLocator, $"[aria-colindex='{index}']", isNeedSleep: false);
            await cellLocator.ClickAsync();
        }
        public Task RemoveRowByRowHeaderAsync(string RowHeader)
        {
            throw new NotImplementedException();
        }
        public async Task DeleteRowByRowHeaderAsync(string RowHeader)
        {
            var rowLocator = await ControlHelper.GetByRoleAndHasTextAsync(Grid_Locator, AriaRole.Row, RowHeader, 0);
            await ControlHelper.ClickByAriaLableAsync(rowLocator, "Click to open context menu", 0);
            await ControlHelper.ClickByClassAndHasTextAsync(this.CurrentIPage, "fxs-contextMenu-item msportalfx-command-like-button", "Delete", 0, iFrameName: null);
            await confirmDialog.ClickDialogOKButtonAsync();
        }
        public async Task ClearGridAllDataAsync(string columnName, string keyWord)
        {
            var gridData = await GetGridAllDataAsync();
            if (gridData.Rows.Count > 0)
            {
                foreach (DataRow dr in gridData.Rows)
                {
                    try
                    {
                        string profileName = dr[columnName].ToString();
                        if (profileName.Contains(keyWord))
                        {
                            await DeleteRowByRowHeaderAsync(profileName);
                        }
                    }
                    catch { }
                }
            }
        }

        public async Task<List<(string? displayName, string? dataItemKey)>> GetDisplayedRowHeadersAsync()
        {
            base.Sleep(3);
            List<(string? displayName, string? dataItemKey)> result = new List<(string? displayName, string? dataItemKey)>();
            var AllRowHeaders = await ControlHelper.GetAllLocatorsByRoleAndClassAsync(Grid_Locator, AriaRole.Columnheader, "fxc-gc-columnheader", isNeedSleep: false);
            foreach (ILocator header in AllRowHeaders)
            {
                string? displayName = await header.TextContentAsync();
                string classAttributeString = await header.GetAttributeAsync("class");
                string dataItemKey = classAttributeString.Split(" ").Where(t => t.Contains("fxc-gc-columnheader_")).FirstOrDefault().Replace("fxc-gc-columnheader_", "fxc-gc-columncell_");
                result.Add((displayName, dataItemKey));
            }
            return result;
        }

        public async Task<DataTable> GetGridAllDataAsync()
        {
            base.Sleep(5);
            List<string> TempCacheRecordRowContent = new List<string>();
            var rowHeaders = await GetDisplayedRowHeadersAsync();
            //Get first row data
            DataTable dt = new DataTable();
            try
            {
                // init DataTable column name               
                foreach (var rowHeader in rowHeaders)
                {
                    var cellLocator = await ControlHelper.GetByRoleAndClassAsync(Grid_Locator, AriaRole.Gridcell, rowHeader.dataItemKey, 0, isNeedSleep: false);
                    var valueList = await cellLocator.AllInnerTextsAsync();
                    string value = valueList.FirstOrDefault();
                    if (rowHeader.displayName == "Priority")
                    {
                        value = "Priority";
                    }
                    dt.Columns.Add(rowHeader.displayName, DataTypeHelper.IdentityDataType(value));
                }
            }
            catch
            {
                return dt;
            }


            var RowLocators = await (await ControlHelper.GetAllLocatorsByClassAsync(Grid_Locator, "fxc-gc-row-content")).AllAsync();
            try
            {
                if (RowLocators != null && RowLocators.Count > 0)
                {
                    foreach (var rowLocator in RowLocators)
                    {
                        DataRow dr = dt.NewRow();
                        foreach (var rowHeader in rowHeaders)
                        {
                            var cellLocator = await ControlHelper.GetByRoleAndClassAsync(rowLocator, AriaRole.Gridcell, rowHeader.dataItemKey, 0, isNeedSleep: false);
                            var valueList = await cellLocator.AllInnerTextsAsync();
                            dr[rowHeader.displayName] = valueList.FirstOrDefault();
                        }
                        dt.Rows.Add(dr);
                    }
                }
            }
            catch (Exception ex)
            {
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
            return await Grid_Locator.InnerTextAsync();
        }
        /// <summary>
        /// Adjust for AllAppsUtils 2025/01/07
        /// </summary>
        /// <returns></returns>
        private async Task ClickLoadMoreButtonAsync()
        {
            await ControlHelper.ClickByClassAndHasTextAsync(Grid_Locator, "azc-grid-pageable-loadMoreContainer", "Load more", 0);
        }

        #region
        private async Task<ILocator> GetGridLocatorAsync()
        {
            return await ControlHelper.GetByRoleAndClassAsync(this.CurrentIPage, AriaRole.Grid, "fxc-gc azc-fabric fxc-gc-dataGrid", 0);
        }

        public Task ClickGridCellToShowDetailsAsync(string RowCell)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> WaitGridDataToLoadAsync()
        {
            ILocator noData = GetGridLocatorAsync().Result.GetByText(NoDataText);
            ILocator DataList = await ControlHelper.GetLocatorByClassAsync(await GetGridLocatorAsync(), "fxc-gc-cell fxc-gc-columncell_", waitUntilElementExist: false);
            LogHelper.Info("Wait grid data to load...");
            return await ControlHelper.CheckDataExitInListPageAsync(noData, DataList);
        }
        #endregion
    }
}
