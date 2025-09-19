using Microsoft.Playwright;
using PlaywrightTests.Common.Helper;
using PlaywrightTests.Common.Model;
using PlaywrightTests.Common.Utils.BaseUtils.UtilInterface;

namespace PlaywrightTests.Common.Utils.BaseUtils.PopUp
{
    public class AssignmentGroupFilterUtils : BaseCommonUtils, InterfaceUtils
    {
        public AssignmentGroupFilterUtils(IPage page, string env) : base(page, env)
        {
        }

        public Task CheckIfCurrentPageIsAvailableAsync()
        {
            throw new NotImplementedException();
        }

        public Task ClearDataAsync()
        {
            throw new NotImplementedException();
        }

        public Task ClearSpecialDataAsync(string name)
        {
            throw new NotImplementedException();
        }

        public string GetCurrentLanguageText(string key)
        {
            throw new NotImplementedException();
        }

        public Task GoToMainPageAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<(bool IsContinue, Dictionary<string, string> Parameter)> RunStepAsync(ControlInfo controlInfo)
        {
            switch (controlInfo.Operation)
            {
                case "SelectBehaveAsync":
                    await SelectBehaveAsync(controlInfo.OperationValue);
                    break;
                case "SelectSpecialFilterAsync":
                    await SelectSpecialFilterAsync(controlInfo.OperationValue);
                    break;
                default:
                    await BaseExecuteStepAsync(controlInfo.Operation);
                    break;
            }
            return (true, controlInfo.Parameter);
        }

        #region business
        private async Task SelectSpecialFilterAsync(string name)
        {
            await SetSearchBoxAsync(name);
            await SelectFilterAsync(name);
            await ClickSelectButtonAsync();
        }
        #endregion

        #region private method
        private async Task<ILocator> GetFilterBladeLocatorAsync()
        {
            return await ControlHelper.GetLocatorByClassAndHasTextAsync(this.CurrentIPage, "fxs-part-content fxs-validationContainer", "Apply a filter to include or exclude certain devices from this assignment.", 0, iframeName: null);
        }
        private async Task SelectBehaveAsync(string behave)
        {
            await ControlHelper.ClickByClassAndHasTextAsync(await GetFilterBladeLocatorAsync(), "fxs-portal-border azc-optionPicker-item", behave, 0);
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
        #endregion

    }
}
