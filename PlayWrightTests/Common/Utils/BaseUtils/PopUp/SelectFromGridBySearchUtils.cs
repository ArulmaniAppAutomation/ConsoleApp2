//using LogService;
using Microsoft.Playwright;
//using Microsoft.VisualStudio.Services.Graph.Client;
using PlaywrightTests.Common.Controller.Grid;
using PlaywrightTests.Common.Helper;
using PlaywrightTests.Common.Model;
using PlaywrightTests.Common.Utils.BaseUtils.UtilInterface;
using PlaywrightTests.Common.Utils.FeatureUtils;

namespace PlaywrightTests.Common.Utils.BaseUtils.PopUp
{
    public class SelectFromGridBySearchUtils : BaseCommonUtils, InterfaceUtils
    {
        public SelectFromGridBySearchUtils(IPage page, string env) : base(page, env)
        {
        }
        public Task RunAsync()
        {
            throw new NotImplementedException();
        }

        public Task ClearDataAsync()
        {
            throw new NotImplementedException();
        }

        public Task GoToMainPageAsync()
        {
            throw new NotImplementedException();
        }
        public Task CheckIfCurrentPageIsAvailableAsync()
        {
            throw new NotImplementedException();
        }
        public async Task<(bool IsContinue, Dictionary<string, string> Parameter)> RunStepAsync(ControlInfo controlInfo)
        {
            throw new NotImplementedException();
        }

        public Task ClearSpecialDataAsync(string name)
        {
            throw new NotImplementedException();
        }
        public async Task SelectDependencyBySearchWithKeywordAsync(string keyword)
        {
            await SelectBySearchWithKeywordAsync("Add dependency", "Search by name, publisher", keyword);
        }
        public async Task SelectAppsBySearchWithKeywordAsync(string keyword)
        {
            await SelectBySearchWithKeywordAsync("Select apps", "Search apps", keyword);

        }
        public async Task SelectAppConfigurationPoliciesBySearchWithKeywordAsync(string keyword)
        {
            await SelectBySearchWithKeywordAsync("Select app configuration policies", "Search by name", keyword);
        }
        public async Task SelectAppProtectionPoliciesBySearchWithKeywordAsync(string keyword)
        {
            await SelectBySearchWithKeywordAsync("Select app protection policies", "Search by name", keyword);
        }
        public async Task SelectDeviceConfigurationsBySearchWithKeywordAsync(string keyword)
        {
            await SelectBySearchWithKeywordAsync("Select device configuration policies", "Search policy", keyword);
        }
        public async Task SelectDeviceCompliancePoliciesBySearchWithKeywordAsync(string keyword)
        {
            await SelectBySearchWithKeywordAsync("Select device compliance policies", "Search policy", keyword);
        }

        public async Task SelectWindowsAutopilotDeploymentProfilesBySearchWithKeywordAsync(string keyword)
        {
            await SelectBySearchWithKeywordAsync("Select Windows Autopilot deployment profiles", "Search by name", keyword);
        }
        public async Task SelectEnrollmentStatusPageProfilesBySearchWithKeywordAsync(string keyword)
        {
            await SelectBySearchWithKeywordAsync("Select Enrollment status page profile", "Search by name", keyword);
        }
        public async Task SelectScopeTagsBySearchWithKeywordAsync(string keyword)
        {
            await SelectBySearchWithKeywordAsync("Select tags", "Search by name", keyword);
        }
        public async Task SelectScopeTagsByNthAsync(int nth, bool isClear = false)
        {
            await SelectByNthAsync("Select tags", nth: nth, IsClear: isClear);
        }
        public async Task SelectReusableSettingsAsync(List<string> keywords, string? iFrameName = null)
        {
            await SelectBySearchWithKeywordThenClickAddAndSaveAsync("Select reusable settings", "Search", keywords, iFrameName: iFrameName);
        }
        public async Task SelectSettingsByCategoryAsync(string category, List<string> settings, bool isExpandAllNode = true)
        {
            var arrayCategory = category.Split("|");
            if (arrayCategory.Length == 1)
            {
                await SelectBySearchWithKeyAndValuesAsync("Settings picker", "Search box", category, settings, isExpandAllNode: isExpandAllNode);
            }
            else
            {
                int index = category.IndexOf("|") + 1;
                var actualName = category.Substring(index, category.Length - index);
                await SelectBySearchWithKeyAndValuesAsync("Settings picker", "Search box", arrayCategory[0], settings, actualName: actualName, isExpandAllNode: isExpandAllNode);
            }
        }


        #region private methods
        public async Task SelectBySearchWithKeywordAsync(string title, string searchBoxAriaLabel, string keyword)
        {
            var complementaryLocator = await ControlHelper.GetByRoleAndHasTextAsync(CurrentIPage, AriaRole.Complementary, title, 0, iFrameName: null);
            await ControlHelper.SetInputByClassAndAriaLabelAsync(complementaryLocator, "azc-input azc-formControl azc-validation-border msportalfx-tooltip-overflow", searchBoxAriaLabel, keyword, 0);
            await ControlHelper.ClickByGridCellAndHasTextAsync(complementaryLocator, keyword, 0);
            await ClickSelectBtnAsync();
        }
        public async Task SelectBySearchWithKeywordClickCheckBoxAsync(string title, string searchBoxLabel, string keyword, string filterValue = null)
        {
            var complementaryLocator = await ControlHelper.GetByRoleAndHasTextAsync(CurrentIPage, AriaRole.Complementary, title, 0, iFrameName: null);

            await ControlHelper.SetInputByClassAndAriaLabelAsync(complementaryLocator, "azc-input azc-formControl azc-validation-border msportalfx-tooltip-overflow", searchBoxLabel, keyword, 0);

            if (!string.IsNullOrEmpty(filterValue))
            {
                var rowLocators = await (await ControlHelper.GetAllLocatorsByClassAsync(complementaryLocator, "fxc-gc-row-content fxc-gc-row-content_")).AllAsync();
                foreach (var rowLocator in rowLocators)
                {
                    var text = await rowLocator.InnerTextAsync();
                    if (text.Contains(filterValue))
                    {
                        await ControlHelper.ClickByClassAsync(rowLocator, "fxc-gc-selectioncheckbox", 0);
                        break;
                    }
                }
            }
            else
            {
                await ControlHelper.ClickByGridCellAndHasTextAsync(complementaryLocator, keyword, 0);
            }
            await ClickSelectBtnAsync();
        }
        public async Task SelectMultiValueByKeywordClickCheckBoxAsync(string title, List<string> keywords)
        {
            var complementaryLocator = await ControlHelper.GetByRoleAndHasTextAsync(CurrentIPage, AriaRole.Complementary, title, 0, iFrameName: null);
            foreach (var keyword in keywords)
            {
                var rowLocator = await ControlHelper.GetLocatorByClassAndHasTextAsync(complementaryLocator, "fxc-gc-row-content fxc-gc-row-content_", keyword, 0);
                await ControlHelper.ClickByClassAsync(rowLocator, "fxc-gc-selectioncheckbox", 0);
            }
            await ClickOKBtnAsync();
        }
        private async Task SelectByNthAsync(string title, int nth, bool IsClear = false)
        {
            var complementaryLocator = await ControlHelper.GetByRoleAndHasTextAsync(CurrentIPage, AriaRole.Complementary, title, 0, iFrameName: null);
            if (IsClear)
            {
                try
                {
                    //while (true)
                    var RemoveLocator = await ControlHelper.GetByRoleAndHasTextAsync(complementaryLocator, AriaRole.Button, "Remove", 0, waitUntilElementExist: false);
                    var noData = await ControlHelper.GetLocatorByLocatorAndTextAsync(complementaryLocator, "No scope tags selected",IsStrongWait:false);
                    
                    var pageLoadStatus = await ControlHelper.CheckDataExitInListPageAsync(RemoveLocator, noData);
                  //  LogHelper.Info($"\"Remove\" exist status: {pageLoadStatus}");
                    while (pageLoadStatus)
                    {
                        await RemoveLocator.ClickAsync();
                        pageLoadStatus = await ControlHelper.CheckDataExitInListPageAsync(RemoveLocator, noData);
                      //  LogHelper.Info($"After clicking, the \"Remove\" exist status: {pageLoadStatus}");
                    }
                }
                catch
                {
                    throw new Exception($"Not found the Remove button, please check it. ");
                }
            }
            var selectTableLocator = await ControlHelper.GetByClassAsync(complementaryLocator, "Selectable RightClickableRow");
            await ControlHelper.ClickByGridCellAndHasTextAsync(selectTableLocator.currentLocator, "", nth);
            await ClickSelectBtnAsync();
        }
        public async Task ExpandAllNodeByLocatorAsync(ILocator locator)
        {
            ILocator allExpandLocator = null;
            try
            {
                allExpandLocator = await ControlHelper.GetAllLocatorsByClassAsync(locator, "fxc-gc-hierarchy-expanderButton", wait: false);
            }
            catch
            {
               // LogHelper.Info("No expand button found.");
            }
            var allExpandCount = allExpandLocator == null ? 0 : await allExpandLocator.CountAsync();
            if (allExpandCount == 0)
            {
                return;
            }
            bool isExpandAllNode = false;
            if (allExpandCount > 0)
            {
                for (var i = 0; i < allExpandCount; i++)
                {
                    var expandLocator = allExpandLocator.Nth(i);
                    var expandVisibleStatus = await expandLocator.IsVisibleAsync();
                    if (expandVisibleStatus)
                    {
                        var expandStatus = await expandLocator.GetAttributeAsync("aria-expanded");
                     //   LogHelper.Info($"Expand status: {expandStatus}");
                        if (expandStatus == "false")
                        {
                            await expandLocator.ClickAsync();
                            isExpandAllNode = true;
                        }
                    }
                }
            }
            if (isExpandAllNode)
            {
                await ExpandAllNodeByLocatorAsync(locator);
            }
            else
            {
                return;
            }
        }
        private async Task SelectBySearchWithKeyAndValuesAsync(string title, string searchBoxLabel, string key, List<string> values, bool isExpandAllNode = true, string? actualName = "")
        {
            var complementaryLocator = await ControlHelper.GetByRoleAndHasTextAsync(CurrentIPage, AriaRole.Complementary, title, 0, iFrameName: null);
            await ControlHelper.SetInputByClassAndAriaLabelAsync(complementaryLocator, "azc-input azc-formControl azc-validation-border msportalfx-tooltip-overflow", searchBoxLabel, key, 0);
            await ControlHelper.ClickByClassAndHasTextAsync(CurrentIPage, "ext-search-button fxc-base fxc-simplebutton", "Search", 0, iFrameName: null);
            CommonOperations.WaitMiddleTime();
            int looptimes = 0;
            while (true)
            {
                var name = string.IsNullOrEmpty(actualName) ? key : actualName;
                var treeItemLocator = await ControlHelper.GetByRoleAndNameAsync(complementaryLocator, AriaRole.Treeitem, name);
                var treeItemLocatorCount = await treeItemLocator.CountAsync();
                if (treeItemLocatorCount > 0)
                {
                    await treeItemLocator.ScrollIntoViewIfNeededAsync();
                }

                await ControlHelper.ClickByTreeItemRoleAndNameAsync(CurrentIPage, name, 0, iFrameName: null);
                //wait for loading disappear , such as : Defender - Loading...
                int i = 0;
                while (true)
                {
                    var locator = await ControlHelper.GetLocatorByClassAndHasTextAsync(CurrentIPage, "azc-treeView-node-content", $"{key} - Loading...", 0, null, waitUntilElementExist: false);
                    var count = await locator.CountAsync();
                   // LogHelper.Info($"Waiting loading disappear, count = {count}");
                    if (count == 0 || i > 10)
                    {
                        break;
                    }
                    i++;
                    CommonOperations.WaitShortTime();
                }
             //   LogHelper.Info("Finished waiting loading disappear");
                ILocator showSettingsLocator = null;
                try
                {
                    showSettingsLocator = await ControlHelper.GetLocatorByClassAndHasTextAsync(complementaryLocator, "fxc-gc-nodata-message fxc-gc-text", "Select a category to show settings", 0, waitUntilElementExist:false);
                }
                catch
                {
                  //  LogHelper.Info($"Select a category to show settings is disappear....");
                }
                if (showSettingsLocator != null && await showSettingsLocator.CountAsync() >0)
                {
                   // LogHelper.Info($"Not found the key {key}, try again.");
                    await ControlHelper.ClickByTreeItemRoleAndNameAsync(CurrentIPage, name, 0, iFrameName: null);
                    CommonOperations.WaitShortTime();
                }
                try
                {
                    String searchResult = null;
                    if (key == "Settings")
                    {
                        searchResult = await ControlHelper.GetTextByClassAndTextKeywordAsync(complementaryLocator, "ext-intune-picker-text ext-intune-text-bold", "results in the \"Settings\" category", 0);
                    }
                    else
                    {
                        searchResult = await ControlHelper.GetTextByClassAndTextKeywordAsync(complementaryLocator, "ext-intune-picker-text ext-intune-text-bold", $"{key}", 0);
                    }
                    if (searchResult != null && searchResult.StartsWith("0"))
                    {
                        await ControlHelper.ClickByTreeItemRoleAndNameAsync(CurrentIPage, name, 0, iFrameName: null);
                    }
                    break;
                }
                catch
                {
                   // LogHelper.Info($"Not found the key {key}, try again.");
                    looptimes++;
                    await ControlHelper.ClickByTreeItemRoleAndNameAsync(CurrentIPage, name, 0, iFrameName: null);
                    CommonOperations.WaitShortTime();
                    if (looptimes > 3)
                    {
                       // LogHelper.Info($"Not found the key {key} after looping 3 times, break.");
                        break;
                    }
                }
            }
            //Check all settings has expand node or not
            if (isExpandAllNode)
            {
                await ExpandAllNodeByLocatorAsync(complementaryLocator);
            }

            //Select settings
            foreach (var setting in values)
            {
                //var locator = await ControlHelper.GetByRoleAndHasTextAsync(CurrentIPage, AriaRole.Gridcell, setting, 0, iFrameName: null);
                ILocator? locator = null;
                if (setting.Contains("|") && !setting.Contains("**|"))
                {
                    var settingArray = setting.Split("|");
                    locator = await ControlHelper.GetLocatorByClassAndHasTextAsync(CurrentIPage, "fxc-gc-cell", settingArray[0], 0, hasNotText: settingArray[1]);
                }
                else if (setting.Contains("**|"))
                {
                    locator = await ControlHelper.GetLocatorByClassAndHasTextAsync(CurrentIPage, "fxc-gc-cell", setting.Replace("**|", "|"), 0);
                }
                else
                {
                    locator = await ControlHelper.GetLocatorByClassAndHasTextAsync(CurrentIPage, "fxc-gc-cell", setting, 0);
                }
                var checkBox = await ElementHelper.GetByCheckboxRoleAsync(locator);
                var status = await checkBox.Nth(0).IsCheckedAsync();
                await checkBox.Nth(0).ScrollIntoViewIfNeededAsync();
                if (!status)
                {
                    await checkBox.Nth(0).ClickAsync();
                }
            }
            //Close content 'Settings picker'
            await ClickCloseContentBtnAsync(CurrentIPage, "Settings picker");
        }

        private async Task SelectBySearchWithKeywordThenClickAddAndSaveAsync(string title, string searchBoxLabel, List<string> keywords, string titleContainerClass = "ms-Panel-contentInner", string iFrameName = null)
        {
            var pannelContainerLocator = await ControlHelper.GetLocatorByClassAndHasTextAsync(CurrentIPage, titleContainerClass, title, 0, iframeName: iFrameName);
            foreach (string keyword in keywords)
            {
                await ControlHelper.SetInputByClassAndAriaLabelAsync(pannelContainerLocator, "ms-SearchBox-field", searchBoxLabel, keyword, 0);
                var rows = await ControlHelper.GetLocatorByRoleAndHasTextAsync(pannelContainerLocator, AriaRole.Row, keyword);
                var count = await rows.CountAsync();
                if (count == 0)
                {
                    throw new Exception($"\"{keyword}\" not exist...");
                }
                else
                    await ControlHelper.ClickByClassAndHasTextAsync(pannelContainerLocator, "ms-Link", "Add", 0);
            }
            await ControlHelper.ClickByClassAndHasTextAsync(this.CurrentIPage, "ms-Button ms-Button--primary", "Save", 0, iFrameName: iFrameName);

        }

        public string GetCurrentLanguageText(string key)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

}
