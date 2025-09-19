using Microsoft.Playwright;
using PlaywrightTests.Common.Model;
using PlaywrightTests.Common.Utils.BaseUtils;
using PlaywrightTests.Common.Utils.BaseUtils.UtilInterface;

namespace PlaywrightTests.Common.Utils.FeatureUtils
{
    public class CommonFeatureMainUtils : BaseCommonUtils, InterfaceUtils
    {
        public CommonFeatureMainUtils(IPage page, string env) : base(page, env)
        {
        }

        public Task ClearDataAsync()
        {
            return Task.CompletedTask;
        }

        public Task ClearSpecialDataAsync(string name)
        {
            return Task.CompletedTask;
        }

        public Task GoToMainPageAsync()
        {
            return Task.CompletedTask;
        }
        public Task CheckIfCurrentPageIsAvailableAsync()
        {
            throw new NotImplementedException();
        }
       public async Task<(bool IsContinue, Dictionary<string, string> Parameter)> RunStepAsync(ControlInfo controlInfo)
        {
            return (true, controlInfo.Parameter);
        }

        public string GetCurrentLanguageText(string key)
        {
            throw new NotImplementedException();
        }
    }
}
