using Account_Management.CommonBase;
using Account_Management.Framework;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account_Management.Pages
{
    public class IntuneIosStoreApps : AllAppUtills
    {
        public static IPage _page;
        private static string _portalUrl;
        private SelectAppTypeUtils selectAppType;
        private AllAppUtills All_Apps;

        public IntuneIosStoreApps(IPage page, string env) : base(page, env)
        {
            _page = page;
            _portalUrl = env;
            selectAppType = new SelectAppTypeUtils(_page, _portalUrl);
            All_Apps = new AllAppUtills(_page, _portalUrl);
        }

        /// <summary>
        /// Create an iOS Store app using the provided testCase data.
        /// Uses shared helpers already present in the project.
        /// Will only run sections that exist in the JSON (assignments, etc.).
        /// </summary>
        public async Task CreateIosStoreAppAsync(RootObject testCase)
        {
            if (testCase == null) throw new ArgumentNullException(nameof(testCase));

            string? typeCategory = testCase.AppType?.ElementAtOrDefault(1);
            string? appTypeValue = testCase.AppType?.ElementAtOrDefault(0);

            // Select app type in UI (should be "iOS" or similar)
            await selectAppType.SelectAppTypeAsync(appTypeValue, typeCategory);

            // Search and select the app from the App Store
            if (!string.IsNullOrEmpty(testCase.AppSearchString))
            {
                
                // Simulate selecting the app from search results
                await All_Apps.ClickSelectTheAppStoreButtonAsync();
                await All_Apps.SelectTheAppStoreAsync(testCase.AppSearchString);

                // await All_Apps.SetAppInformationInputWithAriaLabelAsync("Search the App Store", testCase.AppSearchString);
            }

            // Set basic app information
            var appName = testCase.AppInfo?.Name ?? testCase.TestCaseName;
            if (!string.IsNullOrEmpty(appName))
            {
                try
                {
                    await All_Apps.SetAppinformationNameAsync(appName, async (n) => await All_Apps.SetAppInformationInputWithAriaLabelAsync("Name", n));
                }
                catch
                {
                    try { await All_Apps.SetAppInformationInputWithAriaLabelAsync("Name", appName); } catch { }
                }
            }

            if (testCase.AppInfo != null)
            {
                if (!string.IsNullOrEmpty(testCase.AppInfo.Description))
                    await All_Apps.SetAppInformationDescriptionAsync(testCase.AppInfo.Description);

                if (!string.IsNullOrEmpty(testCase.AppInfo.Publisher))
                {
                    try
                    {
                        await All_Apps.SetAppInformationInputWithAriaLabelAsync("Publisher", testCase.AppInfo.Publisher);
                    }
                    catch { }
                }

                if (!string.IsNullOrEmpty(testCase.AppInfo.MinimumOperatingSystem))
                    await All_Apps.SetMinimumOperationSystemAsync(testCase.AppInfo.MinimumOperatingSystem);

                if (!string.IsNullOrEmpty(testCase.AppInfo.InformationURL))
                    await All_Apps.SetAppInformationUrlAsync(testCase.AppInfo.InformationURL);

                if (!string.IsNullOrEmpty(testCase.AppInfo.PrivacyURL))
                    await All_Apps.SetAppInformationPrivacyURLAsync(testCase.AppInfo.PrivacyURL);

                if (!string.IsNullOrEmpty(testCase.AppInfo.Developer))
                    await All_Apps.SetAppInformationInputWithAriaLabelAsync("Developer", testCase.AppInfo.Developer);

                if (!string.IsNullOrEmpty(testCase.AppInfo.Owner))
                    await All_Apps.SetAppInformationInputWithAriaLabelAsync("Owner", testCase.AppInfo.Owner);

                if (!string.IsNullOrEmpty(testCase.AppInfo.Notes))
                    await All_Apps.SetAppInformationNotesAsync(testCase.AppInfo.Notes);

                // Handle logo upload if needed
                if (!string.IsNullOrEmpty(testCase.AppInfo.Logo))
                {
                    // You may need a specific helper for logo upload
                    // await All_Apps.UploadLogoAsync(testCase.AppInfo.Logo);
                }
            }

            // Proceed to next step
            await All_Apps.ClickBottomNavigationSpecialNameButtonAsync("Next");

            // Assignment
            var win32Helper = new IntuneWin32Apps(_page, _portalUrl);
            if (testCase.AssignmentEntities != null && testCase.AssignmentEntities.Count > 0)
            {
                Console.WriteLine("Found AssignmentEntities in JSON - configuring assignments.");
                await win32Helper.SetAssignmentFS(testCase);
            }
            else
            {
                Console.WriteLine("No assignments in JSON - skipping assignment step.");
            }
            await All_Apps.ClickBottomNavigationSpecialNameButtonAsync("Next");

            // Finalize - create
            await All_Apps.ClickBottomNavigationSpecialNameButtonAsync("Create");
        }
    }
}