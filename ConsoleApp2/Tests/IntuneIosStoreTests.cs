using Account_Management.Framework;
using Account_Management.Pages;
using ConsoleApp2.Framework;
using ConsoleApp2.Pages;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Account_Management.Tests
{
    [TestFixture]
    [Category("IOS")]
    public class IntuneIosStoreTests : BaseTest
    {
        private static IEnumerable<RootObject> LoadIosTestCases()
        {
            var jsonPath = Path.Combine(AppContext.BaseDirectory, "TestData", "IosApps.json");
            if (File.Exists(jsonPath))
            {
                return DataLoader.LoadFromFile(jsonPath);
            }

            // Fallback sample test case when JSON not present
            return new[] {
                new RootObject
                {
                    TestCaseName = "IOS_Sample",
                    TestCaseID = 999999,
                    AppType = new List<string>{ "iOS store app" },
                    AppSearchString = "Microsoft Teams",
                    AppInfo = new AppInfo
                    {
                        Name = null,
                        Description = "UI Automation iOS sample",
                        Publisher = "AppLifeEx",
                        Developer = "SampleDev",
                        Owner = "SampleOwner"
                    },
                    AssignmentEntities = new List<AssignmentEntity>()
                }
            };
        }

        [Test]
        public async Task CanCreateIosStoreApp()
        {
            var testCases = LoadIosTestCases();
            foreach (var testCase in testCases)
            {
                var portalUrl = GetPortalUrl("CTiP");
                var account = GetAccount("CTiP");

                if (string.IsNullOrEmpty(account?.CertName) || string.IsNullOrEmpty(account?.CertCode))
                {
                    Assert.Fail("Certificate information (certName/certCode) is missing in accounts.json.");
                    return;
                }

                try
                {
                    var cert = CertificateManager.InstallCertificate(account.CertName!, account.CertCode!);
                    RegistryHelper.RegistryAutoSelectCert(account.CertName!, s => Console.WriteLine(s));
                }
                catch (Exception ex)
                {
                    Assert.Fail("Certificate setup failed: " + ex.Message);
                    return;
                }

                var home = new IntuneHomePage(Page, portalUrl);
                var apps = new IntuneAppsPage(Page, portalUrl);
                var ios = new IntuneIosStoreApps(Page, portalUrl);

                await home.NavigateAsync();
                await home.LoginIfNeededAsync(account);

                // navigate to create app
                await apps.AllApps_Click();
                await apps.app_Click();
                await apps.CreateBUtton_Click();

                await ios.CreateIosStoreAppAsync(testCase);
            }
        }
    }
}
