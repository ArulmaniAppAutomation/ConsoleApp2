using LogService;
using Microsoft.Playwright;
using PlaywrightTests.Common.Helper;
using System.IO;

namespace PlaywrightTests.Common.Controller.ToolBar
{
    public class FXSCommandBar : ToolBarBase, ICommandBar
    {
        public FXSCommandBar(IPage page, string? frameName, EnumHelper.Language language, ILocator parentLocator = null) : base(page, frameName, language, parentLocator)
        {
        }
        public async Task ClickCommandBarSpecialNameButtonAsync(string buttonName)
        {
            try
            {
                await ClickCommandBarButtonByAriaLabelAsync(buttonName);
            }
            catch
            {
                await ClickCommandBarButtonByTextAAsync(buttonName);
            }
        }
        public async Task ClickCommandBarAddButtonAsync()
        {
            await ClickCommandBarButtonByAriaLabelAsync("Add");
        }
        public async Task ClickCommandBarCreateButtonAsync()
        {
            await ClickCommandBarButtonByAriaLabelAsync("Create");
        }
        public async Task ClickCommandBarCreatePolicyButtonAsync()
        {
            await ClickCommandBarButtonByTextAAsync("Create policy");
        }
        public async Task ClickCommandBarRefreshButtonAsync()
        {
            await ClickCommandBarButtonByAriaLabelAsync("Refresh");
        }
        public async Task ClickCommandBarFilterButtonAsync()
        {
            await ClickCommandBarButtonByAriaLabelAsync("Filter");
        }
        public async Task<string> ClickCommandBarExportButtonAsync()
        {
            var exportFile = await BaseExportFileAsync(GetType().Name, async () =>
            {
                await ClickCommandBarButtonByAriaLabelAsync("Export");
            });
            return exportFile;
        }
        public async Task ClickCommandBarColumnsButtonAsync()
        {
            await ClickCommandBarButtonByAriaLabelAsync("Columns");
        }
        public async Task ClickCommandBarBulkDeviceActionsButtonAsync()
        {
            await ClickCommandBarButtonByTextAAsync("Bulk device actions");
        }
        public async Task ClickCommandBarSaveButtonAsync()
        {
            await ClickCommandBarButtonByAriaLabelAsync("Save");
        }
        public async Task ClickCommandBarDiscardButtonAsync()
        {
            await ClickCommandBarButtonByAriaLabelAsync("Discard");
        }

        #region private function
        private async Task<ILocator> GetCommandBarContainerLocatorAsync()
        {
            if (ParentLocator != null)
            {
                var CommandBarContainerLocator = await ControlHelper.GetByClassAsync(ParentLocator, "azc-toolbar-container fxs-commandBar-itemList fxs-vivaresize");//azc-toolbar-container fxs-commandBar-itemList fxs-vivaresize
                return CommandBarContainerLocator.currentLocator;
            }
            else
            {
                var CommandBarContainerLocator = await ControlHelper.GetByClassAsync(this.CurrentIPage, "azc-toolbar-container fxs-commandBar-itemList fxs-vivaresize", iFrameName: CurrentIFrameName);
                return CommandBarContainerLocator.currentLocator;
            }
          
        }
        private async Task ClickCommandBarButtonByAriaLabelAsync(string ariaLabel)
        {
            var CommandBarContainerLocator = await GetCommandBarContainerLocatorAsync();
            await ControlHelper.ClickByClassWithAriaLableAsync(CommandBarContainerLocator, "azc-toolbarButton-container", ariaLabel, 0);
        }
        private async Task ClickCommandBarButtonByTextAAsync(string hasText)
        {
            var CommandBarContainerLocator = await GetCommandBarContainerLocatorAsync();
            await ControlHelper.ClickByClassAndHasTextAsync(CommandBarContainerLocator, "azc-toolbarButton-container", hasText, 0);
        }
        private async Task ClickCommandBarButtonAndHasTextAsync(string hasText)
        {
            var CommandBarContainerLocator = await GetCommandBarContainerLocatorAsync();
            await ControlHelper.ClickByClassAndHasTextAsync(CommandBarContainerLocator, "azc-toolbarButton-label fxs-commandBar-item-text", hasText, 0);
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

        public async Task ClickCommandBarDeleteAsync()
        {
            await ClickCommandBarButtonByAriaLabelAsync("Delete");
        }

        public Task ClickCommandBarOverFlowAsync()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
