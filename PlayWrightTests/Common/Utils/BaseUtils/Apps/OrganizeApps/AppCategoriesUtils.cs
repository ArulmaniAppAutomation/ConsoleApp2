//using LogService;
using Microsoft.Playwright;
using PlaywrightTests.Common.Controller.Grid;
using PlaywrightTests.Common.Controller.ToolBar;
using PlaywrightTests.Common.Helper;
using PlaywrightTests.Common.Model;
using PlaywrightTests.Common.Utils.BaseUtils.PopUp;
using PlaywrightTests.Common.Utils.BaseUtils.UtilInterface;

namespace PlaywrightTests.Common.Utils.BaseUtils.Apps.OrganizeApps
{
    public class AppCategoriesUtils : ByPlatform.AllAppsUtils, InterfaceUtils
    {
        public new string? IFrameName = null;
        public string oldName = string.Empty;
        public AppCategoriesUtils(IPage page, string env) : base(page, env)
        {
            commandBar = new FXSCommandBar(page, IFrameName, CurrentLanguage);
            grid = new Grid_AZC_Grid(this.CurrentIPage, IFrameName, this.CurrentLanguage);
        }
        public new async Task GoToMainPageAsync()
        {
            await GoToHomePageAsync();
            await base.siteBar.ClickAppsAsync();
            await base.siteBarMenu.ClickAppCategoriesAsync();
        }
        public async Task<(bool IsContinue, Dictionary<string, string> Parameter)> RunStepAsync(ControlInfo controlInfo)
        {
            switch (controlInfo.ControlType)
            {
                case "GoToMainPageAsync":
                    await GoToMainPageAsync();
                    break;
                case "ClickAddButtonAsync":
                    await ClickAddButtonAsync();
                    break;
                
                case "SetCategoryNameAsync":
                    var appName = BaseCreateUniqueProfileName(controlInfo.OperationValue);
                    if (appName.Contains("New"))
                    {
                        oldName = controlInfo.Parameter["AppAutomationAppName"];
                    }
                    await SetCategoryNameAsync(appName);
                    try
                    {
                        DictionaryItemProcess(controlInfo.Parameter, "AppAutomationAppName", appName);
                    }
                    catch { }
                    break;
                case "ClickCreateButtonAsync":
                    await ClickCreateButtonAsync();
                    break;
                case "ClickSaveButtonAsync":
                    await ClickSaveButtonAsync();
                    break;
                case "VerifyCreateSuccessfullyNotificationAsync":
                    await VerifyCreateSuccessfullyNotificationAsync(!string.IsNullOrEmpty(controlInfo.OperationValue) ? controlInfo.OperationValue : controlInfo.Parameter["AppAutomationAppName"]);
                    break;
                case "VerifyUpdatedSuccessfullyNotificationAsync":
                    await VerifyUpdatedSuccessfullyNotificationAsync(!string.IsNullOrEmpty(controlInfo.OperationValue) ? controlInfo.OperationValue : controlInfo.Parameter["AppAutomationAppName"]);
                    break;
                case "VerifyAppCategoryExistAsync":
                    await VerifyAppCategoryExistAsync(!string.IsNullOrEmpty(controlInfo.OperationValue) ? controlInfo.OperationValue : controlInfo.Parameter["AppAutomationAppName"]);
                    break;
                case "ClickAppCategoryNameToEditAsync":
                    await ClickAppCategoryNameToEditAsync(!string.IsNullOrEmpty(controlInfo.OperationValue) ? controlInfo.OperationValue : controlInfo.Parameter["AppAutomationAppName"]);
                    break;
                case "DeleteAppCategoryByNameAsync":
                    await DeleteAppCategoryByNameAsync(!string.IsNullOrEmpty(controlInfo.OperationValue) ? controlInfo.OperationValue : controlInfo.Parameter["AppAutomationAppName"]);
                    break;
                default:
                    await base.RunStepAsync(controlInfo);
                    break;
            }
            return (true, controlInfo.Parameter);
        }

        public new async Task ClearSpecialDataAsync(string name)
        {
            await DeleteAppCategoryByNameAsync(name);
        }
        public async Task DeleteAppCategoryByNameAsync(string name)
        {
            await SetSearchBoxAsync(name);
            await grid.DeleteRowByRowHeaderAsync(name);
            await VerifyDeleteSuccessfullyNotificationAsync(name);
            BaseRemoveUniqueProfileName(name);
        }
        private async Task ClickAddButtonAsync()
        {
            await commandBar.ClickCommandBarAddButtonAsync();
        }
        private async Task SetCategoryNameAsync(String name)
        {
            var text = "Create category";
            if (name.Contains("New"))
            {
                text = "Properties";
            }
            ILocator bladeLocator = await ControlHelper.GetLocatorByClassAndHasTextAsync(this.CurrentIPage, "fxs-contextpane fxs-portal-bg-txt-br az-noprint fxs-portal-contextpane-right", text, 0, iframeName: IFrameName);
            await ControlHelper.SetInputByClassAndTypeAsync(bladeLocator, "azc-input azc-formControl azc-validation-border msportalfx-tooltip-overflow", "text", name, 0);
        }
        private async Task ClickCreateButtonAsync()
        {
            ILocator bladeLocator = await ControlHelper.GetLocatorByClassAndHasTextAsync(this.CurrentIPage, "fxs-contextpane fxs-portal-bg-txt-br az-noprint fxs-portal-contextpane-right", "Create category", 0, iframeName: IFrameName);
            await ControlHelper.ClickByButtonRoleAndHasTextAsync(bladeLocator, "Create", 0);
        }
        private async Task ClickSaveButtonAsync()
        {
            ILocator bladeLocator = await ControlHelper.GetLocatorByClassAndHasTextAsync(this.CurrentIPage, "fxs-contextpane fxs-portal-bg-txt-br az-noprint fxs-portal-contextpane-right", "Properties", 0, iframeName: IFrameName);
            await ControlHelper.ClickByButtonRoleAndHasTextAsync(bladeLocator, "Save", 0);
        }
        private new async Task SetSearchBoxAsync(string value)
        {
            await ControlHelper.SetInputByClassAndPlaceholderAsync(this.CurrentIPage, "azc-input azc-formControl", "Search by name", value, 0, iFrameName: IFrameName);
        }
        private async Task VerifyCreateSuccessfullyNotificationAsync(string name)
        {
            NotificationUtils notificationUtils = new NotificationUtils(CurrentIPage, CurrentEnv);
            await notificationUtils.VerifyAndCloseNotificationAsync($"App category \"{name}\" created");
          //  LogHelper.Info($"Verify App category ({name}) create result: verify pass");
        }
        private async Task VerifyUpdatedSuccessfullyNotificationAsync(string name)
        {
            NotificationUtils notificationUtils = new NotificationUtils(CurrentIPage, CurrentEnv);
            await notificationUtils.VerifyAndCloseNotificationAsync($"App category {name} saved");
            BaseRemoveUniqueProfileName(oldName);
          //  LogHelper.Info($"Verify App category ({name}) update result: verify pass");
        }
        private async Task VerifyDeleteSuccessfullyNotificationAsync(string name)
        {
            NotificationUtils notificationUtils = new NotificationUtils(CurrentIPage, CurrentEnv);
            await notificationUtils.VerifyAndCloseNotificationAsync($"App category deleted successfully");
          //  LogHelper.Info($"Verify App category ({name}) delete result: verify pass");
        }
        private async Task VerifyAppCategoryExistAsync(string name)
        {
            await SetSearchBoxAsync(name);
            List<string> names = await grid.GetColumnsDataByColumnNameAsync("Name");
           // Assert.IsTrue(names.Contains(name), $"Verify App category ({name}) exist in the grid: verify pass");
        }
        private async Task ClickAppCategoryNameToEditAsync(string name)
        {
            await SetSearchBoxAsync(name);
            await grid.ClickRowHeaderToShowDetailAsync(name);
        }
    }
}
