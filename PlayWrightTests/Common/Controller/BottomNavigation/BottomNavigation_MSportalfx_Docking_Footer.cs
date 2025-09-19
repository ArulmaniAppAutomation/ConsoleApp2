using LogService.Extension;
using Microsoft.Playwright;
using PlaywrightTests.Common.Helper;
using PlaywrightTests.Common.Utils.FeatureUtils;

namespace PlaywrightTests.Common.Controller.BottomNavigation
{
    public class BottomNavigation_MSportalfx_Docking_Footer : BottomNavigationBase, IBottomNavigation
    {
        private ILocator buttonSectionLocator
        {
            get { return GetButtonSectionLocator(); }
        }
        public BottomNavigation_MSportalfx_Docking_Footer(IPage? page, string? frameName, EnumHelper.Language language, ILocator? parentLocator = null) : base(page, frameName, language, parentLocator)
        {
        }

        public async Task ClickBottomNavigationSpecialNameButtonAsync(string buttonName)
        {
           await ClickButtonByNameAsync(buttonName);
        }

        public async Task VerifyBottomNavigationSpecialNameButtonStatusAsync(string buttonName, string status)
        {
            var buttonLocator=await GetButtonLocatorByNameAsync(buttonName);    
            // pending
        }

        #region private method
        private async Task ClickButtonByNameAsync(string name)
        {
            var simpleButtonLocator = await ControlHelper.GetLocatorByClassAndHasTextAsync(buttonSectionLocator, "Button fxc-base fxc-simplebutton",name,0);
            await ControlHelper.ClickByClassAndHasTextAsync(simpleButtonLocator, "fxs-button fxt-button", name, 0);
        }
        private async Task<ILocator> GetButtonLocatorByNameAsync(string name)
        {
            return await ControlHelper.GetLocatorByClassAndHasTextAsync(buttonSectionLocator, "fxs-button fxt-button", name, 0);
        }
        private ILocator GetButtonSectionLocator()
        {
            return ControlHelper.GetLocatorByClassAsync(this.CurrentIPage, "msportalfx-docking-footer", -1, iFrameName: this.CurrentIFrameName).Result;
        }
        #endregion
    }
}
