//using LogService;
//using LogService.Extension;
using Microsoft.Playwright;
using System.Text.RegularExpressions;
using System.Threading;

namespace PlaywrightTests.Common.Helper
{
    public class ElementHelper
    {
        /*
         Function name format:GetBy{ElementName/Role/Attribute/Text}[{(And{Text/Attribute})...}]Async
         */
        #region Get by role
        public static async Task<ILocator> GetByRoleAsync(IPage? page, AriaRole role)
        {
            var element = page.GetByRole(role);
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByRoleAsync(IFrameLocator iframe, AriaRole role)
        {
            var element = iframe.GetByRole(role);
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByRoleAsync(ILocator locator, AriaRole role)
        {
            var element = locator.GetByRole(role);
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByRoleAndHasTextAsync(IPage? page, AriaRole role, string hasText, bool waitUntilElementExist = false)
        {
            var element = page.GetByRole(role).Filter(new LocatorFilterOptions() { HasText = hasText });
            if (waitUntilElementExist)
                return await IsExistAsync(element);
            else
                return element;
        }
        public static async Task<ILocator> GetByRoleAndHasTextAsync(IFrameLocator iframe, AriaRole role, string hasText, bool waitUntilElementExist = true)
        {
            var element = iframe.GetByRole(role).Filter(new LocatorFilterOptions() { HasText = hasText });
            if (waitUntilElementExist)
                return await IsExistAsync(element);
            else
                return element;
        }
        public static async Task<ILocator> GetByRoleAndHasTextAsync(ILocator locator, AriaRole role, string hasText, bool waitUntilElementExist = true)
        {
            var element = locator.GetByRole(role).Filter(new LocatorFilterOptions() { HasText = hasText });
            if (waitUntilElementExist)
                return await IsExistAsync(element);
            else
                return element;
        }
        public static async Task<ILocator> GetByRoleAndAriaLabelAsync(IPage? page, AriaRole role, string ariaLabel, bool exact = true, bool waitUntilElementExist = true)
        {
            ariaLabel = ariaLabel.Replace("'", "\\'");
            var element = page.Locator($"[role='{role.ToString().ToLower()}'][aria-label{(!exact ? "*" : "")}='{ariaLabel}']");
            return element;
        }
        public static async Task<ILocator> GetByRoleAndAriaLabelAsync(IFrameLocator iframe, AriaRole role, string ariaLabel, bool exact = true, bool waitUntilElementExist = true)
        {
            ariaLabel = ariaLabel.Replace("'", "\\'");
            var element = iframe.Locator($"[role='{role.ToString().ToLower()}'][aria-label{(!exact ? "*" : "")}='{ariaLabel}']");
            if (waitUntilElementExist)
                return await IsExistAsync(element);
            else
                return element;
        }
        public static async Task<ILocator> GetByRoleAndAriaLabelAsync(ILocator locator, AriaRole role, string ariaLabel, bool waitUntilElementExist, bool exact = true)
        {
            ariaLabel = ariaLabel.Replace("'", "\\'");
            var element = locator.Locator($"[role='{role.ToString().ToLower()}'][aria-label{(!exact ? "*" : "")}='{ariaLabel}']");
            var elementCount = await element.CountAsync();
            if (waitUntilElementExist)
            {
                var elementStatus = await element.IsVisibleAsync();
                return await IsExistAsync(element);
            }
            else
            {
                return element;
            }
        }
        public static async Task<ILocator> GetByRoleAndNameAsync(IFrameLocator iframe, AriaRole role, string name)
        {
            var element = iframe.GetByRole(role, new() { Name = name, Exact = true });
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByRoleAndNameAsync(ILocator parentLocator, AriaRole role, string name)
        {
            var element = parentLocator.GetByRole(role, new() { Name = name, Exact = true });
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByRoleAndNameAsync(IPage? page, AriaRole role, string name)
        {
            var element = page.GetByRole(role, new() { Name = name, Exact = true });
            return element;
        }

        public static async Task<ILocator> GetByRoleAndAriaClassAsync(IPage? page, AriaRole role, string className, bool waitUntilElementExist = true)
        {
            var element = page.Locator($"[role='{role.ToString().ToLower()}'][class*='{className}']");
            if (waitUntilElementExist)
                return await IsExistAsync(element);
            else
                return element;
        }
        public static async Task<ILocator> GetByRoleAndAriaClassAsync(IFrameLocator iframe, AriaRole role, string className, bool waitUntilElementExist = true)
        {
            var element = iframe.Locator($"[role='{role.ToString().ToLower()}'][class*='{className}']");
            if (waitUntilElementExist)
                return await IsExistAsync(element);
            else
                return element;
        }
        public static async Task<ILocator> GetByRoleAndAriaClassAsync(ILocator locator, AriaRole role, string className, bool isNeedSleep = true)
        {
            var element = locator.Locator($"[role='{role.ToString().ToLower()}'][class*='{className}']");
            return await IsExistAsync(element, isNeedSleep: isNeedSleep);
        }
        #endregion
        #region Get by Locator   

        public static async Task<ILocator> GetByLocatorAsync(IPage? page, string locator, bool isNeedSleep = true)
        {
            var element = page.Locator(locator);
            return await IsExistAsync(element, isNeedSleep: isNeedSleep);
        }
        public static async Task<ILocator> GetByLocatorAsync(IFrameLocator iframe, string locator, bool isNeedSleep = true)
        {
            var element = iframe.Locator(locator);
            return await IsExistAsync(element, isNeedSleep: isNeedSleep);
        }
        public static async Task<ILocator> GetByLocatorAsync(ILocator locator, string locatorStr, bool isNeedSleep = true)
        {
            var element = locator.Locator(locatorStr);
            return await IsExistAsync(element, isNeedSleep: isNeedSleep);
        }
        public static IFrameLocator GetIFrameLocator(IPage? page, string iframeName)
        {
           
            page.WaitForTimeoutAsync(15000).Wait();
            var iFrames = page.QuerySelectorAllAsync("iframe").Result;
            int count = 0;
            iframeName += (iframeName.EndsWith("ReactView") ? "" : ".ReactView");
            //LogHelper.Info($"Iframe name: {iframeName}, check if it's loaded...");
            while (count < 200)
            {
                count++;
                //LogHelper.Info($"wait \"{iframeName}\" to load, the {count} times...");
                var htmlContent = page.Locator("body").InnerHTMLAsync().Result;
                if (htmlContent.Contains($"name=\"{iframeName}\""))
                {
                    var countIframe = page.Locator($"iframe[name=\"{iframeName}\"][class*=\"fxs-reactview-frame-active\"]").CountAsync().Result;
                    //LogHelper.Info($"Current iframe count: {countIframe}");
                    if (countIframe > 1)
                    {
                        Thread.Sleep(200);
                       // LogHelper.Info($"Wait {count * 200} ms");
                        continue;
                    }
                    string frameClass = page.Locator($"iframe[name=\"{iframeName}\"][class*=\"fxs-reactview-frame-active\"]").First.GetAttributeAsync("class").Result.ToString();
                    string frameSrc = page.Locator($"iframe[name=\"{iframeName}\"][class*=\"fxs-reactview-frame-active\"]").First.GetAttributeAsync("src").Result.ToString();
                    string hostName = frameSrc.Split("?")[0];
                    if (!frameClass.Contains("fxs-display-none") && iFrames.Any(t => t.GetAttributeAsync("src").Result.Contains(hostName)))
                    {
                        Thread.Sleep(500);
                       
                        return page.FrameLocator($"iframe[name=\"{iframeName}\"][class*=\"fxs-reactview-frame-active\"]").First;
                    }
                    else
                    {
                        
                        Thread.Sleep(200);
                        continue;
                    }
                }
                else
                {
                    Thread.Sleep(200);
                    continue;
                }
            }
            
            throw new();

        }

        public static async Task<ILocator> GetByLocatorAndHasTextAsync(IPage? page, string locator, string hasText, bool IsStrongWait=true)
        {
            var element = page.Locator(locator, new() { HasText = hasText });
            if (IsStrongWait)
                return await IsExistAsync(element);
            else
                return element;
        }
        public static async Task<ILocator> GetByLocatorAndHasTextAsync(IFrameLocator iframe, string locator, string hasText, bool IsStrongWait = true)
        {
            var element = iframe.Locator(locator, new() { HasText = hasText });
            if (IsStrongWait)
                return await IsExistAsync(element);
            else
                return element;
        }
        public static async Task<ILocator> GetByLocatorAndHasTextAsync(ILocator locatorParent, string hasText, bool IsStrongWait = true)
        {
            var parentLocatorStatus = await locatorParent.CountAsync();
            //var text= await locatorParent.InnerTextAsync();
            var element = locatorParent.GetByText(hasText);
            //var elementCount = await element.CountAsync();
            if (IsStrongWait)
                return await IsExistAsync(element);
            else
                return element;
        }
        public static async Task<ILocator> GetByLocatorAndRoleAndHasTextAsync(IFrameLocator iframe, AriaRole role, string hasText)
        {
            var element = iframe.GetByRole(role).Filter(new LocatorFilterOptions() { HasText = hasText });
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByLocatorAndRoleAndHasTextAsync(IPage? page, AriaRole role, string hasText)
        {
            var element = page.GetByRole(role).Filter(new LocatorFilterOptions() { HasText = hasText });
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByLocatorAndRoleAndHasTextAsync(ILocator locatorParent, AriaRole role, string hasText)
        {
            var element = locatorParent.GetByRole(role).Filter(new LocatorFilterOptions() { HasText = hasText });
            return await IsExistAsync(element);
        }
        public static async Task<IReadOnlyList<ILocator>> GetByLocatorsAndRoleAndHasTextAsync(ILocator locatorParent, AriaRole role, string hasText)
        {
            var element = locatorParent.GetByRole(role).Filter(new LocatorFilterOptions() { HasText = hasText });
            await IsExistAsync(element);
            return await element.AllAsync();
        }
        #endregion
        #region Get IFrameILocator
        #endregion
        #region Get Parent By Child Locator
        public static async Task<ILocator> GetParentByChildLocatorAsync(ILocator locator, int lookUpTimes = 1)
        {
            for (int i = 0; i < lookUpTimes; i++)
            {
                locator = locator.Locator("xpath=..");
                //LogHelper.Info($"Parent locator: {await locator.InnerHTMLAsync()}");
            }
            return await IsExistAsync(locator);
        }
        public static async Task<ILocator> GetParentByChildLocatorAsync(IPage? page, string parentClassName, string childClassName, string text, bool exact = false)
        {
            var element = page.Locator($"[class{(exact ? "" : "*")}='{parentClassName}']").Filter(new() { Has = await GetByClassAndHasTextAsync(page, childClassName, text) });
            return await IsExistAsync(element);
        }
        #endregion
        #region Get Brother Locator
        public static async Task<ILocator> GetBrotherByLocatorAsync(ILocator locator, int nth)
        {
            var nthStr = nth > 0 ? $"[{nth}]" : "*";
            var element = locator.Locator($"xpath=following-sibling::{nthStr}");
            return await IsExistAsync(element);
        }
        #endregion

        #region
        public static async Task<ILocator> GetByAttributeTitleAsync(IPage? page, string title)
        {
            var element = page.Locator($"[title='{title}']");
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByAttributeTitleAsync(IFrameLocator iframe, string title)
        {
            var element = iframe.Locator($"[title='{title}']");
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByAttributeTitleAsync(ILocator locator, string title)
        {
            var element = locator.Locator($"[title='{title}']");
            return await IsExistAsync(element);
        }
        #endregion

        #region Get by attribute-name
        #region
        public static async Task<ILocator> GetByAttributeDataTelemetrynameAsync(IPage? page, string telemetryname, bool IsStrongWait = true)
        {
            var element = page.Locator($"[data-telemetryname='{telemetryname}']");
            if (IsStrongWait)
                return await IsExistAsync(element);
            else
                return element;
        }
        public static async Task<ILocator> GetByAttributeDataTelemetrynameAsync(IFrameLocator iframe, string telemetryname, bool IsStrongWait = true)
        {
            var element = iframe.Locator($"[data-telemetryname='{telemetryname}']");
            if (IsStrongWait)
                return await IsExistAsync(element);
            else
                return element;
        }
        public static async Task<ILocator> GetByAttributeDataTelemetrynameAsync(ILocator locator, string telemetryname, bool IsStrongWait = true)
        {
            var element = locator.Locator($"[data-telemetryname='{telemetryname}']");
            if (IsStrongWait)
                return await IsExistAsync(element);
            else
                return element;
        }
        #endregion
        #region Get by attribute-scrollable
        public static async Task<ILocator> GetByAttributeDataIsScrollableAsync(IPage? page, string scrollableStatus)
        {
            var element = page.Locator($"[data-is-scrollable='{scrollableStatus}']");
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByAttributeDataIsScrollableAsync(IFrameLocator iframe, string scrollableStatus)
        {
            var element = iframe.Locator($"[data-is-scrollable='{scrollableStatus}']");
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByAttributeDataIsScrollableAsync(ILocator locator, string scrollableStatus)
        {
            var element = locator.Locator($"[data-is-scrollable='{scrollableStatus}']");
            return await IsExistAsync(element);
        }
        #endregion
        #region Get by attribute-class

        public static async Task<ILocator> GetByClassAndHasLocatorAsync(IPage? page, string className, ILocator hasLocator, bool exact = false)
        {
            var element = page.Locator($"[class{(exact ? "" : "*")}='{className}']", new PageLocatorOptions { Has = hasLocator });
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByClassAndHasLocatorAsync(IFrameLocator iframe, string className, ILocator hasLocator, bool exact = false)
        {
            var element = iframe.Locator($"[class{(exact ? "" : "*")}='{className}']", new FrameLocatorLocatorOptions { Has = hasLocator });
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByClassAndHasLocatorAsync(ILocator locator, string className, ILocator hasLocator, bool exact = false)
        {
            var element = locator.Locator($"[class{(exact ? "" : "*")}='{className}']", new LocatorLocatorOptions { Has = hasLocator });
            return await IsExistAsync(element);
        }

        public static async Task<ILocator> GetByClassAsync(IPage? page, string className, bool exact = false, bool waitUntilElementExist = false)
        {
            var element = page.Locator($"[class{(exact ? "" : "*")}='{className}']");
            if (waitUntilElementExist)
            {
                return await IsExistAsync(element);
            }
            else
            {
                return element;
            }
        }
        public static async Task<ILocator> GetByClassAsync(IFrameLocator iframe, string className, bool exact = false, bool waitUntilElementExist = true)
        {
            var element = iframe.Locator($"[class{(exact ? "" : "*")}='{className}']");
            if (waitUntilElementExist)
            {
                return await IsExistAsync(element);
            }
            else
            {
                return element;
            }
        }
        public static async Task<ILocator> GetByClassAsync(ILocator locator, string className, bool exact = false, bool waitUntilElementExist = true)
        {
            var element = locator.Locator($"[class{(exact ? "" : "*")}='{className}']");
            if (waitUntilElementExist)
            {
                return await IsExistAsync(element);
            }
            else
            {
                return element;
            }
        }
        public static async Task<ILocator> chatgpt_GetByClassAndHasTextAsync(
    IPage page,
    string className,
    string hasText,
    string? hasNotText = null,
    bool exact = false,
    bool waitUntilElementExist = true)
        {
            if (page == null)
                throw new ArgumentNullException(nameof(page), "Page cannot be null");

            if (string.IsNullOrWhiteSpace(className))
                throw new ArgumentException("Class name must be provided", nameof(className));

            if (string.IsNullOrWhiteSpace(hasText))
                throw new ArgumentException("hasText must be provided", nameof(hasText));

            var selector = exact
                ? $".{className}"
                : $"[class*='{className}']";

            var locator = page.Locator(selector).Filter(string.IsNullOrEmpty(hasNotText)
                ? new() { HasText = hasText }
                : new() { HasText = hasText, HasNotText = hasNotText });

            return waitUntilElementExist
                ? await IsExistAsync(locator)
                : locator;
        }

        public static async Task<ILocator> GetByClassAndHasTextAsync(IPage? page, string className, string hasText, string? hasNotText = null, bool exact = false, bool waitUntilElementExist = false)
        {
            var element = page.Locator($"[class{(exact ? "" : "*")}='{className}']").Filter(string.IsNullOrEmpty(hasNotText) ? new() { HasText = hasText } : new() { HasText = hasText, HasNotText = hasNotText });
            if (waitUntilElementExist)
            {
                return await IsExistAsync(element);
            }
            else
            {
                return element;
            }
        }
        public static async Task<ILocator> GetElementAsync(IPage? page, string hasText, string className, bool waitUntilElementExist = true)
        {
            var element = page.GetByText(hasText);
            await page.WaitForSelectorAsync(className);
            
                return element;
            



        }
        public static async Task<ILocator> GetByClassAndHasTextAsync(IFrameLocator iframe, string className, string hasText, string? hasNotText = null, bool exact = false, bool waitUntilElementExist = false)
        {
            var element = iframe.Locator($"[class{(exact ? "" : "*")}='{className}']").Filter(string.IsNullOrEmpty(hasNotText) ? new() { HasText = hasText } : new() { HasText = hasText, HasNotText = hasNotText });
            if (waitUntilElementExist)
            {
                return await IsExistAsync(element);
            }
            else
            {
                return element;
            }
        }
        public static async Task<ILocator> GetByClassAndHasTextAsync(ILocator locator, string className, string hasText, string? hasNotText = null, bool exact = false, bool waitUntilElementExist = false)
        {
            var element = locator.Locator($"[class{(exact ? "" : "*")}='{className}']").Filter(string.IsNullOrEmpty(hasNotText) ? new() { HasText = hasText } : new() { HasText = hasText, HasNotText = hasNotText });
            if (waitUntilElementExist)
            {
                return await IsExistAsync(element);
            }
            else
            {
                return element;
            }
        }
        public static async Task<ILocator> GetByClassAndRegexTextAsync(IFrameLocator iframe, string className, string regexText, string? hasNotText = null, bool exact = false, bool IsStrongWait = true)
        {
            var element = iframe.Locator($"[class{(exact ? "" : "*")}='{className}']").Filter(string.IsNullOrEmpty(hasNotText) ? new() { HasTextRegex = new Regex(regexText) } : new() { HasTextRegex = new Regex(regexText), HasNotText = hasNotText });
            if (!IsStrongWait)
                return element;
            else
                return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByClassAndRegexTextAsync(IPage? page, string className, string regexText, string? hasNotText = null, bool exact = false)
        {
            var element = page.Locator($"[class{(exact ? "" : "*")}='{className}']").Filter(string.IsNullOrEmpty(hasNotText) ? new() { HasTextRegex = new Regex(regexText) } : new() { HasTextRegex = new Regex(regexText), HasNotText = hasNotText });
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByClassAndRegexTextAsync(ILocator locator, string className, string regexText, string? hasNotText = null, bool exact = false)
        {
            var element = locator.Locator($"[class{(exact ? "" : "*")}='{className}']").Filter(string.IsNullOrEmpty(hasNotText) ? new() { HasTextRegex = new Regex(regexText) } : new() { HasTextRegex = new Regex(regexText), HasNotText = hasNotText });
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByClassAndAriaLableAsync(ILocator locator, string className, string arialabel, bool exact = false, bool ariaLabelExact = true, int retry = 150, bool IsStrongWait = true)
        {            
            arialabel = arialabel.Replace("'", "\\'");
            var element = locator.Locator($"[class{(exact ? "" : "*")}='{className}'][aria-label{(ariaLabelExact ? "" : "*")}='{arialabel}']");

            if (!IsStrongWait)
                return element;
            else
                return await IsExistAsync(element, retry);
        }
        public static async Task<ILocator> GetByClassAndAriaLableAsync(IPage? page, string className, string arialabel, bool exact = false, bool ariaLabelExact = true, bool IsStrongWait = false)
        {
            try
            {
                arialabel = arialabel.Replace("'", "\\'");
                var element = page.Locator($"[class{(exact ? "" : "*")}='{className}'][aria-label{(ariaLabelExact ? "" : "*")}='{arialabel}']");

                if (IsStrongWait)
                    return await IsExistAsync(element);
                else
                    return element;
            }
            catch (Exception ex)
            {
                throw;
            }
           
        }
        public static async Task<ILocator> GetByClassAndAriaLableAsync(IFrameLocator iframe, string className, string arialabel, bool exact = false, bool ariaLabelExact = true, bool IsStrongWait = true)
        {
            var element = iframe.Locator($"[class{(exact ? "" : "*")}='{className}'][aria-label{(ariaLabelExact ? "" : "*")}='{arialabel}']");
            if(IsStrongWait)
                return await IsExistAsync(element);
            else
                return element;
        }
        public static async Task<ILocator> GetByClassAndAriaLableAndValueAsync(IPage? page, string className, string arialabel, string value, bool exact = false)
        {
            var element = page.Locator($"[class{(exact ? "" : "*")}='{className}'][aria-label='{arialabel}'][value='{value}']");
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByClassAndAriaLableAndValueAsync(IFrameLocator iframe, string className, string arialabel, string value, bool exact = false)
        {
            var element = iframe.Locator($"[class{(exact ? "" : "*")}='{className}'][aria-label='{arialabel}'][value='{value}']");
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByClassAndAriaLableAndValueAsync(ILocator locator, string className, string arialabel, string value, bool exact = false)
        {
            var element = locator.Locator($"[class{(exact ? "" : "*")}='{className}'][aria-label='{arialabel}'][value='{value}']");
            return await IsExistAsync(element);
        }

        public static async Task<ILocator> GetByClassAndPlaceholderAsync(IPage? page, string className, string Placeholder, bool exact = false)
        {
            var element = page.Locator($"[class{(exact ? "" : "*")}='{className}'][placeholder='{Placeholder}']");
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByClassAndPlaceholderAsync(IFrameLocator iframe, string className, string Placeholder, bool exact = false)
        {
            var element = iframe.Locator($"[class{(exact ? "" : "*")}='{className}'][placeholder='{Placeholder}']");
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByClassAndPlaceholderAsync(ILocator locator, string className, string Placeholder, bool exact = false)
        {
            var element = locator.Locator($"[class{(exact ? "" : "*")}='{className}'][placeholder='{Placeholder}']");
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByPlaceholderAsync(IPage? page, string Placeholder, bool exact = false)
        {
            var element = page.Locator($"[placeholder='{Placeholder}']");
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByPlaceholderAsync(IFrameLocator iframe, string Placeholder, bool exact = false)
        {
            var element = iframe.Locator($"[placeholder='{Placeholder}']");
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByPlaceholderAsync(ILocator locator, string Placeholder, bool exact = false)
        {
            var element = locator.Locator($"[placeholder='{Placeholder}']");
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByClassAndTypeAsync(IPage? page, string className, string type, bool exact = false)
        {
            var element = page?.Locator($"[class{(exact ? "" : "*")}='{className}'][type='{type}']");
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByClassAndTypeAsync(IFrameLocator iframe, string className, string type, bool exact = false)
        {
            var element = iframe?.Locator($"[class{(exact ? "" : "*")}='{className}'][type='{type}']");
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByClassAndTypeAsync(ILocator? locator, string className, string type, bool exact = false)
        {
            var element = locator?.Locator($"[class{(exact ? "" : "*")}='{className}'][type='{type}']");
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByClassAndDataAutomationKeyAsync(IPage? page, string className, string dataAutomationKey, bool exact = false)
        {
            var element = page?.Locator($"[class{(exact ? "" : "*")}='{className}'][data-automation-key='{dataAutomationKey}']");
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByDataAutomationKeyAsync(IPage? page, string dataAutomationKey, IFrameLocator iframeLocator = null)
        {
            ILocator element;
            if (iframeLocator != null)
                element = iframeLocator.Locator($"[data-automation-key='{dataAutomationKey}']");
            else
                element = page?.Locator($"[data-automation-key='{dataAutomationKey}']");
            return await IsExistAsync(element);
        }

        public static async Task<ILocator> GetInputByLabelTextAsync(IPage page, string labelText)
        {
            var label = page.Locator($"label:text('{labelText}')");

            // Assumes input is related to label via `for` / `aria-labelledby`
            var inputId = await label.GetAttributeAsync("for");
            if (inputId == null)
                throw new Exception($"No 'for' attribute found on label '{labelText}'");

            var input = page.Locator($"#{inputId}");
            await input.WaitForAsync(new() { Timeout = 5000 }); // Strong wait
            return input;
        }

        public static async Task<ILocator> GetByClassAndDataAutomationKeyAsync(IFrameLocator iframe, string className, string dataAutomationKey, bool exact = false)
        {
            var element = iframe?.Locator($"[class{(exact ? "" : "*")}='{className}'][data-automation-key='{dataAutomationKey}']");
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByClassAndDataAutomationKeyAsync(ILocator? locator, string className, string dataAutomationKey, bool exact = false, bool isNeedSleep = true)
        {
            var element = locator?.Locator($"[class{(exact ? "" : "*")}='{className}'][data-automation-key='{dataAutomationKey}']");
            return await IsExistAsync(element, isNeedSleep: isNeedSleep);
        }
        #endregion
        #region Get By attribute aria-lable
        public static async Task<ILocator> GetByAriaLabelAsync(IPage? page, string ariaLabel, bool exact = true, bool waitUntilElementExist = true)
        {
            var element = page.Locator($"[aria-label{(exact ? "" : "*")}='{ariaLabel}']");
            if (waitUntilElementExist)
            {
                return await IsExistAsync(element);
            }
            else
            {
                return element;
            }
        }
        public static async Task<ILocator> GetByAriaLabelAsync(IFrameLocator iframe, string ariaLabel, bool exact = true, bool waitUntilElementExist = true)
        {
            var element = iframe.Locator($"[aria-label{(exact ? "" : "*")}='{ariaLabel}']");
            if (waitUntilElementExist)
            {
                return await IsExistAsync(element);
            }
            else
            {
                return element;
            }
        }
        public static async Task<ILocator> GetByAriaLabelAsync(ILocator locator, string ariaLabel, bool exact = true, bool waitUntilElementExist = true)
        {
            var element = locator.Locator($"[aria-label{(exact ? "" : "*")}='{ariaLabel}']");
            if (waitUntilElementExist)
            {
                return await IsExistAsync(element);
            }
            else
            {
                return element;
            }
        }

        #endregion
        #region Get by attribute-id
        public static async Task<ILocator> GetByIDAsync(IPage? page, string id)
        {
            var element = page.Locator($"[id='{id}']");
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByIDAsync(IFrameLocator iframe, string id)
        {
            var element = iframe.Locator($"[id='{id}']");
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByIDAsync(ILocator locator, string id)
        {
            var element = locator.Locator($"[id='{id}']");
            return await IsExistAsync(element);
        }
        #endregion
        #endregion

        #region Get by element-name
        #region Get by element-button and has text
        public static async Task<ILocator> GetByElementButtonWithNameAsync(IPage? page, string text)
        {
            var element = page.Locator("button", new() { HasText = text });
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByElementButtonWithNameAsync(IFrameLocator iframe, string text)
        {
            var element = iframe.Locator("button", new() { HasText = text });
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByElementButtonWithNameAsync(ILocator locator, string text)
        {
            var element = locator.Locator("button", new() { HasText = text });
            return await IsExistAsync(element);
        }

        public static async Task<ILocator> GetByElementButtonWithClassAsync(IPage? page, string className, bool exact = false)
        {
            var element = page.Locator($"button[class{(exact ? "" : "*")}='{className}']");
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByElementButtonWithClassAsync(IFrameLocator iframe, string className, bool exact = false)
        {
            var element = iframe.Locator($"button[class{(exact ? "" : "*")}='{className}']");
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByElementButtonWithClassAsync(ILocator locator, string className, bool exact = false)
        {
            var element = locator.Locator($"button[class{(exact ? "" : "*")}='{className}']");
            return await IsExistAsync(element);
        }

        public static async Task<ILocator> GetByElementButtonWithAriaLabelAsync(IPage? page, string ariaLabel)
        {
            var element = page.Locator($"button[aria-label='{ariaLabel}']");
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByElementButtonWithAriaLabelAsync(IFrameLocator iframe, string ariaLabel)
        {
            var element = iframe.Locator($"button[aria-label='{ariaLabel}']");
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByElementButtonWithAriaLabelAsync(ILocator locator, string ariaLabel)
        {
            var element = locator.Locator($"button[aria-label='{ariaLabel}']");
            return await IsExistAsync(element);
        }
        #endregion

        #region Get by element-textarea
        public static async Task<ILocator> GetByTextareaElementAndAriaLableAsync(IPage? page, string ariaLable)
        {
            var element = page?.Locator($"textarea[aria-label='{ariaLable}']");
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByTextareaElementAndAriaLableAsync(IFrameLocator iframe, string ariaLable)
        {
            var element = iframe.Locator($"textarea[aria-label='{ariaLable}']");
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByTextareaElementAndAriaLableAsync(ILocator locator, string ariaLable)
        {
            var element = locator.Locator($"textarea[aria-label='{ariaLable}']");
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByTextareaElementAndClassAsync(IPage? page, string className, bool exact = false)
        {
            var element = page?.Locator($"textarea[class{(exact ? "" : "*")}='{className}']");
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByTextareaElementAndClassAsync(IFrameLocator iframe, string className, bool exact = false)
        {
            var element = iframe.Locator($"textarea[class{(exact ? "" : "*")}='{className}']");
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByTextareaElementAndClassAsync(ILocator locator, string className, bool exact = false)
        {
            var element = locator.Locator($"textarea[class{(exact ? "" : "*")}='{className}']");
            return await IsExistAsync(element);
        }
        #endregion

        #region Get by element-header with hasText
        public static async Task<ILocator> GetByElementHeaderAndHasTextAsync(IPage? page, string text)
        {
            var element = page.Locator("header", new() { HasText = text });
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByElementHeaderAndHasTextAsync(IFrameLocator iframe, string text)
        {
            var element = iframe.Locator("header", new() { HasText = text });
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByDataTestIdAsync(IFrameLocator iframeLocator, string DataTestId)
        {
            var element = iframeLocator.GetByTestId(DataTestId);
            return element;
        }
        public static async Task<ILocator> GetByElementHeaderAndHasTextAsync(ILocator locator, string text)
        {
            var element = locator.Locator("header", new() { HasText = text });
            return await IsExistAsync(element);
        }
        #endregion

        #endregion

        #region Get by playwright getby api
        #region Get by text
        public static async Task<ILocator> GetByTextAsync(IPage page, string text, bool exact = true, bool IsStrongWait = true)
        {
            var element = page.GetByText(text, new() { Exact = exact });
            if (!IsStrongWait)
                return element;
            else
                return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByTextAsync(IFrameLocator iframe, string text, bool exact = true, bool IsStrongWait = true)
        {
            var element = iframe.GetByText(text, new() { Exact = exact });
            if (!IsStrongWait)
                return element;
            else
                return await IsExistAsync(element);
        }
        public static ILocator GetByTextInIframe(IFrameLocator iframLocator, string text, bool IsStrongWait = true)
        {
            var element = iframLocator.GetByText(text);
            return element;
        }
        public static async Task<ILocator> GetByTextAsync(ILocator locator, string text, bool exact = true, bool IsStrongWait = true)
        {
            var element = locator.GetByText(text, new() { Exact = exact });
            if (!IsStrongWait)
                return element;
            else
                return await IsExistAsync(element);
        }
        #endregion
        #region Get by Alt Text
        public static async Task<ILocator> GetByAltTextAsync(IPage page, string altText, bool exact = true)
        {
            var element = page.GetByAltText(altText, new() { Exact = exact });
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByAltTextAsync(IFrameLocator iframe, string altText, bool exact = true)
        {
            var element = iframe.GetByAltText(altText, new() { Exact = exact });
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByAltTextAsync(ILocator locator, string altText, bool exact = true)
        {
            var element = locator.GetByAltText(altText, new() { Exact = exact });
            return await IsExistAsync(element);
        }
        #endregion
        #region Get by lable
        public static async Task<ILocator> GetByLableAsync(IPage page, string lable, bool exact = true)
        {
            var element = page.GetByLabel(lable, new() { Exact = exact });
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByLableAsync(IFrameLocator iframe, string lable, bool exact = true)
        {
            var element = iframe.GetByLabel(lable, new() { Exact = exact });
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByLableAsync(ILocator locator, string lable, bool exact = true)
        {
            var element = locator.GetByLabel(lable, new() { Exact = exact });
            return await IsExistAsync(element);
        }
        #endregion
        #endregion

        #region Get by playwright AriaRole
        #region Checkbox Role
        public static async Task<ILocator> GetByCheckboxRoleAsync(IPage? page)
        {
            var element = page.GetByRole(AriaRole.Checkbox);
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByCheckboxRoleAsync(IFrameLocator iframe)
        {
            var element = iframe.GetByRole(AriaRole.Checkbox);
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByCheckboxRoleAsync(ILocator locator)
        {
            var element = locator.GetByRole(AriaRole.Checkbox);
            return await IsExistAsync(element);
        }
        #endregion
        #region Menuitem Role
        public static async Task<ILocator> GetMenuitemByAriaLableAsync(IPage? page, string ariaLable)
        {
            var element = page.GetByRole(AriaRole.Menuitem, new() { Name = ariaLable, Exact = true });
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetMenuitemByAriaLableAsync(IFrameLocator iframe, string ariaLable)
        {
            var element = iframe.GetByRole(AriaRole.Menuitem, new() { Name = ariaLable, Exact = true });
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetMenuitemByAriaLableAsync(ILocator locator, string ariaLable)
        {
            var element = locator.GetByRole(AriaRole.Menuitem, new() { Name = ariaLable, Exact = true });
            return await IsExistAsync(element);
        }
        #endregion
        #region Menuitem Chckbox
        public static async Task<ILocator> GetMenuitemCheckboxByAriaLableAsync(IPage? page, string ariaLable, bool exact = false)
        {
            var element = page.GetByRole(AriaRole.Menuitemcheckbox, new() { Name = ariaLable, Exact = exact });
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetMenuitemCheckboxByAriaLableAsync(IFrameLocator iframe, string ariaLable, bool exact = false)
        {
            var element = iframe.GetByRole(AriaRole.Menuitemcheckbox, new() { Name = ariaLable, Exact = exact });
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetMenuitemCheckboxByAriaLableAsync(ILocator locator, string ariaLable, bool exact = false)
        {
            var element = locator.GetByRole(AriaRole.Menuitemcheckbox, new() { Name = ariaLable, Exact = exact });
            return await IsExistAsync(element);
        }
        #endregion
        #region Combox Role
        public static async Task<ILocator> GetByComBoxRoleAndNameAsync(IPage? page, string name)
        {
            var element = page.GetByRole(AriaRole.Combobox, new() { Name = name });
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByComBoxRoleAndNameAsync(IFrameLocator iframe, string name)
        {
            var element = iframe.GetByRole(AriaRole.Combobox, new() { Name = name });
            //return await IsExistAsync(element);
            return element;        
        
        }
        public static async Task<ILocator> GetByComBoxRoleAndNameAsync(ILocator locator, string name)
        {
            var element = locator.GetByRole(AriaRole.Combobox, new() { Name = name });
            return await IsExistAsync(element);
        }
        #endregion
        #region Radio Role
        public static async Task<ILocator> GetByRadioRoleAndHasTextAsync(IPage? page, string hasText)
        {
            var element = page.GetByRole(AriaRole.Radio).Filter(new() { HasText = hasText });
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByRadioRoleAndHasTextAsync(IFrameLocator iframe, string hasText)
        {
            var element = iframe.GetByRole(AriaRole.Radio).Filter(new() { HasText = hasText });
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByRadioRoleAndHasTextAsync(ILocator locator, string hasText)
        {
            var locatorstatus = await locator.CountAsync();
            var innerhtml = await locator.InnerHTMLAsync();
            var element = locator.GetByRole(AriaRole.Radio).Filter(new() { HasText = hasText });
            var status = await element.CountAsync();
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByRadioRoleByNameAsync(IPage? page, string name)
        {
            var element = page.GetByRole(AriaRole.Radio, new() { Name = name });
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByRadioRoleByNameAsync(IFrameLocator iframe, string name)
        {
            var element = iframe.GetByRole(AriaRole.Radio, new() { Name = name });
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByRadioRoleByNameAsync(ILocator locator, string name)
        {
            var element = locator.GetByRole(AriaRole.Radio, new() { Name = name });
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByRadioGroupRoleByNameAsync(IFrameLocator iframe, string name)
        {
            var element = iframe.GetByRole(AriaRole.Radiogroup, new() { Name = name });
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByRadioGroupRoleByNameAsync(IPage? page, string name)
        {
            var element = page.GetByRole(AriaRole.Radiogroup, new() { Name = name });
            return await IsExistAsync(element);
        }
        #endregion
            #region Treeitem Role
        public static async Task<ILocator> GetByTreeItemAndNameAsync(IPage? page, string name)
        {
            var element = page.GetByRole(AriaRole.Treeitem, new() { Name = name, Exact = true });
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByTreeItemAndNameAsync(IFrameLocator iframe, string name)
        {
            var element = iframe.GetByRole(AriaRole.Treeitem, new() { Name = name, Exact = true });
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByTreeItemAndNameAsync(ILocator locator, string name)
        {
            var element = locator.GetByRole(AriaRole.Treeitem, new() { Name = name, Exact = true });
            return await IsExistAsync(element);
        }
        #endregion
        #region Tab Role
        public static async Task<ILocator> GetByTabRoleAndNameAsync(IPage? page, string name)
        {
            var element = page.GetByRole(AriaRole.Tab, new() { Name = name });
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByTabRoleAndNameAsync(IFrameLocator iframe, string name)
        {
            var element = iframe.GetByRole(AriaRole.Tab, new() { Name = name });
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByTabRoleAndNameAsync(ILocator locator, string name)
        {
            var element = locator.GetByRole(AriaRole.Tab, new() { Name = name });
            return await IsExistAsync(element);
        }
        #endregion
        #region Link Role
        public static async Task<ILocator> GetByLinkRoleAndNameAsync(IPage? page, string name, bool IsStrongWait = true)
        {
            var element = page.GetByRole(AriaRole.Link, new() { Name = name });
            if (!IsStrongWait)
                return element;
            else
                return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByLinkRoleAndNameAsync(IFrameLocator iframe, string name, bool IsStrongWait = true)
        {
            var element = iframe.GetByRole(AriaRole.Link, new() { Name = name, Exact = true });
            if (!IsStrongWait)
                return element;
            else
                return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByLinkRoleAndNameAsync(ILocator locator, string name, bool IsStrongWait = true)
        {
            var element = locator.GetByRole(AriaRole.Link, new() { Name = name });
            if (!IsStrongWait)
                return element;
            else
                return await IsExistAsync(element);
        }
        #endregion
        #region Option Role
        public static async Task<ILocator> GetByOptionAndAriaLableAsync(IPage? page, string ariaLable, bool exact = true)
        {
            var element = page.GetByRole(AriaRole.Option, new() { Name = ariaLable, Exact = exact });
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByOptionAndAriaLableAsync(IFrameLocator iframe, string ariaLable)
        {
            var element = iframe.GetByRole(AriaRole.Option, new() { Name = ariaLable, Exact = true });
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByOptionAndAriaLableAsync(ILocator locator, string ariaLable)
        {
            var element = locator.GetByRole(AriaRole.Option, new() { Name = ariaLable, Exact = true });
            return await IsExistAsync(element);
        }

        public static async Task<ILocator> GetByOptionAndTitleAsync(IPage? page, string title)
        {
            var element = page.Locator($"[role='option'][title='{title}']");
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByOptionAndTitleAsync(IFrameLocator iframe, string title)
        {
            var element = iframe.Locator($"[role='option'][title='{title}']");
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByOptionAndTitleAsync(ILocator locator, string title)
        {
            var element = locator.Locator($"[role='option'][title='{title}']");
            return await IsExistAsync(element);
        }
        #endregion
        #region Presentation Role
        public static async Task<ILocator> GetByPresentationAndNameAsync(IPage? page, string name)
        {
            var element = page?.GetByRole(AriaRole.Presentation, new() { Name = name, Exact = true });
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByPresentationAndNameAsync(IFrameLocator iframe, string name)
        {
            var element = iframe.GetByRole(AriaRole.Presentation, new() { Name = name, Exact = true });
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByPresentationAndNameAsync(ILocator locator, string name)
        {
            var element = locator.GetByRole(AriaRole.Presentation, new() { Name = name, Exact = true });
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByPresentationAndTitleAsync(IPage? page, string title)
        {
            var element = page.Locator($"[role='presentation'][title='{title}']");
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByPresentationAndTitleAsync(IFrameLocator iframe, string title)
        {
            var element = iframe.Locator($"[role='presentation'][title='{title}']");
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByPresentationAndTitleAsync(ILocator locator, string title)
        {
            var element = locator.Locator($"[role='presentation'][title='{title}']");
            return await IsExistAsync(element);
        }
        #endregion
        #region Button Role
        public static async Task<ILocator> GetByButtonRoleAndNameAsync(IPage? page, string name, bool exact = true)
        {
            var element = page.GetByRole(AriaRole.Button, new() { Name = name, Exact = exact });
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByButtonRoleAndNameAsync(IFrameLocator iframe, string name, bool exact = true)
        {
            var element = iframe.GetByRole(AriaRole.Button, new() { Name = name, Exact = exact });
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByButtonRoleAndNameAsync(ILocator locator, string name, bool exact = true, int retry = 150)
        {
            var element = locator.GetByRole(AriaRole.Button, new() { Name = name, Exact = exact });
            return await IsExistAsync(element, retry);
        }

        public static async Task<ILocator> GetByButtonRoleAndHasTextAsync(IPage? page, string hasText, bool IsStrongWait = true)
        {
            var element = page.GetByRole(AriaRole.Button).Filter(new LocatorFilterOptions { HasText = hasText });
            if (!IsStrongWait)
                return element;
            else
                return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByButtonRoleAndHasTextAsync(IFrameLocator iframe, string hasText, bool IsStrongWait = true)
        {
            var element = iframe.GetByRole(AriaRole.Button).Filter(new LocatorFilterOptions { HasText = hasText });
            if (!IsStrongWait)
                return element;
            else
                return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByButtonRoleAndHasTextAsync(ILocator locator, string hasText, bool IsStrongWait = true)
        {
            var element = locator.GetByRole(AriaRole.Button).Filter(new LocatorFilterOptions { HasText = hasText });
            if (!IsStrongWait)
                return element;
            else
                return await IsExistAsync(element);
        }
        #endregion
        #region Gridcell Role
        public static async Task<ILocator> GetByGridcellAndHasTextAsync(IPage? page, string? hasText, bool IsStrongWait = true)
        {
            var element = page.GetByRole(AriaRole.Gridcell).Filter(new LocatorFilterOptions { HasTextString = hasText });
            if (!IsStrongWait)
                return element;
            else
                return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByGridcellAndHasTextAsync(IFrameLocator iFrame, string? hasText, bool IsStrongWait = true)
        {
            var element = iFrame.GetByRole(AriaRole.Gridcell).Filter(new LocatorFilterOptions { HasTextString = hasText });
            if (!IsStrongWait)
                return element;
            else
                return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetByGridcellAndHasTextAsync(ILocator locator, string? hasText)
        {
            var element = locator.GetByRole(AriaRole.Gridcell).Filter(new LocatorFilterOptions { HasTextString = hasText });
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetGridcellByNameAsync(IPage? page, string name)
        {
            var element = page.GetByRole(AriaRole.Gridcell, new() { Name = name, Exact = true });
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetGridcellByNameAsync(IFrameLocator iFrame, string name)
        {
            var element = iFrame.GetByRole(AriaRole.Gridcell, new() { Name = name, Exact = true });
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetGridcellByNameAsync(ILocator locator, string name)
        {
            var element = locator.GetByRole(AriaRole.Gridcell, new() { Name = name, Exact = true });
            return await IsExistAsync(element);
        }
        #endregion
        #region Get rowheader role by has text
        public static async Task<ILocator> GetRowheaderByHasTextAsync(IPage? page, string hasText)
        {
            var element = page.GetByRole(AriaRole.Rowheader).Filter(new LocatorFilterOptions { HasTextString = hasText });
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetRowheaderByHasTextAsync(IFrameLocator iFrame, string hasText)
        {
            var element = iFrame.GetByRole(AriaRole.Rowheader).Filter(new LocatorFilterOptions { HasTextString = hasText });
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetRowheaderByHasTextAsync(ILocator locator, string hasText)
        {
            var element = locator.GetByRole(AriaRole.Rowheader).Filter(new LocatorFilterOptions { HasTextString = hasText });
            return await IsExistAsync(element);
        }
        #endregion
        #region Get row role by name
        public static async Task<ILocator> GetRowByNameAsync(IPage? page, string name, bool exact = true)
        {
            var element = page.GetByRole(AriaRole.Row, new() { Name = name, Exact = exact });
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetRowByNameAsync(IFrameLocator iFrame, string name, bool exact = true)
        {
            var element = iFrame.GetByRole(AriaRole.Row, new() { Name = name, Exact = exact });
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetRowByNameAsync(ILocator locator, string name, bool exact = true)
        {
            var element = locator.GetByRole(AriaRole.Row, new() { Name = name, Exact = exact });
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetRowByHasTextAsync(ILocator locator, string hasText, bool exact = true)
        {
            var element = locator.GetByRole(AriaRole.Row).Filter(new LocatorFilterOptions { HasTextString = hasText });
            return await IsExistAsync(element);
        }
        public static async Task<ILocator> GetRowByHasTextAsync(IFrameLocator iFrame, string hasText, bool exact = true)
        {
            var element = iFrame.GetByRole(AriaRole.Row).Filter(new LocatorFilterOptions { HasTextString = hasText });
            return await IsExistAsync(element);
        }
        #endregion
        #endregion

        #region Check locator is exist or not
        public static async Task<ILocator> IsExistAsync(ILocator? locator, int retry = 10, bool isNeedSleep = true)
        {
            if (locator == null)
            {
                throw new();
            }
            if (isNeedSleep)
            {
                Thread.Sleep(500);
            }
            int count = 0;
            while (true)
            {
                try
                {
                    count++;
                    var locatorCount = 0;
                    try
                    {
                        locatorCount = await locator.CountAsync();
                    }
                    catch (Exception ex) { }
                    if (locatorCount != 0)
                    {
                        if (isNeedSleep)
                        {
                            Thread.Sleep(200);
                        }
                        return locator;
                    }
                    else
                    {
                        //LogHelper.Info($"Try `{count}` check if current locator is exist ");
                        if (count >= retry)
                        {
                            //LogHelper.Error($"Get current locator failed {locator.ToString()}");
                            Console.WriteLine($"[IsExistAsync] Failed to find element: {locator} after ms.");
                            throw new Exception($"Element was not found within  ms. Selector: {locator}");

                        }
                        if (isNeedSleep)
                        {
                            Thread.Sleep(200);
                        }
                    }
                }
                
                catch (Exception ex)
                {
                //    throw new CustomLogException(CustomExceptionPrefix.CodeError_Element_Load_Failed, ex);
                }
            }
        }

        #endregion
        #region check element is enable or not
        public static async Task<bool> IsEnabledAsync(ILocator element, int waitTime = 200, int waitTimes = 150)
        {
            try
            {
                var elementStatus = false;
                if (element == null)
                {
                    throw new();              }
                elementStatus = await element.IsEnabledAsync();
                int i = 1;
                while (i <= waitTimes && !elementStatus) // max wait 30s
                {
                    Thread.Sleep(waitTime);
                    elementStatus = await element.IsEnabledAsync();
                    //LogHelper.Info($"Wait {waitTime * i}ms, the element enabled status: {elementStatus}...");
                    i++;
                }
                return elementStatus;
            }
            
            catch (Exception ex)
            {
                //    LogHelper.Error(ex.Message);
                throw new();
            }
        }
        #endregion
    }
}
