using Microsoft.Playwright;
using PlaywrightTests.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account_Management.CommonBase
{
    public class BaseCommonUtils
    {

        protected ISiteBar siteBar;
        protected ISiteBarMenu siteBarMenu;
        public IPage _page { get; set; }
        private readonly string _portalUrl;
        public BaseCommonUtils(IPage page, string env)
        {
            _page = page;
            _portalUrl = env;
            
        }

        public  async Task ClickSelectBtnAsync(string? iFrameName = null)
        {
            await ControlHelper.ClickByButtonRoleAndNameAsync(_page, "Select", 0, iFrameName: iFrameName);
           // ConsoleHelper.ColoredResult(ConsoleColor.Green, "Click \"Select\" done...");
        }
        public async Task RefreshCurrentPageAsync()
        {
            await _page.ReloadAsync();
        }
        public async Task GoToHomePageAsync()
        {
            await RefreshCurrentPageAsync();
            await siteBar.ClickHomeAsync();
        }


        public void DictionaryItemProcess(Dictionary<string, string>? sources, string key, string value)
        {
            if (sources == null)
            {
                return;
            }
            if (sources.ContainsKey(key))
            {
                sources[key] = value;
            }
            else
            {
                sources.Add(key, value);
            }
        }

    }
}
