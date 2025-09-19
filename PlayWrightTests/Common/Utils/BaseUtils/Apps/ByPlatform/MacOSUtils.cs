using Microsoft.Playwright;
using PlaywrightTests.Common.Controller.Grid;
using PlaywrightTests.Common.Controller.SearchBox;
using PlaywrightTests.Common.Controller.ToolBar;
using PlaywrightTests.Common.Helper;
using PlaywrightTests.Common.Model;
using PlaywrightTests.Common.Utils.BaseUtils.UtilInterface;

namespace PlaywrightTests.Common.Utils.BaseUtils.Apps.ByPlatform
{
    public class MacOSUtils : AllAppsUtils, InterfaceUtils
    {
        public MacOSUtils(IPage page, string env) : base(page, env)
        {
        }
        public new async Task GoToMainPageAsync()
        {
            await GoToHomePageAsync();
            await base.siteBar.ClickAppsAsync();
            await base.siteBarMenu.ClickMacOSAsync();
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
                case "SetAppInformationDescriptionAsync":
                    await SetAppInformationDescriptionAsync(controlInfo.OperationValue);
                    break;
                default:
                    await base.RunStepAsync(controlInfo);
                    break;
            }
            return (true, controlInfo.Parameter);
        }
        #region private methods
        #region App Information
        private async Task SetAppInformationDescriptionAsync(string description)
        {
            await ControlHelper.SetInputByClassAndPlaceholderAsync(this.CurrentIPage, "azc-textarea azc-formControl", "Enter a description...", description, 0, iFrameName: IFrameName);
        }
        #endregion

        #endregion
    }
}
