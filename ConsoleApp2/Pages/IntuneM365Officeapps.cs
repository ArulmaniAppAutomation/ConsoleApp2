using Account_Management.CommonBase;
using Account_Management.Framework;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account_Management.Pages
{
    public class IntuneM365Officeapps :AllAppUtills
    {
        public static IPage _page;
        private static  string _portalUrl;
        WindowsUtils windows;
        AllAppUtills All_Apps;
        SelectAppTypeUtils selectAppType;
        Office365RootObject office365 = new Office365RootObject();
        public IntuneM365Officeapps(IPage page,String env) :base (page,env)
        {

            _page = page;
            _portalUrl = env;
            selectAppType = new SelectAppTypeUtils(_page, _portalUrl);
            All_Apps=new AllAppUtills(_page, _portalUrl);
        }

        public async Task CreateM365AppsAsync(Office365RootObject testCase)
        {

            string? typeCategory = testCase.AppType?.ElementAtOrDefault(1);
            string? appTypeValue = testCase.AppType?.ElementAtOrDefault(0);

            // await windows.GoToMainPageAsync();
            await selectAppType.SelectAppTypeAsync(appTypeValue, typeCategory);
          //  (bool success, Dictionary<string, string> parameters) = await SetAppinformationNameAsync(
    //  office365.AppInfo?.Name ?? "DefaultAppName",
      //async (name) => await SetAppinformationNameAsync(name)
  //);


            await All_Apps.SetAppInformationDescriptionAsync(testCase.AppInfo.Description);
            await All_Apps. SetAppInformationUrlAsync(testCase.AppInfo.InformationURL);
            await All_Apps.SetAppInformationPrivacyURLAsync(testCase.AppInfo.PrivacyURL);
            await All_Apps.ClickBottomNavigationSpecialNameButtonAsync("Next");
            if (office365.ExcludedApps != null && office365.ExcludedApps.Any(kvp => kvp.Value))
            {
                var excludedAppNames = office365.ExcludedApps
                    .Where(kvp => kvp.Value)
                    .Select(kvp => kvp.Key)
                    .ToList();

                await All_Apps.SelectOfficeAppsByExcludeAsync(excludedAppNames);
            }

            await All_Apps. SelectOtherOfficeAppsAsync(testCase.ProductIds);
            await All_Apps. SetArchitectureAsync(testCase.OfficePlatformArchitecture);
            await All_Apps. SetdefaultFileFormatAsync(testCase.DefaultFileFormat);
            await All_Apps.SetUpdatechannelAsync(testCase.UpdateChannel);
            await All_Apps.SetLanguagesAsync(testCase.LocalesToInstall);
            //  await SetVersionToInstallAsync(office365.VersionToInstall, office365.SpecificVersion);
            await All_Apps.ClickBottomNavigationSpecialNameButtonAsync("Next");
            var appHelper = new IntuneWin32Apps(_page, _portalUrl);
            await appHelper.SetAssignmentFS(testCase);
            await ClickBottomNavigationSpecialNameButtonAsync("Create");
        }


    }
}
