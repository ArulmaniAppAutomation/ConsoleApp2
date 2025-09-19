using Microsoft.Playwright;
using PlaywrightTests.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaywrightTests.Common.Controller.ConfirmDialog
{
    public class ConfirmDialog_FXS_Blade_Dialog : BaseController, IConfirmDialog
    {
        private string BladeDialogContainText = "?";
        private ILocator FXS_Blade_Dialog_Locator {
            get 
            {
                return GetFXSBladeDialogLocator();
            }
        }
        public ConfirmDialog_FXS_Blade_Dialog(IPage page, string? frameName,EnumHelper.Language language) : base(page, frameName,language)
        {
          
        }
        #region public function
        public async Task ClickDialogYesButtonAsync()
        {
            await ControlHelper.ClickByClassAndHasTextAsync(FXS_Blade_Dialog_Locator, "fxs-button-text", "Yes", 0);
        }
        public async Task ClickDialogNoButtonAsync()
        {
            await ControlHelper.ClickByClassAndHasTextAsync(FXS_Blade_Dialog_Locator, "fxs-button-text", "No", 0);
        }
        public Task ClickDialogDeleteButtonAsync()
        {
            throw new NotImplementedException();
        }
        public async Task ClickDialogOKButtonAsync()
        {
            await ControlHelper.ClickByClassAndHasTextAsync(FXS_Blade_Dialog_Locator, "fxs-button-text", "OK", 0);
        }
        public Task ClickDialogCancelButtonAsync()
        {
            throw new NotImplementedException();
        }
        #endregion
        #region private function
        private ILocator GetFXSBladeDialogLocator()
        {
            return ControlHelper.GetLocatorByClassAndHasTextAsync(this.CurrentIPage, "fxs-messagebox fxs-popup", BladeDialogContainText, 0, iframeName: this.CurrentIFrameName).Result;
        }

        public Task<ILocator> GetDialogButtonLocatorByNameAsync(string buttonName)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
