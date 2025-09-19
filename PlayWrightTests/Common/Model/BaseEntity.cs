using Microsoft.Playwright;
using static PlaywrightTests.Common.Helper.EnumHelper;

namespace PlaywrightTests.Common.Model
{
    public class BaseEntity
    {

        public string TestCaseLink { get; set; } = string.Empty;
        public string TestCaseId
        {
            get
            {
                if (!string.IsNullOrEmpty(this.TestCaseLink))
                    return TestCaseLink.Split("/")[TestCaseLink.Split("/").Length - 1];
                return string.Empty;
            }
        }

        public string Name { get; set; } = string.Empty;
        public string CaseName { get; set; } = string.Empty;
        public string? Type { get; set; }
        public IPage? Ipage { get; set; }
        public IBrowser IBrowser { get; set; }


        public string ExtensionPageVersion { get; set; } = string.Empty;
        #region For AppAutomation 
        public string AppAutomationBrowserOpen { get; set; } = StepResultStatus.Failed.ToString();
        public string AppAutomationMenuItem { get; set; } = string.Empty;
        public string AppAutomationCaseName { get; set; } = string.Empty;
        public string AppAutomationAppName { get; set; } = string.Empty;
        public string AppAutomationType { get; set; } = string.Empty;
        public string AppAutomationAppCreation { get; set; } = StepResultStatus.Failed.ToString();
        public string AppAutomationAppAssignment { get; set; } = string.Empty;
        public string AppAutomationAPPDependency { get; set; } = string.Empty;
        public string AppAutomationAPPSupersede { get; set; } = string.Empty;
        public string AppAutomationAppUpdated { get; set; } = string.Empty;
        public string AppAutomationVerifyResult { get; set; } = StepResultStatus.Failed.ToString();
        public string AppAutomationAppDelete { get; set; } = StepResultStatus.Failed.ToString();
        public string AppAutomationAppURL { get; set; } = string.Empty;
        public string AppAutomationAppPFN { get; set; } = string.Empty;
        public string AppAutomationDownLoadEnv { get; set; } = string.Empty;
        public string AppAutomationFileUrl { get; set; } = string.Empty;
        #endregion
    }

}
