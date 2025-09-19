using Microsoft.Playwright;
using PlaywrightTests.Common.Helper;


namespace PlaywrightTests.Common.Controller.BottomNavigation
{
    public class BottomNavigationBase : BaseController, IController
    {
        public BottomNavigationBase(IPage? page, string? frameName, EnumHelper.Language language, ILocator? parentLocator = null) : base(page, frameName, language, parentLocator)
        {
        }

        public string GetCurrentLanguageText(string key)
        {
            throw new NotImplementedException();
        }
    }

}
