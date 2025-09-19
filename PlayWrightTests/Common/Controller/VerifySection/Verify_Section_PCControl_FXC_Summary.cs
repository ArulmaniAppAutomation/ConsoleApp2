using LogService;
using LogService.Extension;
using Microsoft.Playwright;
using PlaywrightTests.Common.Helper;

namespace PlaywrightTests.Common.Controller.VerifySection
{
    public class Verify_Section_PCControl_FXC_Summary : BaseController, IVerifySection
    {
        private string header;
        private string? node;
        public Verify_Section_PCControl_FXC_Summary(IPage? page, string header, string? frameName, EnumHelper.Language language, ILocator? parentLocator = null, string? node = null) : base(page, frameName, language, parentLocator)
        {
            this.header = header;
            this.node = node;
        }
        public void SetNode(string node)
        {
            this.node = node;
        }
        private ILocator SectionLocator
        {
            get
            {
                return GetSectionLocatorAsync().Result;
            }
        }
        public async Task VerifyBasedOnLabelAsync(string settingName, string? value = null, string? hasNotText = null, int nth = 0)
        {
            if (!string.IsNullOrEmpty(value) && value != "Not Configured" && !value.Contains("DisappearNode:"))
            {
                if (value.ToLower() == "inputclear")
                {
                    value = "";
                }
                await VerifySettingSectionAsync(settingName, value, nth);
            }
            else if (!string.IsNullOrEmpty(value) && value.Contains("DisappearNode:"))
            {
                await VerifyNodeDisappearValueAsync(value.Replace("DisappearNode:", ""));
            }
            else
            {
                await VerifySettingsDisappearValueAsync(settingName, hasNotText: hasNotText);
            }
        }

        public async Task VerifyBasededOnValueAsync(string existSetting, string value)
        {
            if (!string.IsNullOrEmpty(value) && value.Contains("DisappearNode:"))
            {
                await VerifyNodeDisappearValueAsync(value.Replace("DisappearNode:", ""));
            }
            else
            {
                ILocator subSettingsLocator = await ControlHelper.GetLocatorByClassAndHasTextAsync(SectionLocator, "fxc-accordion-panel", existSetting, 0);
                var text = await subSettingsLocator.InnerTextAsync();
                LogHelper.Info(text);
                bool isContains = text.Contains(value.Replace("Disappear:", ""));
                if (!string.IsNullOrEmpty(value) && !value.Contains("Disappear:"))
                {
                    Assert.IsTrue(isContains);
                }
                else
                {
                    Assert.IsFalse(isContains);
                }
            }
            ConsoleHelper.ColoredResult(ConsoleColor.Green, $"Verify setting '{existSetting}' pass...");
        }
        public async Task VerifyBasedOnIdAsync(string id, string settingName, string? value = null)
        {
            var groupLocator = await ControlHelper.GetLocatorByIDAsync(SectionLocator, id, 0);
            var ActualSettingValue = await groupLocator.InnerTextAsync();
            if (!string.IsNullOrEmpty(value))
            {
                LogHelper.Info($"Settings \"{settingName}\" expect value: {value}");
                Assert.That(ActualSettingValue.Contains(value), Is.True);
                Assert.That(ActualSettingValue.Contains(settingName), Is.True);
                ConsoleHelper.ColoredResult(ConsoleColor.Green, $"Setting '{settingName}: {value}' verify pass...");
            }
            else
            {
                LogHelper.Info($"Actual value on page is \"{ActualSettingValue}\", expect disappear setting is: {settingName}");
                Assert.That(ActualSettingValue.Contains(settingName), Is.False);
                ConsoleHelper.ColoredResult(ConsoleColor.Green, $"Disappear setting '{settingName}' verify pass...");
            }
        }
        public async Task VerifyBasedOnLabelAndValueAsync(string setting, string? value = null)
        {
            if (!string.IsNullOrEmpty(value) && value.Contains("DisappearNode:"))
            {
                await VerifyNodeDisappearValueAsync(value.Replace("DisappearNode:", ""));
            }
            else if (string.IsNullOrEmpty(value))
            {
                await VerifySettingsDisappearValueAsync(setting);
            }
            else
            {
                var text = await SectionLocator.InnerTextAsync();
                bool isContains = text.Contains($"{setting}\n{value.Replace("Disappear:", "")}");
                if (!string.IsNullOrEmpty(value) && !value.Contains("Disappear:"))
                {
                    Assert.IsTrue(isContains);
                }
                else
                {
                    Assert.IsFalse(isContains);
                }
            }
            ConsoleHelper.ColoredResult(ConsoleColor.Green, $"Verify setting '{setting}' pass...");
        }
        public async Task VerifyAssignmentsGroupsAsync(string groupName, string labelName = "Included groups")
        {
            ILocator includedGroupsLocator = await ControlHelper.GetLoatorByAriaLabelAsync(SectionLocator, labelName, 0);
            var result = await ControlHelper.ClickByClassAndHasTextAsync(includedGroupsLocator, "fxc-gc-text", groupName, 0, isClick: false);
            Assert.That(result.locatorCount.Equals(1), Is.True);
            ConsoleHelper.ColoredResult(ConsoleColor.Green, $"Assignment group {groupName} verify pass...");
        }
        public async Task VerifyBasicsSectionAsync(string labelName, string value)
        {
            try
            {
                if (labelName.Contains("(") || labelName.Contains(")"))
                {
                    labelName = labelName.Replace("(", "\\(").Replace(")", "\\)");
                }
                if (value.Contains("(") || labelName.Contains(")"))
                {
                    value = value.Replace("(", "\\(").Replace(")", "\\)");
                }
                var targetValueLocator = await ControlHelper.GetLocatorByClassAndRegexTextAsync(SectionLocator, "fxc-summary-item-row", $"{labelName}\\s*{value}");
                var status = await targetValueLocator.IsVisibleAsync();
                LogHelper.Info($"target value locator status: {status}");
                Assert.That(status, Is.True);
                ConsoleHelper.ColoredResult(ConsoleColor.Green, $"\"{labelName}\":\"{value}\" verify passed...");
            }
            catch (CustomLogException)
            {

            }
            catch (Exception)
            {
                throw new CustomLogException($"\"{labelName}\":\"{value}\" verify failed...");
            }
        }
        #region private methods    

        public async Task VerifySettingSectionAsync(string settingName, string expectSettingValue, int nth = 0)
        {
            string? ActualSettingValue = await ControlHelper.GetTextByClassAndTextKeywordAsync(SectionLocator, "fxc-summary-item-row", settingName, nth);
            LogHelper.Info($"Settings \"{settingName}\" Actual Value:{ActualSettingValue} expect value: {expectSettingValue}");
            Assert.IsTrue(ActualSettingValue.Contains(expectSettingValue), $"Settings \"{settingName}\" Actual Value:{ActualSettingValue} expect value: {expectSettingValue}");
            ConsoleHelper.ColoredResult(ConsoleColor.Green, $"Setting '{settingName}: {expectSettingValue}' verify pass...");
        }
        public async Task VerifySettingsDisappearValueAsync(string settingName, string? hasNotText = null)
        {
            var result = await ControlHelper.ClickByClassAndHasTextAsync(SectionLocator, "fxc-summary-item-row", settingName, 0, isClick: false, waitUntilElementExist: false, hasNotText: hasNotText);
            Assert.That(result.locatorCount.Equals(0), Is.True);
            ConsoleHelper.ColoredResult(ConsoleColor.Green, $"Disappear setting '{settingName}' verify pass...");
        }
        public async Task VerifyNodeDisappearValueAsync(string node, string? hasNotText = null)
        {
            LogHelper.Info($"Verify the node \"{node}\" disappeared in the properties page...");
            var result = await ControlHelper.ClickByClassAndHasTextAsync(SectionLocator, "ext-intune-accordion-header-container", node, 0, isClick: false, waitUntilElementExist: false, hasNotText: hasNotText);
            Assert.That(result.locatorCount.Equals(0), Is.True);
            ConsoleHelper.ColoredResult(ConsoleColor.Green, $"Disappear Node '{node}' verify pass...");
        }
        private async Task<ILocator> GetSectionLocatorAsync()
        {
            ILocator locator = null;
            try
            {
                locator = await ControlHelper.GetParentLocatorBySonLocatorAsync(CurrentIPage, "fxc-base fxc-section fxc-section-wrapper", "msportalfx-create-section-label ext-summary-sectionHeader", header);
            }
            catch
            {
                locator = this.CurrentIPage.Locator("body");
            }
            if (node != null)
            {
                var nodeLocator = await ControlHelper.GetLocatorByClassAndHasTextAsync(locator, "fxs-portal-border fxc-accordion-section fxc-accordion-section-expanded", node, 0);
                return nodeLocator;
            }
            else
            {
                return locator;
            }
        }
        #endregion
    }
}
