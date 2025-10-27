using Account_Management.Helper;
using Microsoft.Playwright;
using PlaywrightTests.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account_Management.CommonBase
{
    public class BaseCommonUtils:BaseController
    {

        protected ISiteBar siteBar;
        protected ISiteBarMenu siteBarMenu;
        public static IPage _page { get; set; }
        private readonly string _portalUrl;
        public BaseCommonUtils(IPage page, string env): base(page, null, EnumHelper.Language.English)
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
        public async Task ClickOKBtnAsync(string? iFrameName = null)
        {
            await ControlHelper.ClickByButtonRoleAndNameAsync(_page, "OK", 0, iFrameName: iFrameName);
         //   ConsoleHelper.ColoredResult(ConsoleColor.Green, "Click \"OK\" done...");
        }
        public async Task ClickCloseContentBtnAsync(IPage? page, string profileName, string? iframeName = null)
        {
            await ControlHelper.ClickByButtonRoleAndNameAsync(page, $"Close content '{profileName}'", 0, iframeName);
           // ConsoleHelper.ColoredResult(ConsoleColor.Green, $"Click \"Close content '{profileName}'\" done...");
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
                            case "Home":
                                result = "主页";
                                break;
                            case "Dashboard":
                                result = "仪表板";
                                break;
                            case "Devices":
                                result = "设备";
                                break;
                            case "Apps":
                                result = "应用";
                                break;
                            case "All services":
                                result = "所有服务";
                                break;
                            case "Endpoint security":
                                result = "终结点安全";
                                break;
                            case "Reports":
                                result = "报告";
                                break;
                            case "Users":
                                result = "用户";
                                break;
                            case "Groups":
                                result = "组";
                                break;
                            case "Tenant administration":
                                result = "租户管理";
                                break;
                            case "Troubleshooting + support":
                                result = "疑难解答 + 支持";
                                break;
                        }
                    }
                    break;
            }

            return result;

        }



    }
}
