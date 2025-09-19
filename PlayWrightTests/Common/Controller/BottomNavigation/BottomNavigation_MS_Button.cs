using LogService.Extension;
using Microsoft.Playwright;
using PlaywrightTests.Common.Helper;
using PlaywrightTests.Common.Utils.FeatureUtils;

namespace PlaywrightTests.Common.Controller.BottomNavigation
{
    public class BottomNavigation_MS_Button : BottomNavigationBase, IBottomNavigation
    {
        private LocatorAssertions locatorAssertions = new LocatorAssertions();
        private ILocator buttonSectionLocator
        {
            get { return GetButtonSectionLocator(); }
        }
        public BottomNavigation_MS_Button(IPage? page, string? frameName, EnumHelper.Language language, ILocator? parentLocator = null) : base(page, frameName, language, parentLocator)
        {
        }

        public async Task ClickBottomNavigationSpecialNameButtonAsync(string buttonName)
        {
            await ClickButtonByNameAsync(buttonName);
        }
        public async Task VerifyBottomNavigationSpecialNameButtonStatusAsync(string buttonName, string status)
        {
           var buttonLocator = await GetButtonLocatorByNameAsync(buttonName);
            switch (status)
            {
                case "Disabled":
                    await locatorAssertions.Expect(buttonLocator).ToBeDisabledAsync();
                    break;
                case "Enabled":
                    await locatorAssertions.Expect(buttonLocator).ToBeEnabledAsync();
                    break;
                default:
                    throw new CustomLogException($"Status ({status}) is not excepted");                    
            }
        }

        #region private method
        private ILocator GetButtonSectionLocator()
        {
            return ControlHelper.GetLocatorByClassAsync(this.CurrentIPage, "buttonSection", -1, iFrameName: this.CurrentIFrameName).Result;
        }

        private async Task ClickButtonByNameAsync(string name)
        {
            await ControlHelper.ClickByClassAndHasTextAsync(buttonSectionLocator, "ms-Button", name, 0);
        }
        private async Task<ILocator> GetButtonLocatorByNameAsync(string name)
        {
          return  await ControlHelper.GetLocatorByClassAndHasTextAsync(buttonSectionLocator, "ms-Button", name, 0);
        }
        #endregion
    }
}
