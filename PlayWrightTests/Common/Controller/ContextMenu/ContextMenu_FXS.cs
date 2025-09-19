using Microsoft.Playwright;
using PlaywrightTests.Common.Helper;

namespace PlaywrightTests.Common.Controller.ContextMenu
{
    internal class ContextMenu_FXS : BaseController, IContextMenu
    {
        private ILocator FXS_ContextualMenu_Locator
        {
            get { return GetFXSContextualMenuLocator(); }
        }
        public ContextMenu_FXS(IPage page, string iframeName, EnumHelper.Language language) : base(page, iframeName, language)
        {

        }
        public async Task CLickSpecialNameMenuItemAsync(string menuItemName)
        {
            await ClickContextMenuItemAsync(menuItemName);
        }
        public async Task ClickDeleteContextMenuItemAsync()
        {
            await ClickContextMenuItemAsync("Delete");
        }

        public Task ClickDuplicateContextMenuItemAsync()
        {
            throw new NotImplementedException();
        }

        public Task ClickExportJSONContextMenuItemAsync()
        {
            throw new NotImplementedException();
        }

        private async Task ClickContextMenuItemAsync(string menuName)
        {
            try
            {
                await ControlHelper.ClickByClassAndHasTextAsync(FXS_ContextualMenu_Locator, "fxs-contextMenu-text", menuName, 0);
            }
            catch
            {
                await ControlHelper.ClickByClassWithAriaLableAsync(FXS_ContextualMenu_Locator, "fxs-contextMenu-text", menuName, 0);
            }
        }
        private ILocator GetFXSContextualMenuLocator()
        {
            return ControlHelper.GetLocatorByClassAsync(this.CurrentIPage, "fxs-contextMenu fxs-popup", 0).Result;
        }
    }
}
