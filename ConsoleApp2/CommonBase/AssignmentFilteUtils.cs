using Microsoft.Playwright;
using PlaywrightTests.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account_Management.CommonBase
{
    public class AssignmentFilteUtils
    {
        public static IPage _page;
        private readonly string _portalUrl;
        public string? IFrameName = null;


        public static String SelectBehave = "Exclude filtered devices in assignment";
        public static String SelectSpecialFilter = "Exculde";

        private async Task SelectBehaveAsync(string SelectBehave)
        {
            await ControlHelper.ClickByClassAndHasTextAsync(await GetFilterBladeLocatorAsync(), "fxs-portal-border azc-optionPicker-item", SelectBehave, 0);
        }
        private async Task<ILocator> GetFilterBladeLocatorAsync()
        {
            return await ControlHelper.GetLocatorByClassAndHasTextAsync(_page, "fxs-part-content fxs-validationContainer", "Apply a filter to include or exclude certain devices from this assignment.", 0, iframeName: null);
        }
        private async Task SelectSpecialFilterAsync(string name)
        {
            await SetSearchBoxAsync(name);
            await SelectFilterAsync(name);
            await ClickSelectButtonAsync();
        }

        private async Task SetSearchBoxAsync(string name)
        {
            await ControlHelper.SetInputByClassAndPlaceholderAsync(await GetFilterBladeLocatorAsync(), "azc-input azc-formControl", "Search by name", name, 0);
        }

        private async Task SelectFilterAsync(string name)
        {
            await ControlHelper.ClickByClassAndHasTextAsync(await GetFilterBladeLocatorAsync(), "azc-grid-cellContent", name, 0);
        }

        private async Task ClickSelectButtonAsync()
        {
            await ControlHelper.ClickByClassAndHasTextAsync(await GetFilterBladeLocatorAsync(), "fxs-button fxt-button ", "Select", 0);
        }

    
    }
}
