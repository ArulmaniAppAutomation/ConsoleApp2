//using LogService.Extension;
using Microsoft.Playwright;
using PlaywrightTests.Common.Helper;
using PlaywrightTests.Common.Model;
using PlaywrightTests.Common.Utils.BaseUtils.UtilInterface;

namespace PlaywrightTests.Common.Utils.BaseUtils.PopUp
{
    public class UploadFileUtils : BaseCommonUtils, InterfaceUtils
    {
        public UploadFileUtils(IPage page, string env) : base(page, env)
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
            switch (controlInfo.Operation)
            {
                case "UploadFile":
                    await UploadFileAsync(controlInfo.OperationValue);
                    break;
                case "ClickOKButtonAsync":
                    await ClickOKButtonAsync();
                    break;
                default:
                    await BaseExecuteStepAsync(controlInfo.Operation);
                    break;
            }
            return (true, controlInfo.Parameter);
        }
        public async Task UploadFileAsync(string fileName, string placeholder = "Select a file")
        {
            if (string.IsNullOrEmpty(fileName))
            {
              //  throw new CustomLogException($"Upload file name is null, please set the right value!");
            }
            var uploadFilePath = CommonOperations.GetAllShareFolderFiles().Select(t => t.Trim('\n')).Where(t => !string.IsNullOrEmpty(t)).Where(t => t.Contains(fileName)).FirstOrDefault();
            if (string.IsNullOrEmpty(uploadFilePath))
            {
               // throw new CustomLogException($"Can not find the target file, please ensure the file exists in ShareFiles folder");
            }
            await ControlHelper.UploadFilesAsync(CurrentIPage, placeholder, uploadFilePath);            
        }

        private async Task ClickOKButtonAsync()
        {
            await ControlHelper.ClickByClassAndHasTextAsync(CurrentIPage, "fxs-button fxt-button fxs-inner-solid-border fxs-portal-button-primary", "OK",0,iFrameName:null);
        }

        public string GetCurrentLanguageText(string key)
        {
            throw new NotImplementedException();
        }
    }
}
