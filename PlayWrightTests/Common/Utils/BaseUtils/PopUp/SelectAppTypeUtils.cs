//using LogService.Extension;
using Microsoft.Playwright;
using PlaywrightTests.Common.Helper;
using PlaywrightTests.Common.Model;
using PlaywrightTests.Common.Utils.BaseUtils.UtilInterface;

namespace PlaywrightTests.Common.Utils.BaseUtils.PopUp
{
    public class SelectAppTypeUtils : BaseCommonUtils, InterfaceUtils
    {
        public SelectAppTypeUtils(IPage page, string env) : base(page, env)
        {
        }

        public Task ClearDataAsync()
        {
            throw new NotImplementedException();
        }

        public Task ClearSpecialDataAsync(string name)
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
        public async Task<(bool IsContinue, Dictionary<string, string> Parameter)> RunStepAsync(ControlInfo controlInfo)
        {
            switch (controlInfo.Operation)
            {
                case "SelectAppTypeAsync":
                    var typeCategory = controlInfo.Value != null && controlInfo.Value.Count > 1 ? controlInfo.Value[1] : null;
                    await SelectAppTypeAsync(controlInfo.OperationValue, typeCategory);
                    break;
                default:
                    await BaseExecuteStepAsync(controlInfo.Operation);
                    break;
            }
            return (true, controlInfo.Parameter);
        }

        public async Task SelectAppTypeAsync(string? appType, string? typeCategory = null)
        {
            if (string.IsNullOrEmpty(appType))
            {
               // throw new CustomLogException($"appType is null, please set the right value!");
            }
            if (!string.IsNullOrEmpty(typeCategory))
            {
                await ControlHelper.SetComBoxRoleTreeItemRoleValueAsync(CurrentIPage, "App Type", typeCategory, appType, 0, iFrameName: null);
            }
            else
            {
                await ControlHelper.SetComBoxRoleTreeItemRoleValueAsync(CurrentIPage, "App Type", appType, 0, iFrameName: null);
            }
            await ClickSelectBtnAsync();
        }

        public string GetCurrentLanguageText(string key)
        {
            throw new NotImplementedException();
        }
    }
}
