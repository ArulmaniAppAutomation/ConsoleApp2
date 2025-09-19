using Microsoft.Graph.Beta.Models;
using Microsoft.Playwright;
using PlaywrightTests.Common.Helper;

namespace PlaywrightTests.Common.Controller.SiteBarMenu
{
    public class MSPortalFxMenuSiteBarMenu : SiteBarMenuBase, ISiteBarMenu
    {
        private ILocator siteBarMenuLocator { get { return GetMenuLocatorAsync().Result; } }
        public MSPortalFxMenuSiteBarMenu(IPage? page, string? frameName, EnumHelper.Language language, ILocator? parentLocator = null) : base(page, frameName, language, parentLocator)
        {

        }
        #region A
        public async Task ClickAllAppsAsync()
        {
            await ClickSiteBarMenuAsync("All apps");
        }
        public async Task ClickAllDevicesAsync()
        {
            await ClickSiteBarMenuAsync("All devices");
        }
        public async Task ClickAndroidAsync()
        {
            await ClickSiteBarMenuAsync("Android");
        }
        public async Task ClickAuditLogsAsync()
        {
            await ClickSiteBarMenuAsync("Audit logs");
        }
        public async Task ClickAppCategoriesAsync()
        {
            await ClickSiteBarMenuAsync("App categories");
        }

        public async Task ClickAppInstallStatusAsync()
        {
            await ClickSiteBarMenuAsync("App install status");
        }
        #endregion
        #region C
        public async Task ClickComplianceAsync()
        {
            await ClickSiteBarMenuAsync("Compliance");
        }
        public async Task ClickConnectorsAndTokensAsync()
        {
            await ClickSiteBarMenuAsync("Connectors and tokens");
        }
        #endregion
        #region D
        public async Task ClickDeviceInstallStatusAsync()
        {
            await ClickSiteBarMenuAsync("Device install status");
        }
        #endregion
        #region E
        public async Task ClickEnrollmentAsync()
        {
            await ClickSiteBarMenuAsync("Enrollment");
        }
        #endregion
        #region F
        public async Task ClickFiltersAsync()
        {
            await ClickSiteBarMenuAsync("Filters");
        }
        public async Task ClickAssignmentFiltersAsync()
        {
            await ClickSiteBarMenuAsync("Assignment filters");
        }
        #endregion
        #region H
        public async Task ClickHelpAndSupportAsync()
        {
            await ClickSiteBarMenuAsync("Help and support");
        }
        #endregion
        #region I
        public async Task ClickIOSAppProvisioningProfilesAsync()
        {
            await ClickSiteBarMenuAsync("iOS app provisioning profiles");
        }
        public async Task ClickIOSIpadOSAsync()
        {
            await ClickSiteBarMenuAsync("iOS/iPadOS");
        }
        public async Task ClickIOSIpadOSUpdatesAsync()
        {
            await ClickSiteBarMenuAsync("iOS/iPadOS updates");
        }
        #endregion
        #region M
        public async Task ClickMacOSAsync()
        {
            await ClickSiteBarMenuAsync("macOS");
        }
        public async Task ClickMonitorAsync()
        {
            await ClickSiteBarMenuAsync("Monitor");
        }
        #endregion
        #region T
        public async Task ClickTeamViewerConnectorAsync()
        {
            await ClickSiteBarMenuAsync("TeamViewer Connector");
        }
        public async Task ClickTroubleshootAsync()
        {
            await ClickSiteBarMenuAsync("Troubleshoot");
        }
        #endregion
        #region O
        public async Task ClickOverviewAsync()
        {
            await ClickSiteBarMenuAsync("Overview");
        }

        #endregion
        #region P
        public async Task ClickPartnerComplianceManagementAsync()
        {
            await ClickSiteBarMenuAsync("Partner compliance management");
        }
        public async Task ClickPropertiesAsync()
        {
            await ClickSiteBarMenuAsync("Properties");
        }
        #endregion
        #region S
        public async Task ClickSModeSupplementalPoliciesAsync()
        {
            await ClickSiteBarMenuAsync("S Mode supplemental policies");
        }
        #endregion
        #region U
        public async Task ClickUserInstallStatusAsync()
        {
            await ClickSiteBarMenuAsync("User install status");
        }
        #endregion
        #region W
        public async Task ClickWindowsAsync()
        {
            await ClickSiteBarMenuAsync("Windows");
        }
        #endregion
        #region private functions
        private async Task<ILocator> GetMenuLocatorAsync()
        {
            return await ControlHelper.GetLocatorByClassAsync(this.CurrentIPage, "fxc-menu-scrollarea", -1, iFrameName: null);
        }
        private async Task ClickSiteBarMenuAsync(string name)
        {
            var currentLanguageText = GetCurrentLanguageText(name);
            await ControlHelper.ClickByClassAndHasTextAsync(siteBarMenuLocator, "fxc-menu-listView-item", currentLanguageText, -1);
        }
        #endregion
    }

}
