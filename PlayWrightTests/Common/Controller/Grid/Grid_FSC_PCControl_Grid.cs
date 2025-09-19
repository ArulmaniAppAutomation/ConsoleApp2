using Microsoft.Playwright;
using PlaywrightTests.Common.Helper;
using System.Data;

namespace PlaywrightTests.Common.Controller.Grid
{
    public class Grid_FSC_PCControl_Grid : GridBase, IGrid
    {
        public Grid_FSC_PCControl_Grid(IPage? page, string? frameName, EnumHelper.Language language, ILocator? parentLocator = null) : base(page, frameName, language, parentLocator)
        {
        }

        public async Task ClickColumnNameToSortASCAsync(string ColumnName)
        {
            await SortRowHeaderAsync(ColumnName, "asc");
        }

        public async Task ClickColumnNameToSortDESCAsync(string ColumnName)
        {
            await SortRowHeaderAsync(ColumnName, "desc");
        }

        public Task ClickRowHeaderToShowDetailAsync(string RowHeader)
        {
            throw new NotImplementedException();
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
            string dataItemKeyPrefix = "fxc-gc-columnheader_";
            string dataItemValuePrefix = "fxc-gc-columncell_";
            var rowHeaders = await ControlHelper.GetAllLocatorsByRoleAndClassAsync(await GetGridLocatorAsync(), AriaRole.Columnheader, "fxc-gc-columnheader", isNeedSleep: false);
            foreach (var item in rowHeaders)
            {
                var columnName = await item.TextContentAsync();
                var classValue = await item.GetAttributeAsync("class");
                var itemKey = classValue?.Split(' ').Where(t => t.Contains(dataItemKeyPrefix)).LastOrDefault().Replace(dataItemKeyPrefix, dataItemValuePrefix);
                result.Add((columnName, itemKey));
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
                    var cellLocator = await ControlHelper.GetByRoleAndClassAsync(await GetGridLocatorAsync(), AriaRole.Gridcell, rowHeader.dataItemKey, 0, isNeedSleep: false);
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
            var rowLocators = await ControlHelper.GetAllLocatorsByRoleAndClassAsync(await GetGridLocatorAsync(), AriaRole.Row, "fxc-gc-row", isNeedSleep: false);
            foreach (var rowLocator in rowLocators)
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
            return dt;
        }
        public async Task<List<string>> GetColumnsDataByColumnNameAsync(string columnName)
        {
            var dt = await GetGridAllDataAsync();
            return BaseGetColumnsDataByColumnName(dt, columnName);
        }
        public async Task<string> GetGridContextTextAsync()
        {
            return await (await GetGridLocatorAsync()).InnerTextAsync();
        }
        public Task ClickContextMenuAsync(int n = 0)
        {
            throw new NotImplementedException();
        }
        #region        
        private async Task<ILocator> GetGridLocatorAsync()
        {
            return await ControlHelper.GetByLocatorAsync(this.CurrentIPage, "[data-bind='pcControl: grid'][data-formelement='pcControl: grid']", 0, iFrameName: this.CurrentIFrameName);
        }
        private async Task SortRowHeaderAsync(string ColumnName, string sortType)
        {
            var locator = await GetGridRowHeaderLocatorAsync(ColumnName);
            var sort = await locator.GetAttributeAsync("aria-sort");
            if (string.IsNullOrEmpty(sort) || !sort.ToLower().Contains(sortType.ToLower()))
            {
                await locator.ClickAsync();
            }
        }
        private async Task<ILocator> GetGridRowHeaderLocatorAsync(string rowHeader)
        {
            return await ControlHelper.GetByRoleAndHasTextAsync(await GetGridLocatorAsync(), AriaRole.Columnheader, rowHeader, 0);
        }

        public Task ClickGridCellToShowDetailsAsync(string RowCell)
        {
            throw new NotImplementedException();
        }

        public Task ClearGridAllDataAsync(string columnName, string keyWord)
        {
            throw new NotImplementedException();
        }

        public Task<bool> WaitGridDataToLoadAsync()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
