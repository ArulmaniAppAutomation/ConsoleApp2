using Microsoft.Playwright;
using System;
using System.IO;
using System.Text.Json;
using ConsoleApp2.Config;
using System.Threading.Tasks;

class Program
{
    static string? FindAccountsFilePath()
    {
        const string AccountsJson = "accounts.json";

        var cwdPath = Path.Combine(Directory.GetCurrentDirectory(), AccountsJson);
        if (File.Exists(cwdPath))
            return cwdPath;

        var baseDirPath = Path.Combine(AppContext.BaseDirectory ?? string.Empty, AccountsJson);
        if (File.Exists(baseDirPath))
            return baseDirPath;

        var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (dir != null)
        {
            var candidate = Path.Combine(dir.FullName, AccountsJson);
            if (File.Exists(candidate))
                return candidate;
            dir = dir.Parent;
        }

        return null;
    }

    public static async Task Main()
    {
        // Locate accounts.json robustly
        var accountsPath = FindAccountsFilePath();
        AccountsFile? accountsFile = null;
        if (accountsPath != null)
        {
            try
            {
                var json = File.ReadAllText(accountsPath);
                accountsFile = JsonSerializer.Deserialize<AccountsFile>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to read accounts.json: {ex.Message}");
            }
        }

        var account = accountsFile?.Accounts?.Find(a => a.Environment == "CTiP");
        var portalUrl = account?.PortalUrl ?? "https://endpoint.microsoft.com";
        var username = account?.IbizaUser;
        var password = account?.IbizaUserCode;

        using var playwright = await Playwright.CreateAsync();
        var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Channel = "chrome",
            Headless = false
        });

        BrowserNewContextOptions contextOptions = new BrowserNewContextOptions();
        var context = await browser.NewContextAsync(contextOptions);
        var page = await context.NewPageAsync();

        // Maximize window (set to a large size)
        await page.SetViewportSizeAsync(1920, 1080);

        Console.WriteLine($"Navigating to {portalUrl} ...");
        await page.GotoAsync(portalUrl, new PageGotoOptions { WaitUntil = WaitUntilState.NetworkIdle });

        // Optionally, perform login if username/password present
        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
        {
            var loginField = await page.QuerySelectorAsync("input[name='loginfmt']");
            if (loginField != null || page.Url.Contains("login.microsoftonline.com"))
            {
                Console.WriteLine("Logging in with credentials from JSON...");
                await page.FillAsync("input[name='loginfmt']", username);
                await ClickIfExists(page, "#idSIButton9, input[type='submit'], button[type='submit']");
                await page.WaitForSelectorAsync("input[name='passwd']", new PageWaitForSelectorOptions { Timeout = 15000 });
                await page.FillAsync("input[name='passwd']", password);
                await ClickIfExists(page, "#idSIButton9, input[type='submit'], button[type='submit']");
                try
                {
                    await page.WaitForSelectorAsync("#idSIButton9", new PageWaitForSelectorOptions { Timeout = 5000 });
                    await ClickIfExists(page, "#idSIButton9");
                }
                catch { }
            }
        }

        Console.WriteLine("Press Enter to close the browser and exit...");
        Console.ReadLine();

        await browser.CloseAsync();
    }

    static async Task ClickIfExists(IPage page, string selector)
    {
        try
        {
            var handle = await page.QuerySelectorAsync(selector);
            if (handle != null)
            {
                await handle.ClickAsync();
            }
        }
        catch { }
    }
}