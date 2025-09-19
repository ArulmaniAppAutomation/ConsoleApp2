using Microsoft.Playwright;
using PlaywrightTests.Common.Helper;

namespace PlaywrightTests.Common.Controller.ToolBar
{
    public class PCControlToolBar : ToolBarBase, ICommandBar
    {
        private ILocator ToolBarLocator
        {
            get
            {
                return GetCommandBarContainerLocatorAsync().Result;
            }
        }
        public PCControlToolBar(IPage? page, string? frameName, EnumHelper.Language language, ILocator? parentLocator = null) : base(page, frameName, language, parentLocator)
        {
        }

        public async Task ClickCommandBarSpecialNameButtonAsync(string buttonName)
        {
            await ControlHelper.ClickDivByAttributeDataTelemetryNameAsync(ToolBarLocator, buttonName, 0);
        }
        public Task ClickCommandBarAddButtonAsync()
        {
            throw new NotImplementedException();
        }
        public Task ClickCommandBarCreateButtonAsync()
        {
            throw new NotImplementedException();
        }
        public Task ClickCommandBarCreatePolicyButtonAsync()
        {
            throw new NotImplementedException();
        }
        public Task ClickCommandBarBulkDeviceActionsButtonAsync()
        {
            throw new NotImplementedException();
        }

        public Task ClickCommandBarColumnsButtonAsync()
        {
            throw new NotImplementedException();
        }

        public Task<string> ClickCommandBarExportButtonAsync()
        {
            throw new NotImplementedException();
        }

        public Task ClickCommandBarFilterButtonAsync()
        {
            throw new NotImplementedException();
        }

        public async Task ClickCommandBarRefreshButtonAsync()
        {
            await ClickRefreshButtonAsync();
        }

        #region private function
        private async Task<ILocator> GetCommandBarContainerLocatorAsync()
        {
            var CommandBarContainerLocator = await ControlHelper.GetByLocatorAsync(this.CurrentIPage, "[data-bind='pcControl: toolbar'][data-formelement='pcControl: toolbar'][class*='fxc-base fxc-toolbar azc-toolbar']", 0, iFrameName: this.CurrentIFrameName);
            return CommandBarContainerLocator;
        }

        private async Task ClickRefreshButtonAsync()
        {
            await ControlHelper.ClickDivByAttributeDataTelemetryNameAsync(ToolBarLocator, "Command-Refresh", 0);
        }

        public Task ClickCommandBarSyncAsync()
        {
            throw new NotImplementedException();
        }

        public Task ClickCommandBarImportAsync()
        {
            throw new NotImplementedException();
        }

        public Task ClickCommandBarAssignUserAsync()
        {
            throw new NotImplementedException();
        }

        public Task ClickCommandBarDeleteAsync()
        {
            throw new NotImplementedException();
        }

        public Task ClickCommandBarOverFlowAsync()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
