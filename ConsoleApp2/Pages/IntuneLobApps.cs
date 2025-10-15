using Account_Management.CommonBase;
using Account_Management.Framework;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Account_Management.Pages
{
    public class IntuneLobApps : AllAppUtills
    {
        public static IPage _page;
        private static string _portalUrl;
        private SelectAppTypeUtils selectAppType;
        private AllAppUtills All_Apps;

        public IntuneLobApps(IPage page, string env) : base(page, env)
        {
            _page = page;
            _portalUrl = env;
            selectAppType = new SelectAppTypeUtils(_page, _portalUrl);
            All_Apps = new AllAppUtills(_page, _portalUrl);
        }

        /// <summary>
        /// Create a Line-of-business app using the provided testCase data.
        /// Uses shared helpers already present in the project.
        /// Will only run sections that exist in the JSON (requirements, detection, dependencies, supersedence, assignment).
        /// </summary>
        public async Task CreateLobAppAsync(RootObject testCase)
        {
            if (testCase == null) throw new ArgumentNullException(nameof(testCase));

            string? typeCategory = testCase.AppType?.ElementAtOrDefault(1);
            string? appTypeValue = testCase.AppType?.ElementAtOrDefault(0);

            // Select app type in UI
            await selectAppType.SelectAppTypeAsync(appTypeValue, typeCategory);
            await All_Apps.UploadAppPackageFileAsync(testCase.FilePath);
            // If a package file is provided in the test data, upload it now


            // Set basic app information
            var appName = testCase.AppInfo?.Name ?? testCase.TestCaseName;
            if (!string.IsNullOrEmpty(appName))
            {
                try
                {
                    // Prefer the existing helper to set the name
                    await All_Apps.SetAppinformationNameAsync(appName, async (n) => await All_Apps.SetAppInformationInputWithAriaLabelAsync("Name", n));
                }
                catch
                {
                    // best-effort: try direct input if helper fails
                    try { await All_Apps.SetAppInformationInputWithAriaLabelAsync("Name", appName); } catch { }
                }
            }

            if (testCase.AppInfo != null)
            {
                if (!string.IsNullOrEmpty(testCase.AppInfo.Description))
                    await All_Apps.SetAppInformationDescriptionAsync(testCase.AppInfo.Description);

                // Set Publisher from JSON if present
                if (!string.IsNullOrEmpty(testCase.AppInfo.Publisher))
                {
                    try
                    {
                        await All_Apps.SetAppInformationInputWithAriaLabelAsync("Publisher", testCase.AppInfo.Publisher);
                    }
                    catch
                    {
                        // best-effort: ignore if UI element not found
                    }
                }
                if (!string.IsNullOrEmpty(testCase.AppInfo.TargetedPlatform))
                    await All_Apps.SetTargetedPlatformAsync(testCase.AppInfo.TargetedPlatform);

                if (!string.IsNullOrEmpty(testCase.AppInfo.MinimumOperatingSystem))
                    await All_Apps.SetMinimumOperationSystemAsync(testCase.AppInfo.MinimumOperatingSystem);

                if (!string.IsNullOrEmpty(testCase.AppInfo.InformationURL))
                    await All_Apps.SetAppInformationUrlAsync(testCase.AppInfo.InformationURL);

                if (!string.IsNullOrEmpty(testCase.AppInfo.PrivacyURL))

                    await All_Apps.SetAppInformationPrivacyURLAsync(testCase.AppInfo.PrivacyURL);
            
            
            
            }

            // Proceed to next step
            await All_Apps.ClickBottomNavigationSpecialNameButtonAsync("Next");

            // Use IntuneWin32Apps helpers where applicable. Only invoke sections present in JSON.
            var win32Helper = new IntuneWin32Apps(_page, _portalUrl);

            // Requirements
            if (testCase.RequirementsInfo != null)
            {
                Console.WriteLine("Found RequirementsInfo in JSON - applying requirements.");
                await IntuneWin32Apps.SetRequirementsFS(testCase);
                // move to next if UI requires
                await All_Apps.ClickBottomNavigationSpecialNameButtonAsync("Next");
            }
            else
            {
                Console.WriteLine("No RequirementsInfo in JSON - skipping requirements step.");
            }

            // Detection
            if (!string.IsNullOrEmpty(testCase.RulesFormat) || (testCase.DetectionRules != null && testCase.DetectionRules.Count > 0))
            {
                Console.WriteLine("Found detection info in JSON - applying detection rules.");
                await IntuneWin32Apps.SetDetectionFS(testCase);
            }
            else
            {
                Console.WriteLine("No detection info in JSON - skipping detection step.");
            }

            // Dependencies
            if (testCase.DependencyEntities != null && testCase.DependencyEntities.Count > 0)
            {
                Console.WriteLine("Found DependencyEntities in JSON - configuring dependencies.");
                await IntuneWin32Apps.SetDependenciesFS(testCase);
            }
            else
            {
                Console.WriteLine("No dependencies in JSON - skipping dependency step.");
            }

            // Supersedence
            if (testCase.SupersedenceEntities != null && testCase.SupersedenceEntities.Count > 0)
            {
                Console.WriteLine("Found SupersedenceEntities in JSON - configuring supersedence.");
                await IntuneWin32Apps.SetSupersedenceFS(testCase);
            }
            else
            {
                Console.WriteLine("No supersedence entries in JSON - skipping supersedence step.");
            }

            // Assignment
            if (testCase.AssignmentEntities != null && testCase.AssignmentEntities.Count > 0)
            {
                Console.WriteLine("Found AssignmentEntities in JSON - configuring assignments.");
                await win32Helper.SetAssignmentFS(testCase);
            }
            else
            {
                Console.WriteLine("No assignments in JSON - skipping assignment step.");
            }

            // Finalize - create
            await All_Apps.ClickBottomNavigationSpecialNameButtonAsync("Create");
        }
    }
}
