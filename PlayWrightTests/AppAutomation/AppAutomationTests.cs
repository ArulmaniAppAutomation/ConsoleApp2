using Account_Management;
using System;
using PlaywrightTests.Common.Model;
using PlaywrightTests.Common.SettingsPage;
using PlaywrightTests.Common.Utils.FeatureUtils;

namespace PlaywrightTests.TestCases.AppAutomation
{
    public class AppAutomationTests : SettingsPageBase
    {
        public AppAutomationEntity appAutomationEntity { get; set; }
        private AccountsFile loginAccountInfo { get; set; }
        public AppAutomationTests(AccountsFile account, AppAutomationEntity entity) : base(account, entity)
        {
            appAutomationEntity = entity;
            loginAccountInfo = account;
        }
        public override async Task RunAsync()
        {
            try
            {            
                CommonFeatureMainUtils commonFeatureMainUtils = new CommonFeatureMainUtils(appAutomationEntity.Ipage, loginAccountInfo.Environment);
                await commonFeatureMainUtils.RunAsync(commonFeatureMainUtils, appAutomationEntity);
            }
            catch (Exception ex)
            {
                throw;
            }
           
            finally
            {
                base.Dispose();
            }
        }
    }
}
