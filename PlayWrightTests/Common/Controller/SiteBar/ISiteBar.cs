using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaywrightTests.Common.Controller.SiteBar
{
    public interface ISiteBar
    {
        public Task ClickHomeAsync();
        public Task ClickDashboardAsync();
        public Task ClickAllServicesAsync();
        public Task ClickDevicesAsync();
        public Task ClickAppsAsync();
        public Task ClickEndpointSecurityAsync();
        public Task ClickReportsAsync();
        public Task ClickUsersAsync();
        public Task ClickGroupsAsync();
        public Task ClickTenantAdministrationAsync();
        public Task ClickTroubleshootingSupportAsync();
    }
}
