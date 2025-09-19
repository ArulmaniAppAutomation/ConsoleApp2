using Microsoft.Playwright;
using PlaywrightTests.Common.Helper;
using System.IO;

namespace PlaywrightTests.Common.Controller.ToolBar
{
    public class MSCommandBarWithMenubarRole : ToolBarBase, ICommandBar
    {
        public MSCommandBarWithMenubarRole(IPage page, string? frameName, EnumHelper.Language language) : base(page, frameName, language)
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
                await ClickCommandBarButtonByTextAsync(buttonName);
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
            await ClickCommandBarButtonByTextAsync("Create policy");
        }
        public Task ClickCommandBarFilterButtonAsync()
        {
            throw new NotImplementedException();
        }
        public async Task ClickCommandBarRefreshButtonAsync()
        {
            await ClickCommandBarButtonByAriaLabelAsync("Refresh");
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
        public async Task ClickCommandBarSyncAsync()
        {
            await ClickCommandBarButtonByTextAsync("Sync");
        }
        public async Task ClickCommandBarImportAsync()
        {
            await ClickCommandBarButtonByTextAsync("Import");
        }
        public async Task ClickCommandBarAssignUserAsync()
        {
            await ClickCommandBarButtonByTextAsync("Assign user");
        }
        public async Task ClickCommandBarDeleteAsync()
        {
            await ClickCommandBarButtonByTextAsync("Delete");
        }
        public async Task ClickCommandBarBulkDeviceActionsButtonAsync()
        {
            await ClickCommandBarButtonByTextAsync("Bulk device actions");
        }

        public async Task ClickCommandBarOverFlowAsync()
        {
            var CommandBarContainerLocator = await GetCommandBarContainerLocatorAsync();
            await ControlHelper.ClickByClassAsync(CommandBarContainerLocator, "ms-CommandBar-overflowButton", 0);
        }

        #region private function
        private async Task<ILocator> GetCommandBarContainerLocatorAsync()
        {
            var CommandBarContainerLocator = await ControlHelper.GetByRoleAndClassAsync(CurrentIPage, AriaRole.Menubar, "ms-CommandBar", 0, iFrameName: CurrentIFrameName);
            return CommandBarContainerLocator;
        }
        private async Task ClickCommandBarButtonByAriaLabelAsync(string ariaLabel)
        {
            var CommandBarContainerLocator = await GetCommandBarContainerLocatorAsync();
            await ControlHelper.ClickByClassWithAriaLableAsync(CommandBarContainerLocator, "ms-Button ms-Button--commandBar", ariaLabel, 0);

        }
        private async Task ClickCommandBarButtonByTextAsync(string hasText)
        {
            var CommandBarContainerLocator = await GetCommandBarContainerLocatorAsync();
            await ControlHelper.ClickByClassAndHasTextAsync(CommandBarContainerLocator, "ms-Button ms-Button--commandBar", hasText, 0);
        }
        #endregion
    }
}
