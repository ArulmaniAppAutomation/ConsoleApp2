using Microsoft.Playwright;
using PlaywrightTests.Common.Helper;

namespace PlaywrightTests.Common.Controller.SearchBox
{
    public class SearchBoxBase : BaseController, IController
    {
        public SearchBoxBase(IPage? page, string? frameName, EnumHelper.Language language, ILocator? parentLocator = null) : base(page, frameName, language, parentLocator)
        {
        }

        public string GetCurrentLanguageText(string key)
        {
            string result = string.Empty;
            switch (this.CurrentLanguage)
            {
                case EnumHelper.Language.English:
                    result = key;
                    break;
                case EnumHelper.Language.Chinese:
                    {
                        switch (key)
                        {
                           case "Search":
                                result = "搜索";
                                break;
                        }
                    }
                    break;

            }
            return result;
        }       
    }
}
