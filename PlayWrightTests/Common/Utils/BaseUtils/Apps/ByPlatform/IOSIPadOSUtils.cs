using Microsoft.Playwright;
using PlaywrightTests.Common.Controller.Grid;
using PlaywrightTests.Common.Controller.SearchBox;
using PlaywrightTests.Common.Controller.ToolBar;
using PlaywrightTests.Common.Model;
using PlaywrightTests.Common.Utils.BaseUtils.UtilInterface;

namespace PlaywrightTests.Common.Utils.BaseUtils.Apps.ByPlatform
{
    public class IOSIPadOSUtils : AllAppsUtils, InterfaceUtils
    {
        public IOSIPadOSUtils(IPage page, string env) : base(page, env)
        {
        }
        public new async Task GoToMainPageAsync()
        {
            await GoToHomePageAsync();
            await base.siteBar.ClickAppsAsync();
            await base.siteBarMenu.ClickAllAppsAsync();
            if (!await CheckIfCurrentPageIsReactViewPageAsync())
            {
                commandBar = new FXSCommandBar(this.CurrentIPage, null, CurrentLanguage);
                grid = new Grid_AZC_Grid(this.CurrentIPage, null, this.CurrentLanguage);
                searchBox = new FXC_SearchBox(this.CurrentIPage, null, this.CurrentLanguage);
            }
            await base.siteBarMenu.ClickIOSIpadOSAsync();
        }
        public new async Task<(bool IsContinue, Dictionary<string, string> Parameter)> RunStepAsync(ControlInfo controlInfo)
        {
            switch (controlInfo.ControlType)
            {
                case "GoToMainPageAsync":
                    await GoToMainPageAsync();
                    break;
                #region grid process
                case "DeleteAppAsync":
                    await DeleteAppAsync(!string.IsNullOrEmpty(controlInfo.OperationValue) ? controlInfo.OperationValue : controlInfo.Parameter["AppAutomationAppName"]);
                    await base.RunStepAsync(new ControlInfo { ControlType = "SuccessAppAutomationAppDelete" });
                    break;
                #endregion        
             
                default:
                    await base.RunStepAsync(controlInfo);
                    break;
            }
            return (true, controlInfo.Parameter);
        }
        #region Private Methods
        #region grid process
        private async Task DeleteAppAsync(string appName)
        {
            await base.RunStepAsync(new ControlInfo { ControlType = "DeleteAppByNameAsync" });
            await BaseVerifyWithNotificationAsync("Application deleted successfully");
        }
        #endregion
  
        #endregion
    }
}
