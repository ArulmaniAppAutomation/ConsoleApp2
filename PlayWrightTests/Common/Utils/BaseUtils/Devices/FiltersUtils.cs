//using LogService;
using Microsoft.Playwright;
using PlaywrightTests.Common.Controller.Grid;
using PlaywrightTests.Common.Controller.SearchBox;
using PlaywrightTests.Common.Controller.ToolBar;
using PlaywrightTests.Common.Helper;
using PlaywrightTests.Common.Model;
using PlaywrightTests.Common.Utils.BaseUtils.UtilInterface;
//using System.Windows.Controls;

namespace PlaywrightTests.Common.Utils.BaseUtils.Devices
{
    public class FiltersUtils : BaseCommonUtils, InterfaceUtils
    {
        private string IFrameName = "AssignmentFilterList.ReactView";
        private IGrid Grid;
        public ICommandBar commandBar;
        public ISearchBox searchBox;
        public FiltersUtils(IPage page, string env) : base(page, env)
        {
            commandBar = new MSCommandBarWithMenubarRole(this.CurrentIPage, IFrameName, this.CurrentLanguage);
            Grid = new Grid_MS_DetailsList(this.CurrentIPage, IFrameName, this.CurrentLanguage);
            searchBox = new MS_SearchBox(this.CurrentIPage, IFrameName, this.CurrentLanguage);
        }

        public Task ClearDataAsync()
        {
            throw new NotImplementedException();
        }
        public async Task ClearSpecialDataAsync(string name)
        {
            await SetSearchInputValueAsync(name);
            await Grid.DeleteRowByRowHeaderAsync(name);
        }
        public async Task GoToMainPageAsync()
        {
            await siteBar.ClickDevicesAsync();
            try
            {
                await siteBarMenu.ClickFiltersAsync();
            }
            catch
            {
                await siteBarMenu.ClickAssignmentFiltersAsync();
            }
        }
        public Task CheckIfCurrentPageIsAvailableAsync()
        {
            throw new NotImplementedException();
        }
        public Task RunAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<(bool IsContinue, Dictionary<string, string> Parameter)> RunStepAsync(ControlInfo controlInfo)
        {
            switch (controlInfo.Operation)
            {
                case "CheckFilterIsExistOrNotAsync":
                    if (await CheckFilterIsExistOrNotAsync(controlInfo.OperationValue))
                    {
                        return (false, controlInfo.Parameter);
                    }
                    else break;
                case "SetSearchInputValueAsync":
                    await SetSearchInputValueAsync(controlInfo.OperationValue);
                    break;
                case "ClickCreateManagedDevicesAsync":
                    await ClickCreateManagedDevicesAsync(this.CurrentIPage);
                    break;
                case "ClickCreateManagedAppsAsync":
                    await ClickCreateManagedAppsAsync(this.CurrentIPage);
                    break;
                #region Basics
                case "SetBasicsFilterNameAsync":
                    await SetBasicsFilterNameAsync(controlInfo.OperationValue);
                    break;
                case "SetBasicsFilterDescriptionAsync":
                    await SetBasicsFilterDescriptionAsync(controlInfo.OperationValue);
                    break;
                case "SelectBasicsPlatformAsync":
                    await SelectBasicsPlatformAsync(controlInfo.OperationValue);
                    break;

                #endregion

                #region Rules
                case "SelectRulesPropertyAsync":
                    await SelectRulesPropertyAsync(controlInfo.OperationValue);
                    break;
                case "SelectRulesOperatorAsync":
                    await SelectRulesOperatorAsync(controlInfo.OperationValue);
                    break;
                case "SetRulesValueAsync":
                    await SetRulesValueAsync(controlInfo.OperationValue);
                    break;
                #endregion
                #region
                case "VerifyFilterCreateSuccessfullyAsync":
                    await VerifyFilterCreateSuccessfullyAsync(controlInfo.OperationValue);
                    break;
                case "VerifyFilterDeleteSuccessfullyAsync":
                    await VerifyFilterDeleteSuccessfullyAsync(controlInfo.OperationValue);
                    break;
                case "VerifyFilterUpdateSuccessfullyAsync":
                    await VerifyFilterUpdateSuccessfullyAsync(controlInfo.OperationValue);
                    break;
                #endregion
                default:
                    await BaseExecuteStepAsync(controlInfo.Operation);
                    break;
            }
            return (true, controlInfo.Parameter);
        }
        public async Task<bool> CheckAndCreateFilterAsync(string filterName, List<ControlInfo>? controlInfos = null)
        {
            var isExist = await CheckFilterIsExistOrNotAsync(filterName);
            if (!isExist)
            {
                if (controlInfos != null && controlInfos.Any())
                {
                    foreach (var step in controlInfos)
                    {
                        await RunStepAsync(step);
                    }
                }
            }
            return isExist;
        }
        #region Private Methods     

        private async Task SetSearchInputValueAsync(string filterName)
        {
            await searchBox.SetSearchBoxValueAsync(filterName);
        }
        private async Task<bool> CheckFilterIsExistOrNotAsync(string filterName)
        {
            try
            {
                await GoToMainPageAsync();
                await SetSearchInputValueAsync(filterName);
                var result = await ControlHelper.GetLocatorByRowheaderByHasTextAsync(this.CurrentIPage, filterName, "AssignmentFilterList.ReactView");
                return await result.CountAsync() > 0;
            }
            catch (Exception)
            {
                return true;
            }
        }
        private async Task ClickCreateManagedDevicesAsync(IPage? page)
        {
            await commandBar.ClickCommandBarCreateButtonAsync();
            await ControlHelper.ClickByClassAndHasTextAsync(this.CurrentIPage, "ms-ContextualMenu-itemText", "Managed devices", 0, "AssignmentFilterList.ReactView");
        }
        private async Task ClickCreateManagedAppsAsync(IPage? page)
        {
            await commandBar.ClickCommandBarCreateButtonAsync();
            await ControlHelper.ClickByClassAndHasTextAsync(this.CurrentIPage, "ms-ContextualMenu-itemText", "Managed apps", 0, "AssignmentFilterList.ReactView");
        }

        #region Basics
        private async Task SetBasicsFilterNameAsync(string filterName)
        {
            var parentLocator = await ControlHelper.GetByRoleAndHasTextAsync(this.CurrentIPage, AriaRole.Group, "Filter name", 0, iFrameName: null);
            await ControlHelper.SetInputByClassAndTypeAsync(parentLocator, "azc-input azc-formControl azc-validation-border", "text", filterName, 0);
        }
        private async Task SetBasicsFilterDescriptionAsync(string description)
        {
            await ControlHelper.SetTextAreaValueByClassAsync(this.CurrentIPage, "azc-input azc-formControl azc-validation-border", description, 0, iFrameName: null);
        }
        private async Task SelectBasicsPlatformAsync(string platform)
        {
            await ControlHelper.ClickByButtonRoleAndNameAsync(this.CurrentIPage, "Toggle", 0, iFrameName: null);
            await ControlHelper.ClickByTreeItemRoleAndNameAsync(this.CurrentIPage, platform, 0, iFrameName: null);
        }
        #endregion
        #region Rules
        private async Task SelectRulesPropertyAsync(string propertyName)
        {
            await SelectRulesSettingAsync("Choose a Property", propertyName);
        }
        private async Task SelectRulesOperatorAsync(string operatorName)
        {
            await SelectRulesSettingAsync("Choose an Operator", operatorName);
        }
        private async Task SelectRulesSettingAsync(string key, string value)
        {
            var PropertyParentLocator = await ControlHelper.GetLocatorByClassAndHasTextAsync(this.CurrentIPage, "azc-formElementContainer", key, 0, iframeName: null);
            await ControlHelper.ClickByButtonRoleAndNameAsync(PropertyParentLocator, "Toggle", 0);
            await ControlHelper.ClickByTreeItemRoleAndNameAsync(PropertyParentLocator, value, 0);
        }
        private async Task SetRulesValueAsync(string value)
        {
            await ControlHelper.SetInputByClassAndPlaceholderAsync(this.CurrentIPage, "azc-input azc-formControl azc-validation-border", "Add a value", value, 0, iFrameName: null);
        }



        public string GetCurrentLanguageText(string key)
        {
            throw new NotImplementedException();
        }
        #endregion
        #region verify
        private async Task VerifyFilterCreateSuccessfullyAsync(string filterName)
        {
            string verifyString = $"{filterName} filter has been created";
            await BaseVerifyWithNotificationAsync(verifyString);
        }
        private async Task VerifyFilterDeleteSuccessfullyAsync(string filterName)
        {
            string verifyString = $"Filter has been deleted";
            await BaseVerifyWithNotificationAsync(verifyString);
        }
        private async Task VerifyFilterUpdateSuccessfullyAsync(string filterName)
        {
            string verifyString = $"{filterName} filter has been updated";
            await BaseVerifyWithNotificationAsync(verifyString);
        }
        #endregion
        #endregion
    }
}
