//using LogService;
//using LogService.Extension;
using Microsoft.Playwright;
//using PlaywrightTests.Common.Utils;
//using PlaywrightTests.Common.Utils.BaseUtils;
using System.IO;

namespace PlaywrightTests.Common.Helper
{
    public class ControlHelper
    {
        /// <summary>
        /// wait for the page loaded
        /// </summary>
        /// <param name="elementLocator">the element you </param>
        /// <returns></returns>
        public static async Task WaitPageLoadedAsync(ILocator elementLocator, int waitingTime = 200, int count = 150)
        {
            try
            {
                var isVisible = await elementLocator.CountAsync();//await elementLocator.First.IsVisibleAsync();
                string pageStatus = isVisible == 0 ? "not done" : "done";
                int i = 1;
                while (i <= count && isVisible == 0)
                {
                    Thread.Sleep(waitingTime); // waiting 200ms each time
                    isVisible = await elementLocator.CountAsync(); //await elementLocator.First.IsVisibleAsync();
                    pageStatus = isVisible == 0 ? "not done" : "done";
            //        LogHelper.Info($"After wait {i * waitingTime}ms, the element loading state: \"{pageStatus}\".");
                    i++;
                }
            }
            catch (Exception ex)
            {
                //     LogHelper.Error(ex.Message);
                
            }
        }
        public static async Task<bool> RetryAsync(int retryCount, Func<Task> Action)
        {
            bool isSuccess = false;
            if (retryCount <= 0)
            {
                retryCount = 3;
            }
            for (int i = 0; i < retryCount; i++)
            {
                try
                {
                    await Action();
                    isSuccess = true;
                    break;
                }
                catch (Exception e)
                {
                //    LogHelper.Error($"Retry {i + 1} times, error message: {e.Message}");
                }
            }
            return isSuccess;
        }
        /*
         * Function name format:{Action:Click/Set/Select/Get(Count/Text/Locator...)}{ElementName:Button/Input...}By{Class/Label/Role....}Async
         */
        #region Get Locator by locator
        public static async Task<ILocator> GetLocatorByLocatorAsync(IPage? page, string className, ILocator byLocator, int nth, string? iFrameName = null)
        {
            ILocator resultLocator;
            if (!string.IsNullOrEmpty(iFrameName))
            {
                var iframeLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                resultLocator = await ElementHelper.GetByClassAndHasLocatorAsync(iframeLocator, className, byLocator);
            }
            else
            {
                resultLocator = await ElementHelper.GetByClassAndHasLocatorAsync(page, className, byLocator);
            }
            return resultLocator.Nth(nth);
        }
        public static async Task<ILocator> GetLocatorByLocatorAsync(ILocator? locator, string className, ILocator byLocator, int nth)
        {
            ILocator resultLocator;
            resultLocator = await ElementHelper.GetByClassAndHasLocatorAsync(locator, className, byLocator);
            return resultLocator.Nth(nth);
        }
        public static async Task<ILocator> GetLocatorByRoleAndHasTextAsync(ILocator? parentLocator, AriaRole role, string hasText)
        {
            ILocator resultLocator;
            resultLocator = await ElementHelper.GetByLocatorAndRoleAndHasTextAsync(parentLocator, role, hasText);
            return resultLocator;
        }
        public static async Task<IReadOnlyList<ILocator>> GetLocatorsByRoleAndHasTextAsync(ILocator? parentLocator, AriaRole role, string hasText)
        {
            return await ElementHelper.GetByLocatorsAndRoleAndHasTextAsync(parentLocator, role, hasText);
        }
        public static async Task<ILocator> GetLocatorByRowheaderByHasTextAsync(IPage page, string hasText, string IframeName)
        {
            var iframeLocator = ElementHelper.GetIFrameLocator(page, IframeName);
            return await ElementHelper.GetRowheaderByHasTextAsync(iframeLocator, hasText);
        }
        public static async Task<ILocator> GetLocatorByLocatorAndTextAsync(ILocator? parentLocator, string hasText, bool IsStrongWait = true)
        {
            ILocator resultLocator;
            resultLocator = await ElementHelper.GetByLocatorAndHasTextAsync(parentLocator, hasText, IsStrongWait: IsStrongWait);
            return resultLocator;
        }
        public static async Task<ILocator> GetLocatorByRoleAndTextAsync(IPage page, AriaRole role, string hasText)
        {
            var locator = await ElementHelper.GetByRoleAndHasTextAsync(page, role, hasText);
            return locator;
        }
        #endregion
        #region Get Locator by attribute Id
        public static async Task<ILocator> GetLocatorByIDAsync(IPage? page, string id, int nth, string? iFrameName = null)
        {
            ILocator locator;
            if (!string.IsNullOrEmpty(iFrameName))
            {
                var iframeLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                locator = await ElementHelper.GetByIDAsync(iframeLocator, id);
            }
            else
            {
                locator = await ElementHelper.GetByIDAsync(page, id);
            }
            return locator.Nth(nth);
        }
        public static async Task<ILocator> GetLocatorByIDAsync(ILocator? locator, string id, int nth)
        {
            ILocator resultLocator;
            resultLocator = await ElementHelper.GetByIDAsync(locator, id);
            return resultLocator.Nth(nth);
        }
        #endregion
        #region Get by attribute title
        public static async Task<ILocator> GetLocatorByTitleAsync(IPage? page, string title, int nth, string? iFrameName = null)
        {
            ILocator locator;
            if (!string.IsNullOrEmpty(iFrameName))
            {
                var iframeLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                locator = await ElementHelper.GetByAttributeTitleAsync(iframeLocator, title);
            }
            else
            {
                locator = await ElementHelper.GetByAttributeTitleAsync(page, title);
            }
            return locator.Nth(nth);
        }
        public static async Task<ILocator> GetLocatorByTitleAsync(ILocator? locator, string title, int nth)
        {
            ILocator resultLocator;
            resultLocator = await ElementHelper.GetByAttributeTitleAsync(locator, title);
            return resultLocator.Nth(nth);
        }
        #endregion
        #region Get Locator
        public static async Task<ILocator> GetLocatorByClassAsync(IPage? page, string className, int nth, string? iFrameName = null, bool waitUntilElementExist = true)
        {
            ILocator locator;
            if (!string.IsNullOrEmpty(iFrameName))
            {
                var iframeLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                locator = await ElementHelper.GetByClassAsync(iframeLocator, className, waitUntilElementExist: waitUntilElementExist);
            }
            else
            {
                locator = await ElementHelper.GetByClassAsync(page, className, waitUntilElementExist: waitUntilElementExist);
            }
            if (nth < 0)
            {
                return locator.Last;
            }

            return locator.Nth(nth);
        }
        public static async Task<IReadOnlyList<ILocator>> GetLocatorListByClassAsync(IPage? page, string className, string? iFrameName = null)
        {
            ILocator locator;
            if (!string.IsNullOrEmpty(iFrameName))
            {
                var iframeLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                locator = await ElementHelper.GetByClassAsync(iframeLocator, className);
            }
            else
            {
                locator = await ElementHelper.GetByClassAsync(page, className);
            }
            return await locator.AllAsync();
        }
        public static async Task<IReadOnlyList<ILocator>> GetLocatorListByClassAsync(ILocator? locator, string className)
        {
            var resultLocator = await ElementHelper.GetByClassAsync(locator, className);
            return await resultLocator.AllAsync();
        }
        public static async Task<ILocator> GetLocatorByClassAsync(ILocator? locator, string className, int nth, bool exact = false, bool waitUntilElementExist = true)
        {
            ILocator resultLocator;
            resultLocator = await ElementHelper.GetByClassAsync(locator, className, exact, waitUntilElementExist: waitUntilElementExist);
            return resultLocator.Nth(nth);
        }
        public static async Task<ILocator> GetLocatorByClassAsync(ILocator? locator, string className, bool exact = false, bool waitUntilElementExist = true)
        {
            ILocator resultLocator;
            resultLocator = await ElementHelper.GetByClassAsync(locator, className, exact, waitUntilElementExist: waitUntilElementExist);
            return resultLocator;
        }
        public static async Task<ILocator> GetLocatorByTextAsync(IPage? page, string text, string? iFrameName = null, bool IsStrongWait = true)
        {
            ILocator resultLocator;
            if (!string.IsNullOrEmpty(iFrameName))
            {
                var iframeLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                resultLocator = ElementHelper.GetByTextInIframe(iframeLocator, text, IsStrongWait: IsStrongWait);
            }
            else
                resultLocator = await ElementHelper.GetByTextAsync(page, text, IsStrongWait: IsStrongWait);
            return resultLocator;
        }
        public static async Task<ILocator> GetLocatorByTextInDiffTabAsync(IPage? page, string tabName, string text, bool exact = false)
        {
            ILocator resultLocator;
            var currentTab = page.GetByLabel(tabName, new() { Exact = exact });
        //    LogHelper.Info($"Wait {tabName} to load...");
            await WaitElementEnabledAsync(currentTab);
            var currentTabStatus = await currentTab.IsVisibleAsync();
            //  LogHelper.Info($"{currentTab} status: {currentTabStatus}");
            if (!currentTabStatus)
                throw new InvalidOperationException();
            else
                resultLocator = await ElementHelper.GetByTextAsync(currentTab, text, exact: exact, IsStrongWait: false);

            return resultLocator;
        }
        public static async Task<ILocator> GetLoatorByAriaLabelAsync(IPage? page, string ariaLabel, int nth, string? iFrameName = null, bool waitUntilElementExist = true)
        {
            ILocator locator;
            if (!string.IsNullOrEmpty(iFrameName))
            {
                var iframeLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                locator = await ElementHelper.GetByAriaLabelAsync(iframeLocator, ariaLabel, waitUntilElementExist: waitUntilElementExist);
            }
            else
            {
                locator = await ElementHelper.GetByAriaLabelAsync(page, ariaLabel, waitUntilElementExist: waitUntilElementExist);
            }
            return locator.Nth(nth);
        }
        public static async Task<ILocator> GetAllLocatorsByClassAsync(IPage? page, string className, string? iFrameName = null)
        {
            ILocator locator;
            if (!string.IsNullOrEmpty(iFrameName))
            {
                var iframeLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                locator = await ElementHelper.GetByClassAsync(iframeLocator, className);
            }
            else
            {
                locator = await ElementHelper.GetByClassAsync(page, className);
            }
            return locator;
        }
        public static async Task<ILocator> GetAllLocatorsByClassAsync(ILocator? locator, string className, bool wait = true)
        {
            ILocator targetLocator;
            targetLocator = await ElementHelper.GetByClassAsync(locator, className, waitUntilElementExist: wait);
            return targetLocator;
        }
        public static async Task<ILocator> GetLoatorByAriaLabelAsync(ILocator? locator, string ariaLabel, int nth)
        {
            ILocator targetLocator;
            targetLocator = await ElementHelper.GetByAriaLabelAsync(locator, ariaLabel);
            return targetLocator.Nth(nth);
        }
        public static async Task<ILocator> GetLoatorByClassAndAriaLabelAsync(IPage? page, string className, string ariaLabel, int nth = 0, string iframeName = null, bool IsStrongWait = true)
        {
            ILocator locator;
            if (!string.IsNullOrEmpty(iframeName))
            {
                var iframeLocator = ElementHelper.GetIFrameLocator(page, iframeName);
                locator = await ElementHelper.GetByClassAndAriaLableAsync(iframeLocator, className, ariaLabel, IsStrongWait: IsStrongWait);
            }
            else
            {
                locator = await ElementHelper.GetByClassAndAriaLableAsync(page, className, ariaLabel, IsStrongWait: IsStrongWait);
            }
            if (nth < 0)
            {
                return locator.Last;
            }
            else
            {
                return locator.Nth(nth);
            }
        }
        public static async Task<ILocator> GetLoatorByClassAndAriaLabelAsync(ILocator? page, string className, string ariaLabel, int nth = 0)
        {
            ILocator locator;
            locator = await ElementHelper.GetByClassAndAriaLableAsync(page, className, ariaLabel);
            if (nth < 0)
            {
                return locator.Last;
            }
            else
            {
                return locator.Nth(nth);
            }
        }
        public static async Task<ILocator> GetLocatorByClassAndHasTextAsync(IPage? page, string className, string hasText, int nth, string? iframeName = null, string? hasNotText = null, bool waitUntilElementExist = true, bool exact = false)
        {
            ILocator locator;
            if (!string.IsNullOrEmpty(iframeName))
            {
                var iframeLocator = ElementHelper.GetIFrameLocator(page, iframeName);
                locator = await ElementHelper.GetByClassAndHasTextAsync(iframeLocator, className, hasText, hasNotText: hasNotText, waitUntilElementExist: waitUntilElementExist, exact: exact);
            }
            else
            {
                locator = await ElementHelper.GetByClassAndHasTextAsync(page, className, hasText, hasNotText: hasNotText, waitUntilElementExist: waitUntilElementExist);
            }
            if (nth < 0)
            {
                return locator.Last;
            }
            return locator.Nth(nth);
        }
        public static async Task<ILocator> GetLocatorByClassAndRegexTextAsync(ILocator? locator, string className, string regexText, string? hasNotText = null, int nth = 0)
        {
            ILocator resultLocator;
            resultLocator = await ElementHelper.GetByClassAndRegexTextAsync(locator, className, regexText, hasNotText: hasNotText);
            if (nth < 0)
            {
                return resultLocator.Last;
            }
            else
            {
                return resultLocator.Nth(nth);
            }
        }
        public static async Task<ILocator> GetLocatorByClassAndRegexTextAsync(IPage? page, string className, string regexText, string? iframeName = null, string? hasNotText = null, int nth = 0)
        {
            ILocator locator;
            if (!string.IsNullOrEmpty(iframeName))
            {
                var iframeLocator = ElementHelper.GetIFrameLocator(page, iframeName);
                locator = await ElementHelper.GetByClassAndRegexTextAsync(iframeLocator, className, regexText, hasNotText: hasNotText, IsStrongWait: false);
            }
            else
            {
                locator = await ElementHelper.GetByClassAndRegexTextAsync(page, className, regexText, hasNotText: hasNotText);
            }
            return locator.Nth(nth);
        }
        public static async Task<ILocator> GetLocatorByClassAndHasTextAsync(ILocator? locator, string className, string hasText, int nth, string? hasNotText = null, bool waitUntilElementExist = true)
        {
            ILocator resultLocator;
            resultLocator = await ElementHelper.GetByClassAndHasTextAsync(locator, className, hasText, hasNotText: hasNotText, waitUntilElementExist: waitUntilElementExist);
            if (nth < 0)
            {
                return resultLocator.Last;
            }
            else
            {
                return resultLocator.Nth(nth);
            }
        }
        public static async Task<ILocator> GetLocatorByClassAndNameAsync(IPage? page, string className, string name, int nth, string? iframeName = null)
        {
            ILocator locator;
            if (!string.IsNullOrEmpty(iframeName))
            {
                var iframeLocator = ElementHelper.GetIFrameLocator(page, iframeName);
                locator = await ElementHelper.GetByClassAndAriaLableAsync(iframeLocator, className, name);
            }
            else
            {
                locator = await ElementHelper.GetByClassAndAriaLableAsync(page, className, name);
            }
            return locator.Nth(nth);
        }
        public static async Task<ILocator> GetLocatorByClassAndNameAsync(ILocator locator, string className, string name, int nth)
        {
            ILocator resultLocator = await ElementHelper.GetByClassAndAriaLableAsync(locator, className, name);
            return resultLocator.Nth(nth);
        }
        public static async Task<ILocator> GetAllLocatorByClassAndNameAsync(ILocator parentLocator, string className, string name)
        {
            ILocator locator = await ElementHelper.GetByClassAndAriaLableAsync(parentLocator, className, name);
            return locator;
        }
        public static async Task<ILocator> GetParentLocatorByChildArialaberAsync(IPage? page, string ariaLable, int lookUpTimes = 1, string? iFrameName = null)
        {
            ILocator locator;
            if (!string.IsNullOrEmpty(iFrameName))
            {
                var iframeLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                locator = await ElementHelper.GetByLableAsync(iframeLocator, ariaLable);
            }
            else
            {
                locator = await ElementHelper.GetByLableAsync(page, ariaLable);
            }
            locator = locator.Nth(0);
            locator = await ElementHelper.GetParentByChildLocatorAsync(locator, lookUpTimes);
            return locator;
        }
        public static async Task<ILocator> GetParentLocatorBySonLocatorAsync(ILocator locator, int lookUpTimes)
        {
            return await ElementHelper.GetParentByChildLocatorAsync(locator, lookUpTimes);
        }
        public static async Task<ILocator> GetParentLocatorBySonLocatorAsync(IPage? page, string parentClassName, string siblingClassName, string siblingText)
        {
            return await ElementHelper.GetParentByChildLocatorAsync(page, parentClassName, siblingClassName, siblingText);
        }
        public static async Task<ILocator> GetByRoleAndHasTextAsync(IPage? page, AriaRole role, string hasText, int nth, string? iFrameName = null, bool waitUntilElementExist = true)
        {
            ILocator locator;
            if (!string.IsNullOrEmpty(iFrameName))
            {
                var iframeLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                locator = await ElementHelper.GetByRoleAndHasTextAsync(iframeLocator, role, hasText, waitUntilElementExist);
            }
            else
            {
                locator = await ElementHelper.GetByRoleAndHasTextAsync(page, role, hasText, waitUntilElementExist);
            }
            return locator.Nth(nth);
        }
        public static async Task<ILocator> GetByRoleAndHasTextAsync(ILocator locator, AriaRole role, string hasText, int nth, bool waitUntilElementExist = true)
        {
            ILocator resultLocator;
            resultLocator = await ElementHelper.GetByRoleAndHasTextAsync(locator, role, hasText, waitUntilElementExist: waitUntilElementExist);
            if (nth < 0)
            {
                return resultLocator.Last;
            }
            return resultLocator.Nth(nth);
        }
        public static async Task<ILocator> GetByRoleAndAriaLabelAsync(IPage? page, AriaRole role, string ariaLabel, int nth, string? iFrameName = null, bool exact = true, bool IsStrongWait = true)
        {
            ILocator locator;
            if (!string.IsNullOrEmpty(iFrameName))
            {
                var iframeLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                locator = await ElementHelper.GetByRoleAndAriaLabelAsync(iframeLocator, role, ariaLabel, exact, waitUntilElementExist: IsStrongWait);
            }
            else
            {
                locator = await ElementHelper.GetByRoleAndAriaLabelAsync(page, role, ariaLabel, exact, waitUntilElementExist: IsStrongWait);
            }
            return locator.Nth(nth);
        }
        public static async Task<ILocator> GetByRoleAndAriaLabelAsync(ILocator locator, AriaRole role, string ariaLabel, int nth, bool waitUntilElementExist = true, bool exact = true)
        {
            ILocator resultLocator;
            resultLocator = await ElementHelper.GetByRoleAndAriaLabelAsync(locator, role, ariaLabel, waitUntilElementExist: waitUntilElementExist, exact);

            if (nth < 0)
            {
                return resultLocator.Last;
            }
            return resultLocator.Nth(nth);
        }
        public static async Task<ILocator> GetByRoleAndNameAsync(IPage? page, AriaRole role, string name, string? iFrameName = null)
        {
            ILocator locator;
            if (!string.IsNullOrEmpty(iFrameName))
            {
                var iframeLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                locator = await ElementHelper.GetByRoleAndNameAsync(iframeLocator, role, name);
            }
            else
            {
                locator = await ElementHelper.GetByRoleAndNameAsync(page, role, name);
            }
            return locator;
        }
        public static async Task<ILocator> GetByRoleAndNameAsync(ILocator parentLocator, AriaRole role, string name)
        {
            var locator = await ElementHelper.GetByRoleAndNameAsync(parentLocator, role, name);
            return locator;
        }

        public static async Task<ILocator> GetByRoleAndClassAsync(IPage? page, AriaRole role, string className, int nth, string? iFrameName = null, bool waitUntilElementExist = true)
        {
            ILocator locator;
            if (!string.IsNullOrEmpty(iFrameName))
            {
                var iframeLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                locator = await ElementHelper.GetByRoleAndAriaClassAsync(iframeLocator, role, className, waitUntilElementExist: waitUntilElementExist);
            }
            else
            {
                locator = await ElementHelper.GetByRoleAndAriaClassAsync(page, role, className, waitUntilElementExist: waitUntilElementExist);
            }
            if (nth < 0)
            {
                return locator.Last;
            }
            return locator.Nth(nth);
        }
        public static async Task<ILocator> GetByRoleAndClassAsync(ILocator locator, AriaRole role, string className, int nth, bool isNeedSleep = true)
        {
            ILocator resultLocator;
            resultLocator = await ElementHelper.GetByRoleAndAriaClassAsync(locator, role, className, isNeedSleep: isNeedSleep);
            if (nth < 0)
            {
                return resultLocator.Last;
            }
            return resultLocator.Nth(nth);
        }
        public static async Task<IReadOnlyList<ILocator>> GetAllLocatorsByRoleAndClassAsync(ILocator locator, AriaRole role, string className, bool isNeedSleep = true)
        {
            var resultLocator = await ElementHelper.GetByRoleAndAriaClassAsync(locator, role, className, isNeedSleep: isNeedSleep);
            return await resultLocator.AllAsync();
        }

        public static async Task<ILocator> GetByLocatorAsync(IPage? page, string locatorStr, int nth, string? iFrameName, bool isNeedSleep = true)
        {
            ILocator locator;
            if (!string.IsNullOrEmpty(iFrameName))
            {
                var iframeLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                locator = await ElementHelper.GetByLocatorAsync(iframeLocator, locatorStr, isNeedSleep: isNeedSleep);
            }
            else
            {
                locator = await ElementHelper.GetByLocatorAsync(page, locatorStr, isNeedSleep: isNeedSleep);
            }
            if (nth < 0)
            {
                return locator.Last;
            }
            else
            {
                return locator.Nth(nth);
            }
        }
        public static async Task<ILocator> GetByLocatorAsync(ILocator locator, string locatorStr, int nth, bool isNeedSleep = true)
        {
            var element = await ElementHelper.GetByLocatorAsync(locator, locatorStr, isNeedSleep: isNeedSleep);
            if (nth < 0)
            {
                return element.Last;
            }
            else
            {
                return element.Nth(nth);
            }
        }

        public static async Task<ILocator> GetByElementHeaderAndHasTextAsync(IPage? page, string hasText, int nth, string? iFrameName = null)
        {
            ILocator locator;
            if (!string.IsNullOrEmpty(iFrameName))
            {
                var iframeLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                locator = await ElementHelper.GetByElementHeaderAndHasTextAsync(iframeLocator, hasText);
            }
            else
            {
                locator = await ElementHelper.GetByElementHeaderAndHasTextAsync(page, hasText);
            }
            return locator.Nth(nth);
        }
        public static async Task<ILocator> GetByDataTestIdAsync(IPage page, string dataTestId, string iFrameName)
        {
            var iframeLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
            var resultLocator = await ElementHelper.GetByDataTestIdAsync(iframeLocator, dataTestId);
            var IsEnable = await WaitElementEnabledAsync(resultLocator);

            return resultLocator;
        }
        #endregion
        #region Get by attribute data-automation-key
        public static async Task<ILocator> GetLocatorByClassAndDataAutomationKeyAsync(IPage? page, string className, string dataAutomationKey, int nth, string? iFrameName = null)
        {
            ILocator locator;
            if (!string.IsNullOrEmpty(iFrameName))
            {
                var iframeLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                locator = await ElementHelper.GetByClassAndDataAutomationKeyAsync(iframeLocator, className, dataAutomationKey);
            }
            else
            {
                locator = await ElementHelper.GetByClassAndDataAutomationKeyAsync(page, className, dataAutomationKey);
            }
            if (nth < 0)
            {
                return locator.Last;
            }
            return locator.Nth(nth);
        }
        public static async Task<ILocator> GetLocatorByClassAndDataAutomationKeyAsync(ILocator? locator, string className, string dataAutomationKey, int nth, bool isNeedSleep = true)
        {
            ILocator resultLocator;
            resultLocator = await ElementHelper.GetByClassAndDataAutomationKeyAsync(locator, className, dataAutomationKey, isNeedSleep: isNeedSleep);
            if (nth < 0)
            {
                return resultLocator.Last;
            }
            return resultLocator.Nth(nth);
        }
        #endregion
        #region Get by attribute- Data Is Scrollable
        public static async Task<ILocator> GetByAttributeDataIsScrollableAsync(IPage? page, string scrollableStatus, int nth, string? iFrameName = null)
        {
            ILocator locator;
            if (!string.IsNullOrEmpty(iFrameName))
            {
                var iframeLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                locator = await ElementHelper.GetByAttributeDataIsScrollableAsync(iframeLocator, scrollableStatus);
            }
            else
            {
                locator = await ElementHelper.GetByAttributeDataIsScrollableAsync(page, scrollableStatus);
            }
            return locator.Nth(nth);
        }
        public static async Task<ILocator> GetByAttributeDataIsScrollableAsync(ILocator? locator, string scrollableStatus, int nth)
        {
            ILocator resultLocator;
            resultLocator = await ElementHelper.GetByAttributeDataIsScrollableAsync(locator, scrollableStatus);
            return resultLocator.Nth(nth);
        }
        #endregion

        #region Get by attribute-name
        public static async Task ClickByAttributeDataTelemetryNameAsync(IPage? page, string dataTelemetryName, int nth = 0, string? iFrameName = null, bool IsStrongWait = true)
        {
            ILocator locator;
            if (!string.IsNullOrEmpty(iFrameName))
            {
                var iframeLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                locator = await ElementHelper.GetByAttributeDataTelemetrynameAsync(iframeLocator, dataTelemetryName, IsStrongWait: IsStrongWait);
            }
            else
            {
                locator = await ElementHelper.GetByAttributeDataTelemetrynameAsync(page, dataTelemetryName, IsStrongWait: IsStrongWait);
            }

            if (nth < 0)
            {
                await locator.Last.ClickAsync();
            }
            else
            {
                await locator.Nth(nth).ClickAsync();
            }
        }
        public static async Task ClickDivByAttributeDataTelemetryNameAsync(ILocator locator, string dataTelemetryName, int nth = 0)
        {
            ILocator element = await ElementHelper.GetByAttributeDataTelemetrynameAsync(locator, dataTelemetryName);
            if (nth < 0)
            {
                await element.Last.ClickAsync();
            }
            else
            {
                await element.Nth(nth).ClickAsync();
            }
        }

        public static async Task<ILocator> GetByAttributeDataTelemetryNameAsync(IPage? page, string dataTelemetryName, int nth = 0, string? iFrameName = null, bool IsStrongWait = true)
        {
            ILocator locator;
            if (!string.IsNullOrEmpty(iFrameName))
            {
                var iframeLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                locator = await ElementHelper.GetByAttributeDataTelemetrynameAsync(iframeLocator, dataTelemetryName, IsStrongWait: IsStrongWait);
            }
            else
            {
                locator = await ElementHelper.GetByAttributeDataTelemetrynameAsync(page, dataTelemetryName, IsStrongWait: IsStrongWait);
            }

            return locator;
        }
        #endregion
        #region Get by element-name
        public static async Task SetTextAreaValueByArialableAsync(IPage? page, string value, string ariaLable, int nth, string? iFrameName = null)
        {
            ILocator locator;
            if (!string.IsNullOrEmpty(iFrameName))
            {
                var iframeLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                locator = await ElementHelper.GetByTextareaElementAndAriaLableAsync(iframeLocator, ariaLable);
            }
            else
            {
                locator = await ElementHelper.GetByTextareaElementAndAriaLableAsync(page, ariaLable);
            }
            await locator.Nth(nth).FillAsync(value);
        }
        public static async Task SetTextAreaValueByClassAsync(IPage? page, string className, string value, int nth, string? iFrameName = null)
        {
            ILocator locator;
            if (!string.IsNullOrEmpty(iFrameName))
            {
                var iframeLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                locator = await ElementHelper.GetByClassAsync(iframeLocator, className);
            }
            else
            {
                locator = await ElementHelper.GetByClassAsync(page, className);
            }
            await locator.Nth(nth).FillAsync(value);
        }
        public static async Task SetTextAreaValueByClassAsync(ILocator? locator, string className, string value, int nth)
        {
            ILocator textlocator;
            textlocator = await ElementHelper.GetByClassAsync(locator, className);
            await textlocator.Nth(nth).FillAsync(value);
        }

        #endregion
        #region Get by playwright getby api
        #endregion
        #region Get by playwright AriaRole
        #endregion

        #region Click element:button by has text
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="text"></param>
        /// <param name="nth"></param>
        /// <param name="iFrameName"></param>
        /// <returns></returns>
        public static async Task<ILocator> ClickByElementButtonWithTextAsync(IPage page, string hasText, int nth, string? iFrameName = null, bool isClick = true)
        {
            ILocator button;
            if (!string.IsNullOrEmpty(iFrameName))
            {
                var iframeLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                button = await ElementHelper.GetByElementButtonWithNameAsync(iframeLocator, hasText);
            }
            else
            {
                button = await ElementHelper.GetByElementButtonWithNameAsync(page, hasText);
            }
            if (isClick)
            {
                await button.Nth(nth).ClickAsync();
            }
            return button.Nth(nth);
        }
        public static async Task<ILocator> ClickByElementButtonWithTextAsync(ILocator locator, string hasText, int nth, bool isClick = true)
        {
            ILocator button;
            button = await ElementHelper.GetByElementButtonWithNameAsync(locator, hasText);
            if (isClick)
            {
                await button.Nth(nth).ClickAsync();
            }
            return button.Nth(nth);
        }
        public static async Task<ILocator> ClickByElementButtonWithClassAsync(IPage page, string className, int nth, string? iFrameName = null, bool isClick = true)
        {
            ILocator button;
            if (!string.IsNullOrEmpty(iFrameName))
            {
                var iframeLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                button = await ElementHelper.GetByElementButtonWithClassAsync(iframeLocator, className);
            }
            else
            {
                button = await ElementHelper.GetByElementButtonWithClassAsync(page, className);
            }
            if (isClick)
            {
                await button.Nth(nth).ClickAsync();
            }
            return button.Nth(nth);
        }
        public static async Task<ILocator> ClickByElementButtonWithClassAsync(ILocator locator, string className, int nth, bool isClick = true)
        {
            ILocator button = await ElementHelper.GetByElementButtonWithClassAsync(locator, className);
            if (isClick)
            {
                await button.Nth(nth).ClickAsync();
            }
            return button.Nth(nth);
        }

        public static async Task<(ILocator currentLocator, int locatorCount)> ClickByElementButtonWithAriaLabelAsync(IPage page, string ariaLabel, int nth, string? iFrameName = null, bool isClick = true)
        {
            ILocator button;
            if (!string.IsNullOrEmpty(iFrameName))
            {
                var iframeLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                button = await ElementHelper.GetByElementButtonWithAriaLabelAsync(iframeLocator, ariaLabel);
            }
            else
            {
                button = await ElementHelper.GetByElementButtonWithAriaLabelAsync(page, ariaLabel);
            }
            if (isClick)
            {
                await button.Nth(nth).ClickAsync();
            }
            return (button.Nth(nth), await button.CountAsync());
        }
        #endregion

        #region click by locator and hastext

        public static async Task ClickButtonByLocatorWithHasTextAsync(IPage? page, string locator, string hasText, int nth, string? iFrameName = null)
        {
            ILocator button;
            if (!string.IsNullOrEmpty(iFrameName))
            {
                var iframeLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                button = await ElementHelper.GetByLocatorAndHasTextAsync(iframeLocator, locator, hasText);
            }
            else
            {
                button = await ElementHelper.GetByLocatorAndHasTextAsync(page, locator, hasText);
            }
            await button.Nth(nth).ClickAsync();
        }
        #endregion
        #region click by class and arialable
        public static async Task<(int locatorCount, ILocator currentLocator)> ClickByClassWithAriaLableAsync(IPage? page, string className, string ariaLable, int nth, string? iFrameName = null, bool isClick = true, bool IsStrongWait = true)
        {
            ILocator button;
            try
            {
                if (!string.IsNullOrEmpty(iFrameName))
                {
                    var iframeLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                    button = await ElementHelper.GetByClassAndAriaLableAsync(iframeLocator, className, ariaLable, IsStrongWait: IsStrongWait);
                }
                else
                {
                    button = await ElementHelper.GetByClassAndAriaLableAsync(page, className, ariaLable, IsStrongWait: IsStrongWait);
                }
                if (isClick)
                {
                    await button.Nth(nth).ScrollIntoViewIfNeededAsync();
                    await button.Nth(nth).ClickAsync();
                    return (0, button);
                }
                return (await button.CountAsync(), button.Nth(nth));
            }
            catch (Exception ex)
            {
               // LogHelper.Error(ex.Message);
                return (0, null);
                throw new InsufficientExecutionStackException();
            }
        }
        public static async Task<(int locatorCount, ILocator currentLocator)> ClickByClassWithAriaLableAsync(ILocator? locator, string className, string ariaLable, int nth, bool isClick = true, int retry = 150)
        {
            try
            {
                ILocator button;
                button = await ElementHelper.GetByClassAndAriaLableAsync(locator, className, ariaLable, retry: retry);
                if (isClick)
                {
                    //LogHelper.Info($"Hover \"{ariaLable}\"");
                    await button.Nth(nth).HoverAsync();
                    await button.Nth(nth).ScrollIntoViewIfNeededAsync();
                    //LogHelper.Info($"Click \"{ariaLable}\"");
                    await button.Nth(nth).ClickAsync();
                }
                return (await button.CountAsync(), button.Nth(nth));
            }
            catch
            {
                throw new IndexOutOfRangeException();
            }
        }

        #endregion
        #region click by arialable
        public static async Task<(int locatorCount, ILocator currentLocator)> ClickByAriaLableAsync(IPage? page, string ariaLable, int nth, string? iFrameName = null, bool isClick = true)
        {
            ILocator button;
            if (!string.IsNullOrEmpty(iFrameName))
            {
                var iframeLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                button = await ElementHelper.GetByAriaLabelAsync(iframeLocator, ariaLable);
            }
            else
            {
                button = await ElementHelper.GetByAriaLabelAsync(page, ariaLable);
            }
            if (isClick)
            {
                await button.Nth(nth).ScrollIntoViewIfNeededAsync();
                await button.Nth(nth).ClickAsync();
            }
            return (await button.CountAsync(), button.Nth(nth));
        }
        public static async Task<(int locatorCount, ILocator currentLocator)> ClickByAriaLableAsync(ILocator? locator, string ariaLable, int nth, bool isClick = true)
        {
            ILocator button;
            button = await ElementHelper.GetByAriaLabelAsync(locator, ariaLable);
            if (isClick)
            {
                await button.Nth(nth).ScrollIntoViewIfNeededAsync();
                await button.Nth(nth).ClickAsync();
            }
            return (await button.CountAsync(), button.Nth(nth));
        }

        #endregion
        #region click by class and has text
        public static async Task<(int locatorCount, ILocator currentLocator)> ClickByClassAndHasTextAsync(IPage? page, string className, string hasText, int nth, string? iFrameName = null, bool isClick = true, bool waitUntilElementExist = true, string? hasNotText = null)
        {
            ILocator button;
            if (!string.IsNullOrEmpty(iFrameName))
            {
                var iframeLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                button = await ElementHelper.GetByClassAndHasTextAsync(iframeLocator, className, hasText, waitUntilElementExist: waitUntilElementExist, hasNotText: hasNotText);
            }
            else
            {
                button = await ElementHelper.GetByClassAndHasTextAsync(page, className, hasText, waitUntilElementExist: waitUntilElementExist, hasNotText: hasNotText);
            }
            if (isClick)
            {
                await button.Nth(nth).ClickAsync();
                return (0, button);
            }
            return (await button.CountAsync(), button.Nth(nth));
        }
        public static async Task<(int locatorCount, ILocator currentLocator)> ClickByClassAndHasTextAsync(ILocator? locator, string className, string hasText, int nth, bool isClick = true, bool waitUntilElementExist = true, string? hasNotText = null)
        {
            ILocator button = await ElementHelper.GetByClassAndHasTextAsync(locator, className, hasText, waitUntilElementExist: waitUntilElementExist, hasNotText: hasNotText);
            if (isClick)
            {
                if (nth < 0)
                {
                    await button.Last.ClickAsync();
                }
                else
                {
                    await button.Nth(nth).ClickAsync();
                }
            }
            return (await button.CountAsync(), button.Nth(nth));
        }
        #endregion
        #region click by class and has text
        public static async Task<(int locatorCount, ILocator currentLocator)> ClickByClassAsync(IPage? page, string className, int nth, string? iFrameName = null, bool isClick = true, bool waitUntilElementExist = true)
        {
            ILocator button;
            if (!string.IsNullOrEmpty(iFrameName))
            {
                var iframeLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                button = await ElementHelper.GetByClassAsync(iframeLocator, className, waitUntilElementExist: waitUntilElementExist);
            }
            else
            {
                button = await ElementHelper.GetByClassAsync(page, className, waitUntilElementExist: waitUntilElementExist);
            }
            if (isClick)
            {
                await button.Nth(nth).ScrollIntoViewIfNeededAsync();
                await button.Nth(nth).ClickAsync();
            }
            return (await button.CountAsync(), button.Nth(nth));
        }
        public static async Task<(int locatorCount, ILocator currentLocator, ILocator allLocators)> ClickByClassAsync(ILocator? locator, string className, int nth, bool isClick = true, bool waitUntilElementExist = true)
        {
            ILocator button = await ElementHelper.GetByClassAsync(locator, className, waitUntilElementExist: waitUntilElementExist);
            if (isClick)
            {
                await button.Nth(nth).ScrollIntoViewIfNeededAsync();
                await button.Nth(nth).ClickAsync();
            }
            return (await button.CountAsync(), button.Nth(nth), button);
        }
        public static async Task<(int locatorCount, ILocator currentLocator)> GetByClassAsync(IPage? page, string className, string? iFrameName = null, bool waitUntilElementExist = true)
        {
            ILocator element;
            if (!string.IsNullOrEmpty(iFrameName))
            {
                var iframeLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                element = await ElementHelper.GetByClassAsync(iframeLocator, className, waitUntilElementExist: waitUntilElementExist);
            }
            else
            {
                element = await ElementHelper.GetByClassAsync(page, className, waitUntilElementExist: waitUntilElementExist);
            }

            return (await element.CountAsync(), element);
        }
        public static async Task<(int locatorCount, ILocator? currentLocator)> GetByClassAsync(ILocator locator, string className, bool waitUntilElementExist = true)
        {
            ILocator element = await ElementHelper.GetByClassAsync(locator, className, waitUntilElementExist: waitUntilElementExist);
            return (await element.CountAsync(), element);
        }
        #endregion

        #region click by data-automation-key
        public static async Task ClickByDataAutomationKeyAsync(IPage? page, string dataAutomationKey, int nth, string? iFrameName = null)
        {
            ILocator control;
            if (!string.IsNullOrEmpty(iFrameName))
            {
                var iframeLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                control = await ElementHelper.GetByDataAutomationKeyAsync(page, dataAutomationKey, iframeLocator);
            }
            else
            {
                control = await ElementHelper.GetByDataAutomationKeyAsync(page, dataAutomationKey);
            }
            await control.Nth(nth).ClickAsync();
        }
        #endregion
        #region Set Input Value
        public static async Task SetInputByClassAndAriaLabelAndValueAsync(IPage? page, string className, string araiLable, string byValue, string setValue, int nth, string? iFrameName = null)
        {
            try
            {
                ILocator? label = null;
                if (!string.IsNullOrEmpty(iFrameName))
                {
                    var iframeLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                    label = await ElementHelper.GetByClassAndAriaLableAndValueAsync(iframeLocator, className, araiLable, byValue);
                }
                else
                {
                    label = await ElementHelper.GetByClassAndAriaLableAndValueAsync(page, className, araiLable, byValue);
                }
                if (nth >= 0)
                {
                    await label.Nth(nth).FillAsync(setValue);
                }
                else
                {
                    await label.Last.FillAsync(setValue);
                }
            }
            catch (Exception ex)
            {
                //        LogHelper.Error(ex.Message);
                throw new IndexOutOfRangeException();
            }
        }
        public static async Task<ILocator?> SetInputByClassAndAriaLabelAndValueAsync(ILocator? page, string className, string araiLable, string byValue, string setValue, int nth, bool toFill = true)
        {
            try
            {
                ILocator? label = null;
                label = await ElementHelper.GetByClassAndAriaLableAndValueAsync(page, className, araiLable, byValue);
                if (toFill)
                {
                    if (nth >= 0)
                    {
                        await label.Nth(nth).FillAsync(setValue);
                    }
                    else
                    {
                        await label.Last.FillAsync(setValue);
                    }
                    return null;
                }
                else
                {
                    if (nth >= 0)
                    {
                        return label.Nth(nth);
                    }
                    else
                    {
                        return label.Last;
                    }

                }
            }
            
            catch (Exception ex)
            {
                //    LogHelper.Error(ex.Message);
                throw new IndexOutOfRangeException();
            }
        }

        public static async Task SetInputByClassAndAriaLabelAsync(IPage? page, string className, string ariaLable, string value, int nth, string? iFrameName = null, bool exact = false)
        {
            try
            {
                ILocator? label = null;
                if (!string.IsNullOrEmpty(iFrameName))
                {
                    var iframeLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                    label = await ElementHelper.GetByClassAndAriaLableAsync(iframeLocator, className, ariaLable, exact: exact);
                }
                else
                {
                    label = await ElementHelper.GetByClassAndAriaLableAsync(page, className, ariaLable, exact: exact);
                }
                if (nth < 0)
                {
                    await label.Last.FillAsync(value);
                }
                else
                {
                    await label.Nth(nth).FillAsync(value);
                }
            }
            
            catch (Exception ex)
            {
                //    LogHelper.Error(ex.Message);
                throw new IndexOutOfRangeException();
            }
        }
        public static async Task SetInputByClassAndAriaLabelWithExactFalseAsync(IPage? page, string className, string araiLable, string value, int nth, string? iFrameName = null)
        {
            try
            {
                ILocator? label = null;
                if (!string.IsNullOrEmpty(iFrameName))
                {
                    var iframeLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                    label = await ElementHelper.GetByClassAndAriaLableAsync(iframeLocator, className, araiLable, ariaLabelExact: false);
                }
                else
                {
                    label = await ElementHelper.GetByClassAndAriaLableAsync(page, className, araiLable, ariaLabelExact: false);
                }
                await label.Nth(nth).FillAsync(value);
            }
            
            catch (Exception ex)
            {
                //    LogHelper.Error(ex.Message);
                throw new IndexOutOfRangeException();
            }
        }
        public static async Task SetInputByClassAndAriaLabelAsync(ILocator? page, string className, string araiLable, string value, int nth, bool exact = false)
        {
            try
            {
                ILocator? label = null;
                label = await ElementHelper.GetByClassAndAriaLableAsync(page, className, araiLable, exact);
                await label.Nth(nth).FillAsync(value);
            }
            
            catch (Exception ex)
            {
                //   LogHelper.Error(ex.Message);
                throw new IndexOutOfRangeException();
            }
        }
        public static async Task SetInputByClassAndPlaceholderAsync(IPage? page, string className, string placeholder, string value, int nth, string? iFrameName = null)
        {
            try
            {
                ILocator? label = null;
                if (!string.IsNullOrEmpty(iFrameName))
                {
                    var iframeLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                    label = await ElementHelper.GetByClassAndPlaceholderAsync(iframeLocator, className, placeholder);
                }
                else
                {
                    label = await ElementHelper.GetByClassAndPlaceholderAsync(page, className, placeholder);
                }
                await label.Nth(nth).FillAsync(value);
            }
            
            catch (Exception ex)
            {
                //  LogHelper.Error(ex.Message);
                throw new IndexOutOfRangeException();
            }
        }
        public static async Task SetInputByPlaceholderAsync(IPage? page, string placeholder, string value, int nth, string? iFrameName = null)
        {
            try
            {
                ILocator? label = null;
                if (!string.IsNullOrEmpty(iFrameName))
                {
                    var iframeLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                    label = await ElementHelper.GetByPlaceholderAsync(iframeLocator, placeholder);
                }
                else
                {
                    label = await ElementHelper.GetByPlaceholderAsync(page, placeholder);
                }
                await label.Nth(nth).FillAsync(value);
            }
            
            catch (Exception ex)
            {
            //    LogHelper.Error(ex.Message);
              //  throw new CustomLogException("Element load failed: Set value for label failed...", ex);
            }
        }
        public static async Task SetInputByPlaceholderAsync(ILocator locator, string placeholder, string value, int nth)
        {
            try
            {
                ILocator? label = await ElementHelper.GetByPlaceholderAsync(locator, placeholder);
                await label.Nth(nth).FillAsync(value);
            }
           
            catch (Exception ex)
            {
           //     LogHelper.Error(ex.Message);
             //   throw new CustomLogException("Element load failed: Set value for label failed...", ex);
            }
        }
        public static async Task SetInputByClassAndPlaceholderAsync(ILocator? locator, string className, string placeholder, string value, int nth)
        {
            try
            {
                ILocator? label = null;
                label = await ElementHelper.GetByClassAndPlaceholderAsync(locator, className, placeholder);
                await label.Nth(nth).FillAsync(value);
            }
            
            catch (Exception ex)
            {
                //    LogHelper.Error(ex.Message);
                //  throw new CustomLogException("Element load failed: Set value for label failed...", ex);
                throw new IndexOutOfRangeException();
            }
        }

        public static async Task SetInputByClassAndTypeAsync(IPage? page, string className, string type, string value, int nth, string? iFrameName = null)
        {
            try
            {
                ILocator? label = null;
                if (!string.IsNullOrEmpty(iFrameName))
                {
                    var iframeLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                    label = await ElementHelper.GetByClassAndTypeAsync(iframeLocator, className, type);
                }
                else
                {
                    label = await ElementHelper.GetByClassAndTypeAsync(page, className, type);
                }
                await label.Nth(nth).FillAsync(value);
            }
           
            catch (Exception ex)
            {
           //     LogHelper.Error(ex.Message);
             //   throw new CustomLogException("Element load failed: Set value for label failed...", ex);
            }
        }
        public static async Task SetInputByClassAndTypeAsync(ILocator? locator, string className, string type, string value, int nth)
        {
            try
            {
                ILocator? label = null;
                label = await ElementHelper.GetByClassAndTypeAsync(locator, className, type);
                await label.Nth(nth).FillAsync(value);
            }
            
            catch (Exception ex)
            {
            //    LogHelper.Error(ex.Message);
              //  throw new CustomLogException("Element load failed: Set value for label failed...", ex);
            }
        }
        public static async Task SetInputByClassAndNameAsync(IPage? page, string className, string name, string value, int nth, string? iFrameName = null)
        {
            try
            {
                ILocator? label = null;
                if (!string.IsNullOrEmpty(iFrameName))
                {
                    var iframeLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                    label = await ElementHelper.GetByClassAndAriaLableAsync(iframeLocator, className, name);
                }
                else
                {
                    label = await ElementHelper.GetByClassAndAriaLableAsync(page, className, name);
                }
                await label.Nth(nth).FillAsync(value);
            }
            
            catch (Exception ex)
            {
            //    LogHelper.Error(ex.Message);
              //  throw new CustomLogException("Element load failed: Set value for label failed...", ex);
            }
        }
        public static async Task SetInputByClassAndNameAsync(ILocator locator, string className, string name, string value, int nth)
        {
            try
            {
                ILocator? label = await ElementHelper.GetByClassAndAriaLableAsync(locator, className, name);
                await label.Nth(nth).FillAsync(value);
            }
            
            catch (Exception ex)
            {
            //    LogHelper.Error(ex.Message);
              //  throw new CustomLogException("Element load failed: Set value for label failed...", ex);
            }
        }

        public static async Task SetInputByLabelAsync(IPage? page, string labelName, string value, int nth, string? iFrameName = null)
        {
            try
            {
                ILocator? label = null;
                if (!string.IsNullOrEmpty(iFrameName))
                {
                    var iframeLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                    label = await ElementHelper.GetByLableAsync(iframeLocator, labelName);
                }
                else
                {
                    label = await ElementHelper.GetByLableAsync(page, labelName);
                }
                await label.Nth(nth).FillAsync(value);
            }
            
            catch (Exception ex)
            {
            //    LogHelper.Error(ex.Message);
              //  throw new CustomLogException("Element load failed: Set value for label failed...", ex);
            }
        }

        public static async Task SetInputByTitleAsync(IPage? page, string title, string value, int nth, string? iFrameName = null)
        {
            try
            {
                ILocator? label = null;
                if (!string.IsNullOrEmpty(iFrameName))
                {
                    var iframeLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                    label = await ElementHelper.GetByAttributeTitleAsync(iframeLocator, title);
                }
                else
                {
                    label = await ElementHelper.GetByAttributeTitleAsync(page, title);
                }
                await label.Nth(nth).FillAsync(value);
            }
            
            catch (Exception ex)
            {
            //    LogHelper.Error(ex.Message);
              //  throw new CustomLogException("Element load failed: Set value for label failed...", ex);
            }
        }
        #endregion
        #region
        public static async Task<(int locatorCount, ILocator currentLocator)> ClickRoleLinkByNameAsync(IPage? page, string name, int nth, string? iFrameName, bool isClick = true)
        {
           // LogHelper.Info($"Click link `{name}`");
            ILocator linkEle;
            if (iFrameName != null)
            {
                IFrameLocator frameLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                linkEle = await ElementHelper.GetByLinkRoleAndNameAsync(frameLocator, name, IsStrongWait: false);
            }
            else
            {
                linkEle = await ElementHelper.GetByLinkRoleAndNameAsync(page, name);
            }
            if (isClick)
            {
                await linkEle.Nth(0).ClickAsync();
            }
            return (await linkEle.CountAsync(), linkEle.Nth(nth));
        }
        #endregion
        #region Button role
        public static async Task<ILocator> ClickByButtonRoleAndNameAsync(IPage? page, string name, int nth, string? iFrameName, bool isClick = true)
        {
            try
            {
               // LogHelper.Info($"Click button `{name}`");
                ILocator buttonEle;
                if (iFrameName != null)
                {
                    IFrameLocator frameLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                    buttonEle = await ElementHelper.GetByButtonRoleAndNameAsync(frameLocator, name);
                    //buttonEle = await ElementHelper.GetByRoleAndHasTextAsync(frameLocator, AriaRole.Button, name);
                }
                else
                {
                    buttonEle = await ElementHelper.GetByButtonRoleAndNameAsync(page, name);
                    //buttonEle =await ElementHelper.GetByRoleAndHasTextAsync(page, AriaRole.Button, name);
                }
                if (isClick)
                {
                    await buttonEle.Nth(nth).ClickAsync();
                }
                return buttonEle.Nth(nth);
            }
            catch (Exception err)
            {
                throw new IndexOutOfRangeException();
                //throw new CustomLogException($"Click {name} failed...");
            }
        }
        public static async Task ClickByButtonRoleAndNameAsync(ILocator locator, string name, int nth, int retry = 150)
        {
            //LogHelper.Info($"Click button `{name}`");
            var buttonEle = await ElementHelper.GetByButtonRoleAndNameAsync(locator, name, retry: retry);
            await buttonEle.Nth(nth).ClickAsync();
        }
        public static async Task<int> ClickByButtonRoleAndHasTextAsync(IPage? page, string hasText, int nth, string? iFrameName, bool isClick = true, bool IsStrongWait = true)
        {
            bool buttonEleStatus = false;
            try
            {
                ILocator buttonEle;
                if (iFrameName != null)
                {
                    IFrameLocator frameLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                    buttonEle = await ElementHelper.GetByButtonRoleAndHasTextAsync(frameLocator, hasText, IsStrongWait: IsStrongWait);
                }
                else
                {
                    buttonEle = await ElementHelper.GetByButtonRoleAndHasTextAsync(page, hasText, IsStrongWait: IsStrongWait);
                }
                buttonEleStatus = await buttonEle.Nth(nth).IsEnabledAsync();
              //  LogHelper.Info($"{hasText} is enabled: {buttonEleStatus}");
                if (isClick && !buttonEleStatus)
                {
                //    LogHelper.Info($"Wait {hasText} to enable...");
                    await WaitElementEnabledAsync(buttonEle);
                    buttonEleStatus = await buttonEle.Nth(nth).IsEnabledAsync();
                    if (!buttonEleStatus)
                    {
                  //      throw new CustomLogException($"{hasText} is not enabled, the iframeName: {iFrameName}, failed to click...");
                    }
                }
                if (isClick && buttonEleStatus)
                {
                    //LogHelper.Info($"Click \"{hasText}\"...");
                    await buttonEle.Nth(nth).ClickAsync();
                }
                return await buttonEle.CountAsync();
            }
            catch (Exception err)
            {
                throw new IndexOutOfRangeException();
                //     throw new CustomLogException($"{hasText} isEnabled status: {buttonEleStatus},click failed...", err);
            }
        }
        public static async Task ClickByButtonRoleAndHasTextAsync(ILocator locator, string hasText, int nth, bool IsStrongWait = true)
        {
            //LogHelper.Info($"Click button `{hasText}`");
            var buttonEle = await ElementHelper.GetByButtonRoleAndHasTextAsync(locator, hasText, IsStrongWait: IsStrongWait);
            await buttonEle.Nth(nth).ClickAsync();
        }
        #endregion
        #region Option role
        public static async Task<ILocator> ClickByOptionRoleAndAriaLableAsync(IPage? page, string name, int nth, string? iFrameName, bool isClick = true)
        {
          //  LogHelper.Info($"Click button `{name}`");
            ILocator buttonEle;
            if (iFrameName != null)
            {
                IFrameLocator frameLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                buttonEle = await ElementHelper.GetByOptionAndAriaLableAsync(frameLocator, name);
            }
            else
            {
                buttonEle = await ElementHelper.GetByOptionAndAriaLableAsync(page, name);
            }
            if (isClick)
            {
                await buttonEle.Nth(nth).ClickAsync();
            }
            return buttonEle.Nth(nth);

        }
        public static async Task ClickByOptionRoleAndAriaLableAsync(ILocator locator, string name, int nth)
        {
            //LogHelper.Info($"Click button `{name}`");
            var buttonEle = await ElementHelper.GetByOptionAndAriaLableAsync(locator, name);
            await buttonEle.Nth(nth).ClickAsync();
        }
        #endregion
        #region Combox Role
        public static async Task ClickComBoxRoleAsync(ILocator? locator, int comboxNth)
        {
            ILocator dropDownObject = await ElementHelper.GetByRoleAsync(locator, AriaRole.Combobox);
            await dropDownObject.Nth(comboxNth).HoverAsync();
            await dropDownObject.Nth(comboxNth).ClickAsync();
        }
        public static async Task ClickComBoxRoleByTextAsync(IPage page, string text)
        {
            var dropDownObject = await ElementHelper.GetByRoleAndHasTextAsync(page, AriaRole.Combobox, text);
            await dropDownObject.ClickAsync();
            var ExpandStatus = await dropDownObject.GetAttributeAsync("aria-expanded");
            if (ExpandStatus == "false")
            {
                //LogHelper.Info("Click again to expand the dropdown list");
                await dropDownObject.ClickAsync();
            }
        }
        public static async Task ClickComBoxRoleByNameAsync(ILocator locator, string name, int comboxNth)
        {
            ILocator dropDownObject = await ElementHelper.GetByRoleAndAriaLabelAsync(locator, AriaRole.Combobox, name, waitUntilElementExist: true);
            await dropDownObject.Nth(comboxNth).ClickAsync();
            var ExpandStatus = await dropDownObject.Nth(comboxNth).GetAttributeAsync("aria-expanded");
            if (ExpandStatus == "false")
            {
                //LogHelper.Info("Click again to expand the dropdown list");
                await dropDownObject.Nth(comboxNth).ClickAsync();
            }
        }
        public static async Task ClickComBoxRoleByNameAsync(IPage page, string name, int comboxNth)
        {
            ILocator dropDownObject = await ElementHelper.GetByRoleAndAriaLabelAsync(page, AriaRole.Combobox, name);
            await dropDownObject.Nth(comboxNth).ClickAsync();
            var ExpandStatus = await dropDownObject.Nth(comboxNth).GetAttributeAsync("aria-expanded");
            if (ExpandStatus == "false")
            {
                //LogHelper.Info("Click again to expand the dropdown list");
                await dropDownObject.Nth(comboxNth).ClickAsync();
            }
        }
        /// <summary>
        /// role="combobox" role="option"
        /// </summary>
        /// <param name="page"></param>
        /// <param name="value"></param>
        /// <param name="iFrameName"></param>
        /// <returns></returns>
        public static async Task SetComBoxRoleOptionRoleValueAsync(IPage? page, string value, int nth, string? iFrameName = null)
        {
            try
            {
                ILocator dropDownObject;
                if (!string.IsNullOrEmpty(iFrameName))
                {
                    var iframeLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                    dropDownObject = await ElementHelper.GetByRoleAsync(iframeLocator, AriaRole.Combobox);
                }
                else
                {
                    dropDownObject = await ElementHelper.GetByRoleAsync(page, AriaRole.Combobox);
                }
                await dropDownObject.Nth(nth).HoverAsync();
                await dropDownObject.Nth(nth).ClickAsync();
                ILocator optionObject;
                if (!string.IsNullOrEmpty(iFrameName))
                {
                    var iframeLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                    optionObject = await ElementHelper.GetByOptionAndAriaLableAsync(iframeLocator, value);
                }
                else
                {
                    optionObject = await ElementHelper.GetByOptionAndAriaLableAsync(page, value);
                }
                await optionObject.Nth(nth).ClickAsync();
            }
            
            catch (Exception err)
            {
            //    throw new CustomLogException(CustomExceptionPrefix.CodeError_SettingValueToControl_Failed + ",set ComBoxRoleOptionRole failed...", err);
            }

        }
        public static async Task SetComBoxRoleOptionRoleValueAsync(IPage? page, string comBoxName, string value, int nth, string? iFrameName = null, bool exact = false)
        {
            ILocator dropDownObject;
            if (!string.IsNullOrEmpty(iFrameName))
            {
                var iframeLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                dropDownObject = await ElementHelper.GetByComBoxRoleAndNameAsync(iframeLocator, comBoxName);
            }
            else
            {
                dropDownObject = await ElementHelper.GetByComBoxRoleAndNameAsync(page, comBoxName);
            }
            await dropDownObject.Nth(nth).HoverAsync();
            await dropDownObject.Nth(nth).ClickAsync();
            ILocator optionObject;
            if (!string.IsNullOrEmpty(iFrameName))
            {
                var iframeLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                optionObject = await ElementHelper.GetByOptionAndAriaLableAsync(iframeLocator, value);
            }
            else
            {
                optionObject = await ElementHelper.GetByTreeItemAndNameAsync(page, value);
            }
            await optionObject.ScrollIntoViewIfNeededAsync();
            //LogHelper.Info($"{value} display status: {await optionObject.IsVisibleAsync()}");
            await optionObject.Nth(nth).ClickAsync();
        }

        public static async Task SetComBoxRoleOptionRoleWithTitleValueAsync(ILocator? locator, int comboxNth, IPage? page, string title, int optionNth, string? iFrameName = null)
        {
            ILocator dropDownObject;
            dropDownObject = await ElementHelper.GetByRoleAsync(locator, AriaRole.Combobox);
            await dropDownObject.Nth(comboxNth).HoverAsync();
            await dropDownObject.Nth(comboxNth).ClickAsync();
            ILocator optionObject;
            if (!string.IsNullOrEmpty(iFrameName))
            {
                var iframeLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                optionObject = await ElementHelper.GetByOptionAndTitleAsync(iframeLocator, title);
            }
            else
            {
                optionObject = await ElementHelper.GetByOptionAndTitleAsync(page, title);
            }
            await optionObject.Nth(optionNth).ClickAsync();
        }
        public static async Task SetComBoxRoleOptionRoleHasTextAsync(ILocator? locator, string hasText, int nth, IPage? page, string? iFrameName = null)
        {
            ILocator dropDownObject;
            dropDownObject = await ElementHelper.GetByRoleAsync(locator, AriaRole.Combobox);
            await dropDownObject.Nth(nth).HoverAsync();
            await dropDownObject.Nth(nth).ClickAsync();
            ILocator optionObject;
            if (!string.IsNullOrEmpty(iFrameName))
            {
                var iframeLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                optionObject = await ElementHelper.GetByOptionAndAriaLableAsync(iframeLocator, hasText);
            }
            else
            {
                optionObject = await ElementHelper.GetByOptionAndAriaLableAsync(page, hasText);
            }
            await optionObject.Nth(nth).ClickAsync();
        }
        public static async Task<ILocator> GetDropDownByNameAsync(IPage? page, string comBoxName, int nth, string? iFrameName = null)
        {
            ILocator dropDownObject;
            if (!string.IsNullOrEmpty(iFrameName))
            {
                var iframeLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                dropDownObject = await ElementHelper.GetByComBoxRoleAndNameAsync(iframeLocator, comBoxName);
            }
            else
            {
                dropDownObject = await ElementHelper.GetByComBoxRoleAndNameAsync(page, comBoxName);
            }
            return dropDownObject.Nth(nth);
        }
        public static async Task<ILocator> GetDropDownByNameAsync(ILocator locator, string comBoxName, int nth)
        {
            ILocator dropDownObject = await ElementHelper.GetByComBoxRoleAndNameAsync(locator, comBoxName);
            return dropDownObject.Nth(nth);
        }
        public static async Task<ILocator> GetComBoxRoleTreeItemByValueAsync(IPage? page, string value, int nth, string? iFrameName = null)
        {
            ILocator optionObject;
            if (!string.IsNullOrEmpty(iFrameName))
            {
                var iframeLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                optionObject = await ElementHelper.GetByTreeItemAndNameAsync(iframeLocator, value);
            }
            else
            {
                optionObject = await ElementHelper.GetByTreeItemAndNameAsync(page, value);
            }
            return optionObject.Nth(nth);
        }
        public static async Task<ILocator> GetComBoxRoleTreeItemByValueAsync(ILocator locator, string value, int nth)
        {
            ILocator optionObject = await ElementHelper.GetByTreeItemAndNameAsync(locator, value);
            return optionObject.Nth(nth);
        }
        public static async Task SetComBoxRoleTreeItemRoleValueAsync(IPage? page, string comBoxName, string value, int nth, string? iFrameName = null, bool doubleCheck = false)
        {
            ILocator dropDownObject;
            if (!string.IsNullOrEmpty(iFrameName))
            {
                var iframeLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                dropDownObject = await ElementHelper.GetByComBoxRoleAndNameAsync(iframeLocator, comBoxName);
            }
            else
            {
                dropDownObject = await ElementHelper.GetByComBoxRoleAndNameAsync(page, comBoxName);
            }
            await dropDownObject.Nth(nth).HoverAsync();
            await dropDownObject.Nth(nth).ClickAsync();
            if (doubleCheck)
            {
                Thread.Sleep(1000);
                var expandStatus = await dropDownObject.Nth(nth).GetAttributeAsync("aria-expanded");
                //LogHelper.Info($"expandStataus: {expandStatus}");
                if (expandStatus == "false")
                {
                    await dropDownObject.Nth(nth).HoverAsync();
                    await dropDownObject.Nth(nth).ClickAsync();
                }
            }
            ILocator optionObject;
            if (!string.IsNullOrEmpty(iFrameName))
            {
                var iframeLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                optionObject = await ElementHelper.GetByTreeItemAndNameAsync(iframeLocator, value);
            }
            else
            {
                optionObject = await ElementHelper.GetByTreeItemAndNameAsync(page, value);
            }
            await optionObject.Nth(0).ClickAsync();
        }
        public static async Task SetComBoxRoleTreeItemRoleValueAsync(IPage? page, string comBoxName, string category, string value, int nth, string? iFrameName = null, bool doubleCheck = false)
        {
            ILocator dropDownObject;
            if (!string.IsNullOrEmpty(iFrameName))
            {
                var iframeLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                dropDownObject = await ElementHelper.GetByComBoxRoleAndNameAsync(iframeLocator, comBoxName);
            }
            else
            {
                dropDownObject = await ElementHelper.GetByComBoxRoleAndNameAsync(page, comBoxName);
            }
            await dropDownObject.Nth(nth).HoverAsync();
            await dropDownObject.Nth(nth).ClickAsync();
            if (doubleCheck)
            {
                Thread.Sleep(1000);
                var expandStatus = await dropDownObject.Nth(nth).GetAttributeAsync("aria-expanded");
              //  LogHelper.Info($"expandStataus: {expandStatus}");
                if (expandStatus == "false")
                {
                    await dropDownObject.Nth(nth).HoverAsync();
                    await dropDownObject.Nth(nth).ClickAsync();
                }
            }

            ILocator categoryObject;

            if (!string.IsNullOrEmpty(iFrameName))
            {
                var iframeLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                categoryObject = await ElementHelper.GetByAriaLabelAsync(iframeLocator, category);
            }
            else
            {
                categoryObject = await ElementHelper.GetByAriaLabelAsync(page, category);
            }
            var appTypeLocators = categoryObject.Locator("~ div");
            var optionObject = (await appTypeLocators.AllAsync()).First(t => t.InnerTextAsync().Result == value);
            await optionObject.Nth(0).ClickAsync();
        }
        public static async Task SetComBoxRoleTreeItemRoleValueAsync(ILocator? locator, string comBoxName, string value, int nth)
        {
            ILocator dropDownObject = await ElementHelper.GetByComBoxRoleAndNameAsync(locator, comBoxName);
            await dropDownObject.Nth(nth).HoverAsync();
            await dropDownObject.Nth(nth).ClickAsync();
            ILocator optionObject = await ElementHelper.GetByTreeItemAndNameAsync(locator, value);
            await optionObject.Nth(nth).ClickAsync();
        }
        public static async Task SetComBoxRoleTreeItemRoleValueAsync(ILocator? locator, string value, int nth = 0)
        {
            try
            {
                ILocator dropDownObject;
                dropDownObject = await ElementHelper.GetByRoleAsync(locator, AriaRole.Combobox);
                await dropDownObject.Nth(nth).HoverAsync();
                await dropDownObject.Nth(nth).ClickAsync();
                ILocator optionObject;
                optionObject = await ElementHelper.GetByTreeItemAndNameAsync(locator, value);
                await optionObject.Nth(nth).ClickAsync();
                if (locator.GetAttributeAsync("class").Result.Contains("fxc-dropdown-multiselect"))
                {
                    await dropDownObject.Nth(nth).HoverAsync();
                    await dropDownObject.Nth(nth).ClickAsync();
                }
            }
            catch (Exception err)
            {
                //throw new CustomLogException($"Click \"{value}\" failed", err);
            }
        }
        public static async Task SetComBoxRoleTreeItemRoleValueAsync(IPage? page, string comBoxName, List<string> values, int nth, string? iFrameName = null)
        {
            try
            {
                ILocator dropDownObject;
                if (!string.IsNullOrEmpty(iFrameName))
                {
                    var iframeLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                    dropDownObject = await ElementHelper.GetByComBoxRoleAndNameAsync(iframeLocator, comBoxName);
                }
                else
                {
                    dropDownObject = await ElementHelper.GetByComBoxRoleAndNameAsync(page, comBoxName);
                }
                await dropDownObject.Nth(nth).HoverAsync();
                await dropDownObject.Nth(nth).ClickAsync();
                ILocator optionObject;

                foreach (var value in values)
                {
                    if (!string.IsNullOrEmpty(iFrameName))
                    {
                        var iframeLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                        optionObject = await ElementHelper.GetByTreeItemAndNameAsync(iframeLocator, value);
                    }
                    else
                    {
                        optionObject = await ElementHelper.GetByTreeItemAndNameAsync(page, value);
                    }
                    var ariaselected = await optionObject.Nth(0).GetAttributeAsync("aria-selected");
                    if (ariaselected != "true")
                    {
                        await optionObject.Nth(0).ClickAsync();
                    }
                }
               
                var ariaExpend = await dropDownObject.GetAttributeAsync("aria-expanded");
                if (ariaExpend == "true")
                {
                    await dropDownObject.Nth(nth).HoverAsync();
                    await dropDownObject.Nth(nth).ClickAsync();
                }
            }
            catch (Exception err)
            {
                //throw new CustomLogException($"Set ComBox:\"{comBoxName}\", TreeItem {string.Join(",", values)} failed...");
            }
        }


        #endregion
        #region Get By Text
        public static async Task<(int locatorCount, ILocator currentLocator, bool isDisable)> ClickByTextAsync(IPage? page, string text, int nth, string? iFrameName = null, bool isClick = true)
        {
            try
            {
                ILocator? elementObj = null;
                if (!string.IsNullOrEmpty(iFrameName))
                {
                    var iFrameLator = ElementHelper.GetIFrameLocator(page, iFrameName);
                    elementObj = await ElementHelper.GetByTextAsync(iFrameLator, text);
                }
                else
                {
                    elementObj = await ElementHelper.GetByTextAsync(page, text);
                }
                if (isClick)
                {
                    await elementObj.Nth(nth).ClickAsync();
                    return (1, null, false);
                }
                return (await elementObj.CountAsync(), elementObj.Nth(nth), await elementObj.Nth(nth).IsDisabledAsync());
            }
            catch (Exception ex)
            {
                //        LogHelper.Error(ex.Message);
                throw new IndexOutOfRangeException();
            }
        }
        public static async Task<int> ClickByTextAsync(ILocator locator, string text, int nth, bool isClick = true)
        {
            try
            {
                var elementObj = await ElementHelper.GetByTextAsync(locator, text);
                if (isClick)
                {
                    await elementObj.Nth(nth).ClickAsync();
                }
                return await elementObj.CountAsync();
            }
            
            catch (Exception ex)
            {
                //    LogHelper.Error(ex.Message);
                //  throw new CustomLogException("Element load failed: Set value failed...", ex);

                throw new IndexOutOfRangeException();
            }
        }

        #endregion
        #region Checkbox role
        public static async Task SelectCheckBoxByNthAsync(IPage? page, int nth, string? iFrameName)
        {
            try
            {
                ILocator? checkBox = null;
                if (!string.IsNullOrEmpty(iFrameName))
                {
                    var iFrameLator = ElementHelper.GetIFrameLocator(page, iFrameName);
                    checkBox = await ElementHelper.GetByCheckboxRoleAsync(iFrameLator);
                }
                else
                {
                    checkBox = await ElementHelper.GetByCheckboxRoleAsync(page);
                }
                await checkBox.Nth(nth).ClickAsync();
            }
            catch (Exception ex)
            {
                //     LogHelper.Error(ex.Message);
                //   throw new CustomLogException("Element load failed: Set value for checkbox failed...", ex);
                throw new IndexOutOfRangeException();


            }
        }
        public static async Task SelectCheckBoxByNthAsync(ILocator locator, int nth)
        {
            try
            {
                var checkBox = await ElementHelper.GetByCheckboxRoleAsync(locator);
                await checkBox.Nth(nth).ClickAsync();
            }
            
            catch (Exception ex)
            {
                //    LogHelper.Error(ex.Message);
                //  throw new CustomLogException("Element load failed: Set value for checkbox failed...", ex);
                throw new IndexOutOfRangeException();


            }
        }
        #endregion
        #region GridCell Role
        public static async Task ClickByGridCellAndAriaLableAsync(IPage? page, string ariaLable, int nth, string? iFrameName = null)
        {
            try
            {
                ILocator? gridCell = null;
                if (!string.IsNullOrEmpty(iFrameName))
                {
                    var iFrame = ElementHelper.GetIFrameLocator(page, iFrameName);
                    gridCell = await ElementHelper.GetGridcellByNameAsync(iFrame, ariaLable);
                }
                else
                {
                    gridCell = await ElementHelper.GetGridcellByNameAsync(page, ariaLable);
                }
                await gridCell.Nth(nth).ClickAsync();
            }
            catch (Exception ex)
            {
                //        LogHelper.Error(ex.Message);
                //
                //throw new CustomLogException("Element load failed: click gridcell failed...", ex);
                throw new IndexOutOfRangeException();


            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="hasText"></param>
        /// <param name="nth"></param>
        /// <param name="iFrameName"></param>
        /// <returns></returns>
        /// <exception cref="CustomLogException"></exception>
        public static async Task<(int locatorCount, ILocator currentLocator)> ClickByGridCellAndHasTextAsync(IPage? page, string? hasText, int nth, string? iFrameName = null, bool isClick = true, bool IsStrongWait = true)
        {
            ILocator? gridCell = null;
            if (!string.IsNullOrEmpty(iFrameName))
            {
                var iFrame = ElementHelper.GetIFrameLocator(page, iFrameName);
                gridCell = await ElementHelper.GetByGridcellAndHasTextAsync(iFrame, hasText, IsStrongWait: IsStrongWait);
            }
            else
            {
                gridCell = await ElementHelper.GetByGridcellAndHasTextAsync(page, hasText, IsStrongWait: IsStrongWait);
            }
           // BaseCommonUtils.BaseThreadSleepMiddle();
            var gridCellCount = await gridCell.CountAsync();

            if (isClick)
            {
                await gridCell.Nth(nth).ClickAsync();
                return (0, null);
            }
            Thread.Sleep(2000);
            var count = await gridCell.CountAsync();
            return (count, gridCell.Nth(nth));
        }
        public static async Task<(int locatorCount, ILocator currentLocator)> ClickByGridCellAndHasTextAsync(ILocator? locator, string? hasText, int nth, bool isClick = true)
        {
            ILocator? gridCell = null;
            gridCell = await ElementHelper.GetByGridcellAndHasTextAsync(locator, hasText);

            if (isClick)
            {
                await gridCell.Nth(nth).ClickAsync();
                return (1, gridCell.Nth(0));
            }
            Thread.Sleep(2000);
            var count = await gridCell.CountAsync();
            return (count, gridCell.Nth(nth));
        }
        public static async Task ClickGridcellByRowTextAndClassAsync(ILocator? locator, string gridcellClass, string? rowText, int nth, string? iFrameName = null)
        {
            try
            {
                ILocator? row = null;
                ILocator? gridcell = null;
                if (!string.IsNullOrEmpty(iFrameName))
                {
                    row = await ElementHelper.GetRowByHasTextAsync(locator, rowText);
                    gridcell = await ElementHelper.GetByClassAsync(row, gridcellClass);
                }
                else
                {
                    row = await ElementHelper.GetRowByHasTextAsync(locator, rowText);
                    gridcell = await ElementHelper.GetByClassAsync(row, gridcellClass);
                }

                await gridcell.Nth(nth).ClickAsync();
            }
            catch (Exception ex)
            {
                //    LogHelper.Error(ex.Message);
                //  throw new CustomLogException("Element load failed: click gridcell failed...", ex);

                throw new IndexOutOfRangeException();
            }
        }
        public static async Task ClickGridcellsByRowTextAndClassAsync(ILocator? locator, string gridcellClass, List<string>? rowText, int nth, string? iFrameName = null)
        {
            ILocator? row = null;
            ILocator? gridcell = null;
            foreach (var text in rowText)
            {
                await ClickGridcellByRowTextAndClassAsync(locator, gridcellClass, text, nth, iFrameName);

            }
        }
        #endregion
        #region rowheader Role
        public static async Task ClickByRowHeaderRoleAndHasTextAsync(IPage? page, string hasText, int nth, string? iFrameName = null)
        {
            try
            {
                ILocator? rowHeader = null;
                if (!string.IsNullOrEmpty(iFrameName))
                {
                    var iFrame = ElementHelper.GetIFrameLocator(page, iFrameName);
                    rowHeader = await ElementHelper.GetRowheaderByHasTextAsync(iFrame, hasText);
                }
                else
                {
                    rowHeader = await ElementHelper.GetRowheaderByHasTextAsync(page, hasText);
                }
                try
                {
                    await rowHeader.Nth(nth).ClickAsync();
                }
                catch
                {
                    await rowHeader.Nth(nth).EvaluateAsync("this.click()");
                }
            }
            
            catch (Exception ex)
            {
                //    LogHelper.Error(ex.Message);
                //  throw new CustomLogException("Element load failed: click rowheader failed...", ex);
                throw new IndexOutOfRangeException();

            }
        }
        public static async Task ClickByRowHeaderRoleAndHasTextAsync(ILocator? locator, string hasText, int nth)
        {
            try
            {
                ILocator? rowHeader = null;
                rowHeader = await ElementHelper.GetRowheaderByHasTextAsync(locator, hasText);
                try
                {
                    await rowHeader.Nth(nth).ClickAsync();
                }
                catch
                {

                    await rowHeader.Nth(nth).EvaluateAsync("this.click()");
                }
            }
            
            catch (Exception ex)
            {
                //    LogHelper.Error(ex.Message);
                //  throw new CustomLogException("Element load failed: click rowheader failed...", ex);
                throw new IndexOutOfRangeException();

            }
        }
        #endregion
        #region Radio Role
        public static async Task SelectRadioByHasTextAsync(IPage? page, string hasText, int nth, string? iFrameName = null)
        {
            try
            {
                ILocator? radio = null;
                if (!string.IsNullOrEmpty(iFrameName))
                {
                    var iFrame = ElementHelper.GetIFrameLocator(page, iFrameName);
                    radio = await ElementHelper.GetByRadioRoleAndHasTextAsync(iFrame, hasText);
                }
                else
                {
                    radio = await ElementHelper.GetByRadioRoleAndHasTextAsync(page, hasText);
                }
                await radio.Nth(nth).ClickAsync();
            }
            catch (Exception ex)
            {
                //        LogHelper.Error(ex.Message);
                //      throw new CustomLogException("Element load failed: click radio failed...", ex);
                throw new IndexOutOfRangeException();

            }
        }
        public static async Task SelectRadioByHasTextAsync(ILocator? locator, string hasText, int nth)
        {
            try
            {
                ILocator? radio = null;
                radio = await ElementHelper.GetByRadioRoleAndHasTextAsync(locator, hasText);
                await radio.Nth(nth).ClickAsync();
            }
            
            catch (Exception ex)
            {
                //    LogHelper.Error(ex.Message);
                //  throw new CustomLogException("Element load failed: click radio failed...", ex);
                throw new IndexOutOfRangeException();

            }
        }

        public static async Task SelectRadioByRadioGroupNameAsync(IPage? page, string radioGroupName, string value, int nth, string? iFrameName = null)
        {
            try
            {
                ILocator? radioGroup = null;
                ILocator? radio = null;
                if (!string.IsNullOrEmpty(iFrameName))
                {
                    var iFrame = ElementHelper.GetIFrameLocator(page, iFrameName);
                    radioGroup = await ElementHelper.GetByRadioGroupRoleByNameAsync(iFrame, radioGroupName);
                    radio = await ElementHelper.GetByRadioRoleAndHasTextAsync(radioGroup.Nth(nth), value);
                }
                else
                {
                    radioGroup = await ElementHelper.GetByRadioGroupRoleByNameAsync(page, radioGroupName);
                    radio = await ElementHelper.GetByRadioRoleAndHasTextAsync(radioGroup.Nth(nth), value);
                }
                await radio.Nth(nth).ClickAsync();
            }
            
            catch (Exception ex)
            {
                //    LogHelper.Error(ex.Message);
                //  throw new CustomLogException("Element load failed: click radio failed...", ex);
                throw new IndexOutOfRangeException();

            }
        }
        #endregion
        #region menuitem role
        public static async Task<ILocator> ClickByMenuItemRoleAndAriaLableAsync(IPage? page, string ariaLabel, int nth, string? iFrameName, bool isClick = true)
        {
           // LogHelper.Info($"Click menuItem `{ariaLabel}`");
            ILocator menuItemEle;
            if (iFrameName != null)
            {
                IFrameLocator frameLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                menuItemEle = await ElementHelper.GetMenuitemByAriaLableAsync(frameLocator, ariaLabel);
            }
            else
            {
                menuItemEle = await ElementHelper.GetMenuitemByAriaLableAsync(page, ariaLabel);
            }
            if (isClick)
            {
                await menuItemEle.Nth(nth).ClickAsync();
            }
            return menuItemEle.Nth(nth);
        }
        public static async Task ClickByMenuItemRoleAndAriaLableAsync(ILocator locator, string ariaLabel, int nth)
        {
            //LogHelper.Info($"Click menuItem `{ariaLabel}`");
            var menuItemEle = await ElementHelper.GetMenuitemByAriaLableAsync(locator, ariaLabel);
            await menuItemEle.Nth(nth).ClickAsync();
        }
        public static async Task<ILocator> ClickByMenuItemRoleAndHasTextAsync(IPage? page, string text, int nth, string? iFrameName, bool isClick = true)
        {
            //LogHelper.Info($"Click menuItem `{text}`");
            ILocator menuItemEle;
            if (iFrameName != null)
            {
                IFrameLocator frameLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                menuItemEle = await ElementHelper.GetByLocatorAndRoleAndHasTextAsync(frameLocator, AriaRole.Menuitem, text);
            }
            else
            {
                menuItemEle = await ElementHelper.GetByLocatorAndRoleAndHasTextAsync(page, AriaRole.Menuitem, text);
            }
            if (isClick)
            {
                await menuItemEle.Nth(nth).ClickAsync();
            }
            return menuItemEle.Nth(nth);
        }
        #endregion

        #region menuitem checkbox role
        public static async Task ClickByMenuItemCheckBoxAsync(IPage? page, string text, int nth, string? iFrameName, bool exact = false)
        {
        //    LogHelper.Info($"Click menuItem checkbox `{text}`");
            ILocator menuItemEle;
            if (iFrameName != null)
            {
                IFrameLocator frameLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                menuItemEle = await ElementHelper.GetMenuitemCheckboxByAriaLableAsync(frameLocator, text, exact: exact);
            }
            else
            {
                menuItemEle = await ElementHelper.GetMenuitemCheckboxByAriaLableAsync(page, text, exact: exact);
            }

            await menuItemEle.Nth(nth).ClickAsync();
        }
        public static async Task ClickByMenuItemCheckBoxAsync(ILocator locator, string ariaLabel, int nth, bool exact = false)
        {
          //  LogHelper.Info($"Click menuItem `{ariaLabel}`");
            var menuItemEle = await ElementHelper.GetMenuitemCheckboxByAriaLableAsync(locator, ariaLabel, exact: exact);
            await menuItemEle.Nth(nth).ClickAsync();
        }
        #endregion
        #region TextBox Role
        public static async Task SetValueForTextBoxRoleByNthAsync(IPage? page, string value, int nth, string? iframeName = null)
        {
            try
            {
                ILocator? textBox = null;
                if (!string.IsNullOrEmpty(iframeName))
                {
                    var iFrameLocator = ElementHelper.GetIFrameLocator(page, iframeName);
                    textBox = await ElementHelper.GetByRoleAsync(iFrameLocator, AriaRole.Textbox);

                }
                else
                {
                    textBox = await ElementHelper.GetByRoleAsync(page, AriaRole.Textbox);
                }
                await textBox.Nth(nth).ClearAsync();
                await textBox.Nth(nth).FillAsync(value);
            }
            catch (Exception ex)
            {
                //        LogHelper.Error(ex.Message);
                //      throw new CustomLogException("Element load failed: Set value for input box failed...", ex);
                throw new IndexOutOfRangeException();

            }

        }
        #endregion
        #region Tab Role
        public static async Task ClickByTabRoleAndNameAsync(IPage? page, string name, int nth, string? iFrameName)
        {
        //    LogHelper.Info($"Click tab `{name}`");
            ILocator tabEle;
            if (iFrameName != null)
            {
                IFrameLocator frameLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                tabEle = await ElementHelper.GetByTabRoleAndNameAsync(frameLocator, name);
            }
            else
            {
                tabEle = await ElementHelper.GetByTabRoleAndNameAsync(page, name);
            }

            await tabEle.Nth(nth).ClickAsync();
        }
        public static async Task ClickByTabRoleAndNameAsync(ILocator? page, string name, int nth)
        {
          //  LogHelper.Info($"Click tab `{name}`");
            ILocator tabEle;
            tabEle = await ElementHelper.GetByTabRoleAndNameAsync(page, name);
            await tabEle.Nth(nth).ClickAsync();
        }
        #endregion
        #region TreeItem Role
        public static async Task ClickByTreeItemRoleAndNameAsync(IPage? page, string name, int nth, string? iFrameName)
        {
        //    LogHelper.Info($"Click treeItem `{name}`");
            ILocator treeItemEle;
            if (iFrameName != null)
            {
                IFrameLocator frameLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                treeItemEle = await ElementHelper.GetByTreeItemAndNameAsync(frameLocator, name);
            }
            else
            {
                treeItemEle = await ElementHelper.GetByTreeItemAndNameAsync(page, name);
            }

            await treeItemEle.Nth(nth).ClickAsync();
        }
        public static async Task ClickByTreeItemRoleAndNameAsync(ILocator locator, string name, int nth)
        {
          //  LogHelper.Info($"Click treeItem `{name}`");
            ILocator treeItemEle;
            treeItemEle = await ElementHelper.GetByTreeItemAndNameAsync(locator, name);
            await treeItemEle.Nth(nth).ClickAsync();
        }
        public static async Task ClickByTreeItemRoleAndHasTextAsync(IPage page, string hasText, int nth)
        {
            //LogHelper.Info($"Click treeItem `{hasText}`");
            var treeItemEle = await ElementHelper.GetByRoleAndHasTextAsync(page, AriaRole.Treeitem, hasText);
            await treeItemEle.Nth(nth).ClickAsync();
        }
        #endregion
        #region Presentation Role
        public static async Task<(int locatorCount, ILocator currentLocator)> ClickByPresentationAndNameAsync(IPage? page, string name, int nth, string? iFrameName, bool isClick = true)
        {
        //    LogHelper.Info($"Click presentation `{name}`");
            ILocator presentationEle;
            if (iFrameName != null)
            {
                IFrameLocator frameLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                presentationEle = await ElementHelper.GetByPresentationAndNameAsync(frameLocator, name);
            }
            else
            {
                presentationEle = await ElementHelper.GetByPresentationAndNameAsync(page, name);
            }

            if (isClick)
            {
                await presentationEle.Nth(nth).ClickAsync();
            }
            return (await presentationEle.CountAsync(), presentationEle.Nth(nth));
        }
        public static async Task<(int locatorCount, ILocator currentLocator)> ClickByPresentationAndTitleAsync(IPage? page, string title, int nth, string? iFrameName, bool isClick = true)
        {
          //  LogHelper.Info($"Click presentation `{title}`");
            ILocator presentationEle;
            if (iFrameName != null)
            {
                IFrameLocator frameLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                presentationEle = await ElementHelper.GetByPresentationAndTitleAsync(frameLocator, title);
            }
            else
            {
                presentationEle = await ElementHelper.GetByPresentationAndTitleAsync(page, title);
            }

            if (isClick)
            {
                await presentationEle.Nth(nth).ClickAsync();
            }
            return (await presentationEle.CountAsync(), presentationEle.Nth(nth));
        }
        #endregion
        #region Get Text Content
        public static async Task<string?> GetTextByClassAndTextKeywordAsync(IPage? page, string className, string textKeyWord, int nth, string? iFrameName = null, bool waitUntilElementExist = true)
        {
            try
            {
                ILocator targetLocator;
                if (!string.IsNullOrEmpty(iFrameName))
                {
                    var iFrameLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                    targetLocator = await ElementHelper.GetByClassAndHasTextAsync(iFrameLocator, className, textKeyWord, waitUntilElementExist: waitUntilElementExist);
                }
                else
                {
                    targetLocator = await ElementHelper.GetByClassAndHasTextAsync(page, className, textKeyWord, waitUntilElementExist: waitUntilElementExist);
                }
                return await targetLocator.Nth(nth).TextContentAsync();
            }
            catch (Exception ex)
            {
                //        throw new CustomLogException("Element load failed: Get text by role failed...", ex);

                throw new IndexOutOfRangeException();
            }
        }
        public static async Task<string?> GetTextByClassAndTextKeywordAsync(ILocator? locator, string className, string textKeyWord, int nth)
        {
            try
            {
                ILocator targetLocator;
                targetLocator = await ElementHelper.GetByClassAndHasTextAsync(locator, className, textKeyWord);
                return await targetLocator.Nth(nth).InnerTextAsync();
            }
            
            catch (Exception ex)
            {
                //    throw new CustomLogException("Element load failed: Get text by role failed...", ex);
                throw new IndexOutOfRangeException();

            }
        }

        public static async Task<string?> GetTextByClassAsync(IPage? page, string className, int nth, string? iFrameName = null)
        {
            try
            {
                ILocator targetLocator;
                if (!string.IsNullOrEmpty(iFrameName))
                {
                    var iFrameLocator = ElementHelper.GetIFrameLocator(page, iFrameName);
                    targetLocator = await ElementHelper.GetByClassAsync(iFrameLocator, className);
                }
                else
                {
                    targetLocator = await ElementHelper.GetByClassAsync(page, className);
                }
                return await targetLocator.Nth(nth).TextContentAsync();
            }
            catch (Exception ex)
            {
                //     throw new CustomLogException("Element load failed: Get text by role failed...", ex);
                throw new IndexOutOfRangeException();

            }
        }
        public static async Task<string?> GetTextByClassAsync(ILocator? locator, string className, int nth)
        {
            try
            {
                ILocator targetLocator;
                targetLocator = await ElementHelper.GetByClassAsync(locator, className);
                return await targetLocator.Nth(nth).InnerTextAsync();
            }
            catch (Exception ex)
            {
                //      throw new CustomLogException("Element load failed: Get text by role failed...", ex);
                throw new IndexOutOfRangeException();

            }
        }
        public static async Task<string?> GetTextByRoleAndClassAsync(IPage page, string className, AriaRole role, int nth, string iFrameName = null, bool waitUntilElementExist = true)
        {
            try
            {
                var targetLocator = await GetByRoleAndClassAsync(page, role, className, nth, iFrameName, waitUntilElementExist: waitUntilElementExist);
                return await targetLocator.Nth(nth).TextContentAsync();
            }
            
            catch (Exception ex)
            {
                //    throw new CustomLogException("Element load failed: Get text by role and class failed...", ex);

                throw new IndexOutOfRangeException();
            }
        }
        #endregion

        #region Scroll
        public static async Task ScrollToTopAsync(ILocator locator)
        {
            await locator.EvaluateAsync("e => e.scrollTop -= 10000000");
        }
        public static async Task ScrollToBottomAsync(ILocator locator, int pix = 10000000)
        {
            await locator.EvaluateAsync($"e => e.scrollTop += {pix}");
        }
        public static async Task ScrollToBottomAsync(ILocator allRowsLocator, ILocator scrollLocator)
        {
            if (scrollLocator.GetAttributeAsync("data-is-scrollable").Result != "true")
            {
               // throw new CustomLogException(CustomExceptionPrefix.CodeError_Element_Load_Failed + "The element is not scrollable...");
            }
            var lastData = string.Empty;
            int defaultCount = 0;
            while (true)
            {
                var rows = allRowsLocator.AllInnerTextsAsync().Result.ToList();
                if (rows != null && rows.Any())
                {
                    if (defaultCount == 0)
                    {
                        defaultCount = rows.Count;  // determine the scroll length based on defaultCount displayed on page
                    }
                    if (lastData != rows.LastOrDefault())
                    {
                        lastData = rows.LastOrDefault();
                        await ScrollToBottomAsync(scrollLocator, defaultCount * 32);
                        Thread.Sleep(5000);
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
        }

        public static async Task<(ILocator gcRowsLocator, int count)> WaitForListPageGridDataLoadAsync(IPage page, string iFrameName)
        {
            ILocator gcTable = await GetLocatorByClassAsync(page, "ms-DetailsList-contentWrapper", 0, iFrameName: iFrameName);
            (int gcRowsCount, ILocator currentgcRow, ILocator gcRows) = await ClickByClassAsync(gcTable, "ms-DetailsRow-fields", 0, isClick: false);
            ILocator scrollLocator = await GetByAttributeDataIsScrollableAsync(page, "true", 0, iFrameName: iFrameName);
            await ScrollToBottomAsync(gcRows, scrollLocator);
            (gcRowsCount, currentgcRow, gcRows) = await ClickByClassAsync(gcTable, "ms-DetailsRow-fields", 0, isClick: false);
            return (gcRows, gcRowsCount);
        }
        #endregion
        #region Download and Upload Files
        public static async Task<string> DownLoadFileAsync(IPage? page, ILocator clickLocator, string filePath, string fileSuffix = ".json")
        {
            try
            {
                bool status = await clickLocator.IsVisibleAsync();
                if (status)
                {
                  //  LogHelper.Info($"Start DownLoad...");
                    IDownload download = await page.RunAndWaitForDownloadAsync(async () =>
                    {
                        await clickLocator.ClickAsync();
                    }, new PageRunAndWaitForDownloadOptions() { Timeout = 5000 });
                    string fileName = download.SuggestedFilename;
                    fileName = fileName[..fileName.LastIndexOf(".")] + DateTime.Now.ToString("yyyyMMddHHmmss") + fileSuffix;
                    filePath = System.IO.Path.Combine(filePath, fileName);
                    //LogHelper.Info($"downLoad - filePath: {filePath}");

                    await download.SaveAsAsync(filePath);
                    //Assert.True(new FileInfo(filePath).Exists);

                    //ConsoleHelper.ColoredResult(ConsoleColor.Green, "Downloaded Successfully");
                    return filePath;
                }
                else
                {
                    throw new IndexOutOfRangeException();
                    //throw new CustomLogException(CustomExceptionPrefix.CodeError_Element_Load_Failed);
                }
            }
            catch (Exception ex)
            {
                //      LogHelper.Info(ex.ToString());
                //    throw new CustomLogException($"Upload Failed: Failed to upload file for \"{clickLocator.ToString()}\".", ex);

                throw new IndexOutOfRangeException();
            
            
            }

        }
        public static async Task UploadFilesAsync(IPage? page, string placeholder, string filePath, int nth = 0)
        {
            try
            {
                //LogHelper.Info($"Upload Files");
                var fileChooser = await page.RunAndWaitForFileChooserAsync(async () =>
                {
                    //await page.GetByPlaceholder(placeholder).Nth(nth).ClickAsync();                   
                    var button = await ElementHelper.GetByPlaceholderAsync(page, placeholder);
                    await button.Nth(nth).ClickAsync();
                });
                await fileChooser.SetFilesAsync(filePath);
            }
            
            catch (Exception ex)
            {
                //    LogHelper.Info(ex.ToString());
                //  throw new CustomLogException($"Upload Failed: Failed to upload file for path : {filePath}.", ex);
                throw new IndexOutOfRangeException();
            }
        }
        #endregion
        #region Drag and Drop locator
        public static async Task DragAndDropAsync(ILocator sourceLocator, ILocator targetLocator)
        {
            try
            {
                await sourceLocator.ClickAsync();
                await sourceLocator.DragToAsync(targetLocator);
            }
            catch (Exception ex)
            {
                //     LogHelper.Info(ex.ToString());
                //   throw new CustomLogException($"Drag and Drop Failed...", ex);

                throw new IndexOutOfRangeException();
            }
        }
        #endregion

        #region Wait page to load
        public static async Task<bool> CheckElementIsLoadedListPageAsync(ILocator listData, ILocator noResult, ILocator? refreshBtn = null)
        {
            bool result = false;
            if (refreshBtn != null)
            {
            //    LogHelper.Info("Click Refresh...");
                await refreshBtn.ClickAsync();
            }
            var dataListCount = await listData.CountAsync();
            var noResultCount = await noResult.CountAsync();
            //LogHelper.Info("Wait the element to load...");
            int i = 1;
            while (i <= 120 && dataListCount == 0 && noResultCount == 0)
            {
              //  LogHelper.Info($"Wait for the list loaded, waiting {i} times...");
                if (refreshBtn != null)
                {
                //    LogHelper.Info($"Click Refresh {i} times...");
                    await refreshBtn.ClickAsync();
                }
                //CommonOperations.WaitShortTime();
                dataListCount = await listData.CountAsync();
                //LogHelper.Info($"After waiting {i} times, the target search result:{dataListCount}...");

                noResultCount = await noResult.CountAsync();
                //LogHelper.Info($"After waiting {i} times, no data appears result:{noResultCount}...");
                i++;
            }
            if (dataListCount > 0 || noResultCount > 0)
            {
                result = true;
            }

            return result;
        }
        public static async Task<bool> CheckDataExitInListPageAsync(ILocator listData, ILocator noResult)
        {
            bool result = false;
            var dataListCount = await listData.CountAsync();
            var noResultCount = await noResult.CountAsync();
            //LogHelper.Info("Wait the element to load...");
            int i = 1;
            while (i <= 120 && dataListCount == 0 && noResultCount == 0)
            {
              //  LogHelper.Info($"Wait for the list loaded, waiting {i} times...");
                //CommonOperations.WaitShortTime();
                dataListCount = await listData.CountAsync();
                //LogHelper.Info($"After waiting {i} times, the target search result:{dataListCount}...");

                noResultCount = await noResult.CountAsync();
                //LogHelper.Info($"After waiting {i} times, no data appears result:{noResultCount}...");
                i++;
            }
            if (dataListCount > 0)
            {
                result = true;
            }
            return result;
        }
        public static async Task WaitElementToLoadAsync(ILocator element)
        {
            await ElementHelper.IsExistAsync(element);
        }
        public static async Task<bool> WaitElementEnabledAsync(ILocator element)
        {
            return await ElementHelper.IsEnabledAsync(element);
        }
        public static async Task<bool> WaitPageToLoadByTextAsync(ILocator element)
        {
            var result = false;
            var text = await element.TextContentAsync();
            int i = 1;
            while (string.IsNullOrEmpty(text) && i <= 120)
            {
                //LogHelper.Info($"Wait for the page loaded by page text, waiting {i} times...");
                //BaseCommonUtils.BaseThreadSleepMiddle(1);
                text = await element.TextContentAsync();
                i++;
            }
            if (!string.IsNullOrEmpty(text))
                result = true;
            return result;
        }
        #endregion
    }
}
