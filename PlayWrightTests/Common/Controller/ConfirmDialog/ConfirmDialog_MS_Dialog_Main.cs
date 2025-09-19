using Microsoft.Playwright;
using PlaywrightTests.Common.Helper;

namespace PlaywrightTests.Common.Controller.ConfirmDialog
{
    public class ConfirmDialog_MS_Dialog_Main : BaseController, IConfirmDialog
    {
        private ILocator MS_Dialog_Main_Locator
        {
            get
            {
                return GetMSDialogMainLocator();
            }
        }
        public ConfirmDialog_MS_Dialog_Main(IPage page, string iframeName, EnumHelper.Language language) : base(page, iframeName, language)
        {
        }
        public async Task<ILocator> GetDialogButtonLocatorByNameAsync(string buttonName)
        {
            return await GetDialogButtonLocatorAsync(buttonName);
        }
        public async Task ClickDialogDeleteButtonAsync()
        {
            await ClickDialogButtonAsync("Delete");
        }
        public async Task ClickDialogCancelButtonAsync()
        {
            await ClickDialogButtonAsync("Cancel");
        }
        public async Task ClickDialogYesButtonAsync()
        {
                await ClickDialogButtonAsync("Yes");
        }
        public async Task ClickDialogOKButtonAsync()
        {
            await ClickDialogButtonAsync("OK");
        }

        public Task ClickDialogNoButtonAsync()
        {
            throw new NotImplementedException();
        }
        #region private function
        private ILocator GetMSDialogMainLocator()
        {
            return ControlHelper.GetLocatorByClassAsync(this.CurrentIPage, "ms-Dialog-main", 0, iFrameName: this.CurrentIFrameName).Result;
        }
        private async Task ClickDialogButtonAsync(string buttonName)
        {
            await ControlHelper.ClickByClassAndHasTextAsync(MS_Dialog_Main_Locator, "ms-Button", buttonName, 0);
        }
        private async Task<ILocator> GetDialogButtonLocatorAsync(string buttonName)
        {
            var buttonLocator= await ControlHelper.ClickByClassAndHasTextAsync(MS_Dialog_Main_Locator, "ms-Button", buttonName, 0,isClick:false,waitUntilElementExist:false);
            return buttonLocator.currentLocator;
        }
        #endregion
    }
}
