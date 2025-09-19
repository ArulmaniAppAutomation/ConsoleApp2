using Microsoft.Playwright;
using PlaywrightTests.Common.Controller.Grid;
using PlaywrightTests.Common.Controller.SearchBox;
using PlaywrightTests.Common.Controller.ToolBar;
using PlaywrightTests.Common.Model;
using PlaywrightTests.Common.Utils.BaseUtils.UtilInterface;

namespace PlaywrightTests.Common.Utils.BaseUtils.Apps.ByPlatform
{
    public class WindowsUtils : AllAppsUtils, InterfaceUtils
    {
        public WindowsUtils(IPage page, string env) : base(page, env)
        {

        }
        public new async Task GoToMainPageAsync()
        {
            await GoToHomePageAsync();
            await base.siteBar.ClickAppsAsync();
            await base.siteBarMenu.ClickWindowsAsync();
            if (!await CheckIfCurrentPageIsReactViewPageAsync())
            {
                commandBar = new FXSCommandBar(this.CurrentIPage, null, CurrentLanguage);
                grid = new Grid_AZC_Grid(this.CurrentIPage, null, this.CurrentLanguage);
                searchBox = new FXC_SearchBox(this.CurrentIPage, null, this.CurrentLanguage);
            }
        }
        public new async Task<(bool IsContinue, Dictionary<string, string> Parameter)> RunStepAsync(ControlInfo controlInfo)
        {
            
            switch (controlInfo.ControlType)
            {
                case "GoToMainPageAsync":
                    await GoToMainPageAsync();
                    break;
                default:
                    return await base.RunStepAsync(controlInfo);
            }
            return (true, controlInfo.Parameter);
        }
    }
}
