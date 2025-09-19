using Microsoft.Playwright;
using PlaywrightTests.Common.Helper;

namespace PlaywrightTests.Common.Controller.ToolBar
{
    public class AZCToolBar : ToolBarBase, ICommandBar
    {
        private ILocator ToolBarLocator
        {
            get
            {
                return GetCommandBarContainerLocatorAsync().Result;
            }
        }
        public AZCToolBar(IPage? page, string? frameName, EnumHelper.Language language, ILocator? parentLocator = null) : base(page, frameName, language, parentLocator)
        {
        }

        public async Task ClickCommandBarSpecialNameButtonAsync(string buttonName)
        {
            await ClickButtonAsync(buttonName);
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
            if (this.ParentLocator != null)
            {
                return await ControlHelper.GetLocatorByClassAsync(this.ParentLocator, "azc-toolbar-container fxs-commandBar-itemList");
            }
            var CommandBarContainerLocator = await ControlHelper.GetLocatorByClassAsync(this.CurrentIPage, "azc-toolbar-container fxs-commandBar-itemList", 0, iFrameName: this.CurrentIFrameName);
            return CommandBarContainerLocator;
        }

        private  Task ClickRefreshButtonAsync()
        {
            throw new NotImplementedException();
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

        #region
        private async Task ClickButtonAsync(string name)
        {
            await ControlHelper.ClickByClassAndHasTextAsync(ToolBarLocator, "azc-toolbarButton-label fxs-commandBar-item-text", name, 0);
        }
        #endregion
    }
}
