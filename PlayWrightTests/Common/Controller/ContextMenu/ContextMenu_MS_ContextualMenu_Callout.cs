using Microsoft.Playwright;
using PlaywrightTests.Common.Helper;

namespace PlaywrightTests.Common.Controller.ContextMenu
{
    public class ContextMenu_MS_ContextualMenu_Callout : BaseController, IContextMenu
    {
        private ILocator MS_ContextualMenu_Callout_Locator { get { return GetMSContextualMenuCalloutLocator(); } }
        public ContextMenu_MS_ContextualMenu_Callout(IPage page, string iframeName, EnumHelper.Language language) : base(page, iframeName, language)
        {
          
        }
        public async Task CLickSpecialNameMenuItemAsync(string menuItemName)
        {
            await ClickContextMenuItemAsync(menuItemName);
        }
        public async Task ClickDuplicateContextMenuItemAsync()
        {
            await ClickContextMenuItemAsync("Duplicate");
        }
        public async Task ClickExportJSONContextMenuItemAsync()
        {
            await ClickContextMenuItemAsync("Export JSON");
        }
        public async Task ClickDeleteContextMenuItemAsync()
        {
            await ClickContextMenuItemAsync("Delete");
        }
        #region private function
        private ILocator GetMSContextualMenuCalloutLocator()
        {
            return ControlHelper.GetLocatorByClassAsync(this.CurrentIPage, "ms-Callout ms-ContextualMenu-Callout", 0, iFrameName: this.CurrentIFrameName).Result;
        }
        private async Task ClickContextMenuItemAsync(string menuName)
        {
            await ControlHelper.ClickByClassWithAriaLableAsync(MS_ContextualMenu_Callout_Locator, "ms-ContextualMenu-link", menuName, 0);
        }               
        #endregion
    }
}
