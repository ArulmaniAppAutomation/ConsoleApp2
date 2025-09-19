using PlaywrightTests.Common.Model;
using PlaywrightTests.Common.Utils.BaseUtils;

namespace PlaywrightTests.Common.Utils.BaseUtils.UtilInterface
{
    public interface InterfaceUtils
    {
        Task RunAsync<T>(T sonUtils, CommonEntity entity) where T : BaseCommonUtils, InterfaceUtils;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controlInfo"></param>
        /// <returns>Whether to continue the execution, false is not to continue, true is to continue</returns>
        Task<(bool IsContinue,Dictionary<string,string> Parameter)> RunStepAsync(ControlInfo controlInfo);
        Task ClearDataAsync();
        Task ClearSpecialDataAsync(string name);
        Task GoToMainPageAsync();
        Task CheckIfCurrentPageIsAvailableAsync();
        string GetCurrentLanguageText(string key);
    }
}
