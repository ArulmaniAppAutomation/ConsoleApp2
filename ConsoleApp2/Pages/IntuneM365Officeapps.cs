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
        private readonly string _portalUrl;
        WindowsUtils windows;
        SelectAppTypeUtils selectAppType;
        Office365RootObject office365 = new Office365RootObject();
        public IntuneM365Officeapps(IPage page,String env) :base (page,env)
        {

            _page = page;
            _portalUrl = env;

        }

        public async Task CreateM365AppsAsync()
        {



            await windows.GoToMainPageAsync();
            await selectAppType.SelectAppTypeAsync(office365.AppType);
          //  (bool success, Dictionary<string, string> parameters) = await SetAppinformationNameAsync(
    //  office365.AppInfo?.Name ?? "DefaultAppName",
      //async (name) => await SetAppinformationNameAsync(name)
  //);


            await SetAppInformationDescriptionAsync(office365.AppInfo.Description);
            await SetAppInformationUrlAsync(office365.AppInfo.InformationURL);
            await SetAppInformationPrivacyURLAsync(office365.AppInfo.PrivacyURL);
            if (office365.ExcludedApps != null && office365.ExcludedApps.Any(kvp => kvp.Value))
            {
                var excludedAppNames = office365.ExcludedApps
                    .Where(kvp => kvp.Value)
                    .Select(kvp => kvp.Key)
                    .ToList();

                await SelectOfficeAppsByExcludeAsync(excludedAppNames);
            }

            await SelectOtherOfficeAppsAsync(office365.ProductIds);



        }


    }
}
