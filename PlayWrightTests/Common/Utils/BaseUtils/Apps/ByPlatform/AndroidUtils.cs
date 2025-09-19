//using LogService.Extension;
using Microsoft.Playwright;
using PlaywrightTests.Common.Controller.Grid;
using PlaywrightTests.Common.Controller.SearchBox;
using PlaywrightTests.Common.Controller.ToolBar;
using PlaywrightTests.Common.Helper;
using PlaywrightTests.Common.Model;
using PlaywrightTests.Common.Utils.BaseUtils.UtilInterface;
using static PlaywrightTests.Common.Helper.EnumHelper;

namespace PlaywrightTests.Common.Utils.BaseUtils.Apps.ByPlatform
{
    public class AndroidUtils : AllAppsUtils, InterfaceUtils
    {
        private FXSCommandBar FXSCommandBar;
        private ISearchBox searchBox;
        public AndroidUtils(IPage page, string env) : base(page, env)
        {
            grid = new Grid_MS_DetailsList(this.CurrentIPage, "AppList.ReactView", this.CurrentLanguage, noDataText: "No data found");
            searchBox = new MS_SearchBox(this.CurrentIPage, "AppList.ReactView", this.CurrentLanguage);
            FXSCommandBar = new FXSCommandBar(page, IFrameName, CurrentLanguage);

        }

        public new async Task CheckIfCurrentPageIsAvailableAsync()
        {
            await ClickRefreshButtonAsync();
        }

        public new async Task GoToMainPageAsync()
        {
            await GoToHomePageAsync();
            await base.siteBar.ClickAppsAsync();
            await base.siteBarMenu.ClickAndroidAsync();
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
                case "CheckIfCurrentPageIsAvailableAsync":
                    await CheckIfCurrentPageIsAvailableAsync();
                    break;
                #region Bottom Navigation          
                case "ClickCreateButtonAsync":
                    await bottomNavigation.ClickBottomNavigationSpecialNameButtonAsync("Create");
                    BaseThreadSleepMiddle();
                    DictionaryItemProcess(controlInfo.Parameter, "AppAutomationAppCreation", StepResultStatus.Success.ToString());
                    await BaseVerifyWithNotificationAsync($"Application \"{controlInfo.Parameter["AppAutomationAppName"]}\" created successfully");
                    break;           
                #endregion
                #region Command Bar
                case "ClickAddButtonAsync":
                    await ClickAddButtonAsync();
                    break;
                case "ClickRefreshButtonAsync":
                    await ClickRefreshButtonAsync();
                    break;
                case "ClickFilterButtonAsync":
                    await ClickFilterButtonAsync();
                    break;
                case "ClickExportButtonAsync":
                    await ClickExportButtonAsync();
                    break;
                case "ClickColumnsButtonAsync":
                    await ClickColumnsButtonAsync();
                    break;
                #endregion
                #region GooglePlay
                case "SearchGooglePlayAppAndSelectedAsync":
                    await SearchGooglePlayAppAndSelectedAsync(controlInfo.OperationValue);
                    break;
                #endregion
                #region Grid Operation
                case "SetSearchBoxValueAsync":
                    await SetSearchBoxValueAsync(controlInfo.OperationValue);
                    break;
                case "VerifyAppIsExistOrNotAsync":
                    await VerifyAppIsExistOrNotAsync(controlInfo.OperationValue);
                    break;
                case "ClickAppToShowDetailAsync":
                    await ClickAppToShowDetailAsync(controlInfo.OperationValue);
                    break;
                case "DeleteAppAsync":
                    await DeleteAppAsync(!string.IsNullOrEmpty(controlInfo.OperationValue) ? controlInfo.OperationValue : controlInfo.Parameter["AppAutomationAppName"]);
                    await base.RunStepAsync(new ControlInfo { ControlType = "SuccessAppAutomationAppDelete" });
                    break;
                #endregion
                #region App Information
                case "SetAppInformationDescriptionAsync":
                    await SetAppInformationDescriptionAsync(controlInfo.OperationValue);
                    break;                     
                #endregion
                #region Verify
             
                #endregion
                default:
                    await base.RunStepAsync(controlInfo);
                    break;
            }
            return (true, controlInfo.Parameter);
        }


        #region private function

        #region Command Bar
        private async Task ClickAddButtonAsync()
        {
            await FXSCommandBar.ClickCommandBarAddButtonAsync();
        }
        private async Task ClickRefreshButtonAsync()
        {
            await FXSCommandBar.ClickCommandBarRefreshButtonAsync();
        }
        private async Task ClickFilterButtonAsync()
        {
            await FXSCommandBar.ClickCommandBarFilterButtonAsync();
        }
        private async Task ClickExportButtonAsync()
        {
            await FXSCommandBar.ClickCommandBarExportButtonAsync();
        }
        private async Task ClickColumnsButtonAsync()
        {
            await FXSCommandBar.ClickCommandBarColumnsButtonAsync();
        }
        #endregion

        #region GooglePlay
        private async Task SearchGooglePlayAppAndSelectedAsync(string appName)
        {
            var frameLocator = await this.GetGooglePlayFrameLocatorAsync();
            BaseThreadSleepLong();
            await ControlHelper.SetInputByClassAndAriaLabelAsync(frameLocator, "VfPpkd-fmcmS-wGMbrd", "search", appName, 0);
            await ControlHelper.ClickByClassWithAriaLableAsync(frameLocator, "VfPpkd-LgbsSe VfPpkd-LgbsSe-OWXEXe-k8QpJ", "Search", 0);
            BaseThreadSleepMiddle();

            try
            {
                var appLocator = frameLocator.Locator($"div[title='{appName}']").Last;
                var isvisible = await appLocator.IsVisibleAsync();
                await appLocator.ClickAsync();
            }
            catch (Exception ex)
            {

            }

            //await ControlHelper.ClickByClassAndHasTextAsync(frameLocator, "vU6FJ p63iDd", appName, -1);
            BaseThreadSleepMiddle();
            await ControlHelper.ClickByClassAndHasTextAsync(frameLocator, "VfPpkd-LgbsSe VfPpkd-LgbsSe-OWXEXe-INsAgc", "Select", 0);
            BaseThreadSleepMiddle();
            await ControlHelper.ClickByClassWithAriaLableAsync(this.CurrentIPage, "azc-toolbarButton-container fxs-fxclick fxs-portal-hover", "Sync", 0);
        }

        private async Task<ILocator> GetGooglePlayFrameLocatorAsync()
        {
            var bodyLocator = this.CurrentIPage.FrameLocator("iframe[class='fxs-part-frame']").FrameLocator("iframe").Locator("body");
            await ElementHelper.IsExistAsync(bodyLocator);
            return bodyLocator;
        }
        #endregion

        #region Grid Operation
        private async Task SetSearchBoxValueAsync(string appName)
        {
            await searchBox.SetSearchBoxValueAsync(appName);
        }
        private async Task VerifyAppIsExistOrNotAsync(string appName)
        {
            int reTry = 0;
            while (reTry < 20)
            {
                try
                {
                    await ControlHelper.ClickByClassAndHasTextAsync(this.CurrentIPage, "azc-grid-cellContent", appName, 0, iFrameName: IFrameName, isClick: false);
                    break;
                }
                catch (Exception ex)
                {
                    reTry++;
                    await ClickRefreshButtonAsync();
                }
            }
            if (reTry >= 20)
            {
              //  throw new CustomLogException($"{CustomExceptionPrefix.CodeError_Verify_Failed}:App is not exist in the list");
            }
        }
        private async Task ClickAppToShowDetailAsync(string appName)
        {
            await grid.ClickRowHeaderToShowDetailAsync(appName);
        }
        private async Task DeleteAppAsync(string appName)
        {
            await grid.DeleteRowByRowHeaderAsync(appName);
            await BaseVerifyWithNotificationAsync("Application deleted successfully");
            base.BaseRemoveUniqueProfileName(appName);
        }
        
        public string GetCurrentLanguageText(string key)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region App Information

        private async Task SetAppInformationDescriptionAsync(string description)
        {
            await ControlHelper.SetInputByClassAndPlaceholderAsync(this.CurrentIPage, "azc-textarea azc-formControl", "Enter a description...", description, 0, iFrameName: IFrameName);
        }
     
        #endregion
        #endregion
    }
}
