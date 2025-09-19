using Microsoft.Playwright;
using PlaywrightTests.Common.Helper;

namespace PlaywrightTests.Common.Controller.SiteBar
{
    public class SiteBarBase : BaseController, IController
    {
        public SiteBarBase(IPage? page, string? frameName, EnumHelper.Language language, ILocator? parentLocator = null) : base(page, frameName, language, parentLocator)
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
