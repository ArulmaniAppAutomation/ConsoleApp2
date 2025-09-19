using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaywrightTests.Common.Controller.SiteBarMenu
{
    public interface ISiteBarMenu
    {
        #region A
        public Task ClickAllAppsAsync();
        public Task ClickAllDevicesAsync();
        public Task ClickAndroidAsync();
        public Task ClickAuditLogsAsync();

        public Task ClickAppCategoriesAsync();
        public Task ClickAppInstallStatusAsync();
        #endregion
        #region C
        public Task ClickComplianceAsync();
        public Task ClickConnectorsAndTokensAsync();
        #endregion
        #region D
        public Task ClickDeviceInstallStatusAsync();
        #endregion
        #region E
        public Task ClickEnrollmentAsync();
        #endregion
        #region F
        public Task ClickFiltersAsync();
        public Task ClickAssignmentFiltersAsync();
        #endregion
        #region H
        public Task ClickHelpAndSupportAsync();
        #endregion
        #region I
        public Task ClickIOSAppProvisioningProfilesAsync();
        public Task ClickIOSIpadOSAsync();
        public Task ClickIOSIpadOSUpdatesAsync();
       
        #endregion
        #region M
        public Task ClickMacOSAsync();
        public Task ClickMonitorAsync();
        #endregion
        #region T
        public Task ClickTeamViewerConnectorAsync();
        public Task ClickTroubleshootAsync();
        #endregion
        #region O
        public Task ClickOverviewAsync();
        #endregion
        #region P
        public Task ClickPartnerComplianceManagementAsync();
        public Task ClickPropertiesAsync();
        #endregion
        #region S
        public Task ClickSModeSupplementalPoliciesAsync();
        #endregion
        #region U
        public Task ClickUserInstallStatusAsync();
        #endregion
        #region W
        public Task ClickWindowsAsync();
        #endregion
    }
}
