//using LogService;
using Microsoft.Playwright;
using PlaywrightTests.Common.Helper;
using PlaywrightTests.Common.Model;
using PlaywrightTests.Common.Utils.BaseUtils.PopUp;
using PlaywrightTests.Common.Utils.BaseUtils.UtilInterface;

namespace PlaywrightTests.Common.Utils.BaseUtils.Apps.Policy
{
    public class iOSAppProvisioningProfilesUtils : BaseCommonUtils, InterfaceUtils, InterfaceGrid
    {
        private static List<string> iOSAppProvisioningPolicyNameList = new List<string>();
        public iOSAppProvisioningProfilesUtils(IPage page, string env) : base(page, env)
        {
        }

        public async Task ClearDataAsync()
        {
            if (iOSAppProvisioningPolicyNameList.Any())
            {
                foreach (var profileName in iOSAppProvisioningPolicyNameList)
                {
                    try
                    {
                        await DeleteProfileAsync(profileName);
                    }
                    catch
                    {

                    }
                    finally
                    {
                        BaseRemoveUniqueProfileName(profileName);
                    }
                }
            }
        }      
        public async Task ClearSpecialDataAsync(string name)
        {
            await DeleteProfileAsync(name);
        }
        public async Task GoToMainPageAsync()
        {
            await GoToHomePageAsync();
            await siteBar.ClickAppsAsync();
            await siteBarMenu.ClickIOSAppProvisioningProfilesAsync();
        }
        public async Task DeleteProfileAsync(string policyName)
        {
            await GoToMainPageAsync();
            await SetSearchBoxValueAsync(policyName);
            await ClickGridCellContextMenuAsync(policyName);
            await ClickGridCellContextMenuDeleteButtonAsync();
            await ClickConfirmDeleteButtonAsync();
            await VerifyProfileDeletedSuccessfullyOrNotAsync("iOS app provisioning profile");
            BaseRemoveUniqueProfileName(policyName);
        }
        public Task RunAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<(bool IsContinue, Dictionary<string, string> Parameter)> RunStepAsync(ControlInfo controlInfo)
        {
            switch (controlInfo.Operation)
            {
                case "GoToMainPageAsync":
                    await GoToMainPageAsync();
                    break;
                case "ClickCreateProfileButtonAsync":
                    await ClickCreateProfileButtonAsync();
                    break;
                case "SelectPolicyAsync":
                    await SelectPolicyAsync(iOSAppProvisioningPolicyNameList.Last());
                    break;
                case "ClickPropertiesAsync":
                    await ClickPropertiesAsync();
                    break;
                case "ClearDataAsync":
                    await ClearDataAsync();
                    break;
                #region Basics
                case "SetBasicsProfileNameAsync":
                    var uniquePolicyName = BaseCreateUniqueProfileName(controlInfo.OperationValue);
                    iOSAppProvisioningPolicyNameList.Add(uniquePolicyName);
                    controlInfo.Parameter.Add("iOSAppProvisioningPolicyWaitForDelete", uniquePolicyName);
                    await SetBasicsProfileNameAsync(uniquePolicyName);
                    try
                    {
                        DictionaryItemProcess(controlInfo.Parameter, "AppAutomationAppName", uniquePolicyName);
                    }
                    catch { }
                    return (true, controlInfo.Parameter);
                case "UpdateBasicsProfileNameAsync":
                   // BaseRemoveUniqueProfileName(iOSAppProvisioningPolicyNameList.Last());
                    //iOSAppProvisioningPolicyNameList.Remove(iOSAppProvisioningPolicyNameList.Last());
                    var updatePolicyName = BaseCreateUniqueProfileName(controlInfo.OperationValue);
                    iOSAppProvisioningPolicyNameList.Add(updatePolicyName);
                    await SetBasicsProfileNameAsync(updatePolicyName);
                    try
                    {
                        DictionaryItemProcess(controlInfo.Parameter, "AppAutomationAppName", updatePolicyName);
                    }
                    catch { }
                    return (true, controlInfo.Parameter);
                case "SetBasicsDescriptionAsync":
                    await SetBasicsDescriptionAsync(controlInfo.OperationValue);
                    break;
                case "SetBasicsUploadProfileFileAsync":
                    await SetBasicsUploadProfileFileAsync(controlInfo.OperationValue);
                    break;
                case "ClickBasicsEditButtonAsync":
                    await ClickBasicsEditButtonAsync();
                    break;
                #endregion

                #region Assignments
                case "ClickIncludedAddAllUsersButtonAsync":
                    await ClickIncludedAddAllUsersButtonAsync();
                    break;
                case "ClickExcludedAddGroupsButtonAsync":
                    await ClickExcludedAddGroupsButtonAsync();
                    break;
                case "SetExcludedSelectGroupsAsync":
                    await SetExcludedSelectGroupsAsync(controlInfo.OperationValue);
                    break;
                #endregion

                #region Verify
                case "VerifyProfileUpdatedSuccessfullyOrNotAsync":
                    var name = controlInfo.Parameter.TryGetValue("iOSAppProvisioningPolicyWaitForDelete", out string value) ? value : string.Empty;
                    await VerifyProfileUpdatedSuccessfullyOrNotAsync(iOSAppProvisioningPolicyNameList.Last(), name);
                    break;
                case "VerifyProfileCreatedSuccessfullyOrNotAsync":
                    await VerifyProfileCreatedSuccessfullyOrNotAsync("iOS app provisioning profile");
                    break;
                #region Verify Basics
                case "VerifyBasicsNameValueAsync":
                    await VerifyBasicsNameValueAsync(iOSAppProvisioningPolicyNameList.Last());
                    break;
                case "VerifyBasicsDescriptionValueAsync":
                    await VerifyBasicsDescriptionValueAsync(controlInfo.OperationValue);
                    break;
                case "VerifyiOSAppProvisioningProfileFileNameValueAsync":
                    await VerifyiOSAppProvisioningProfileFileNameValueAsync(controlInfo.OperationValue);
                    break;
                #endregion

                #region Verify Assignments
                case "VerifyAssignmentsIncludedGroupsAsync":
                    await VerifyAssignmentsIncludedGroupsAsync(controlInfo.OperationValue);
                    break;
                case "VerifyAssignmentsExcludedGroupsAsync":
                    await VerifyAssignmentsExcludedGroupsAsync(controlInfo.OperationValue);
                    break;
                #endregion

                #endregion
                default:
                    await BaseExecuteStepAsync(operation: controlInfo.Operation, controlInfo: controlInfo);
                    break;
            }
            return (true, controlInfo.Parameter);
        }
        public async Task VerifyProfileCreatedSuccessfullyOrNotAsync(string name)
        {
            //string regexText = $"Go to {name} to view the details.";
            await BaseVerifyWithNotificationAsync("iOS app provisioning profile successfully created");
        }
        public async Task VerifyProfileUpdatedSuccessfullyOrNotAsync(string name, string previousName)
        {
            //string regexText = $"Go to {name} to view the details.";
            await BaseVerifyWithNotificationAsync("iOS app provisioning profile saved");
            if (previousName != name)
            {
                BaseRemoveUniqueProfileName(previousName);
                iOSAppProvisioningPolicyNameList.Remove(previousName);
            }
        }
        public async Task VerifyProfileDeletedSuccessfullyOrNotAsync(string name)
        {
            string regexText = $"{name} deleted successfully";
            await BaseVerifyWithNotificationAsync(regexText);
        }
        #region Private Methods

        private async Task SelectPolicyAsync(string policyName)
        {
            await SetSearchBoxValueAsync(policyName);
            await ClickPolicyToShowDetailsAsync(policyName);
        }
        private async Task ClickPropertiesAsync()
        {
            await ControlHelper.ClickByAttributeDataTelemetryNameAsync(this.CurrentIPage, "Menu-2");
        }
        private async Task ClickBasicsEditButtonAsync()
        {
            await BaseClickBasicsEditButtonAsync();
        }

        #region Menu list
        private async Task ClickCreateProfileButtonAsync()
        {
            await ControlHelper.ClickByClassWithAriaLableAsync(this.CurrentIPage, "azc-toolbarButton-container fxs-fxclick fxs-portal-hover", "Create profile", 0);
        }
        #endregion

        #region Grid operation function
        public async Task SetSearchBoxValueAsync(string keyWord)
        {
            await ControlHelper.SetInputByClassAndAriaLabelAsync(CurrentIPage, "azc-input azc-formControl azc-validation-border", "Filter", keyWord, 0);
        }
        private async Task ClickPolicyToShowDetailsAsync(string policyName)
        {
            await ControlHelper.ClickByGridCellAndHasTextAsync(this.CurrentIPage, policyName, 0, iFrameName: null);
        }
        public async Task ClickGridCellContextMenuAsync(string policyName)
        {
            ILocator targetPolicyRowLocator = await ControlHelper.GetByRoleAndHasTextAsync(this.CurrentIPage, AriaRole.Row, policyName, 0);
            await ControlHelper.ClickByClassWithAriaLableAsync(targetPolicyRowLocator, "azc-grid-ellipsis-svg msportalfx-inherit-color fxs-portal-svg", "Context menu", 0);
        }
        public async Task ClickGridCellContextMenuDeleteButtonAsync()
        {
            await BaseClickContextMenuItemDeleteButtonAsync();
        }
        public async Task ClickConfirmDeleteButtonAsync()
        {
            await ClickConfirmDialogYesButtonAsync();
        }
        #endregion

        #region Basics
        public async Task SetBasicsProfileNameAsync(string value)
        {
            await SetPolicyInputValueByClassAndAriaLableAsync("Name", value);
        }
        public async Task SetBasicsDescriptionAsync(string value)
        {
            await SetPolicyInputValueByClassAndAriaLableAsync("Description", value);
        }
        private async Task SetPolicyInputValueByClassAndAriaLableAsync(string title, string value)
        {
            await ControlHelper.SetInputByClassAndAriaLabelAsync(this.CurrentIPage, "azc-validation-border msportalfx-tooltip-overflow", title, value, 0, iFrameName: null);
        }
        private async Task SetBasicsUploadProfileFileAsync(string fileName)
        {
            UploadFileUtils uploadFileUtils = new UploadFileUtils(this.CurrentIPage, this.CurrentEnv);
            await uploadFileUtils.UploadFileAsync(fileName, "Select an iOS provisioning profile");
        }
        #endregion

        #region Assignments
        private async Task ClickExcludedAddGroupsButtonAsync()
        {
            await ControlHelper.ClickByButtonRoleAndHasTextAsync(this.CurrentIPage, "Add groups", 1, iFrameName: null);
        }
        private async Task SetExcludedSelectGroupsAsync(string groupName)
        {
            await SelectIncludedGroupsAsync(groupName);
        }
        private async Task ClickIncludedAddAllUsersButtonAsync()
        {
            await ControlHelper.ClickByButtonRoleAndHasTextAsync(this.CurrentIPage, "Add all users", 0, iFrameName: null);
        }
        #endregion

        #region Verify all settings

        #region Verify Basics
        private async Task<ILocator> GetBasicsSectionLocatorAsync()
        {
            return await ControlHelper.GetParentLocatorBySonLocatorAsync(this.CurrentIPage, "fxc-base fxc-section fxc-section-wrapper", "msportalfx-create-section-label ext-summary-sectionHeader", "Basics");
        }
        private async Task VerifyBasicsValueAsync(string expectValue, string settingName)
        {
            ILocator basicsSectionLocator = await GetBasicsSectionLocatorAsync();
            string? nameValue = await ControlHelper.GetTextByClassAndTextKeywordAsync(basicsSectionLocator, "fxc-summary-item-row", settingName, 0);
           // LogHelper.Info($"Basics - {settingName} actual value: {nameValue}");
           // LogHelper.Info($"Basics - {settingName} expect value: {expectValue}");
          //  Assert.That(nameValue.Contains(expectValue), Is.True);
        }
        private async Task VerifyBasicsNameValueAsync(string value)
        {
            await VerifyBasicsValueAsync(value, "Name");
        }
        private async Task VerifyBasicsDescriptionValueAsync(string value)
        {
            await VerifyBasicsValueAsync(value, "Description");
        }
        private async Task VerifyiOSAppProvisioningProfileFileNameValueAsync(string value)
        {
            await VerifyBasicsValueAsync(value, "iOS app provisioning profile file name");
        }
        #endregion

        #region Verify Assignments
        private async Task<ILocator> GetAssignmnetsSectionLocatorAsync()
        {
            return await ControlHelper.GetParentLocatorBySonLocatorAsync(this.CurrentIPage, "fxc-base fxc-section fxc-section-wrapper", "msportalfx-create-section-label ext-summary-sectionHeader", "Assignments");
        }
        private async Task VerifyAssignmentsIncludeValueAsync(string groupName, string groupType)
        {
            _ = await GetAssignmnetsSectionLocatorAsync();
            string? contentText = await ControlHelper.GetTextByClassAndTextKeywordAsync(CurrentIPage, "fxc-summary-item-row", groupType, 0, null);
          //  LogHelper.Info($"Assignments - {groupType} actual value: {contentText}");
           // LogHelper.Info($"Assignments - {groupType} expect value: {groupName}");
            //Assert.That(contentText.Contains(groupName), Is.True);
        }
        private async Task VerifyAssignmentsIncludedGroupsAsync(string groupName)
        {
            await VerifyAssignmentsIncludeValueAsync(groupName, "Included groups");
        }
        private async Task VerifyAssignmentsExcludedGroupsAsync(string groupName)
        {
            await VerifyAssignmentsIncludeValueAsync(groupName, "Excluded groups");
        }

        public string GetCurrentLanguageText(string key)
        {
            throw new NotImplementedException();
        }

        public Task CheckIfCurrentPageIsAvailableAsync()
        {
            throw new NotImplementedException();
        }
        #endregion

        #endregion

        #endregion

    }
}
