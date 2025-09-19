using LogService;
using Microsoft.Playwright;
using PlaywrightTests.Common.Controller.ConfirmDialog;
using PlaywrightTests.Common.Helper;
using System.Data;

namespace PlaywrightTests.Common.Controller.Grid
{
    public class Grid_AZC_Grid : GridBase, IGrid
    {
        private ILocator? Grid_Locator { get { return GetGridLocatorAsync().Result; } }
        private IConfirmDialog confirmDialog;
        public Grid_AZC_Grid(IPage? page, string? frameName, EnumHelper.Language language, ILocator? parentLocator = null) : base(page, frameName, language, parentLocator)
        {
            confirmDialog = new ConfirmDialog_FXS_Blade_Dialog(this.CurrentIPage, this.CurrentIFrameName, this.CurrentLanguage);
        }

        public async Task ClickColumnNameToSortASCAsync(string ColumnName)
        {
            await GridColumnSortAsync(ColumnName, "ascending");
        }

        public async Task ClickColumnNameToSortDESCAsync(string ColumnName)
        {
            await GridColumnSortAsync(ColumnName, "descending");
        }

        public Task ClickContextMenuAsync(int nth = 0)
        {
            throw new NotImplementedException();
        }

        public async Task ClickRowHeaderToShowDetailAsync(string RowHeader)
        {
            var rowLocator = await ControlHelper.GetByRoleAndHasTextAsync(Grid_Locator, AriaRole.Row, RowHeader, 0);
            await ControlHelper.ClickByClassAndHasTextAsync(rowLocator, "azc-grid-cellContent", RowHeader, 0);
        }
        public Task ClickSpecialColumnToShowDetailAsync(string policyName, string ColumnName)
        { throw new NotImplementedException(); }
        public Task RemoveRowByRowHeaderAsync(string RowHeader)
        {
            throw new NotImplementedException();
        }
        public async Task DeleteRowByRowHeaderAsync(string RowHeader)
        {
            // var rowLocator = await ControlHelper.GetLoatorByAriaLabelAsync(Grid_Locator, RowHeader, 0);
            var rowLocator = await ControlHelper.GetByRoleAndHasTextAsync(Grid_Locator, AriaRole.Row, RowHeader, 0);
            await ControlHelper.ClickByAriaLableAsync(rowLocator, "Context menu", 0);
            await ControlHelper.ClickByClassAndHasTextAsync(this.CurrentIPage, "fxs-contextMenu-item msportalfx-command-like-button", "Delete", 0, iFrameName: null);
            try
            {
                await confirmDialog.ClickDialogYesButtonAsync();
            }
            catch
            {
                await confirmDialog.ClickDialogOKButtonAsync();
            }
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
            base.Sleep(5);
            List<string> TempCacheRecordRowContent = new List<string>();
            var rowHeaders = await GetDisplayedRowHeadersAsync();
            //Get first row data
            DataTable dt = new DataTable();
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
            if (ParentLocator == null)
            {
                return (await ControlHelper.GetByClassAsync(this.CurrentIPage, "azc-grid-root", iFrameName: this.CurrentIFrameName)).currentLocator;
            }
            else
            {
                return (await ControlHelper.GetByClassAsync(ParentLocator, "azc-grid-root")).currentLocator;
            }
            
        }
        private async Task GridColumnSortAsync(string columnName, string sortType)
        {
            var gridHeaderLocator = await ControlHelper.GetByRoleAndClassAsync(Grid_Locator, AriaRole.Presentation, "azc-grid-tableHeader", 0);
            var columnLocator = await ControlHelper.GetLocatorByClassAndHasTextAsync(gridHeaderLocator, "azc-grid-headerCell", columnName, 0);
            await columnLocator.ClickAsync();
            var ariaSort = await columnLocator.GetAttributeAsync("aria-sort");
            if (ariaSort == sortType)
            {
                await columnLocator.ClickAsync();
            }        
        }

        public Task<bool> WaitGridDataToLoadAsync()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
