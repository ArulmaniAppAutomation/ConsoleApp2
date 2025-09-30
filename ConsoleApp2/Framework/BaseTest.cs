using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Playwright;
using NUnit.Framework;
using ConsoleApp2.Config;

namespace ConsoleApp2.Framework
{
    public abstract class BaseTest
    {
        protected IPlaywright PlaywrightInstance { get; private set; } = null!;
        protected IBrowser Browser { get; private set; } = null!;
        protected IBrowserContext Context { get; private set; } = null!;
        protected IPage Page { get; private set; } = null!;

        private const string StorageStatePath = "storage.json";
        private const string AccountsJson = "accounts.json";

        private Account? _activeAccount;

        [OneTimeSetUp]
        public async Task OneTimeSetUpAsync()
        {
            _activeAccount = GetAccount() ?? new Account();

            PlaywrightInstance = await Microsoft.Playwright.Playwright.CreateAsync();
            Browser = await PlaywrightInstance.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Channel = "chrome",
                Headless = false //
                              //set to true for CI
            });
        }

        [SetUp]
        public async Task SetUpAsync()
        {
           // var contextOptions = new BrowserNewContextOptions();
            //if (File.Exists(StorageStatePath))
           // {
           //     contextOptions.StorageStatePath = StorageStatePath;
          //  }

            Context = await Browser.NewContextAsync();
            Page = await Context.NewPageAsync();
            await Page.SetViewportSizeAsync(1900, 1080);

            await Context.Tracing.StartAsync(new TracingStartOptions
            {
                Screenshots = true,
                Snapshots = true,
                Sources = true
            });

        }

        [TearDown]
        public async Task TearDownAsync()
        {

            Console.WriteLine("Current working directory: " + Directory.GetCurrentDirectory());


            if (Context != null)
            {




                await Context.Tracing.StopAsync(new TracingStopOptions
                {
                    Path = "trace.zip"
                });

                // Save storage state
                try
                {
                    await Context.StorageStateAsync(new BrowserContextStorageStateOptions { Path = StorageStatePath });
                }
                catch { }

                if (!(_activeAccount?.KeepBrowserOpen ?? true))
                {
                    await Context.CloseAsync(); // ? Now this only runs if KeepBrowserOpen is false
                } }

                
            }
        

        
        public async Task OneTimeTearDownAsync()
        {
            if (Browser != null)
            {
             //   await Browser.CloseAsync();
            }

           // PlaywrightInstance?.Dispose();
        }

        /// <summary>
        /// Locates accounts.json by checking:
        ///  - current working directory
        ///  - the test/app base directory (AppContext.BaseDirectory)
        ///  - parent directories of the working directory
        /// </summary>
        private string? FindAccountsFilePath()
        {
            // 1) current working directory
            var cwdPath = Path.Combine(Directory.GetCurrentDirectory(), AccountsJson);
            if (File.Exists(cwdPath))
                return cwdPath;

            // 2) app base directory (where test runner loads assemblies)
            var baseDirPath = Path.Combine(AppContext.BaseDirectory ?? string.Empty, AccountsJson);
            if (File.Exists(baseDirPath))
                return baseDirPath;

            // 3) walk parent dirs from current directory upward (useful when running from different working folders)
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

        protected Account? GetAccount(string environment = "CTiP")
        {
            var path = FindAccountsFilePath();
            if (string.IsNullOrEmpty(path))
                return null;

            try
            {
                var json = File.ReadAllText(path);
                var accounts = JsonSerializer.Deserialize<AccountsFile>(json);
                return accounts?.Accounts?.Find(a => string.Equals(a.Environment, environment, StringComparison.OrdinalIgnoreCase));
            }
            catch
            {
                return null;
            }
        }

        protected string GetPortalUrl(string environment = "CTiP")
        {
            var account = GetAccount(environment);
            return account?.PortalUrl ?? "https://endpoint.microsoft.com";
        }
    }
}
