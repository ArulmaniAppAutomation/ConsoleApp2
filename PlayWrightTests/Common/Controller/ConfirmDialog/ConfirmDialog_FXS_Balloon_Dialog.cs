using Microsoft.Playwright;
using PlaywrightTests.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaywrightTests.Common.Controller.ConfirmDialog
{
    public class ConfirmDialog_FXS_Balloon_Dialog : BaseController, IConfirmDialog
    {
        private string BladeDialogContainText = "?";
        private ILocator FXS_Blade_Dialog_Locator {
            get 
            {
                return GetFXSBalloonDialogLocator();
            }
        }
        public ConfirmDialog_FXS_Balloon_Dialog(IPage page, string? frameName,EnumHelper.Language language) : base(page, frameName,language)
        {
          
        }
        #region public function
        public Task ClickDialogYesButtonAsync()
        {
            throw new NotImplementedException();
        }
        public Task ClickDialogNoButtonAsync()
        {
            throw new NotImplementedException();
        }
        public Task ClickDialogDeleteButtonAsync()
        {
            throw new NotImplementedException();
        }
        public async Task ClickDialogOKButtonAsync()
        {
            await ControlHelper.ClickByClassAndHasTextAsync(FXS_Blade_Dialog_Locator, "fxs-button", "OK", 0);
        }
        public Task ClickDialogCancelButtonAsync()
        {
            throw new NotImplementedException();
        }
        #endregion
        #region private function
        private ILocator GetFXSBalloonDialogLocator()
        {
            return ControlHelper.GetLocatorByClassAndHasTextAsync(this.CurrentIPage, "fxc-base azc-control azc-balloon azc-balloon-forcedisplayblock fxs-dialogballoon fxs-portal-bg-txt-br azc-balloon-dialog", BladeDialogContainText, 0, iframeName: this.CurrentIFrameName).Result;
        }

        public Task<ILocator> GetDialogButtonLocatorByNameAsync(string buttonName)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
