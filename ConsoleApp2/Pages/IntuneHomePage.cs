using ConsoleApp2.Config;
using Microsoft.Playwright;
using System;
using System.Threading.Tasks;

namespace ConsoleApp2.Pages
{
    public class IntuneHomePage
    {
        private readonly IPage _page;
        private readonly string _portalUrl;

        // Common selectors for Devices
        private readonly string[] _devicesSelectors = new[]
        {
            "a[title='Devices']",
            "a[aria-label='Devices']",
            "text=Devices"
        };

        // Common selectors for Enrollment
        private readonly string[] _enrollmentSelectors = new[]
        {
            "//div[text()='Enrollment']",
            "a[aria-label='Enrollment']",
            "text=Enrollment",
            "text=Enrollments",
            "[data-test-id*='enrollment']"
        };

        public IntuneHomePage(IPage page, string portalUrl)
        {
            _page = page;
            _portalUrl = portalUrl;
        }

        public async Task NavigateAsync()
        {
            // Use a longer timeout and fallback to WaitUntilState.DOMContentLoaded if networkidle fails
            try
            {
                await _page.GotoAsync(_portalUrl, new PageGotoOptions { WaitUntil = WaitUntilState.NetworkIdle, Timeout = 90000 });
            }
            catch (TimeoutException)
            {
                Console.WriteLine($"NetworkIdle timed out, retrying with DOMContentLoaded for {_portalUrl}");
                await _page.GotoAsync(_portalUrl, new PageGotoOptions { WaitUntil = WaitUntilState.DOMContentLoaded, Timeout = 90000 });
            }
        }

        public async Task<bool> IsLoadedAsync()
        {
            try
            {
                await _page.WaitForURLAsync($"**{_portalUrl.Replace("https://", "" )}**", new PageWaitForURLOptions { Timeout = 60000 });
                return _page.Url.Contains(_portalUrl.Replace("https://", ""));
            }
            catch
            {
                return false;
            }
        }

        public async Task ClickDevicesAsync()
        {
            await ClickBySelectorsAsync(_devicesSelectors, "Devices");
        }

        public async Task ClickEnrollmentAsync()
        {
            await ClickBySelectorsAsync(_enrollmentSelectors, "Enrollment");
        }

        private async Task ClickBySelectorsAsync(string[] selectors, string label)
        {
            foreach (var sel in selectors)
            {
                try
                {
                    var locator = _page.Locator(sel).First;
                    await locator.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 30000 });
                    await locator.ScrollIntoViewIfNeededAsync();
                    await locator.ClickAsync(new LocatorClickOptions { Timeout = 30000 });
                    await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
                    Console.WriteLine($"Clicked {label} using selector: {sel}");
                    return;
                }
                catch (PlaywrightException)
                {
                    Console.WriteLine($"Selector '{sel}' for {label} not found or not clickable. Trying next selector...");
                }
            }
            // Fallback: try JS click by link text
            try
            {
                await _page.EvaluateAsync($@"() => {{
                    const anchors = Array.from(document.querySelectorAll('a'));
                    const match = anchors.find(a => /{label}/i.test(a.textContent || ''));
                    if (match) match.click();
                }}");
                await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
                Console.WriteLine($"Clicked {label} using JS fallback.");
            }
            catch
            {
                Console.WriteLine($"Failed to click {label} using all selectors and JS fallback.");
            }
        }

        public async Task<bool> LoginIfNeededAsync(Account account)
        {
            if (!_page.Url.Contains("login.microsoftonline.com") && !_page.Url.Contains("login.windows.net"))
            {
                foreach (var sel in _devicesSelectors)
                {
                    var count = await _page.Locator(sel).CountAsync();
                    if (count > 0)
                        return true;
                }
            }
            var loginField = await _page.QuerySelectorAsync("input[name='loginfmt']");
            if (loginField == null && !_page.Url.Contains("login.microsoftonline.com"))
                return true;
            await _page.FillAsync("input[name='loginfmt']", account.IbizaUser);
            await ClickIfExistsAsync("#idSIButton9, input[type='submit'], button[type='submit']");
            //await _page.WaitForSelectorAsync("input[name='passwd']", new PageWaitForSelectorOptions { Timeout = 30000 });
            //await _page.FillAsync("input[name='passwd']", account.IbizaUserCode);
            //await ClickIfExistsAsync("#idSIButton9, input[type='submit'], button[type='submit']");
            try
            {
                //await _page.WaitForSelectorAsync("#idSIButton9", new PageWaitForSelectorOptions { Timeout = 10000 });
                //await ClickIfExistsAsync("//*[@id='idSIButton9']");
                await _page.WaitForSelectorAsync("//*[@id='idSIButton9']", new PageWaitForSelectorOptions { Timeout = 10000 });
                await _page.ClickAsync("//*[@id='idSIButton9']");
            }
            catch { }
            try
            {
                var timeout = 120000;
                var sw = System.Diagnostics.Stopwatch.StartNew();
                while (sw.ElapsedMilliseconds < timeout)
                {
                    if (_page.Url.Contains(_portalUrl.Replace("https://", "")))
                        break;
                    foreach (var sel in _devicesSelectors)
                    {
                        var count = await _page.Locator(sel).CountAsync();
                        if (count > 0)
                            return true;
                    }
                    await Task.Delay(1000);
                }
                if (_page.Url.Contains(_portalUrl.Replace("https://", "")))
                    return true;
                return false;
            }
            catch
            {
                return false;
            }
        }

        private async Task ClickIfExistsAsync(string selector)
        {
            try
            {
                var handle = await _page.QuerySelectorAsync(selector);
                if (handle != null)
                {
                    await handle.ClickAsync();
                }
            }
            catch { }
        }
    }
}
