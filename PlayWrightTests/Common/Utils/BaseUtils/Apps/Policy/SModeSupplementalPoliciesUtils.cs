//using LogService;
using Microsoft.Playwright;
using PlaywrightTests.Common.Controller.Grid;
using PlaywrightTests.Common.Controller.VerifySection;
using PlaywrightTests.Common.Helper;
using PlaywrightTests.Common.Model;
using PlaywrightTests.Common.Utils.BaseUtils.PopUp;
using PlaywrightTests.Common.Utils.BaseUtils.UtilInterface;

namespace PlaywrightTests.Common.Utils.BaseUtils.Apps.Policy
{
    public class SModeSupplementalPoliciesUtils : BaseCommonUtils, InterfaceUtils, InterfaceGrid
    {
        private static List<string> SModeSupplementalPoliciesNameList = new List<string>();
        public new string? IFrameName = "WdacPoliciesList.ReactView";
        IVerifySection verify_Basic_Section_PCControl;
        IVerifySection verify_Assignment_Section_PCControl;
        private static string uploadPlaceholder = "Select a policy";
        IGrid grid;
        public SModeSupplementalPoliciesUtils(IPage page, string env) : base(page, env)
        {
            verify_Basic_Section_PCControl = new Verify_Section_PCControl_FXC_Summary(page, "Basics", null, EnumHelper.Language.English, null);
            verify_Assignment_Section_PCControl = new Verify_Section_PCControl_FXC_Summary(page, "Assignments", null, EnumHelper.Language.English, null);
            grid = new Grid_MS_DetailsList(page, IFrameName, EnumHelper.Language.English, noDataText: "No data");
        }

        public async Task ClearDataAsync()
        {
            uploadPlaceholder = "Select a policy";
            if (SModeSupplementalPoliciesNameList.Any())
            {
                foreach (var profileName in SModeSupplementalPoliciesNameList)
                {
                    try
                    {
                        await DeleteProfileAsync(profileName);                       
                    }
                    catch
                    {
                      //  LogHelper.Warning($"Delete {profileName} failed, please check it manually!");
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
            await siteBarMenu.ClickSModeSupplementalPoliciesAsync();
        }
        public async Task DeleteProfileAsync(string policyName)
        {
            await GoToMainPageAsync();
            await SetSearchBoxValueAsync(policyName);
            //await ClickGridCellContextMenuAsync(policyName);
            //await ClickGridCellContextMenuDeleteButtonAsync();
            //await ClickConfirmDeleteButtonAsync();
            await grid.DeleteRowByRowHeaderAsync(policyName);
            await VerifyProfileDeletedSuccessfullyOrNotAsync(SModeSupplementalPoliciesNameList.Last());
            BaseRemoveUniqueProfileName(policyName);
        }
        public async Task<(bool IsContinue, Dictionary<string, string> Parameter)> RunStepAsync(ControlInfo controlInfo)
        {
            switch (controlInfo.Operation)
            {
                case "GoToMainPageAsync":
                    await GoToMainPageAsync();
                    break;
                case "ClearHistoryDataByKeywordAsync":
                    await ClearHistoryDataByKeywordAsync(controlInfo.OperationValue);
                    break;
                case "ClickCreatePolicyButtonAsync":
                    await ClickCreatePolicyButtonAsync();
                    break;
                case "SelectPolicyAsync":
                    await SelectPolicyAsync(SModeSupplementalPoliciesNameList.Last());
                    break;
                case "ClearDataAsync":
                    await ClearDataAsync();
                    break;
                #region Basics
                case "SetBasicsProfileNameAsync":
                    var uniquePolicyName = BaseCreateUniqueProfileName(controlInfo.OperationValue);
                    SModeSupplementalPoliciesNameList.Add(uniquePolicyName);
                    controlInfo.Parameter.Add("SModeSupplementalPolicyWaitForDelete", uniquePolicyName);
                    await SetBasicsProfileNameAsync(uniquePolicyName);
                    try
                    {
                        DictionaryItemProcess(controlInfo.Parameter, "AppAutomationAppName", uniquePolicyName);
                    }
                    catch { }
                    return (true, controlInfo.Parameter);
                case "UpdateBasicsPolicyNameAsync":
                    var updatePolicyName = BaseCreateUniqueProfileName(controlInfo.OperationValue);
                    SModeSupplementalPoliciesNameList.Add(updatePolicyName);
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
                case "SetBasicsUploadPolicyFileAsync":
                    await SetBasicsUploadPolicyFileAsync(controlInfo.OperationValue);
                    break;
                case "ClickBasicsEditButtonAsync":
                    await ClickBasicsEditButtonAsync();
                    break;
                #endregion

                #region Assignments
                case "ClickIncludedAddAllDevicesButtonAsync":
                    await ClickIncludedAddAllDevicesButtonAsync();
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
                    var name = controlInfo.Parameter.TryGetValue("SModeSupplementalPolicyWaitForDelete", out string value) ? value : string.Empty;
                    await VerifyProfileUpdatedSuccessfullyOrNotAsync(SModeSupplementalPoliciesNameList.Last(), name);
                    break;
                case "VerifyProfileCreatedSuccessfullyOrNotAsync":
                    await VerifyProfileCreatedSuccessfullyOrNotAsync(SModeSupplementalPoliciesNameList.Last());
                    break;
                #region Verify Basics
                case "VerifyBasicsNameValueAsync":
                    await VerifyBasicsNameValueAsync(SModeSupplementalPoliciesNameList.Last());
                    break;
                case "VerifyBasicsDescriptionValueAsync":
                    await VerifyBasicsDescriptionValueAsync(controlInfo.OperationValue);
                    break;
                case "VerifyPolicyFileNameValueAsync":
                    await VerifyPolicyFileNameValueAsync(controlInfo.OperationValue);
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
                    await BaseExecuteStepAsync(controlInfo.Operation, operationValue: controlInfo.OperationValue, controlInfo: controlInfo);
                    break;
            }
            return (true, controlInfo.Parameter);
        }
        public async Task ClearHistoryDataByKeywordAsync(string keyword)
        {
            uploadPlaceholder = "Select a policy";
            await SetSearchBoxValueAsync(keyword);
            await grid.ClearGridAllDataAsync("Name", keyword);
        }
        public async Task VerifyProfileUpdatedSuccessfullyOrNotAsync(string name, string previousName)
        {
            string regexText = $"Go to {name} to view the details.";
            await BaseVerifyWithNotificationAsync(regexText);
            if (previousName != name)
            {
                BaseRemoveUniqueProfileName(previousName);
                SModeSupplementalPoliciesNameList.Remove(previousName);
            }
        }

        public async Task VerifyProfileCreatedSuccessfullyOrNotAsync(string name)
        {
            await BaseVerifyWithNotificationAsync("Policy successfully created");
        }
        public async Task VerifyProfileDeletedSuccessfullyOrNotAsync(string name)
        {
            await BaseVerifyWithNotificationAsync("Policy deleted.");
        }
        #region Private Methods

        private async Task SelectPolicyAsync(string policyName)
        {
            await SetSearchBoxValueAsync(policyName);
            await grid.ClickSpecialColumnToShowDetailAsync(policyName, "Name");
        }

        private async Task ClickBasicsEditButtonAsync()
        {
            await BaseClickBasicsEditButtonAsync();
        }

        #region Menu list
        private async Task ClickCreatePolicyButtonAsync()
        {
            await ControlHelper.ClickByClassWithAriaLableAsync(this.CurrentIPage, "ms-Button ms-Button--commandBar ms-CommandBarItem-link", "Create policy", 0, iFrameName: IFrameName);
        }
        #endregion

        #region Grid operation function
        public async Task SetSearchBoxValueAsync(string keyWord)
        {
            await ControlHelper.SetInputByClassAndAriaLabelAsync(CurrentIPage, "ms-SearchBox-field", "Search", keyWord, 0, iFrameName: IFrameName);
        }
        public async Task ClickGridCellContextMenuAsync(string policyName)
        {
            ILocator targetPolicyRowLocator = await ControlHelper.GetByRoleAndHasTextAsync(this.CurrentIPage, AriaRole.Row, policyName, 0, iFrameName: IFrameName);
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
        private async Task SetBasicsUploadPolicyFileAsync(string fileName)
        {
            UploadFileUtils uploadFileUtils = new UploadFileUtils(this.CurrentIPage, this.CurrentEnv);
            await uploadFileUtils.UploadFileAsync(fileName, uploadPlaceholder);
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
        private async Task ClickIncludedAddAllDevicesButtonAsync()
        {
            await ControlHelper.ClickByButtonRoleAndHasTextAsync(this.CurrentIPage, "Add all devices", 0, iFrameName: null);
        }
        #endregion

        #region Verify all settings

        #region Verify Basics

        private async Task VerifyBasicsNameValueAsync(string value)
        {
            await verify_Basic_Section_PCControl.VerifyBasedOnLabelAsync("Name", value);
        }
        private async Task VerifyBasicsDescriptionValueAsync(string value)
        {
            await verify_Basic_Section_PCControl.VerifyBasedOnLabelAsync("Description", value);
        }
        private async Task VerifyPolicyFileNameValueAsync(string value)
        {
            await verify_Basic_Section_PCControl.VerifyBasedOnLabelAsync("Policy file name", value);
            uploadPlaceholder = value;
        }
        #endregion

        #region Verify Assignments
        private async Task VerifyAssignmentsIncludedGroupsAsync(string groupName)
        {
            await verify_Assignment_Section_PCControl.VerifyBasedOnLabelAsync("Included groups", groupName);
        }
        private async Task VerifyAssignmentsExcludedGroupsAsync(string groupName)
        {
            await verify_Assignment_Section_PCControl.VerifyBasedOnLabelAsync("Excluded groups", groupName);
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
