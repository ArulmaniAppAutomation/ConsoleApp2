using Account_Management.CommonBase;
using Microsoft.Playwright;
using PlaywrightTests.Common.Helper;
using PlaywrightTests.Common.Utils.BaseUtils.PopUp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Account_Management.CommonBase
{
   public  class AllAppUtills:BaseCommonUtils
    {

        public ICommandBar commandBar;
        public static IPage Page { get; private set; }
        private readonly string _portalUrl;
        public static string? IFrameName = null;

        public AllAppUtills(IPage page, string env):base (page,env) 
        {
            Page=page;
            _portalUrl = env;
           

        }
        /// <summary>
        /// Uploads an app package file via the LOB "Select app package file" flow.
        /// Resolves relative paths against the test run base directory.
        /// </summary>
        public async Task UploadAppPackageFileAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return;

            try
            {
                var packagePath = filePath;
                if (packagePath.StartsWith(".\\") || packagePath.StartsWith("./"))
                {
                    packagePath = Path.Combine(AppContext.BaseDirectory, packagePath.Substring(2));
                }
                packagePath = Path.GetFullPath(packagePath);

                var selectAppPackageFile = await ElementHelper.GetByButtonRoleAndNameAsync(_page, "Select app package file", true);
                await selectAppPackageFile.ClickAsync();

                var fileInput = await ElementHelper.GetByClassAsync(_page, "fxs-async-fileupload-overlay");
                await fileInput.SetInputFilesAsync(packagePath);

                var okButton = await ElementHelper.GetByRoleAndNameAsync(_page, AriaRole.Button, "OK");
                await okButton.ClickAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Warning: failed to upload package file: {ex.Message}");
            }
        }
        private int GetAssignmentBehaveNth(string behaveName)
        {
            List<string> behaveTextList = new List<string>() { "Required", "Available for enrolled devices" };
            //var tabPannelLocator = await GetTabPanelLocatorAsync("Assignments");
            //var behaveTextList = await (await ControlHelper.GetLocatorByClassAsync(tabPannelLocator, "fxc-weave-pccontrol fxc-section-control fxc-base msportalfx-customHtml msportalfx-form-formelement")).AllInnerTextsAsync();
            return behaveTextList.ToList().IndexOf(behaveName);
        }

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
        private int GetAssignmentsRequiredNth()
        {
            return GetAssignmentBehaveNth("Required");
        }

        public async Task ClickRequiredAddAllUsersAsync()
        {
            await ClickAddAllUsersButtonAsync(GetAssignmentsRequiredNth());
        }
        private async Task ClickAddAllUsersButtonAsync(int nth)
        {
            await ClickAssignmentGroupButtonAsync("+ Add all users", nth);
        }

        private async Task ClickAssignmentGroupButtonAsync(string buttonName, int nth)
        {
            await ControlHelper.ClickByClassAndHasTextAsync(_page, "msportalfx-text-primary ext-controls-selectLink", buttonName, nth, iFrameName: IFrameName);
        }

        private async Task ClickRequiredAllUsersFilterModeAsync()
        {
            var behaveDataGridLocator = await GetAssignmentBehaveDataGridLocatorByGridAriaLabelAsync("Required Assignments");
            await ClickAssignmentGroupDataCellByClumnHeaderAndGroupNameAsync(behaveDataGridLocator, "Filter mode", "All users");
        }
        private async Task ClickRequiredAllDevicesFilterModeAsync()
        {
            var behaveDataGridLocator = await GetAssignmentBehaveDataGridLocatorByGridAriaLabelAsync("Required Assignments");
            await ClickAssignmentGroupDataCellByClumnHeaderAndGroupNameAsync(behaveDataGridLocator, "Filter mode", "All devices");
        }
        private async Task ClickRequiredAddGroupFilterModeAsync(string groupName)
        {
            var behaveDataGridLocator = await GetAssignmentBehaveDataGridLocatorByGridAriaLabelAsync("Required Assignments");
            await ClickAssignmentGroupDataCellByClumnHeaderAndGroupNameAsync(behaveDataGridLocator, "Filter mode", groupName);
        }

        private async Task<ILocator> GetAssignmentBehaveDataGridLocatorByGridAriaLabelAsync(string GridAriaLabel)
        {
            return await ControlHelper.GetLoatorByClassAndAriaLabelAsync(_page, "fxc-gc azc-fabric fxc-gc-dataGrid", GridAriaLabel, 0, iframeName: IFrameName);
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

        public  async Task ClickRequiredAddAllDevicesAsync()
        {
            await ClickAddAllDevicesButtonAsync(GetAssignmentsRequiredNth());
        }
        private async Task ClickAddAllDevicesButtonAsync(int nth)
        {
            await ClickAssignmentGroupButtonAsync("+ Add all devices", nth);
        }
        public async Task ClickRequiredAddGroupAsync(string groupName)
        {
            await ClickAddGroupButtonAsync(GetAssignmentsRequiredNth());
            SelectGroupUtills selectGroupUtils = new SelectGroupUtills(_page, _portalUrl);
            await selectGroupUtils.SelectGroupAsync(groupName);
        }
        private async Task ClickAddGroupButtonAsync(int nth)
        {
            await ClickAssignmentGroupButtonAsync("+ Add group", nth);
        }


        public async Task ClickAvailableForEnrolledDevicesAllUsersAsync()
        {
            await ClickAddAllUsersButtonAsync(GetAssignmentsAvailableForEnrolledDevicesNth());
        }

        private int GetAssignmentsAvailableForEnrolledDevicesNth()
        {
            return GetAssignmentBehaveNth("Available for enrolled devices");
        }

        public async Task ClickAvailableForEnrolledDevicesAddGroupAsync(string groupName)
        {
            await ClickAddGroupButtonAsync(GetAssignmentsAvailableForEnrolledDevicesNth());
            SelectGroupUtills selectGroupUtils = new SelectGroupUtills(_page, _portalUrl);
            await selectGroupUtils.SelectGroupAsync(groupName);
        }

        public async Task<(bool, Dictionary<string, string>)> SetAppinformationNameAsync(string operationValue, Func<string, Task> setFunction)
        {
           // var appName = BaseCreateUniqueProfileName(operationValue);
            var appName=operationValue.ToLower();
            
            await setFunction(appName);

            var parameter = new Dictionary<string, string>();

            try
            {
                DictionaryItemProcess(parameter, "AppAutomationAppName", appName);
            }
            catch
            {
                // handle or ignore
            }

            return (true, parameter);
        }

        public async Task SetAppInformationDescriptionAsync(string description)
        {
            await SetAppInformationTextAreaByBannerAsync("Description", description);
        }
        public async Task SetAppInformationTextAreaByBannerAsync(string title, string value)
        {
            var DescriptionBannerLocator = await GetAppInformationBannerLocatorByTitleAsync(title);
            await ControlHelper.SetInputByClassAndTypeAsync(DescriptionBannerLocator, "azc-textarea azc-formControl azc-input", "text", value, 0);
        }
        public async Task<ILocator> GetAppInformationBannerLocatorByTitleAsync(string title)
        {
            return await ControlHelper.GetLocatorByClassAndHasTextAsync(_page, "fxc-section-control", title, -1, iframeName: IFrameName);
        }
        public async Task SelectTheAppStoreAsync(string name)
        {
            SelectFromGridBySearchUtils selectFromGridBySearchUtils = new SelectFromGridBySearchUtils(_page, _portalUrl);
            await selectFromGridBySearchUtils.SelectBySearchWithKeywordAsync("Search the App Store", "Search the App Store", name);
        }
        public async Task SetAppInformationUrlAsync(string value)
        {
            await SetAppInformationInputWithAriaLabelAsync("Information URL", value);
        }

        public async Task SetAppInformationInputWithAriaLabelAsync(string ariaLabel, string value)
        {
            await ControlHelper.SetInputByClassAndAriaLabelAsync(_page, "azc-input azc-formControl", ariaLabel, value, 0, iFrameName: IFrameName);
        }
        public async Task SetTargetedPlatformAsync(string value)
        {
            await SetAppInformationComboxWithAriaLabelAsync("Targeted platform", new List<string> { value });
        }
        public async Task SetMinimumOperationSystemAsync(string operationSystem)
        {
            await ControlHelper.SetComBoxRoleTreeItemRoleValueAsync(_page, "Minimum operating system", operationSystem, 0, iFrameName: IFrameName);
        }
        public async Task SetAppInformationComboxWithAriaLabelAsync(string ariaLabel, List<string> values)
        {
            await ControlHelper.SetComBoxRoleTreeItemRoleValueAsync(_page, ariaLabel, values, 0, iFrameName: IFrameName);
        }
        public async Task SetAppInformationPrivacyURLAsync(string value)
        {
            await SetAppInformationInputWithAriaLabelAsync("Privacy URL", value);
        }
        public async Task SetAppInformationNotesAsync(string value)
        {
            await SetAppInformationTextAreaByBannerAsync("Notes", value);
        }
        public async Task ClickBottomNavigationSpecialNameButtonAsync(string buttonName)
        {
            await ClickButtonByNameAsync(buttonName);
        }
        public async Task ClickSelectTheAppStoreButtonAsync()
        {
            await ClickAppInformationSelectFileButtonAsync("Search the App Store");
        }
        public async Task ClickAppInformationSelectAppButtonAsync()
        {
            await ClickAppInformationSelectFileButtonAsync("Select app");
        }
        public async Task ClickAppInformationSelectFileButtonAsync(string buttonName)
        {
            await ControlHelper.ClickByClassWithAriaLableAsync(_page, "msportalfx-text-primary ext-controls-selectLink", buttonName, 0, iFrameName: IFrameName);
        }
        private static async Task ClickButtonByNameAsync(string name)
        {

            var locator = await GetButtonSectionLocatorAsync();
            await ControlHelper.ClickByClassAndHasTextAsync(locator, "fxs-button", name, 0);

            // await ControlHelper.ClickByClassAndHasTextAsync(buttonSectionLocator, "fxs-button", name, 0);
        }
        private static async Task<ILocator> GetButtonSectionLocatorAsync()
        {
            return await ControlHelper.GetLocatorByClassAsync(_page, "ext-wizardNextButton", -1, iFrameName: IFrameName);
        }

        private ILocator buttonSectionLocator
        {
            get { return GetButtonSectionLocator(); }
        }
        private ILocator GetButtonSectionLocator()
        {
            return ControlHelper.GetLocatorByClassAsync(_page, "ext-wizardPrevButton", -1, iFrameName: IFrameName).Result;
        }
        public async Task SelectOfficeAppsByExcludeAsync(List<string> values)
        {
            await ControlHelper.SetComBoxRoleTreeItemRoleValueAsync(_page, "Select Office apps", values, 0, iFrameName: IFrameName);
        }
        public async Task SelectOtherOfficeAppsAsync(List<string> values)
        {
            await ControlHelper.SetComBoxRoleTreeItemRoleValueAsync(_page, "Select other Office apps (license required)", values, 0, iFrameName: IFrameName);
        }
        public async Task SetLanguagesAsync(List<string> values)
        {
            await ControlHelper.ClickByButtonRoleAndHasTextAsync(_page, "No languages selected", 0, iFrameName: IFrameName);
            await ControlHelper.ClickGridcellsByRowTextAndClassAsync(await GetPaneByRoleAsync("Languages"), "fxc-gc-selectioncheckbox azc-fill-text", values, 0);
            await ControlHelper.ClickByButtonRoleAndHasTextAsync(_page, "OK", 0, iFrameName: IFrameName);
        }
        private async Task<ILocator> GetPaneByRoleAsync(string paneName)
        {
            return await ControlHelper.GetByRoleAndHasTextAsync(_page, AriaRole.Complementary, paneName, 0, iFrameName: IFrameName);
        }
        public  async Task SetRulePathAsync(string path)
        {
            await SetAzcInputBoxAsync("Path", path);
        }

        public  async Task SetAzcInputBoxAsync(string name, string value, string noText = null)
        {
            var locator = await ControlHelper.GetLocatorByClassAndHasTextAsync(Page, "fxc-weave-pccontrol fxc-section-control fxc-base msportalfx-form-formelement fxc-has-label azc-textField fxc-TextField azc-fabric azc-validationBelowCtrl", name, 0, hasNotText: noText, iframeName: IFrameName);
            await ControlHelper.SetInputByClassAndTypeAsync(locator, "azc-input azc-formControl", "text", value, 0);
        }
        public  async Task SetRuleFileOrFolderAsync(string file)
        {
            await SetAzcInputBoxAsync("File or folder", file);
        }

        public async Task SetArchitectureAsync(string value)
        {
            await ControlHelper.SelectRadioByHasTextAsync(_page, value, 0, iFrameName: IFrameName);
        }
        public async Task SetdefaultFileFormatAsync(string value)
        {
            await ControlHelper.SetComBoxRoleTreeItemRoleValueAsync(_page, "Default file format", value, 0, iFrameName: IFrameName);
        }
        public async Task SetUpdatechannelAsync(string value)
        {
            await ControlHelper.SetComBoxRoleTreeItemRoleValueAsync(_page, "Update channel", value, 0, iFrameName: IFrameName);
        }

    }
}
