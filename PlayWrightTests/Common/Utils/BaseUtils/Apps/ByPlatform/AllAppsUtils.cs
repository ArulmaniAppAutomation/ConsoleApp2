//using LogService;
//using LogService.Extension;
//  using Microsoft.Graph.Beta.Models;
//using Microsoft.Graph.Beta.Models.Security;
using Microsoft.Playwright;
using PlaywrightTests.Common.Controller.Blade;
using PlaywrightTests.Common.Controller.BottomNavigation;
using PlaywrightTests.Common.Controller.ConfirmDialog;
using PlaywrightTests.Common.Controller.Grid;
using PlaywrightTests.Common.Controller.PageFooterNavigate;
using PlaywrightTests.Common.Controller.SearchBox;
using PlaywrightTests.Common.Controller.ToolBar;
using PlaywrightTests.Common.Controller.Upload;
using PlaywrightTests.Common.Helper;
using PlaywrightTests.Common.Model;
using PlaywrightTests.Common.Utils.BaseUtils.PopUp;
using PlaywrightTests.Common.Utils.BaseUtils.UtilInterface;
using System.ComponentModel.Design;
using System.Data;
using System.Formats.Asn1;
using System.Reflection.PortableExecutable;
//using System.Windows.Controls.Ribbon;
using static PlaywrightTests.Common.Helper.EnumHelper;
using static System.Net.Mime.MediaTypeNames;

namespace PlaywrightTests.Common.Utils.BaseUtils.Apps.ByPlatform
{
    public class AllAppsUtils : BaseCommonUtils, InterfaceUtils
    {
        public string? IFrameName = null;
        public ICommandBar commandBar ;
        public IGrid grid;
        public IBottomNavigation bottomNavigation;
        public ISearchBox searchBox;
        public AllAppsUtils(IPage page, string env) : base(page, env)
        {
            commandBar = new MSCommandBarWithMenubarRole(this.CurrentIPage, "AppList.ReactView", this.CurrentLanguage);
            grid = new Grid_MS_DetailsList(this.CurrentIPage, "AppList.ReactView", this.CurrentLanguage, noDataText: "No data found");
            searchBox = new MS_SearchBox(this.CurrentIPage, "AppList.ReactView", this.CurrentLanguage);
            bottomNavigation = new BottomNavigation_MSportalfx_Docking_Footer(this.CurrentIPage, null, this.CurrentLanguage);
        }

        public async Task CheckIfCurrentPageIsAvailableAsync()
        {
            await ClickRefreshButtonAsync();
        }
        public async Task<bool> CheckIfCurrentPageIsReactViewPageAsync()
        {
            try
            {
                ILocator AppPageLocator =  await ControlHelper.GetLocatorByClassAsync(this.CurrentIPage, "ms-DetailsList", 0, "AppList.ReactView");
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Task ClearDataAsync()
        {
            throw new NotImplementedException();
        }

        public async Task ClearSpecialDataAsync(string name)
        {
            await DeleteAppByNameAsync(name);
        }

        public async Task GoToMainPageAsync()
        {
            await GoToHomePageAsync();
            await base.siteBar.ClickAppsAsync();
            await base.siteBarMenu.ClickAllAppsAsync();
            if (! await CheckIfCurrentPageIsReactViewPageAsync())
            {
                commandBar = new FXSCommandBar(this.CurrentIPage, null, CurrentLanguage);
                grid = new Grid_AZC_Grid(this.CurrentIPage, null, this.CurrentLanguage);
                searchBox = new FXC_SearchBox(this.CurrentIPage, null, this.CurrentLanguage);
            }
        }

        public async Task<(bool IsContinue, Dictionary<string, string> Parameter)> RunStepAsync(ControlInfo controlInfo)
        {
            switch (controlInfo.ControlType)
            {
                case "GoToMainPageAsync":
                    await GoToMainPageAsync();
                    break;
                case "CheckIfCurrentPageIsAvailableAsync":
                    await CheckIfCurrentPageIsAvailableAsync();
                    break;
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
                #region Grid Operation               
                case "DeleteAppByNameAsync":
                    var profileName = !string.IsNullOrEmpty(controlInfo.OperationValue) ? controlInfo.OperationValue : controlInfo.Parameter["AppAutomationAppName"];
                    await DeleteAppByNameAsync(profileName);
                    DictionaryItemProcess(controlInfo.Parameter, "AppAutomationAppDelete", StepResultStatus.Success.ToString());
                    BaseRemoveUniqueProfileName(profileName);
                    break;
                case "ClickAppsNameToShowDetailAsync":
                    await ClickAppsNameToShowDetailAsync(!string.IsNullOrEmpty(controlInfo.OperationValue) ? controlInfo.OperationValue : controlInfo.Parameter["AppAutomationAppName"]);
                    break;
                case "DeleteAllRebundantAppsAsync":
                    await DeleteAllRebundantAppsAsync(controlInfo.Value);
                    break;
                #endregion
                #region Bottom Navigation
                case "ClickNextButtonAsync":
                    await bottomNavigation.ClickBottomNavigationSpecialNameButtonAsync("Next");
                    break;
                case "ClickCreateButtonAsync":
                    await bottomNavigation.ClickBottomNavigationSpecialNameButtonAsync("Create");
                    BaseThreadSleepMiddle();
                    DictionaryItemProcess(controlInfo.Parameter, "AppAutomationAppCreation", StepResultStatus.Success.ToString());
                    BaseThreadSleepLong(15);
                    await BaseVerifyWithNotificationAsync($"{controlInfo.Parameter["AppAutomationAppName"]} upload finished");
                    break;
                case "ClickCreateButtonWithoutWaitForUploadAsync":
                    await bottomNavigation.ClickBottomNavigationSpecialNameButtonAsync("Create");
                    BaseThreadSleepMiddle();
                    DictionaryItemProcess(controlInfo.Parameter, "AppAutomationAppCreation", StepResultStatus.Success.ToString());
                    break;
                case "ClickCreateButtonWithoutAppNameAsync":
                    await bottomNavigation.ClickBottomNavigationSpecialNameButtonAsync("Create");
                    BaseThreadSleepMiddle();
                    DictionaryItemProcess(controlInfo.Parameter, "AppAutomationAppCreation", StepResultStatus.Success.ToString());
                    await BaseVerifyWithNotificationAsync($"Application \"\" created successfully");
                    break;
                case "ClickReviewSaveButtonAsync":
                    await bottomNavigation.ClickBottomNavigationSpecialNameButtonAsync("Review + save");
                    break;
                case "ClickSaveButtonAsync":
                    await bottomNavigation.ClickBottomNavigationSpecialNameButtonAsync("Save");
                    break;
                #endregion
                #region verify tab is exist or not
                case "VerifyTabIsExistOrNotAsync":
                    var verifyResult = await VerifyTabIsExistOrNotAsync(controlInfo.OperationValue);
                    return (verifyResult, controlInfo.Parameter);
                #endregion
                #region Select Built-in Apps             
                case "SelectBuiltInAppsAsync":
                    await SelectBuiltInAppsAsync(controlInfo.Value[0], controlInfo.Value[1]);
                    break;
                #endregion
                #region App information
                case "ClickAppInformationEditButtonAsync":
                    await ClickAppInformationEditButtonAsync();
                    break;
                case "ClickSelectFileToUpdateButtonAsync":
                    await ClickSelectFileToUpdateButtonAsync();
                    break;
                case "SelectTheMicrosoftStoreAppNewAsync":
                    await SelectTheMicrosoftStoreAppNewAsync(controlInfo.OperationValue);
                    break;
                case "ClickSelectAppPackageFileButtonAsync":
                    await ClickSelectAppPackageFileButtonAsync();
                    break;
                case "ClickSelectTheAppStoreButtonAsync":
                    await ClickSelectTheAppStoreButtonAsync();
                    break;
                case "ClickAppInformationSelectAppButtonAsync":
                    await ClickAppInformationSelectAppButtonAsync();
                    break;

                case "SelectTheAppStoreAsync":
                    await SelectTheAppStoreAsync(controlInfo.OperationValue);
                    break;
                case "SetAppInformationNameAsync":
                    return await SetAppinformationNameAsync(controlInfo, async (name) => { await SetAppInformationNameAsync(name); });
                case "SetAppInformationDescriptionAsync":
                    await SetAppInformationDescriptionAsync(controlInfo.OperationValue);
                    break;
                case "SetAppInformationSuiteNameAsync":
                    return await SetAppinformationNameAsync(controlInfo, async (name) => { await SetAppInformationSuiteNameAsync(name); });

                case "SetAppInformationAppURLAsync":
                    await SetAppInformationAppURLAsync(controlInfo.OperationValue);
                    break;
                case "SetAppInformationCategoryAsync":
                    await SetAppInformationCategoryAsync(controlInfo.Value);
                    break;
                case "SetAppInformationSuiteDescription0Async":
                    await SetAppInformationSuiteDescription0Async(controlInfo.OperationValue);
                    break;
                case "SetAppInformationSuiteDescriptionAsync":
                    await SetAppInformationSuiteDescriptionAsync(controlInfo.OperationValue);
                    break;
                case "SetAppInformationPublisherAsync":
                    await SetAppInformationPublisherAsync(controlInfo.OperationValue);
                    break;

                case "SetShowThisAsAFeaturedAppInTheCompanyPortalAsync":
                    await SetShowThisAsAFeaturedAppInTheCompanyPortalAsync(controlInfo.OperationValue);
                    break;
                case "SetTargetedPlatformAsync":
                    await SetTargetedPlatformAsync(controlInfo.OperationValue);
                    break;
                case "SetAppInformationUrlAsync":
                    await SetAppInformationUrlAsync(controlInfo.OperationValue);
                    break;
                case "SetAppInformationDeveloperAsync":
                    await SetAppInformationDeveloperAsync(controlInfo.OperationValue);
                    break;
                case "SetAppInformationOwnerAsync":
                    await SetAppInformationOwnerAsync(controlInfo.OperationValue);
                    break;
                case "SetAppInformationAppstoreUrlAsync":
                    await SetAppInformationAppstoreUrlAsync(controlInfo.OperationValue);
                    break;
                case "ClickAppInformationSelectImageButtonAsync":
                    await ClickAppInformationSelectImageButtonAsync();
                    break;
                case "ClickAppInformationChangeImageButtonAsync":
                    await ClickAppInformationChangeImageButtonAsync();
                    break;
                case "SetAppInformationPrivacyURLAsync":
                    await SetAppInformationPrivacyURLAsync(controlInfo.OperationValue);
                    break;
                case "SetAppInformationNotesAsync":
                    await SetAppInformationNotesAsync(controlInfo.OperationValue);
                    break;
                #endregion

                #region ConfigureAppSuiteForM365Office365App
                case "SelectOtherOfficeAppsAsync":
                    await SelectOtherOfficeAppsAsync(controlInfo.Value);
                    break;
                case "SelectOfficeAppsByExcludeAsync":
                    await SelectOfficeAppsByExcludeAsync(controlInfo.Value);
                    break;
                case "SetArchitectureAsync":
                    await SetArchitectureAsync(controlInfo.OperationValue);
                    break;
                case "SetdefaultFileFormatAsync":
                    await SetdefaultFileFormatAsync(controlInfo.OperationValue);
                    break;
                case "SetUpdatechannelAsync":
                    await SetUpdatechannelAsync(controlInfo.OperationValue);
                    break;
                case "SetVersionToInstallAsync":
                    await SetVersionToInstallAsync(controlInfo.OperationValue);
                    break;
                case "SetspecificVersionToInstallAsync":
                    await SetspecificVersionToInstallAsync(controlInfo.OperationValue);
                    break;
                case "SetAcceptEulaAsync":
                    await SetAcceptEulaAsync(controlInfo.OperationValue);
                    break;
                case "SetInstallBackgroundServiceAsync":
                    await SetInstallBackgroundServiceAsync(controlInfo.OperationValue);
                    break;
                case "SetRemoveOtherVersionsAsync":
                    await SetRemoveOtherVersionsAsync(controlInfo.OperationValue);
                    break;
                case "SetAppSettingChannelAsync":
                    await SetAppSettingChannelAsync(controlInfo.OperationValue);
                    break;
                case "SetUseSharedComputerActivationAsync":
                    await SetUseSharedComputerActivationAsync(controlInfo.OperationValue);
                    break;
                case "SetLanguagesAsync":
                    await SetLanguagesAsync(controlInfo.Value);
                    break;
                #endregion
                #region Requirements
                case "SetOperationSystemArchitectureAsync":
                    await SetOperationSystemArchitectureAsync(controlInfo.Value);
                    break;
                case "SetMinimumOperationSystemAsync":
                    await SetMinimumOperationSystemAsync(controlInfo.Value);
                    break;
                case "SetDiskSpaceRequiredAsync":
                    await SetDiskSpaceRequiredAsync(controlInfo.OperationValue);
                    break;
                case "SetPhysicalMemoryRequiredAsync":
                    await SetPhysicalMemoryRequiredAsync(controlInfo.OperationValue);
                    break;
                case "SetMinimumNumberOfLogicalProcessorsRequiredAsync":
                    await SetMinimumNumberOfLogicalProcessorsRequiredAsync(controlInfo.OperationValue);
                    break;
                case "SetMinimumCPUSpeedRequiredAsync":
                    await SetMinimumCPUSpeedRequiredAsync(controlInfo.OperationValue);
                    break;
                case "ClickRequirementAddButtonAsync":
                    await ClickRequirementAddButtonAsync();
                    break;

                #endregion
                #region Detection rules
                case "SetRulesFormatAsync":
                    await SetRulesFormatAsync(controlInfo.OperationValue);
                    break;

                case "ClickRulesFormatAddButtonAsync":
                    await ClickRulesFormatAddButtonAsync();
                    break;

                #region Use a custom detection script

                case "SelectScriptFileAsync":
                    await SelectScriptFileAsync(controlInfo.OperationValue);
                    break;
                case "SetRunScriptAs32BitProcessOn64BitClientsAsync":
                    await SetRunScriptAs32BitProcessOn64BitClientsAsync(controlInfo.OperationValue);
                    break;
                case "SetEnforceScriptSignatureCheckAndRunScriptSilentlyAsync":
                    await SetEnforceScriptSignatureCheckAndRunScriptSilentlyAsync(controlInfo.OperationValue);
                    break;
                #endregion
                #endregion
                #region Dependencies
                case "ClickDependenciesAddButtonAsync":
                    await ClickDependenciesAddButtonAsync();
                    break;
                case "SelectDependencyAppsAsync":
                    await SelectDependencyAppsAsync(controlInfo.Value[0], controlInfo.Value[1]);
                    break;
                case "ClickDependenciesEditButtonAsync":
                    await ClickDependenciesEditButtonAsync();
                    break;
                case "DeleteDependenciesAppsAsync":
                    await DeleteDependenciesAppsAsync(controlInfo.OperationValue);
                    break;
                #endregion
                #region Supersedence
                case "ClickSupersedenceAddButtonAsync":
                    await ClickSupersedenceAddButtonAsync();
                    break;
                case "SelectSupersedenceAppsAsync":
                    await SelectSupersedenceAppsAsync(controlInfo.Value[0], controlInfo.Value[1]);
                    break;
                case "ClickSupersedenceEditButtonAsync":
                    await ClickSupersedenceEditButtonAsync();
                    break;
                case "DeleteSupersedenceAppsAsync":
                    await DeleteSupersedenceAppsAsync(controlInfo.OperationValue);
                    break;

                #endregion
                #region Assignments
                case "ClickAssignmentEditButtonAsync":
                    await ClickAssignmentEditButtonAsync();
                    break;
                #region Assignments Required
                case "ClickRequiredAddAllUsersAsync":
                    await ClickRequiredAddAllUsersAsync();
                    break;
                case "ClickRequiredAddAllDevicesAsync":
                    await ClickRequiredAddAllDevicesAsync();
                    break;
                case "ClickRequiredAddGroupAsync":
                    await ClickRequiredAddGroupAsync(controlInfo.OperationValue);
                    break;
                case "ClickRequiredAllDevicesFilterModeAsync":
                    await ClickRequiredAllDevicesFilterModeAsync();
                    break;
                case "ClickRequiredAllUsersFilterModeAsync":
                    await ClickRequiredAllUsersFilterModeAsync();
                    break;
                case "ClickRequiredAddGroupFilterModeAsync":
                    await ClickRequiredAddGroupFilterModeAsync(controlInfo.OperationValue);
                    break;
                case "ClickRequiredBehaveInstallContextCellAsync":
                    await ClickRequiredBehaveInstallContextCellAsync(controlInfo.OperationValue);
                    break;
                case "ClickRequiredBehaveUninstallOnDeviceRemovalCellAsync":
                    await ClickRequiredBehaveUninstallOnDeviceRemovalCellAsync(controlInfo.OperationValue);
                    break;
                #endregion
                #region Assignments Available for enrolled devices
                case "ClickAvailableForEnrolledDevicesAllUsersAsync":
                    await ClickAvailableForEnrolledDevicesAllUsersAsync();
                    break;
                case "ClickAvailableForEnrolledDevicesAddGroupAsync":
                    await ClickAvailableForEnrolledDevicesAddGroupAsync(controlInfo.OperationValue);
                    break;
                case "ClickAvailableForEnrolledDevicesBehaveUninstallOnDeviceRemovalCellAsync":
                    await ClickAvailableForEnrolledDevicesBehaveUninstallOnDeviceRemovalCellAsync(controlInfo.OperationValue);
                    break;
                #endregion
                #region Assignments Common Function
                case "SetUninstallOnDeviceRemovalAsync":
                    await SetUninstallOnDeviceRemovalAsync(controlInfo.OperationValue);
                    break;
                case "SetInstallContextAsync":
                    await SetInstallContextAsync(controlInfo.OperationValue);
                    break;
                #endregion
                case "MarkAssignmentsCompleteAsync":
                    DictionaryItemProcess(controlInfo.Parameter, "AppAutomationAppAssignment", StepResultStatus.Success.ToString());
                    break;
                #endregion
                #region Verify
                case "VerifyPropertyAsync":
                    await VerifyPropertyAsync(controlInfo.Value[0], controlInfo.Value[1]);
                    break;
                case "VerifyPropertyAssignmentsAsync":
                    await VerifyPropertyAssignmentsAsync(controlInfo.Value[0], controlInfo.Value[1]);
                    break;
                case "VerifyPropertyAssignmentsGridDataCellAsync":
                    await VerifyPropertyAssignmentsGridDataCellAsync(controlInfo.Value[0], controlInfo.Value[1], controlInfo.Value[2], controlInfo.Value[3]);
                    break;
                case "VerifyDeviceInstallStatusAsync":
                    await VerifyDeviceInstallStatusAsync();
                    break;
                case "VerifyUserInstallStatusAsync":
                    await VerifyUserInstallStatusAsync();
                    break;
                case "VerifyMonitorAppInstallStatusAsync":
                    await VerifyMonitorAppInstallStatusAsync(!string.IsNullOrEmpty(controlInfo.OperationValue) ? controlInfo.OperationValue : controlInfo.Parameter["AppAutomationAppName"]);
                    break;
                #endregion
                #region Rules 

                case "SelectRequirementTypeAsync":
                    await SelectRequirementTypeAsync(controlInfo.OperationValue);
                    break;
                case "SetRuleTypeValueAsync":
                    await SetRuleTypeValueAsync(controlInfo.OperationValue);
                    break;
                case "SetMSIProductVersionCheckAsync":
                    await SetMSIProductVersionCheckAsync(controlInfo.OperationValue);
                    break;
                case "SetRuleOperatorAsync":
                    await SetRuleOperatorAsync(controlInfo.OperationValue);
                    break;
                case "SetRuleValueAsync":
                    await SetRuleValueAsync(controlInfo.OperationValue);
                    break;
                case "SetRulePathAsync":
                    await SetRulePathAsync(controlInfo.OperationValue);
                    break;
                case "SetRulePropertyAsync":
                    await SetRulePropertyAsync(controlInfo.OperationValue);
                    break;
                case "SetRuleRegistryKeyRequirementAsync":
                    await SetRuleRegistryKeyRequirementAsync(controlInfo.OperationValue);
                    break;
                case "SetRuleFileOrFolderAsync":
                    await SetRuleFileOrFolderAsync(controlInfo.OperationValue);
                    break;
                case "SetRuleDetectionMethodAsync":
                    await SetRuleDetectionMethodAsync(controlInfo.OperationValue);
                    break;
                case "SetRuleDateTimeValueAsync":
                    await SetRuleDateTimeValueAsync(controlInfo.OperationValue);
                    break;
                case "SetRuleAssociatedWithA32BitAppOn64BitClientsAsync":
                    await SetRuleAssociatedWithA32BitAppOn64BitClientsAsync(controlInfo.OperationValue);
                    break;
                case "SetRuleKeyPathAsync":
                    await SetRuleKeyPathAsync(controlInfo.OperationValue);
                    break;
                case "SetRuleValueNameAsync":
                    await SetRuleValueNameAsync(controlInfo.OperationValue);
                    break;
                case "SetRuleRunScriptAs32BitProcessOn64BitClientsAsync":
                    await SetRuleRunScriptAs32BitProcessOn64BitClientsAsync(controlInfo.OperationValue);
                    break;
                case "SetRuleRunThisScriptUsingTheLoggedOnCredentialsAsync":
                    await SetRuleRunThisScriptUsingTheLoggedOnCredentialsAsync(controlInfo.OperationValue);
                    break;
                case "SetRuleSelectOutputDataTypeAsync":
                    await SetRuleSelectOutputDataTypeAsync(controlInfo.OperationValue);
                    break;
                case "SetRuleEnforceScriptSignatureCheckAsync":
                    await SetRuleEnforceScriptSignatureCheckAsync(controlInfo.OperationValue);
                    break;
                case "SetRuleScriptFileAsync":
                    await SetRuleScriptFileAsync(controlInfo.OperationValue);
                    break;

                case "ClickDetectionRuleOKButtonAsync":
                    await ClickDetectionRuleOKButtonAsync();
                    break;
                #endregion

                default:
                    await BaseExecuteStepAsync(operation: controlInfo.Operation, operationValue: controlInfo.OperationValue, controlInfo: controlInfo);
                    break;
            }
            return (true, controlInfo.Parameter);
        }
        public async Task<DataTable> GetGridTableDataAsync()
        {
            return await grid.GetGridAllDataAsync();
        }
        public async Task SetSearchBoxAsync(string value)
        {
            await searchBox.SetSearchBoxValueAsync(value);
        }
        #region Command Bar
        private async Task ClickAddButtonAsync()
        {
            try
            {
                await commandBar.ClickCommandBarCreateButtonAsync();
            }
            catch
            {
                await commandBar.ClickCommandBarAddButtonAsync();
            }
        }
        private async Task ClickRefreshButtonAsync()
        {
            await commandBar.ClickCommandBarRefreshButtonAsync();
        }
        private async Task ClickFilterButtonAsync()
        {
            await commandBar.ClickCommandBarFilterButtonAsync();
        }
        private async Task ClickExportButtonAsync()
        {
            await commandBar.ClickCommandBarExportButtonAsync();
        }
        private async Task ClickColumnsButtonAsync()
        {
            await commandBar.ClickCommandBarColumnsButtonAsync();
        }
        #endregion

        #region Grid Operation
        public async Task ClickAppToShowDetailByNthAsync(int nth)
        {
            await ControlHelper.ClickByClassAsync(this.CurrentIPage, "fxs-portal-hover fxs-portal-focus azc-grid-row", nth, iFrameName: IFrameName);
        }
        private async Task ClickAppsNameToShowDetailAsync(string name)
        {
            await SetSearchBoxAsync(name);
            var result = await ControlHelper.RetryAsync(retryCount: 3, async () =>
            {
                await commandBar.ClickCommandBarRefreshButtonAsync();
                await grid.ClickRowHeaderToShowDetailAsync(name);
            });
        }
        public async Task DeleteAppByNameAsync(string name)
        {
            await GoToMainPageAsync();
            await SetSearchBoxAsync(name);
            var result = await ControlHelper.RetryAsync(retryCount: 3, async () =>
            {
                await commandBar.ClickCommandBarRefreshButtonAsync();
                await grid.DeleteRowByRowHeaderAsync(name);
            });
        }
        public async Task DeleteAppFromOverViewpageAsync()
        {
            await siteBarMenu.ClickOverviewAsync();
            FXSCommandBar deletebutton = new FXSCommandBar(this.CurrentIPage, "", this.CurrentLanguage);
            await deletebutton.ClickCommandBarDeleteAsync();
            IConfirmDialog confirmDialog = new ConfirmDialog_FXS_Blade_Dialog(this.CurrentIPage, "", this.CurrentLanguage);
            await confirmDialog.ClickDialogYesButtonAsync();

        }

        public async Task DeleteAllRebundantAppsAsync(List<string> skippedApp)
        {
            await GoToMainPageAsync();
            var Applist = await grid.GetGridAllDataAsync();
            if (Applist.Rows.Count > 0)
            {
                foreach (DataRow dr in Applist.Rows)
                {
                    string RowHeader = dr["Name"].ToString();
                    try
                    {                       
                        if (skippedApp.Count() != 0 && skippedApp.Contains(RowHeader))
                        {
                            continue;
                        }
                        if (RowHeader.Contains("25533644") || RowHeader.Contains("Manually configure Requirement rules"))
                        {
                            await ClickAppsNameToShowDetailAsync(RowHeader);
                            await ClickDependenciesEditButtonAsync();
                            await DeleteAllDependenciesAppsAsync();
                            await DeleteAppFromOverViewpageAsync();
                            continue;
                        }
                        await SetSearchBoxAsync(RowHeader);
                        var result = await ControlHelper.RetryAsync(retryCount: 3, async () =>
                        {
                            await commandBar.ClickCommandBarRefreshButtonAsync();
                            await grid.DeleteRowByRowHeaderAsync(RowHeader);
                        });
                    }
                    catch 
                    {
                      //  LogHelper.Info($"failed to delete {RowHeader}");
                    };
                }
            }
        }

        public string GetCurrentLanguageText(string key)
        {
            throw new NotImplementedException();
        }
        #endregion
        #region verify tab is exist or not
        private async Task<bool> VerifyTabIsExistOrNotAsync(string tabName)
        {
            try
            {
                var tabLocator = await ControlHelper.GetLocatorByClassAndHasTextAsync(this.CurrentIPage, "fxc-section-tab-item", tabName, 0, iframeName: IFrameName);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
        #region Select Built-in Apps

        private async Task SelectBuiltInAppsAsync(string name, string platform)
        {
            SelectFromGridBySearchUtils selectFromGridBySearchUtils = new SelectFromGridBySearchUtils(this.CurrentIPage, this.CurrentEnv);
            await selectFromGridBySearchUtils.SelectBySearchWithKeywordClickCheckBoxAsync("Select Built-in apps", "Search Built-In apps", name, filterValue: platform);
        }
        #endregion
        #region App information
        private async Task ClickAppInformationEditButtonAsync()
        {
            await ControlHelper.RetryAsync(3, async () =>
                {
                    await siteBarMenu.ClickPropertiesAsync();
                    await siteBarMenu.ClickOverviewAsync();
                    await siteBarMenu.ClickPropertiesAsync();
                    await ClickEditButtonAsync("Edit App information");
                });
        }
        private async Task ClickSelectFileToUpdateButtonAsync()
        {
            await ControlHelper.ClickByClassAsync(await GetAppInformationBannerLocatorByTitleAsync("Select file to update"), "ext-controls-selectLink fxs-fxclick", 0);
        }
        private async Task SelectTheMicrosoftStoreAppNewAsync(string name)
        {
            await ClickAppInformationSelectFileButtonAsync("Search the Microsoft Store app (new)");
            SelectFromGridBySearchUtils selectFromGridBySearchUtils = new SelectFromGridBySearchUtils(this.CurrentIPage, this.CurrentEnv);
            await selectFromGridBySearchUtils.SelectBySearchWithKeywordAsync("Search the Microsoft Store app (new)", "Search the Microsoft Store app (new)", name);
        }
        private async Task ClickSelectAppPackageFileButtonAsync()
        {
            await ClickAppInformationSelectFileButtonAsync("Select app package file");
        }
        private async Task ClickSelectTheAppStoreButtonAsync()
        {
            await ClickAppInformationSelectFileButtonAsync("Search the App Store");
        }
        private async Task ClickAppInformationSelectAppButtonAsync()
        {
            await ClickAppInformationSelectFileButtonAsync("Select app");
        }
        private async Task SelectTheAppStoreAsync(string name)
        {
            SelectFromGridBySearchUtils selectFromGridBySearchUtils = new SelectFromGridBySearchUtils(this.CurrentIPage, this.CurrentEnv);
            await selectFromGridBySearchUtils.SelectBySearchWithKeywordAsync("Search the App Store", "Search the App Store", name);
        }
        private async Task SetAppInformationNameAsync(string name)
        {
            await SetAppInformationInputWithPlaceholderAsync("Enter a name", name);
        }
        private async Task SetAppInformationDescriptionAsync(string description)
        {
            await SetAppInformationTextAreaByBannerAsync("Description", description);
        }
        private async Task SetAppInformationSuiteNameAsync(string suiteName)
        {
            await SetAppInformationInputWithAriaLabelAsync("Suite Name", suiteName);
        }
        private async Task SetAppInformationSuiteDescription0Async(string description)
        {
            //M365 windows app
            await SetAppInformationTextAreaByAriaLabelAsync("Suite Description", description);
        }
        private async Task SetAppInformationSuiteDescriptionAsync(string description)
        {
            //M365 Mac app
            await SetAppInformationTextAreaByBannerAsync("Suite Description", description);
        }
        private async Task SetAppInformationPublisherAsync(string publisher)
        {
            await SetAppInformationInputWithPlaceholderAsync("Enter a publisher name", publisher);
        }
        private async Task SetAppInformationAppURLAsync(string appUrl)
        {
            await SetAppInformationInputWithAriaLabelAsync("App URL", appUrl);
        }
        private async Task SetAppInformationCategoryAsync(List<string> value)
        {
            await SetAppInformationComboxWithAriaLabelAsync("Category", value);
        }
        private async Task SetShowThisAsAFeaturedAppInTheCompanyPortalAsync(string value)
        {
            await SetOptionPickerAsync("Show this as a featured app in the Company Portal", value);
        }
        private async Task SetTargetedPlatformAsync(string value)
        {
            await SetAppInformationComboxWithAriaLabelAsync("Targeted platform", new List<string> { value });
        }
        private async Task SetAppInformationUrlAsync(string value)
        {
            await SetAppInformationInputWithAriaLabelAsync("Information URL", value);
        }
        private async Task SetAppInformationDeveloperAsync(string value)
        {
            await SetAppInformationInputWithAriaLabelAsync("Developer", value);
        }
        private async Task SetAppInformationOwnerAsync(string value)
        {
            await SetAppInformationInputWithAriaLabelAsync("Owner", value);
        }
        private async Task SetAppInformationAppstoreUrlAsync(string appstoreUrl)
        {
            await SetAppInformationInputWithAriaLabelAsync("Appstore URL", appstoreUrl);
        }
        private async Task ClickAppInformationSelectImageButtonAsync()
        {
            await ClickAppInformationSelectFileButtonAsync("Select image");
        }
        private async Task ClickAppInformationChangeImageButtonAsync()
        {
            await ClickAppInformationSelectFileButtonAsync("Change image");
        }
        private async Task SetAppInformationPrivacyURLAsync(string value)
        {
            await SetAppInformationInputWithAriaLabelAsync("Privacy URL", value);
        }
        private async Task SetAppInformationNotesAsync(string value)
        {
            await SetAppInformationTextAreaByBannerAsync("Notes", value);
        }

        private async Task<(bool, Dictionary<string, string>)> SetAppinformationNameAsync(ControlInfo controlInfo, Func<string, Task> setFunction)
        {
            var appName = BaseCreateUniqueProfileName(controlInfo.OperationValue);
            await setFunction(appName);
            try
            {
                DictionaryItemProcess(controlInfo.Parameter, "AppAutomationAppName", appName);
            }
            catch { }
            return (true, controlInfo.Parameter);
        }

        public async Task ClickAppInformationSelectFileButtonAsync(string buttonName)
        {
            await ControlHelper.ClickByClassWithAriaLableAsync(this.CurrentIPage, "msportalfx-text-primary ext-controls-selectLink", buttonName, 0, iFrameName: IFrameName);
        }
        public async Task SetAppInformationInputWithPlaceholderAsync(string placeholder, string value)
        {
            await ControlHelper.SetInputByClassAndPlaceholderAsync(this.CurrentIPage, "azc-input azc-formControl", placeholder, value, 0, iFrameName: IFrameName);
        }
        public async Task SetAppInformationInputWithAriaLabelAsync(string ariaLabel, string value)
        {
            await ControlHelper.SetInputByClassAndAriaLabelAsync(this.CurrentIPage, "azc-input azc-formControl", ariaLabel, value, 0, iFrameName: IFrameName);
        }
        public async Task SetAppInformationTextAreaByBannerAsync(string title, string value)
        {
            var DescriptionBannerLocator = await GetAppInformationBannerLocatorByTitleAsync(title);
            await ControlHelper.SetInputByClassAndTypeAsync(DescriptionBannerLocator, "azc-textarea azc-formControl azc-input", "text", value, 0);
        }
        public async Task SetAppInformationTextAreaByAriaLabelAsync(string title, string value)
        {
            await ControlHelper.SetTextAreaValueByArialableAsync(this.CurrentIPage, value, title, 0, iFrameName: IFrameName);
        }
        public async Task SetAppInformationComboxWithAriaLabelAsync(string ariaLabel, List<string> values)
        {
            await ControlHelper.SetComBoxRoleTreeItemRoleValueAsync(this.CurrentIPage, ariaLabel, values, 0, iFrameName: IFrameName);
        }
        public async Task<ILocator> GetAppInformationBannerLocatorByTitleAsync(string title)
        {
            return await ControlHelper.GetLocatorByClassAndHasTextAsync(this.CurrentIPage, "fxc-section-control", title, -1, iframeName: IFrameName);
        }

        #endregion
        #region Requirements
        private async Task SetOperationSystemArchitectureAsync(List<string> architecture)
        {
            await ControlHelper.SetComBoxRoleTreeItemRoleValueAsync(this.CurrentIPage, "Operating system architecture", architecture, 0, iFrameName: IFrameName);
        }
        private async Task SetMinimumOperationSystemAsync(List<string> operationSystem)
        {
            await ControlHelper.SetComBoxRoleTreeItemRoleValueAsync(this.CurrentIPage, "Minimum operating system", operationSystem, 0, iFrameName: IFrameName);
        }
        private async Task SetDiskSpaceRequiredAsync(string diskSpace)
        {
            await SetAzcInputBoxAsync("Disk space required (MB)", diskSpace);
        }
        private async Task SetPhysicalMemoryRequiredAsync(string memory)
        {
            await SetAzcInputBoxAsync("Physical memory required (MB)", memory);
        }
        private async Task SetMinimumNumberOfLogicalProcessorsRequiredAsync(string value)
        {
            await SetAzcInputBoxAsync("Minimum number of logical processors required", value);
        }
        private async Task SetMinimumCPUSpeedRequiredAsync(string value)
        {
            await SetAzcInputBoxAsync("Minimum CPU speed required (MHz)", value);
        }
        private async Task ClickRequirementAddButtonAsync()
        {
            await ControlHelper.ClickByClassAndHasTextAsync(await GetTabPanelLocatorAsync("Requirements"), "msportalfx-text-primary ext-controls-selectLink fxs-fxclick", "+ Add", 0);
        }

        #endregion
        #region Detection rules
        private async Task SetRulesFormatAsync(string rulesFormat)
        {
            await ControlHelper.SetComBoxRoleTreeItemRoleValueAsync(this.CurrentIPage, "Rules format", rulesFormat, 0, iFrameName: IFrameName);
        }
        private async Task ClickRulesFormatAddButtonAsync()
        {
            await ControlHelper.ClickByClassAndHasTextAsync(await GetTabPanelLocatorAsync("Detection rules"), "msportalfx-text-primary ext-controls-selectLink fxs-fxclick", "+ Add", 0);
        }
        #region Use a custom detection script
        private async Task SelectScriptFileAsync(string file)
        {
            UploadController uploadController = new UploadController(this.CurrentIPage, IFrameName, this.CurrentLanguage);
            await uploadController.UploadFileAsync(file);
        }
        private async Task SetRunScriptAs32BitProcessOn64BitClientsAsync(string value)
        {
            await SetOptionPickerAsync("Run script as 32-bit process on 64-bit clients", value);
        }
        private async Task SetEnforceScriptSignatureCheckAndRunScriptSilentlyAsync(string value)
        {
            await SetOptionPickerAsync("Enforce script signature check and run script silently", value);
        }
        #endregion

        #endregion
        #region Dependencies
        private async Task ClickDependenciesAddButtonAsync()
        {
            await ControlHelper.ClickByClassAndHasTextAsync(await GetTabPanelLocatorAsync("Dependencies"), "msportalfx-text-primary ext-controls-selectLink fxs-fxclick", "+ Add", 0);
        }
        private async Task SelectDependencyAppsAsync(string apps, string isAutomaticallyInstall)
        {
            SelectFromGridBySearchUtils selectFromGridBySearchUtils = new SelectFromGridBySearchUtils(this.CurrentIPage, this.CurrentEnv);
            await selectFromGridBySearchUtils.SelectBySearchWithKeywordAsync("Add dependency", "Search by name, publisher", apps);
            await SetAppInstallRequirementAsync("Dependencies", apps, isAutomaticallyInstall);
        }
        private async Task ClickDependenciesEditButtonAsync()
        {
            await siteBarMenu.ClickPropertiesAsync();
            await ClickEditButtonAsync("Edit Dependencies");
        }
        private async Task DeleteDependenciesAppsAsync(string name)
        {
            var TabLocator = await GetTabPanelLocatorAsync("Dependencies");
            IGrid superGrid = new Grid_FXC_GC_Table(this.CurrentIPage, IFrameName, this.CurrentLanguage, parentLocator: TabLocator);
            await superGrid.RemoveRowByRowHeaderAsync(name);
        }
        private async Task DeleteAllDependenciesAppsAsync()
        {
            var TabLocator = await GetTabPanelLocatorAsync("Dependencies");
            Grid_FXC_GC_Table superGrid = new Grid_FXC_GC_Table(this.CurrentIPage, IFrameName, this.CurrentLanguage, parentLocator: TabLocator);
            var griddata = await superGrid.GetGridAllDataAsync();
            if (griddata.Rows.Count != 0)
            {
                foreach (DataRow dr in griddata.Rows)
                {
                    string RowHeader = dr["Name"].ToString();
                    await superGrid.RemoveRowByRowHeaderAsync(RowHeader);
                }
            }

            await bottomNavigation.ClickBottomNavigationSpecialNameButtonAsync("Review + save");
            await bottomNavigation.ClickBottomNavigationSpecialNameButtonAsync("Review + save");
            await bottomNavigation.ClickBottomNavigationSpecialNameButtonAsync("Save");
        }
        #endregion
        #region Supersedence
        private async Task ClickSupersedenceAddButtonAsync()
        {
            await ControlHelper.ClickByClassAndHasTextAsync(await GetTabPanelLocatorAsync("Supersedence"), "msportalfx-text-primary ext-controls-selectLink fxs-fxclick", "+ Add", 0);
        }
        private async Task SelectSupersedenceAppsAsync(string apps, string isUninstallPreviousVersion)
        {
            SelectFromGridBySearchUtils selectFromGridBySearchUtils = new SelectFromGridBySearchUtils(this.CurrentIPage, this.CurrentEnv);
            await selectFromGridBySearchUtils.SelectBySearchWithKeywordAsync("Add Apps", "Search by name, publisher", apps);
            await SetAppInstallRequirementAsync("Supersedence", apps, isUninstallPreviousVersion);
        }
        private async Task SetAppInstallRequirementAsync(string tabSectionName, string appName, string value)
        {
            var tabLocator = await GetTabPanelLocatorAsync(tabSectionName);
            var rowContentLocator = await ControlHelper.GetLocatorByClassAndHasTextAsync(tabLocator, "fxc-gc-row-content", appName, 0);
            await ControlHelper.ClickByClassAndHasTextAsync(rowContentLocator, "fxs-portal-border azc-optionPicker-item", value, 0);
        }
        private async Task ClickSupersedenceEditButtonAsync()
        {
            await siteBarMenu.ClickPropertiesAsync();
            await ClickEditButtonAsync("Edit Supersedence");
        }
        private async Task DeleteSupersedenceAppsAsync(string name)
        {
            var TabLocator = await GetTabPanelLocatorAsync("Supersedence");
            IGrid superGrid = new Grid_FXC_GC_Table(this.CurrentIPage, IFrameName, this.CurrentLanguage, parentLocator: TabLocator);
            await superGrid.RemoveRowByRowHeaderAsync(name);
        }
        #endregion
        #region ConfigureAppSuiteForM365Office365App
        private async Task SelectOfficeAppsByExcludeAsync(List<string> values)
        {
            await ControlHelper.SetComBoxRoleTreeItemRoleValueAsync(this.CurrentIPage, "Select Office apps", values, 0, iFrameName: IFrameName);
        }
        private async Task SelectOtherOfficeAppsAsync(List<string> values)
        {
            await ControlHelper.SetComBoxRoleTreeItemRoleValueAsync(this.CurrentIPage, "Select other Office apps (license required)", values, 0, iFrameName: IFrameName);
        }
        private async Task SetArchitectureAsync(string value)
        {
            await ControlHelper.SelectRadioByHasTextAsync(this.CurrentIPage, value, 0, iFrameName: IFrameName);
        }
        private async Task SetdefaultFileFormatAsync(string value)
        {
            await ControlHelper.SetComBoxRoleTreeItemRoleValueAsync(this.CurrentIPage, "Default file format", value, 0, iFrameName: IFrameName);
        }
        private async Task SetUpdatechannelAsync(string value)
        {
            await ControlHelper.SetComBoxRoleTreeItemRoleValueAsync(this.CurrentIPage, "Update channel", value, 0, iFrameName: IFrameName);
        }
        private async Task SetAppSettingChannelAsync(string value)
        {
            await ControlHelper.SetComBoxRoleTreeItemRoleValueAsync(this.CurrentIPage, "Channel", value, 0, iFrameName: IFrameName);
        }
        private async Task SetRemoveOtherVersionsAsync(string value)
        {
            await ControlHelper.SelectRadioByRadioGroupNameAsync(this.CurrentIPage, "Remove other versions", value, 0, iFrameName: IFrameName);
        }
        private async Task SetVersionToInstallAsync(string value)
        {
            await ControlHelper.SelectRadioByRadioGroupNameAsync(this.CurrentIPage, "Version to install", value, 0, iFrameName: IFrameName);
        }
        private async Task SetspecificVersionToInstallAsync(string value)
        {
            await ControlHelper.SetComBoxRoleTreeItemRoleValueAsync(this.CurrentIPage, "Specific version", value, 0, iFrameName: IFrameName);
        }
        private async Task SetAcceptEulaAsync(string value)
        {
            await ControlHelper.SelectRadioByRadioGroupNameAsync(this.CurrentIPage, "Accept the Microsoft Software License Terms on behalf of users", value, 0, iFrameName: IFrameName);
        }
        private async Task SetInstallBackgroundServiceAsync(string value)
        {
            await ControlHelper.SelectRadioByRadioGroupNameAsync(this.CurrentIPage, "Install background service for Microsoft Search in Bing", value, 0, iFrameName: IFrameName);
        }
        private async Task SetUseSharedComputerActivationAsync(string value)
        {
            await ControlHelper.SelectRadioByRadioGroupNameAsync(this.CurrentIPage, "Use shared computer activation", value, 0, iFrameName: IFrameName);
        }
        private async Task SetLanguagesAsync(List<string> values)
        {
            await ControlHelper.ClickByButtonRoleAndHasTextAsync(this.CurrentIPage, "No languages selected", 0, iFrameName: IFrameName);
            await ControlHelper.ClickGridcellsByRowTextAndClassAsync(await GetPaneByRoleAsync("Languages"), "fxc-gc-selectioncheckbox azc-fill-text", values, 0);
            await ControlHelper.ClickByButtonRoleAndHasTextAsync(this.CurrentIPage, "OK", 0, iFrameName: IFrameName);
        }
        #endregion
        #region Assignments
        private async Task ClickAssignmentEditButtonAsync()
        {
            await siteBarMenu.ClickPropertiesAsync();
            await ClickEditButtonAsync("Edit Assignments");
        }
        #region Assignments Required
        private async Task ClickRequiredAddAllUsersAsync()
        {
            await ClickAddAllUsersButtonAsync(GetAssignmentsRequiredNth());
        }
        private async Task ClickRequiredAddAllDevicesAsync()
        {
            await ClickAddAllDevicesButtonAsync(GetAssignmentsRequiredNth());
        }
        private async Task ClickRequiredAddGroupAsync(string groupName)
        {
            await ClickAddGroupButtonAsync(GetAssignmentsRequiredNth());
            SelectGroupUtils selectGroupUtils = new SelectGroupUtils(this.CurrentIPage, this.CurrentEnv);
            await selectGroupUtils.SelectGroupAsync(groupName);
        }
        private async Task ClickRequiredBehaveInstallContextCellAsync(string groupName)
        {
            var behaveDataGridLocator = await GetAssignmentBehaveDataGridLocatorByGridAriaLabelAsync("Required Assignments");
            await ClickAssignmentGroupDataCellByClumnHeaderAndGroupNameAsync(behaveDataGridLocator, "Install context", groupName);
        }
        private async Task ClickRequiredAllDevicesFilterModeAsync()
        {
            var behaveDataGridLocator = await GetAssignmentBehaveDataGridLocatorByGridAriaLabelAsync("Required Assignments");
            await ClickAssignmentGroupDataCellByClumnHeaderAndGroupNameAsync(behaveDataGridLocator, "Filter mode", "All devices");
        }
        private async Task ClickRequiredAllUsersFilterModeAsync()
        {
            var behaveDataGridLocator = await GetAssignmentBehaveDataGridLocatorByGridAriaLabelAsync("Required Assignments");
            await ClickAssignmentGroupDataCellByClumnHeaderAndGroupNameAsync(behaveDataGridLocator, "Filter mode", "All users");
        }
        private async Task ClickRequiredAddGroupFilterModeAsync(string groupName)
        {
            var behaveDataGridLocator = await GetAssignmentBehaveDataGridLocatorByGridAriaLabelAsync("Required Assignments");
            await ClickAssignmentGroupDataCellByClumnHeaderAndGroupNameAsync(behaveDataGridLocator, "Filter mode", groupName);
        }
        private async Task ClickRequiredBehaveUninstallOnDeviceRemovalCellAsync(string groupName)
        {
            var behaveDataGridLocator = await GetAssignmentBehaveDataGridLocatorByGridAriaLabelAsync("Required Assignments");
            await ClickAssignmentGroupDataCellByClumnHeaderAndGroupNameAsync(behaveDataGridLocator, "Uninstall on device removal", groupName);
        }
        #endregion

        #region Assignments Available for enrolled devices
        private async Task ClickAvailableForEnrolledDevicesAllUsersAsync()
        {
            await ClickAddAllUsersButtonAsync(GetAssignmentsAvailableForEnrolledDevicesNth());
        }
        private async Task ClickAvailableForEnrolledDevicesAddGroupAsync(string groupName)
        {
            await ClickAddGroupButtonAsync(GetAssignmentsAvailableForEnrolledDevicesNth());
            SelectGroupUtils selectGroupUtils = new SelectGroupUtils(this.CurrentIPage, this.CurrentEnv);
            await selectGroupUtils.SelectGroupAsync(groupName);
        }
        private async Task ClickAvailableForEnrolledDevicesBehaveUninstallOnDeviceRemovalCellAsync(string groupName)
        {
            var behaveDataGridLocator = await GetAssignmentBehaveDataGridLocatorByGridAriaLabelAsync("Available for enrolled devices Assignments");
            await ClickAssignmentGroupDataCellByClumnHeaderAndGroupNameAsync(behaveDataGridLocator, "Uninstall on device removal", groupName);
        }
        #endregion
        #region Assignments Common Function
        private async Task SetUninstallOnDeviceRemovalAsync(string value)
        {
            await SetOptionPickerAsync("Uninstall on device removal", value);
            await ClickEditAssignmentBladeOKAsync();
        }
        private async Task SetInstallContextAsync(string value)
        {
            await SetOptionPickerAsync("Install context", value);
            await ClickEditAssignmentBladeOKAsync();
        }
        private async Task ClickEditAssignmentBladeOKAsync()
        {
            FXS_Blade_Title_Content fXS_Blade_Title_Content = new FXS_Blade_Title_Content(this.CurrentIPage, IFrameName, this.CurrentLanguage, bladeName: "Edit assignment");
            var bladeLocator = await fXS_Blade_Title_Content.GetBladeLocatorAsync();
            MSportalfxDockingFooterController mSportalfxDockingFooterController = new MSportalfxDockingFooterController(this.CurrentIPage, IFrameName, this.CurrentLanguage, parentLocator: bladeLocator);
            var buttonStatus = await mSportalfxDockingFooterController.ClickSpecialNameButtonAsync("OK");
            if (!buttonStatus)
            {
                await fXS_Blade_Title_Content.ClickCloseAsync();
            }
        }
        private int GetAssignmentsRequiredNth()
        {
            return GetAssignmentBehaveNth("Required");
        }

        private int GetAssignmentsAvailableForEnrolledDevicesNth()
        {
            return GetAssignmentBehaveNth("Available for enrolled devices");
        }
        private int GetAssignmentBehaveNth(string behaveName)
        {
            List<string> behaveTextList = new List<string>() { "Required", "Available for enrolled devices" };
            //var tabPannelLocator = await GetTabPanelLocatorAsync("Assignments");
            //var behaveTextList = await (await ControlHelper.GetLocatorByClassAsync(tabPannelLocator, "fxc-weave-pccontrol fxc-section-control fxc-base msportalfx-customHtml msportalfx-form-formelement")).AllInnerTextsAsync();
            return behaveTextList.ToList().IndexOf(behaveName);
        }
        #endregion
        #region Assignments Base Function

        private async Task ClickAddAllUsersButtonAsync(int nth)
        {
            await ClickAssignmentGroupButtonAsync("+ Add all users", nth);
        }
        private async Task ClickAddAllDevicesButtonAsync(int nth)
        {
            await ClickAssignmentGroupButtonAsync("+ Add all devices", nth);
        }
        private async Task ClickAddGroupButtonAsync(int nth)
        {
            await ClickAssignmentGroupButtonAsync("+ Add group", nth);
        }

        private async Task ClickAssignmentGroupButtonAsync(string buttonName, int nth)
        {
            await ControlHelper.ClickByClassAndHasTextAsync(this.CurrentIPage, "msportalfx-text-primary ext-controls-selectLink", buttonName, nth, iFrameName: IFrameName);
        }

        private async Task ClickAssignmentGroupDataCellByClumnHeaderAndGroupNameAsync(ILocator behaveDataGridLocator, string columnHeader, string groupName)
        {
            var rowHeaderLocator = await ControlHelper.GetByRoleAndHasTextAsync(behaveDataGridLocator, AriaRole.Columnheader, columnHeader, 0);
            var rowHeaderClass = await rowHeaderLocator.GetAttributeAsync("class");
            var rowHeaderUniqueClass = rowHeaderClass.Split(" ").Where(t => t.Contains("fxc-gc-columnheader_")).FirstOrDefault();

            var dataRowLocator = await ControlHelper.GetLocatorByClassAndHasTextAsync(behaveDataGridLocator, "fxc-gc-row-content", groupName, 0);

            var dataCellUniqueClass = rowHeaderUniqueClass.Replace("fxc-gc-columnheader_", "fxc-gc-columncell_");
            var dataCellLocator = await ControlHelper.GetLocatorByClassAsync(dataRowLocator, dataCellUniqueClass, 0);
            await ControlHelper.ClickByClassAsync(dataCellLocator, "fxs-fxclick", 0);
        }
        private async Task<ILocator> GetAssignmentBehaveDataGridLocatorByGridAriaLabelAsync(string GridAriaLabel)
        {
            return await ControlHelper.GetLoatorByClassAndAriaLabelAsync(this.CurrentIPage, "fxc-gc azc-fabric fxc-gc-dataGrid", GridAriaLabel, 0, iframeName: IFrameName);
        }
        #endregion
        #endregion
        #region Verify
        private async Task VerifyPropertyAsync(string propertyName, string propertyValue)
        {
            await siteBarMenu.ClickPropertiesAsync();
            var propertyKeyLocator = await ControlHelper.GetLocatorByClassAndHasTextAsync(this.CurrentIPage, "fxc-summary-item fxc-summary-label", propertyName, 0, iframeName: IFrameName);
            var propertyRowLocator = await ControlHelper.GetParentLocatorBySonLocatorAsync(propertyKeyLocator, 1);
            await ControlHelper.GetLocatorByClassAndHasTextAsync(propertyRowLocator, "ext-summary-stringItem", propertyValue, 0);
        }
        private async Task VerifyPropertyAssignmentsGridDataCellAsync(string groupBehave, string groupName, string columnHeader, string cellValue)
        {
            await VerifyPropertyAssignmentsAsync(groupBehave, groupName, columnHeader, cellValue);
        }
        private async Task VerifyPropertyAssignmentsAsync(string groupBehave, string groupName, string columnHeader = null, string cellValue = null)
        {
            await siteBarMenu.ClickPropertiesAsync();
            var AssignmentPropertyLocator = await ControlHelper.GetLoatorByClassAndAriaLabelAsync(this.CurrentIPage, "fxc-gc azc-fabric fxc-gc-dataGrid", "Assignments", 0, iframeName: IFrameName);
            var GroupBehaveLocator = await ControlHelper.GetLocatorByClassAndHasTextAsync(AssignmentPropertyLocator, "azc-br-muted fxc-gc-rowgroup fxs-portal-hover", groupBehave, 0);

            var GroupBehaveDataLocators = await GroupBehaveLocator.Locator("~ div").AllAsync();

            foreach (var GroupBehaveDataLocator in GroupBehaveDataLocators)
            {
                var GroupBehaveDataClass = await GroupBehaveDataLocator.GetAttributeAsync("class");
                if (GroupBehaveDataClass.Contains("azc-br-muted fxs-portal-hover"))
                {
                    var GroupDataContent = await GroupBehaveDataLocator.InnerTextAsync();
                    if (GroupDataContent.ToLower().Contains(groupName.ToLower()))
                    {
                        if (!string.IsNullOrEmpty(columnHeader) && !string.IsNullOrEmpty(cellValue))
                        {
                            var columnHeaderLocator = await ControlHelper.GetLoatorByClassAndAriaLabelAsync(AssignmentPropertyLocator, "fxc-gc-columnheader", columnHeader, 0);
                            var columnHeaderClass = await columnHeaderLocator.GetAttributeAsync("class");
                            var columnHeaderUniqueClass = columnHeaderClass.Split(" ").Where(t => t.Contains("fxc-gc-columnheader_")).FirstOrDefault();
                            var dataCellUniqueClass = columnHeaderUniqueClass.Replace("fxc-gc-columnheader_", "fxc-gc-columncell_");
                            var dataCellLocator = await ControlHelper.GetLocatorByClassAsync(GroupBehaveDataLocator, dataCellUniqueClass, 0);
                            var dataCellValue = await dataCellLocator.InnerTextAsync();
                            if (dataCellValue.ToLower() == cellValue.ToLower())
                            {
                                break;
                            }
                            else
                            {
                                throw new CustomLogException($"The cell value is not correct, please check the value:{cellValue}");
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else
                {
                    throw new CustomLogException($"The group behave is not correct, please check the value:{groupBehave}");
                }
            }
        }
        private async Task VerifyDeviceInstallStatusAsync()
        {
            await siteBarMenu.ClickDeviceInstallStatusAsync();
            try
            {
                var textlocaltor = await ControlHelper.GetLocatorByClassAndHasTextAsync(this.CurrentIPage, "fxs-mode-light", "No items found", 0, "DeviceInstallStatus.ReactView", null, true);
                if (await textlocaltor.CountAsync() != 0)
                {
                    LogHelper.Info("Verify Device install status page success!");
                }
                else
                {
                    throw new CustomLogException("Verify Device install status page failed");
                }
            }
            catch
            {
                var textlocaltor = await ControlHelper.GetLocatorByClassAndHasTextAsync(this.CurrentIPage, "fxc-gc-table", "No Results", 0, "", null, true);
                if (await textlocaltor.CountAsync() != 0)
                {
                    LogHelper.Info("Verify Device install status page success!");
                }
                else
                {
                    throw new CustomLogException("Verify Device install status page failed");
                }
            }

        }

        private async Task VerifyUserInstallStatusAsync()
        {
            await siteBarMenu.ClickUserInstallStatusAsync();
            try
            {
                var textlocaltor = await ControlHelper.GetLocatorByClassAndHasTextAsync(this.CurrentIPage, "fxs-mode-light", "No items found", 0, "UserInstallStatus.ReactView", null, true);
                if (await textlocaltor.CountAsync() != 0)
                {
                    LogHelper.Info("Verify Device User status page success!");
                }
                else
                {
                    throw new CustomLogException("Verify User install status page failed");
                }
            }
            catch
            {
                var textlocaltor = await ControlHelper.GetLocatorByClassAndHasTextAsync(this.CurrentIPage, "fxc-gc-table", "No Results", 0, "", null, true);
                if (await textlocaltor.CountAsync() != 0)
                {
                    LogHelper.Info("Verify User install status page success!");
                }
                else
                {
                    throw new CustomLogException("Verify User install status page failed");
                }
            }
        }
        private async Task VerifyMonitorAppInstallStatusAsync(string name)
        {
            await siteBar.ClickAppsAsync();
            await siteBarMenu.ClickMonitorAsync();
            try
            {
                var frame = await ControlHelper.GetLocatorByClassAsync(this.CurrentIPage, "ms-DetailsList-contentWrapper", 0, "AppsMonitoringReports.ReactView", true);
                var appInstallStatusLink = await ControlHelper.GetLocatorByTextAsync(this.CurrentIPage, "App install status", "AppsMonitoringReports.ReactView");
                await appInstallStatusLink.ClickAsync();
            }
            catch
            {
                await siteBarMenu.ClickAppInstallStatusAsync();

            }
            finally
            {
                MS_SearchBox searchBox0 = new MS_SearchBox(this.CurrentIPage, "AppsInstallStatus.ReactView", this.CurrentLanguage);
                await searchBox0.SetSearchBoxValueAsync(name);
                bool result = await ControlHelper.RetryAsync(retryCount: 12, async () =>
                {
                    MSCommandBarWithMenubarRole commandBar = new MSCommandBarWithMenubarRole(this.CurrentIPage, "AppsInstallStatus.ReactView", this.CurrentLanguage);
                    await commandBar.ClickCommandBarRefreshButtonAsync();
                    Thread.Sleep(3 * 1000);
                    var locator = await ControlHelper.GetLocatorByClassAndHasTextAsync(this.CurrentIPage, "ms-DetailsList-contentWrapper", name, 0, "AppsInstallStatus.ReactView", null, false, true);
                    if (await locator.CountAsync() == 0)
                    {
                        Thread.Sleep(30 * 1000);
                        throw new CustomLogException("app is still not showing, continue to wait 30s");
                    }
                });
                if (result)
                {
                    LogHelper.Info("Verify App install status page success!");
                }
                else
                {
                    throw new CustomLogException("Verify App install status page failed");
                }
            }
            
        }
        #endregion
        #region Update
        private async Task ClickEditButtonAsync(string sectionArialable)
        {
            await ControlHelper.ClickByClassWithAriaLableAsync(this.CurrentIPage, "ext-summary-editSectionHeader", sectionArialable, 0, iFrameName: IFrameName);
        }
        #endregion
        #region Rules

        private async Task SelectRequirementTypeAsync(string type)
        {
            await ControlHelper.SetComBoxRoleTreeItemRoleValueAsync(this.CurrentIPage, "Requirement type", type, 0, iFrameName: IFrameName);
        }
        private async Task SetRuleTypeValueAsync(string ruleType)
        {
            await ControlHelper.SetComBoxRoleTreeItemRoleValueAsync(this.CurrentIPage, "Rule type", ruleType, 0, iFrameName: IFrameName);
        }

        private async Task SetMSIProductVersionCheckAsync(string isCheck)
        {
            await SetOptionPickerAsync("MSI product version check", isCheck);
        }


        private async Task SetRuleAssociatedWithA32BitAppOn64BitClientsAsync(string value)
        {
            await SetOptionPickerAsync("Associated with a 32-bit app on 64-bit clients", value);
        }
        private async Task SetRuleDateTimeValueAsync(string time)
        {
            var valueDate = DateTime.Now;
            if (DateTime.TryParse(time, out valueDate))
            {
                await ControlHelper.SetInputByClassAndPlaceholderAsync(this.CurrentIPage, "azc-input", "MM/DD/YYYY", valueDate.ToString("d"), 0, iFrameName: IFrameName);
                await ControlHelper.SetInputByClassAndPlaceholderAsync(this.CurrentIPage, "azc-input", "h:mm:ss AM/PM", valueDate.ToString("T"), 0, iFrameName: IFrameName);
            }
            else
            {
              //  throw new CustomLogException($"The time format is not correct, please check the value:{time}");
            }

        }
        private async Task SetRuleDetectionMethodAsync(string method)
        {
            await ControlHelper.SetComBoxRoleTreeItemRoleValueAsync(this.CurrentIPage, "Detection method", method, 0, iFrameName: IFrameName);
        }
        private async Task SetRuleEnforceScriptSignatureCheckAsync(string value)
        {
            await SetOptionPickerAsync("Enforce script signature check", value);
        }
        private async Task SetRuleFileOrFolderAsync(string file)
        {
            await SetAzcInputBoxAsync("File or folder", file);
        }
        private async Task SetRuleKeyPathAsync(string path)
        {
            await SetAzcInputBoxAsync("Key path", path);
        }
        private async Task SetRuleOperatorAsync(string oper)
        {
            await ControlHelper.SetComBoxRoleTreeItemRoleValueAsync(this.CurrentIPage, "Operator", oper, 0, iFrameName: IFrameName);
        }
        private async Task SetRulePathAsync(string path)
        {
            await SetAzcInputBoxAsync("Path", path);
        }
        private async Task SetRulePropertyAsync(string property)
        {
            await ControlHelper.SetComBoxRoleTreeItemRoleValueAsync(this.CurrentIPage, "Property", property, 0, iFrameName: IFrameName);
        }
        private async Task SetRuleRegistryKeyRequirementAsync(string registryKey)
        {
            await ControlHelper.SetComBoxRoleTreeItemRoleValueAsync(this.CurrentIPage, "Registry key requirement", registryKey, 0, iFrameName: IFrameName);
        }
        private async Task SetRuleRunThisScriptUsingTheLoggedOnCredentialsAsync(string value)
        {
            await SetOptionPickerAsync("Run this script using the logged-on credentials", value);
        }
        private async Task SetRuleRunScriptAs32BitProcessOn64BitClientsAsync(string value)
        {
            await SetOptionPickerAsync("Run script as 32-bit process on 64-bit clients", value);
        }
        private async Task SetRuleScriptFileAsync(string filePath)
        {
            UploadController uploadController = new UploadController(this.CurrentIPage, IFrameName, this.CurrentLanguage);
            await uploadController.UploadFileAsync(filePath);
        }
        private async Task SetRuleSelectOutputDataTypeAsync(string type)
        {
            await ControlHelper.SetComBoxRoleTreeItemRoleValueAsync(this.CurrentIPage, "Select output data type", type, 0, iFrameName: IFrameName);
        }
        private async Task SetRuleValueAsync(string value)
        {
            await SetAzcInputBoxAsync("Value", value, noText: "name");
        }
        private async Task SetRuleValueNameAsync(string name)
        {
            await SetAzcInputBoxAsync("Value name", name);
        }
        private async Task ClickDetectionRuleOKButtonAsync()
        {
            await ControlHelper.ClickByClassAndHasTextAsync(this.CurrentIPage, "fxs-button fxt-button fxs-inner-solid-border fxs-portal-button-primary", "OK", 0, iFrameName: IFrameName);
        }

        #endregion
        #region Base
        private async Task<ILocator> GetTabPanelLocatorAsync(string panelName)
        {
            return await ControlHelper.GetByRoleAndHasTextAsync(this.CurrentIPage, AriaRole.Tabpanel, panelName, 0, iFrameName: IFrameName);
        }
        private async Task<ILocator> GetPaneByRoleAsync(string paneName)
        {
            return await ControlHelper.GetByRoleAndHasTextAsync(this.CurrentIPage, AriaRole.Complementary, paneName, 0, iFrameName: IFrameName);
        }
        public async Task SetOptionPickerAsync(string title, string value)
        {
            var optionPickerLocator = await ControlHelper.GetLocatorByClassAndHasTextAsync(this.CurrentIPage, "fxc-weave-pccontrol fxc-section-control fxc-base msportalfx-form-formelement fxc-has-label", title, 0, iframeName: IFrameName);
            await ControlHelper.ClickByClassAndHasTextAsync(optionPickerLocator, "fxs-portal-border azc-optionPicker-item", value, 0);

        }
        public async Task SetAzcInputBoxAsync(string name, string value, string noText = null)
        {
            var locator = await ControlHelper.GetLocatorByClassAndHasTextAsync(this.CurrentIPage, "fxc-weave-pccontrol fxc-section-control fxc-base msportalfx-form-formelement fxc-has-label azc-textField fxc-TextField azc-fabric azc-validationBelowCtrl", name, 0, hasNotText: noText, iframeName: IFrameName);
            await ControlHelper.SetInputByClassAndTypeAsync(locator, "azc-input azc-formControl", "text", value, 0);
        }
        #endregion
    }
}
