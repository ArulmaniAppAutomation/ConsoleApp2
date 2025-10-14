using Account_Management.Framework;
using Account_Management.Pages;
using ConsoleApp2.Framework;
using ConsoleApp2.Pages;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account_Management.Tests
{
    [TestFixture]
    [Category("LOB")]
    public class IntuneLobTests : BaseTest
    {
        public static IEnumerable<TestCaseData> GetLobTestCases()
        {
            var jsonFilePath = Path.Combine(AppContext.BaseDirectory, "TestData", "LobApps.json");
            var testCases = DataLoader.LoadFromFile(jsonFilePath);

            foreach (var testCase in testCases)
            {
                yield return new TestCaseData(testCase)
                    .SetName($"LOBAppTest_{testCase.TestCaseName.Replace(" ", "_")}");
            }
        }

        [Test, TestCaseSource(nameof(GetLobTestCases))]
        public async Task CanCreateLobApp(RootObject testCase)
        {
            var portalUrl = GetPortalUrl("CTiP");
            var account = GetAccount("CTiP");

            // Require certificate info
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
            var lob = new IntuneLobApps(Page, portalUrl);

            await home.NavigateAsync();
            await home.LoginIfNeededAsync(account);

            // navigate to create app
            await apps.AllApps_Click();
            await apps.app_Click();
            await apps.CreateBUtton_Click();

            await lob.CreateLobAppAsync(testCase);
        }
    }
}
