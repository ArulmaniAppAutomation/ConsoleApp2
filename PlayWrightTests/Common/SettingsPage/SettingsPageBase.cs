using AccountManagement;
using EWags.TaskFactory;
using LogService;
using LogService.Extension;
using Microsoft.Playwright;
using Microsoft.VisualStudio.Threading;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using PlaywrightTests.Common.Helper;
using PlaywrightTests.Common.Model;
using System.IO;
using static PlaywrightTests.Common.Helper.EnumHelper;

namespace PlaywrightTests.Common.SettingsPage
{
    public abstract class SettingsPageBase
    {
        public string CurrentEnvironment { get; set; }
        public BaseEntity baseEntity;
        public static AsyncEventHandler SetEvent;
        public async Task ExecuteSetStepEventAsync(BaseEntity entity)
        {
            await SetEvent.InvokeAsync(entity, EventArgs.Empty);
        }

        public SettingsPageBase(TestAccount account, BaseEntity entity)
        {
            CurrentEnvironment = account.Environment;
            LogHelper.Info($">>> Get certificate content from Secret Identifier");
            var certContent = E2ERunner.GetKeyVaultSecretValue(account.CertSecretIdentifier);
            LogHelper.Info($">>> Install certificate");
            CertificateManager.InstallCertificate(certContent, account);
            LogHelper.Info($">>> Edit registry");
           // RegistryAutoSelectCertAsync(entity.Ipage, account.CertName).Wait();
           // LoginAsync(account, entity).Wait(120000);
           // entity.AppAutomationBrowserOpen = StepResultStatus.Success.ToString();
        }
        public static async Task LoginAsync(TestAccount accountInfo, BaseEntity entity)
        {
            try
            {
                LogHelper.Info($">>> Sign in Intune Portal");
                // this.Page = await browser?.NewPageAsync();
                //LogHelper.Info($"Entry ${accountInfo.EntryURL}");

                await ControlHelper.RetryAsync(3, async () =>
                {
                    Thread.Sleep(3000);
                    await entity.Ipage.GotoAsync(accountInfo.EntryURL);
                });
                // you need to add your tenant account
                var inputBox = entity.Ipage.GetByPlaceholder("Email, phone, or Skype");
                bool inputIsvisible = await inputBox.IsVisibleAsync();
                LogHelper.Info("Wait for page loaded...");
                await ControlHelper.WaitPageLoadedAsync(inputBox);
                LogHelper.Info($"Input Username: {accountInfo.IbizaUser}");
                await inputBox.FillAsync(accountInfo.IbizaUser);
                LogHelper.Info($"Click \"Next\"");
                await entity.Ipage.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Next" }).ClickAsync();

                //CommonOperations.loginByPassWithCert(accountInfo.Environment);              

                //LogHelper.Info($"Input Password");
                //// you need to add your tenant account password
                //await entity.Ipage.GetByRole(AriaRole.Textbox, new PageGetByRoleOptions { Name = "Password" }).FillAsync(accountInfo.Password);
                //LogHelper.Info($"Click \"Sign in\"");
                //await entity.Ipage.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Sign in" }).ClickAsync();

                #region check if "Use a certificate or smart card" exists, if so, click it
                var useCert = entity.Ipage.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "Use a certificate or smart card" });
                Thread.Sleep(3000);
                var useCertVisible = await useCert.IsVisibleAsync();
                LogHelper.Info($"Use a certificate or smart card is visible: {useCertVisible}");
                if (useCertVisible)
                {
                    LogHelper.Info("Click \"Use a certificate or smart card\".");
                    await useCert.ClickAsync();
                }
                #endregion

                LogHelper.Info($"Click\"Yes\"");
                await entity.Ipage.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Yes" }).ClickAsync();
                LogHelper.Info($"Current environment \"{accountInfo.Environment}\".");
                //copilot doesn't required an environment, and doesn't need to navigate to a new page
                if (!accountInfo.SupportFeatureType.Contains("Copilot"))
                {
                    await ControlHelper.RetryAsync(3, async () =>
                    {
                        Thread.Sleep(3000);
                        await entity.Ipage.GotoAsync(accountInfo.EntryURL);
                    });
                }
                accountInfo.SetPrefixUrl(entity.Ipage.Url.Replace("#home", ""));
                LogHelper.Info($"url prefix is {accountInfo.PrefixUrl}");
                //await GetPortalDiagnosticsInfoAsync(entity);

                LogHelper.Info($">>> Sign in success");

                // check and switch language to target language
                //BaseCommonUtils baseCommonUtils = new BaseCommonUtils(entity.Ipage, accountInfo.Environment);
                //var currentLanguage = baseCommonUtils.LanguageTransfer(entity.Ipage.Locator("html").GetAttributeAsync("lang").Result);
                //if (currentLanguage.Equals( accountInfo.Language))
                //{
                //    LogHelper.Info($"Current language is {currentLanguage}");
                //}
                //else
                //{
                //    LogHelper.Info($"Current language is {currentLanguage}, switch to {accountInfo.Language}");
                //    await baseCommonUtils.SwitchLanguageAsync(accountInfo.Language);
                //}

                entity.ExtensionPageVersion = await GetPageVersionFromJsonFileAsync(entity.Ipage,entity.TestCaseId);
            }
            catch (CustomLogException)
            {
                throw;
            }
            catch (Exception ex)
            {
                LogHelper.Error("login failed...");
                throw new CustomLogException("login failed...", ex);
            }
        }

        public virtual async Task RunAsync()
        {

        }

        public virtual async void Dispose()
        {

        }
        public static async Task RegistryAutoSelectCertAsync(IPage? page, string certName)
        {
            var browserType = AccountsHelper.ConfigInfo.TestConfigInfo.TestBrowser;
            LogHelper.Info($"Target browser: {browserType}");
            LogHelper.Info($"Cert name: {certName}");
            var browserCompany = BrowserTypeMapping().browserCompany;
            LogHelper.Info($"Browser company: {browserCompany}");
            var browserName = BrowserTypeMapping().browserName;
            LogHelper.Info($"Browser: {browserName}");

            var registryPath = $"SOFTWARE\\Policies\\{browserCompany}\\{browserName}\\AutoSelectCertificateForUrls";
            LogHelper.Info($"Registry path: {registryPath}");

            #region if the registry key is not exist, create it
            string regValueKey = "1";
            string regValueData = "{\"pattern\":\"https://[*.]microsoftonline.com/*\", \"filter\":{\"SUBJECT\":{\"CN\":\"" + certName + "\"}}}";

            RegistryKey regKey = Registry.LocalMachine;
            var targetPolicy = regKey.OpenSubKey(registryPath, true);
            if (targetPolicy == null)
            {
                LogHelper.Info("Create new registry key...");
                regKey.CreateSubKey(registryPath);

            }
            targetPolicy = regKey.OpenSubKey(registryPath, true);
            var isExistKey = targetPolicy.GetValue(regValueKey);
            if (isExistKey == null || !isExistKey.ToString().Contains(certName))
            {
                LogHelper.Info("Start setting policy's value...");
                targetPolicy.SetValue(regValueKey, regValueData, RegistryValueKind.String);
            }
            LogHelper.Info($"Reload {browserName}'s policies...");
            await ReloadBrowserPolicyAsync(page, browserName, certName);
            ConsoleHelper.ColoredResult(ConsoleColor.Green, $"Policy is updated successfully...");
            ConsoleHelper.ColoredResult(ConsoleColor.Green, $"The current certificate is target value, start testing...");
            #endregion
        }
        private static async Task ReloadBrowserPolicyAsync(IPage? page, string browserName, string certName)
        {
            LogHelper.Info($"Go to {browserName}://policy/");
            await page.GotoAsync($"{browserName}://policy/");
            var reloadPolicies = page.GetByText("Reload Policies");
            LogHelper.Info("Wait for page loaded...");
            await ControlHelper.WaitPageLoadedAsync(reloadPolicies);
            var reloadPoliciesStatus = await reloadPolicies.IsVisibleAsync();
            LogHelper.Info("Reload Policies: " + reloadPoliciesStatus);
            if (!reloadPoliciesStatus)
            {

                throw new CustomLogException("Reload Policies button is not visible...");
            }
            LogHelper.Info("Click \"Reload Policies\"");
            await reloadPolicies.ClickAsync();

            var policy = page.GetByText("AutoSelectCertificateForUrls");
            var targetCert = page.GetByText(certName);
            //CommonOperations.WaitLongTime();
            LogHelper.Info("Wait for policy loaded...");
            //await Controls.WaitPageLoadedAsync(policy);
            await ControlHelper.WaitPageLoadedAsync(targetCert);

            var policyStatus = await policy.IsVisibleAsync();
            var targetCertStatus = await targetCert.CountAsync();
            LogHelper.Info("Target policy display status: " + policyStatus);
            LogHelper.Info("Target cert display status: " + targetCertStatus);

            if (!policyStatus && targetCertStatus > 0)
            {

                throw new CustomLogException("New add policy invalid...");
            }
            ConsoleHelper.ColoredResult(ConsoleColor.Green, $"Reload {browserName}'s policies successfully...");
        }
        public static (string browserCompany, string browserName) BrowserTypeMapping()
        {
            string browserCompany = string.Empty;
            string browserName = string.Empty;
            var browserType = AccountsHelper.ConfigInfo.TestConfigInfo.TestBrowser;
            switch (Convert.ToString(browserType))
            {
                case "chrome":
                    browserCompany = "Google";
                    browserName = "Chrome";

                    break;
                case "msedge":
                    browserCompany = "Microsoft";
                    browserName = "Edge";

                    break;
            }
            return (browserCompany, browserName);
        }

        public static async Task<string> GetPageVersionFromJsonFileAsync(IPage currentPage,string testID)
        {
            string pageVersion = string.Empty;
            string jsonFilePath = System.Environment.GetEnvironmentVariable("USERPROFILE") + @"\Downloads\" + testID + "_PortalDiagnostics.json";
            try
            {               
                if (File.Exists(jsonFilePath))
                {
                    File.Delete(jsonFilePath);
                }              
                if (!File.Exists(jsonFilePath))
                {
                    await currentPage.WaitForLoadStateAsync(LoadState.DOMContentLoaded);                   
                    int count = 0;
                    do
                    {
                        try
                        {
                            var download = await currentPage.RunAndWaitForDownloadAsync(async () =>
                            {
                                await currentPage.Keyboard.DownAsync("Control");
                                await currentPage.Keyboard.DownAsync("Alt");
                                await currentPage.Keyboard.DownAsync("a");
                                await currentPage.Keyboard.UpAsync("Control");
                                await currentPage.Keyboard.UpAsync("Alt");
                                await currentPage.Keyboard.UpAsync("a");
                                LogHelper.Info("Wait the PortalDiagnostics.json download completed");

                            }, new PageRunAndWaitForDownloadOptions() { Timeout = 15000 });
                            await download.SaveAsAsync(jsonFilePath);
                            Thread.Sleep(500);
                        }
                        catch
                        {
                            LogHelper.Warning("The PortalDiagnostics.json file is not downloaded yet, please wait a moment.");
                        }
                        count++;
                    }
                    while (!File.Exists(jsonFilePath) && count < 20);

                }
                if (File.Exists(jsonFilePath))
                {
                    JObject jObject = JObject.Parse(File.ReadAllText(jsonFilePath));
                    JToken accounts = jObject.Children<JProperty>().FirstOrDefault(x => x.Name == "extensions").Value;

                    try
                    {
                        foreach (JToken account in accounts.Children())
                        {
                            JEnumerable<JProperty> accountProperties = account.Children<JProperty>();
                            if ((string)accountProperties.FirstOrDefault(x => x.Name == "name").Value == "Microsoft_Intune_Apps")
                            {
                                pageVersion = (string)accountProperties.FirstOrDefault(x => x.Name == "pageVersion").Value;
                                if (pageVersion.IndexOf("-") > 0)
                                {
                                    pageVersion = pageVersion.Substring(0, pageVersion.IndexOf("-"));
                                }
                                LogHelper.Info("Successfully get pageVersion : " + pageVersion);
                            }
                        }
                    }
                    catch
                    {
                        LogHelper.Warning("Cannot get the Microsft_Intune_Apps name protery in the json file, used the last value instead of.");
                        JToken a = accounts.Last();
                        JEnumerable<JProperty> aProperties = a.Children<JProperty>();
                        pageVersion = (string)aProperties.FirstOrDefault(x => x.Name == "pageVersion").Value;
                        LogHelper.Info("Successfully get pageVersion : " + pageVersion);
                    }
                }
                else
                {
                    LogHelper.Error("Please verify your Chrome download path is: " + jsonFilePath + ", the file cannot be downloaded successfully");
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("Please verify your json file, the download path is: " + jsonFilePath + ", " + ex.Message);
            }
            LogHelper.Info("The Extension Page Version is: " + pageVersion);
            return pageVersion;
        }
    }
}
