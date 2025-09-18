using System.Threading.Tasks;
using NUnit.Framework;
using ConsoleApp2.Framework;
using ConsoleApp2.Pages;
using System;

namespace ConsoleApp2.Tests
{
    [TestFixture]
    public class IntunePortalTests : BaseTest
    {
        [Test]
        public async Task CanNavigateToIntunePortalAndDevicesSection()
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
            await home.NavigateAsync();
            await home.LoginIfNeededAsync(account);
            // Wait for portal to load (certificate-based SSO)
            var sw = System.Diagnostics.Stopwatch.StartNew();
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

            await home.ClickDevicesAsync();
            await home.ClickEnrollmentAsync();

            Assert.Pass("Navigated to Devices and Enrollment section using certificate-based login.");
        }
    }
}
