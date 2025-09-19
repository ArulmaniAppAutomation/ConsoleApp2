using Microsoft.Playwright;
using PlaywrightTests.Common.Helper;
using PlaywrightTests.Common.Model;
using PlaywrightTests.Common.Utils.BaseUtils.PopUp;
using PlaywrightTests.Common.Utils.BaseUtils.UtilInterface;
using static PlaywrightTests.Common.Helper.EnumHelper;

namespace PlaywrightTests.Common.Utils.BaseUtils.TenantAdministration.ConnectorsAndTokens.Windows
{
    public class WindowsEnterpriseCertificateUtils : BaseCommonUtils, InterfaceUtils
    {
        public WindowsEnterpriseCertificateUtils(IPage page, string env) : base(page, env)
        {

        }
        public Task ClearDataAsync()
        {
            throw new NotImplementedException();
        }
        public Task ClearSpecialDataAsync(string name)
        {
            throw new NotImplementedException();
        }
        public async Task GoToMainPageAsync()
        {
            await RefreshCurrentPageAsync();
            await siteBar.ClickTenantAdministrationAsync();
            await siteBarMenu.ClickConnectorsAndTokensAsync();
        }
        public Task CheckIfCurrentPageIsAvailableAsync()
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
                case "UploadCodeSigningCertificateFileAsync":
                    await UploadCodeSigningCertificateFileAsync(controlInfo.OperationValue);
                    break;
                case "ClickUploadButtonAsync":
                    await ClickUploadButtonAsync();
                    break;
                case "VerifyCertificateUploadedSuccessWithNotificationAsync":
                    await VerifyCertificateUploadedSuccessWithNotificationAsync();
                    break;
                case "initAppAutomationVerifyResult":
                    DictionaryItemProcess(controlInfo.Parameter, "AppAutomationVerifyResult", StepResultStatus.Failed.ToString());
                    break;
                case "SuccessAppAutomationVerifyResult":
                    DictionaryItemProcess(controlInfo.Parameter, "AppAutomationVerifyResult", StepResultStatus.Success.ToString());
                    break;
                default:
                    await BaseExecuteStepAsync(controlInfo.Operation);
                    break;
            }
            return (true, controlInfo.Parameter);
        }

        #region Private Method
        private async Task UploadCodeSigningCertificateFileAsync(string fileName)
        {
            UploadFileUtils uploadFileUtils = new UploadFileUtils(this.CurrentIPage, this.CurrentEnv);
            await uploadFileUtils.UploadFileAsync(fileName);
        }
        private async Task ClickUploadButtonAsync()
        {
            await ControlHelper.ClickByClassAndHasTextAsync(this.CurrentIPage, "fxs-button-text", "Upload", 0);
        }
        private async Task VerifyCertificateUploadedSuccessWithNotificationAsync()
        {
            string regexText = "Windows Enterprise Certificate uploaded.";
            await BaseVerifyWithNotificationAsync(regexText);
        }

        public string GetCurrentLanguageText(string key)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
