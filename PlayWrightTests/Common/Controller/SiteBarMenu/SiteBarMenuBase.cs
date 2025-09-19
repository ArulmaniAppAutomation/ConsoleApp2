using Microsoft.Playwright;
using PlaywrightTests.Common.Helper;

namespace PlaywrightTests.Common.Controller.SiteBarMenu
{
    public class SiteBarMenuBase : BaseController, IController
    {
        public SiteBarMenuBase(IPage? page, string? frameName, EnumHelper.Language language, ILocator? parentLocator = null) : base(page, frameName, language, parentLocator)
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
                            #region Devices ->
                            case "Overview":
                                result = "概述";
                                break;
                            case "All devices":
                                result = "所有设备";
                                break;
                            case "Monitor":
                                result = "监视器";
                                break;
                            #region Devices -> By platform
                            case "Windows":
                                result = "Windows";
                                break;
                            #region Devices -> By platform -> Windows
                            case "Windows devices":
                                result = "Windows 设备";
                                break;
                            #endregion

                            case "iOS/iPadOS":
                                result = "iOS/iPadOS";
                                break;
                            #region Devices -> By platform -> iOS/iPadOS
                            case "iOS/iPadOS devices":
                                result = "iOS/iPadOS 设备";
                                break;
                            #endregion

                            case "macOS":
                                result = "macOS";
                                break;
                            #region Devices -> By platform -> macOS
                            case "macOS devices":
                                result = "macOS 设备";
                                break;
                            #endregion

                            case "Android":
                                result = "Android";
                                break;
                            #region Devices -> By platform -> Android
                            case "Android devices":
                                result = "Android设备";
                                break;
                            #endregion

                            case "Linux":
                                result = "Linux";
                                break;
                            #region Devices -> By platform -> Linux
                            case "Linux devices":
                                result = "Linux设备";
                                break;
                            #endregion

                            #endregion

                            #region Devices -> Device onboarding
                            case "Windows 365":
                                result = "Windows 365";
                                break;
                            case "Enrollment":
                                result = "注册";
                                break;
                            #endregion

                            #region Devices -> Manage devices
                            case "Configuration":
                                result = "配置";
                                break;
                            case "Compliance":
                                result = "合规性";
                                break;
                            case "Conditional access":
                                result = "条件访问";
                                break;
                            case "Scripts and remediations":
                                result = "脚本和修正";
                                break;
                            case "Group Policy analytics":
                                result = "组策略分析";
                                break;
                            case "eSIM cellular profiles (preview)":
                                result = "eSIM 手机网络配置文件（预览）";
                                break;
                            case "Policy sets":
                                result = "策略集";
                                break;
                            case "Device categories":
                                result = "设备类别";
                                break;
                            case "Partner portals":
                                result = "合作伙伴门户";
                                break;
                            #endregion

                            #region Devices -> Manage updates
                            case "Windows updates":
                                result = "Windows 更新";
                                break;
                            case "Apple updates":
                                result = "Apple 更新";
                                break;
                            case "Android FOTA deployments":
                                result = "Android FOTA 部署";
                                break;
                            #endregion

                            #region Devices -> Organize devices
                            case "Device clean-up rules":
                                result = "设备清理规则";
                                break;
                            case "Filters":
                                result = "筛选器";
                                break;
                            #endregion


                           
                            case "Windows enrollment":
                                result = "Windows注册";
                                break;
                            case "Update rings for Windows 10 and later":
                                result = "Windows 10及更高版本的更新环";
                                break;                         
                            case "Update policies for iOS/iPadOS":
                                result = "iOS/iPadOS的更新策略";
                                break;                        
                            case "Android enrollment":
                                result = "Android注册";
                                break;
                            case "ChromeOS":
                                result = "ChromeOS";
                                break;
                            case "ChromeOS devices":
                                result = "ChromeOS设备";
                                break;
                            case "Enroll Devices":
                                result = "注册设备";
                                break;
                            case "Cloud PC creation":
                                result = "云PC创建";
                                break;
                            case "Configuration Profiles":
                                result = "配置文件";
                                break;
                            case "Compliance Policies":
                                result = "合规性策略";
                                break;
                            case "Notifications":
                                result = "通知";
                                break;
                            case "Conditional Access":
                                result = "有条件访问";
                                break;
                            case "Scripts":
                                result = "脚本";
                                break;
                            case "Windows 10 and later updates":
                                result = "Windows 10及更高版本的更新";
                                break;                       
                            case "Remediations":
                                result = "补救措施";
                                break;
                            case "Enrollment device platform restrictions":
                                result = "注册设备平台限制";
                                break;


                            #endregion
                            #region APP
                            case "All apps":
                                result = "所有应用";
                                break;                           
                            case "App protection policies":
                                result = "保护";
                                break;
                            case "App configuration policies":
                                result = "配置";
                                break;
                            case "iOS app provisioning profiles":
                                result = "iOS 应用预配配置文件";
                                break;
                            #endregion

                            #region Endpoint security
                            #region Endpoint security -> Overview
                            case "Security baselines":
                                result = "安全基线";
                                break;
                            #endregion

                            #region Endpoint security -> Manage
                            case "Antivirus":
                                result = "反恶意软件";
                                break;
                            case "Disk encryption":
                                result = "磁盘加密";
                                break;
                            case "Endpoint Privilege Management":
                                result = "终结点特权管理";
                                break;
                            case "Endpoint detection and response":
                                result = "终结点检测和响应";
                                break;
                            case "Attack surface reduction":
                                result = "攻击面减少";
                                break;
                            case "Firewall":
                                result = "防火墙";
                                break;
                            #endregion

                            #endregion

                            #region Reports
                            #region Reports -> Device management
                            case "Group policy analytics":
                                result = "组策略分析";
                                break;
                            case "Device compliance":
                                result = "设备合规性";
                                break;
                            #endregion
                            #region Reports -> Intune data warehouse
                            case "Data warehouse":
                                result = "数据仓库";
                                break;
                            #endregion
                            #endregion

                            #region Tenant administration
                            case "Roles":
                                result = "角色";
                                break;
                            #region Tenant administration -> Roles
                            #region Tenant administration -> Roles -> Manage
                            case "All roles":
                                result = "所有角色";
                                break;
                            case "Scope tags":
                                result = "作用域标签";
                                break;
                            #endregion
                            #region Tenant administration -> Roles -> Monitor
                            case "My permissions":
                                result = "我的权限";
                                break;
                            #endregion
                            #endregion

                            case "Audit logs":
                                result = "审核日志";
                                break;
                            case "Connectors and tokens":
                                result = "连接器和令牌";
                                break;

                            #region Tenant administration -> End user experience
                            case "Terms and conditions":
                                result = "条款和条件";
                                break;
                            #endregion

                         
                            #endregion

                            #region Troubleshooting + Support
                            case "Guided scenarios (preview)":
                                result = "引导式方案（预览）";
                                break;
                            case "Troubleshoot":
                                result = "疑难解答";
                                break;
                                #endregion
                        }
                    }
                    break;
            }

            return result;

        }
    }
}
