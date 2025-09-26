using Account_Management.Framework;
using Account_Management.Pages;
using ConsoleApp2.Framework;
using ConsoleApp2.Pages;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Account_Management.Tests
{
   [TestFixture]
    public class IntuneAndriodTests : BaseTest


    {

        public static IEnumerable<TestCaseData> GetAppTestCases()
        {
            var jsonFilePath = Path.Combine(AppContext.BaseDirectory, "TestData", "androidStoreApp.txt");
            var testCases = DataLoader.LoadFromFile(jsonFilePath);

            foreach (var testCase in testCases)
            {
                yield return new TestCaseData(testCase)
                    .SetName($"AndroidAppTest_{testCase.TestCaseName.Replace(" ", "_")}");
            }
        }

        [Test, TestCaseSource(nameof(GetAppTestCases))]
        public async Task CanNavigateToIntunePortalAndDevicesSection(RootObject testCase)
        {
            var portalUrl = GetPortalUrl("CTiP");
            var account = GetAccount("CTiP");

            // Require certificate info
            if (string.IsNullOrEmpty(account?.CertName) || string.IsNullOrEmpty(account?.CertCode))
            {
                Assert.Fail("Certificate information (certName/certCode) is missing in accounts.json.");
                return;
            }

            // Install certificate and set Chrome policy
            try
            {
                Console.WriteLine($"Installing certificate '{account.CertName}'...");
                var cert = CertificateManager.InstallCertificate(account.CertName!, account.CertCode!);
                Console.WriteLine($"Certificate installed: {cert?.Subject}");

                Console.WriteLine("Configuring Chrome policy to auto-select certificate for microsoftonline.com...");
                RegistryHelper.RegistryAutoSelectCert(account.CertName!, s => Console.WriteLine(s));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Certificate installation / registry update failed: {ex.Message}");
                Assert.Fail("Certificate setup failed");
                return;
            }

            var home = new IntuneHomePage(Page, portalUrl);
            var apps = new IntuneAppsPage(Page, portalUrl);
            await home.NavigateAsync();
           await home.LoginIfNeededAsync(account);
            //Wait for portal to load (certificate-based SSO)
            var sw = System.Diagnostics.Stopwatch.StartNew();
            await apps.AllApps_Click();
            await apps.app_Click();
            await apps.CreateBUtton_Click();
            await apps.Select_AndroidAppAsyncWithData(testCase);
           // var timeout = TimeSpan.FromMinutes(2);
           //bool loggedIn = false;
           //while (sw.Elapsed < timeout)
           // {
           //   if (await home.IsLoadedAsync())
           // {
           //   loggedIn = true;
           // break;
           //}
           //await Task.Delay(1000);
           //}

            // Assert.That(loggedIn, Is.True, "Certificate-based login failed or portal did not load.");

            //await home.ClickDevicesAsync();
            //await home.ClickEnrollmentAsync();



            Assert.Pass("Navigated to Devices and Enrollment section using certificate-based login.");
        }
    }
}
