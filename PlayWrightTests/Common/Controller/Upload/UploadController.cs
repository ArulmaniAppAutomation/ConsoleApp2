using LogService.Extension;
using Microsoft.Playwright;
using PlaywrightTests.Common.Helper;
using PlaywrightTests.Common.Utils;

namespace PlaywrightTests.Common.Controller.Upload
{
    public class UploadController : BaseController
    {
        public UploadController(IPage? page, string? frameName,EnumHelper.Language language, ILocator? parentLocator = null) : base(page, frameName,language, parentLocator)
        {
        }
        public async Task UploadFileAsync(string fileName, string placeholder = "Select a file")
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new CustomLogException($"Upload file name is null, please set the right value!");
            }
            var uploadFilePath = CommonOperations.GetAllShareFolderFiles().Select(t => t.Trim('\n')).Where(t => !string.IsNullOrEmpty(t)).Where(t => t.Contains(fileName)).FirstOrDefault();
            if (string.IsNullOrEmpty(uploadFilePath))
            {
                throw new CustomLogException($"Can not find the target file, please ensure the file exists in ShareFiles folder");
            }
            await ControlHelper.UploadFilesAsync(CurrentIPage, placeholder, uploadFilePath);
        }
    }
}
