using Account_Management.CommonBase;
using Account_Management.Framework;
using Microsoft.Playwright;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account_Management.Pages
{
    public class AppsUpdate:BaseCommonUtils
    {
        public static IPage _page;
        private static string _portalUrl;
        WindowsUtils windows;
        AllAppUtills All_Apps;
        SelectAppTypeUtils selectAppType;
        Office365RootObject office365 = new Office365RootObject();
        public AppsUpdate(IPage page, String env) : base(page, env)
        {

            _page = page;
            _portalUrl = env;
            selectAppType = new SelectAppTypeUtils(_page, _portalUrl);
            All_Apps = new AllAppUtills(_page, _portalUrl);
        }


        public  async Task updateApps(RootObject testcase)
        {

       
            await All_Apps.ClickAppsNameToShowDetailAsync(testcase.TestCaseName);
            await All_Apps.ClickAppInformationEditButtonAsync();
            // await All_Apps.ClickSelectFileToUpdateButtonAsync();
            await All_Apps.UploadAppPackageFileAsync(testcase.FilePath);
            await All_Apps.ClickBottomNavigationSpecialNameButtonAsync("Review + save");
            await All_Apps.ClickBottomNavigationSpecialNameButtonAsync("Save");



        }





    }
}
