using LogService;
using Microsoft.Playwright;
using PlaywrightTests.Common.Controller.ConfirmDialog;
using PlaywrightTests.Common.Helper;
using System.Data;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace PlaywrightTests.Common.Controller.Grid
{
    public class Grid_PCControl_GridViewModel : GridBase, IGrid
    {
        private ILocator? Grid_Locator { get { return GetGridLocatorAsync().Result; } }
        private IConfirmDialog confirmDialog;
        public Grid_PCControl_GridViewModel(IPage? page, string? frameName, EnumHelper.Language language, ILocator? parentLocator = null, string noDataText = null) : base(page, frameName, language, parentLocator,noDataText)
        {
            confirmDialog = new ConfirmDialog_FXS_Blade_Dialog(this.CurrentIPage, this.CurrentIFrameName, this.CurrentLanguage);
        }

        public Task ClickColumnNameToSortASCAsync(string ColumnName)
        {
            throw new NotImplementedException();
        }

        public Task ClickColumnNameToSortDESCAsync(string ColumnName)
        {
            throw new NotImplementedException();
        }

        public async Task ClickContextMenuAsync(int nth = 0)
        {
            await ControlHelper.ClickByButtonRoleAndNameAsync(Grid_Locator, "Context menu", 0);
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
            await ControlHelper.ClickByAriaLableAsync(rowLocator, "Context menu", 0);
            await ControlHelper.ClickByClassAndHasTextAsync(this.CurrentIPage, "fxs-contextMenu-item msportalfx-command-like-button", "Delete", 0, iFrameName: null);
            await confirmDialog.ClickDialogYesButtonAsync();
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
        public async Task<bool> WaitGridDataToLoadAsync()
        {
            ILocator noData = Grid_Locator.GetByText(NoDataText);
            ILocator DataList = await ControlHelper.GetLocatorByClassAsync(Grid_Locator, "azc-grid-cellContent", waitUntilElementExist: false);
            LogHelper.Info("Wait grid data to load...");
            return await ControlHelper.CheckDataExitInListPageAsync(noData, DataList);
        }
        public async Task<List<(string? displayName, string? dataItemKey)>> GetDisplayedRowHeadersAsync()
        {
            //base.Sleep(5);
            List<(string? displayName, string? dataItemKey)> result = new List<(string? displayName, string? dataItemKey)>();
            var AllRowHeaders = await ControlHelper.GetAllLocatorsByRoleAndClassAsync(Grid_Locator, AriaRole.Columnheader, "azc-grid-headerCell", isNeedSleep: false);
            foreach (ILocator header in AllRowHeaders)
            {
                string? displayName = await header.TextContentAsync();
                result.Add((displayName, null));
            }
            return result;
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
            try
            {
                int rowIndex = 1;
                int columnIndex = 0;
                // init DataTable column name               
                foreach (var rowHeader in rowHeaders)
                {
                    columnIndex++;
                    var cellLocator = await ElementHelper.GetByLocatorAsync(Grid_Locator, $"[aria-colindex='{columnIndex}'][aria-rowindex='{rowIndex}']", isNeedSleep: false);
                    var valueList = await cellLocator.AllInnerTextsAsync();
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

                int rowIndex = dt.Rows.Count > 0 ? dt.Rows.Count + 1 : 1;
                int columnIndex = 0;
                DataRow dr = dt.NewRow();
                try
                {
                    foreach (var rowHeader in rowHeaders)
                    {
                        columnIndex++;
                        var cellLocator = await ElementHelper.GetByLocatorAsync(Grid_Locator, $"[aria-colindex='{columnIndex}'][aria-rowindex='{rowIndex}']", isNeedSleep: false);
                        var valueList = await cellLocator.AllInnerTextsAsync();
                        dr[rowHeader.displayName] = valueList.FirstOrDefault();
                    }
                    dt.Rows.Add(dr);
                }
                catch
                {
                    if (retryCount < 1)
                    {
                        retryCount++;
                        try
                        {
                            await ClickLoadMoreButtonAsync();
                        }
                        catch
                        {
                            LogHelper.Info("No more data to load");
                        }
                        continue;
                    }
                    else
                    {
                        break;
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
            return await ControlHelper.GetByLocatorAsync(this.CurrentIPage, "[data-bind='pcControl: gridViewModel'][data-formelement='pcControl: gridViewModel']", 0, iFrameName: this.CurrentIFrameName);
        }

        public Task ClickGridCellToShowDetailsAsync(string RowCell)
        {
            throw new NotImplementedException();
        }       
        #endregion
    }
}
