using Microsoft.Playwright;
using PlaywrightTests.Common.Helper;
using PlaywrightTests.Common.Model;
using PlaywrightTests.Common.Utils.BaseUtils.UtilInterface;

namespace PlaywrightTests.Common.Utils.BaseUtils.PopUp
{
    public class SelectGroupUtils : BaseCommonUtils, InterfaceUtils
    {
        private string IFrameName = "ObjectPicker";
        public SelectGroupUtils(IPage page, string env) : base(page, env)
        {
        }

        public Task RunAsync()
        {
            throw new NotImplementedException();
        }

        public Task ClearDataAsync()
        {
            throw new NotImplementedException();
        }

        public async Task GoToMainPageAsync()
        {
            throw new NotImplementedException();
        }
        public Task CheckIfCurrentPageIsAvailableAsync()
        {
            throw new NotImplementedException();
        }
       public async Task<(bool IsContinue, Dictionary<string, string> Parameter)> RunStepAsync(ControlInfo controlInfo)
        {
            throw new NotImplementedException();
        }
        public async Task SelectGroupAsync(string groupName)
        {
            BaseThreadSleepLong();
            await SetSearchBoxValueAsync(groupName);
            await SelectTheGroupAsync(groupName);
            await ClickSelectBtnAsync(iFrameName: IFrameName);
        }
        private async Task SetSearchBoxValueAsync(string groupName)
        {
            await ControlHelper.SetInputByClassAndAriaLabelAsync(CurrentIPage, "ms-SearchBox-field", "Search", groupName, 0, iFrameName: IFrameName);
        }
        private async Task SelectTheGroupAsync(string groupName)
        {
            var rowParentLocator = await ControlHelper.GetLocatorByClassAndHasTextAsync(CurrentIPage, "ms-List-cell", groupName, 0, iframeName: IFrameName);
            await ControlHelper.SelectCheckBoxByNthAsync(rowParentLocator, 0);
        }

        public Task ClearSpecialDataAsync(string name)
        {
            throw new NotImplementedException();
        }

        public string GetCurrentLanguageText(string key)
        {
            throw new NotImplementedException();
        }
    }

}
