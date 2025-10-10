using Microsoft.Playwright;
using PlaywrightTests.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account_Management.CommonBase
{
    public class SelectGroupUtills
    {
        public static IPage _page;
        private readonly string _portalUrl;
       // private BaseCommonUtils base1 = new BaseCommonUtils(_page, _portalUrl);

        private string IFrameName = "ObjectPicker";
        public SelectGroupUtills(IPage page, string portalUrl)
        {

            _page = page;              
            _portalUrl = portalUrl;
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
        public async Task<(bool IsContinue, Dictionary<string, string> Parameter)> RunStepAsync()
        {
            throw new NotImplementedException();
        }
        public async Task SelectGroupAsync(string groupName)
        {
         //   BaseThreadSleepLong();
            await SetSearchBoxValueAsync(groupName);
            await SelectTheGroupAsync(groupName);
            //await base1.ClickSelectBtnAsync(iFrameName: IFrameName);
        }
        private async Task SetSearchBoxValueAsync(string groupName)
        {
            await ControlHelper.SetInputByClassAndAriaLabelAsync(_page, "ms-SearchBox-field", "Search", groupName, 0, iFrameName: IFrameName);
        }
        private async Task SelectTheGroupAsync(string groupName)
        {
            var rowParentLocator = await ControlHelper.GetLocatorByClassAndHasTextAsync(_page, "ms-List-cell", groupName, 0, iframeName: IFrameName);
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

