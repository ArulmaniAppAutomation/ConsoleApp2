using Microsoft.Playwright;
using PlaywrightTests.Common.Helper;

namespace PlaywrightTests.Common.Controller.SearchBox
{
    public class FXC_SearchBox : SearchBoxBase, ISearchBox
    {
        private ILocator SearchBoxLocator
        {
            get
            {
                return GetSearchBoxLocator();
            }
        }
        public FXC_SearchBox(IPage page, string? frameName, EnumHelper.Language language) : base(page, frameName, language)
        {

        }
        #region public function
        public async Task SetSearchBoxValueAsync(string value)
        {
            await ControlHelper.SetInputByClassAndPlaceholderAsync(SearchBoxLocator, "azc-input azc-formControl", GetCurrentLanguageText("Search by name or publisher"), value, 0);
        }
        public async Task ClearSearchBoxValueAsync()
        {
            await ControlHelper.SetInputByClassAndPlaceholderAsync(SearchBoxLocator, "azc-input azc-formControl", GetCurrentLanguageText("Search by name or publisher"), "", 0);
        }
        #endregion
        #region private function

        private ILocator GetSearchBoxLocator()
        {
            return ControlHelper.GetLocatorByClassAsync(this.CurrentIPage, "fxc-searchbox-container", -1, iFrameName: this.CurrentIFrameName).Result;
        }

        #endregion
    }
}
