using LogService;
using Microsoft.Playwright;
using PlaywrightTests.Common.Controller.ConfirmDialog;
using PlaywrightTests.Common.Controller.ContextMenu;
using PlaywrightTests.Common.Helper;
using System.Data;
using System.Windows.Controls;

namespace PlaywrightTests.Common.Controller.Grid
{
    public class Grid_PCControl_DeviceTypeGrid : GridBase, IGrid
    {
        private ILocator? Grid_Locator { get { return GetGridLocatorAsync().Result; } }
        private IConfirmDialog confirmDialog;
        private IContextMenu contextMenu;
        public Grid_PCControl_DeviceTypeGrid(IPage? page, string? frameName, EnumHelper.Language language, ILocator? parentLocator = null) : base(page, frameName, language, parentLocator)
        {
            confirmDialog = new ConfirmDialog_FXS_Blade_Dialog(this.CurrentIPage, this.CurrentIFrameName, this.CurrentLanguage);
            contextMenu = new ContextMenu_FXS(this.CurrentIPage, this.CurrentIFrameName, this.CurrentLanguage);
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
        public async Task DeleteRowByRowHeaderAsync(string RowHeader)
        {
            var rowLocator = await ControlHelper.GetLocatorByClassAndHasTextAsync(Grid_Locator, "fxc-gc-row-content", RowHeader, 0);
            await ControlHelper.ClickByAriaLableAsync(rowLocator, "Click to open context menu", 0);
            await contextMenu.ClickDeleteContextMenuItemAsync();
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
            catch(Exception ex)
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
            string locator = "[data-bind='pcControl: deviceTypeGrid'][data-formelement='pcControl: deviceTypeGrid']";
            if (this.ParentLocator != null)
            {
                return await ControlHelper.GetByLocatorAsync(this.ParentLocator, locator, 0);
            }
            return await ControlHelper.GetByLocatorAsync(this.CurrentIPage, locator, 0, iFrameName: this.CurrentIFrameName);
        }

        public Task ClickGridCellToShowDetailsAsync(string RowCell)
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
