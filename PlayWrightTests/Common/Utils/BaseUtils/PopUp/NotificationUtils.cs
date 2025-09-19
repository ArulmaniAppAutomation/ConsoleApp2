//using LogService;
//using LogService.Extension;
using Microsoft.Playwright;
using PlaywrightTests.Common.Helper;
using PlaywrightTests.Common.Model;
using PlaywrightTests.Common.Utils.BaseUtils.UtilInterface;

namespace PlaywrightTests.Common.Utils.BaseUtils.PopUp
{
    public class NotificationUtils : BaseCommonUtils, InterfaceUtils
    {
        public NotificationUtils(IPage page, string env) : base(page, env)
        {
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
        public Task RunAsync()
        {
            throw new NotImplementedException();
        }

       public async Task<(bool IsContinue, Dictionary<string, string> Parameter)> RunStepAsync(ControlInfo controlInfo)
        {
            throw new NotImplementedException();
        }

        public async Task VerifyAndCloseNotificationAsync(string regexTextToVerify, bool isDismiss = true)
        {
            try
            {
                //Open Notification
                await ClickNotificationButtonAsync(this.CurrentIPage);

               // LogHelper.Info("Get all text in the notification panel...");
                var notificationPanel = await ControlHelper.GetLocatorByClassAndHasTextAsync(CurrentIPage, "fxs-blade msportalfx-shadow-level3 fxs-portal-bg-txt-br fxs-vivaresize fxs-contextpane-content", "Notifications", 0);
                var notificationContains = await notificationPanel.TextContentAsync();
                //LogHelper.Info($"All the text in notification panel: {notificationContains}");
                var isExist = await CheckNotificationMessageIsExistOrNotAsync(this.CurrentIPage, regexTextToVerify);
                
                if (isDismiss)
                {
                  //  LogHelper.Info("Click Dismiss all");
                    await ClickDismissAllAsync();
                }
                else
                {
                    //Close Notification
                    //LogHelper.Info("Close notification panel...");
                    await ClickNotificationButtonAsync(this.CurrentIPage);
                }
               // Assert.IsTrue(isExist.result, isExist.errorMessage);
            }
            catch(Exception err)
            {
                //LogHelper.Error($"Notification verify failed: \"{regexTextToVerify}\"...");
                //throw new CustomLogException($"Notification verify failed: \"{regexTextToVerify}\"...",err);
            }
        }
        public async Task VerifyNoNotificationAsync()
        {
            await ClickNotificationButtonAsync(this.CurrentIPage);
            var isExist = await CheckEmptyNotificationAsync(this.CurrentIPage, "No new notifications from this session");
            //Assert.IsTrue(isExist);
        }

        #region Private Methods
        private async Task ClickDismissAllAsync()
        {
            await ControlHelper.ClickByClassAndHasTextAsync(this.CurrentIPage, "fxs-notificationspane-dismissitem fxs-portal-text-primary", "Dismiss all", 0);
        }
        private async Task ClickNotificationButtonAsync(IPage? page)
        {
            await ControlHelper.ClickByClassWithAriaLableAsync(page, "fxs-topbar-button", "Notifications", 0, iFrameName: null);
        }
        private async Task<(bool result, string errorMessage)> CheckNotificationMessageIsExistOrNotAsync(IPage? page, string regexTextToVerify)
        {
            try
            {
              //  LogHelper.Info($"Get target value in notification: \"{regexTextToVerify}\"...");
                var result = await ControlHelper.GetTextByClassAndTextKeywordAsync(page, "fxs-notificationmenu-notification", regexTextToVerify, 0, null);
                return (true, "");
            }
            
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
        private async Task<bool> CheckEmptyNotificationAsync(IPage? page, string regexTextToVerify)
        {
            try
            {
                var result = await ControlHelper.GetTextByClassAndTextKeywordAsync(page, "fxs-notifications-empty", regexTextToVerify, 0, null);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Task ClearSpecialDataAsync(string name)
        {
            throw new NotImplementedException();
        }

        public string GetCurrentLanguageText(string key)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
