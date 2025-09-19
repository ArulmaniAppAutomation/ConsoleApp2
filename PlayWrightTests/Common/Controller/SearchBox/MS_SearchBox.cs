using Microsoft.Playwright;
using Newtonsoft.Json.Linq;
using PlaywrightTests.Common.Helper;

namespace PlaywrightTests.Common.Controller.SearchBox
{
    public class MS_SearchBox : SearchBoxBase, ISearchBox
    {
        private ILocator SearchBoxLocator
        {
            get
            {
                return GetSearchBoxLocator();
            }
        }
        public MS_SearchBox(IPage page, string frameName, EnumHelper.Language language) : base(page, frameName, language)
        {

        }

        #region public function
        public async Task SetSearchBoxValueAsync(string value)
        {
            await ControlHelper.SetInputByClassAndPlaceholderAsync(SearchBoxLocator, "ms-SearchBox-field", GetCurrentLanguageText("Search"), value, 0);
        }
        public async Task ClearSearchBoxValueAsync()
        {
            await ControlHelper.SetInputByClassAndPlaceholderAsync(SearchBoxLocator, "ms-SearchBox-field", GetCurrentLanguageText("Search"), "", 0);
        }
        #endregion
        #region private function

        private ILocator GetSearchBoxLocator()
        {
            var iconLocator= ControlHelper.GetLocatorByClassAsync(this.CurrentIPage, "ms-SearchBox-iconContainer", -1, iFrameName: this.CurrentIFrameName).Result;
            return ControlHelper.GetParentLocatorBySonLocatorAsync(iconLocator, 1).Result;
        }

        #endregion
    }
}
