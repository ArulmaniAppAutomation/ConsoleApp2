using Microsoft.Playwright;
using PlaywrightTests.Common.Helper;

namespace PlaywrightTests.Common.Controller.SiteBar
{
    public class FXSSiteBar : SiteBarBase, ISiteBar
    {
        private ILocator? _siteBarLocator { get { return GetSiteBarLocatorAsync().Result; } }
        public FXSSiteBar(IPage? page, string? frameName, EnumHelper.Language language, ILocator? parentLocator = null) : base(page, frameName, language, parentLocator)
        {
        }
        public async Task ClickHomeAsync()
        {
            await ClickSiteBarItemAsync("Home");
        }
        public async Task ClickDashboardAsync()
        {
            await ClickSiteBarItemAsync("Dashboard");
        }
        public async Task ClickAllServicesAsync()
        {
            await ClickSiteBarItemAsync("All services");
        }
        public async Task ClickDevicesAsync()
        {
            await ClickSiteBarItemAsync("Devices");
        }
        public async Task ClickAppsAsync()
        {
            await ClickSiteBarItemAsync("Apps");
        }
        public async Task ClickEndpointSecurityAsync()
        {
            await ClickSiteBarItemAsync("Endpoint security");
        }
        public async Task ClickReportsAsync()
        {
            await ClickSiteBarItemAsync("Reports");
        }
        public async Task ClickUsersAsync()
        {
            await ClickSiteBarItemAsync("Users");
        }
        public async Task ClickGroupsAsync()
        {
            await ClickSiteBarItemAsync("Groups");
        }
        public async Task ClickTenantAdministrationAsync()
        {
            await ClickSiteBarItemAsync("Tenant administration");
        }
        public async Task ClickTroubleshootingSupportAsync()
        {
            await ClickSiteBarItemAsync("Troubleshooting + support");
        }

        #region private 
        private async Task<ILocator> GetSiteBarLocatorAsync()
        {
            return await ControlHelper.GetByRoleAndClassAsync(this.CurrentIPage, AriaRole.Presentation, "fxs-sidebar az-noprint msportalfx-unselectable", 0);
        }
        private async Task ClickSiteBarItemAsync(string name)
        {
            var currentLanguageText = GetCurrentLanguageText(name);
            await ControlHelper.ClickByClassAndHasTextAsync(_siteBarLocator, "fxs-sidebar-item-link", currentLanguageText, 0);
        }
        #endregion
    }
}
