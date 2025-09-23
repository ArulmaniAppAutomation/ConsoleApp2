//using Account_Management.Framework.ElementHelper;
using Account_Management.Framework;
using Microsoft.Playwright;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Newtonsoft.Json.Linq;
using PlaywrightTests.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using static System.Net.Mime.MediaTypeNames;
namespace Account_Management.Pages
{
    public class IntuneAppsPage :ElementHelper
    {
      public static  IPage _page;
        private readonly string _portalUrl;
        public static string App_hasText = "All Apps";
        public static string App_className = "fxc-menu-listView-item";
        public static string create_className = "ms-Button-label";
        public static string Create_hasText = "Create";
        public static string AllApps_hasText = "Apps";
        public static string AllApps_className = "fxs-sidebar-item-link";
        public static string comBoxName = "App type";
        public static string value = "Android store app";

       

        public IntuneAppsPage(IPage page, string portalUrl)
        {
            _page = page;
            _portalUrl = portalUrl;
        }

        public async Task app_Click()
        {
            try
            {
                var element = await ElementHelper.GetByClassAndHasTextAsync(_page, App_className, App_hasText);
                element.ClickAsync();
            }
            catch (Exception ex) { }
        
        
        
        }
        public async Task AllApps_Click()
        {


            try
            {

                var element = await ElementHelper.GetByClassAndHasTextAsync(_page,AllApps_className, AllApps_hasText);
               await element.ClickAsync();

            }
            catch(Exception ex) { 
            
            
            
            
            
            }

        }

        public async Task CreateBUtton_Click()
        {
            try
            {


                await _page.WaitForSelectorAsync("iframe[name='AppList.ReactView']", new() { Timeout = 10000 });
                var frameLocator = _page.FrameLocator("iframe[name='AppList.ReactView']");

                var element = await ElementHelper.GetByClassAndHasTextAsync(frameLocator, create_className, Create_hasText);

                await element.ClickAsync();
            }
       
            catch(Exception ex) { 
            
            
            
            }
            
            
            
            
            
            
            
            }


        public async Task Select_AndroidAppAsyncWithData(RootObject testCase)
        {
            
            
            ILocator dropDownObject;
            IFrameLocator? frameLocator = null;
            var element=await ElementHelper.GetByClassAndHasTextAsync(_page, "fxc-dropdown-placeholder", "Select app type");
            element.ClickAsync();
            //await _page.ClickAsync("//*[@id='form-label-id-2textbox']");
            await _page.ClickAsync("//span[text()='Android store app']");
            var select_button= _page.Locator("//span[text()='Select']");
            await select_button.ClickAsync();
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);


            await _page.ScreenshotAsync(new PageScreenshotOptions
            {
                Path = "C:\\Users\\Screenshot\\SetAppinformation.png" // Path where the screenshot will be saved
            });

            //string jsonFilePath = Path.Combine(AppContext.BaseDirectory, "TestData", "androidStoreApp.txt");
            //DataLoader.LoadFromFile(jsonFilePath);
            //var appinfo_Name = DataLoader.TestCases[0].AppInfo.Name;
            //var appinfo_des = DataLoader.TestCases[0].AppInfo.Description;

            var appinfo_Name = testCase.AppInfo.Name;
            var appinfo_des = testCase.AppInfo.Description;
            var publisher = testCase.AppInfo.Publisher;
            var storeUrl = testCase.AppInfo.AppstoreURL;


            var App_Name =  await ElementHelper.GetByClassAndPlaceholderAsync(_page, "azc-input", "Enter a name");
            await App_Name.FillAsync(appinfo_Name);

            await _page.ScreenshotAsync(new PageScreenshotOptions
            {
                Path = "C:\\Users\\Screenshot\\appinfo_Name.png" // Path where the screenshot will be saved
            });

            var App_Desciption = await ElementHelper.GetByClassAndPlaceholderAsync(_page, "azc-textarea", "Enter a description...");

            await App_Desciption.FillAsync(appinfo_des);
            await _page.ScreenshotAsync(new PageScreenshotOptions
            {
                Path = "C:\\Users\\Screenshot\\appdes_Name.png" // Path where the screenshot will be saved
            });
            var App_Publisher = await ElementHelper.GetByClassAndPlaceholderAsync(_page, "azc-input", "Enter a publisher name");
            await App_Publisher.FillAsync(publisher);
            var App_url = await ElementHelper.GetByClassAndPlaceholderAsync(_page, "azc-input", "Enter a valid url similar to https://play.google.com/store/apps/details?id=com.microsoft.bing&hl=en");
            
            await App_url.FillAsync(storeUrl);

            var next_button = await ElementHelper.GetByRoleAndHasTextAsync(_page, AriaRole.Button, "Next");
            await next_button.ClickAsync();

            await _page.ScreenshotAsync(new PageScreenshotOptions
            {
                Path = "C:\\Users\\Screenshot\\Next.png" // Path where the screenshot will be saved
            });


            var locator = await ElementHelper.GetByRoleAndAriaLabelAsync(_page, AriaRole.Button, "Add all users");
            await locator.Nth(0).ClickAsync();

            var All_Devices = await ElementHelper.GetByRoleAndAriaLabelAsync(_page, AriaRole.Button, "Add all devices");
            await All_Devices.Nth(0).ClickAsync();
            await _page.ScreenshotAsync(new PageScreenshotOptions
            {
                Path = "C:\\Users\\Screenshot\\Devices.png" // Path where the screenshot will be saved
            });



           
                   var nextButton = await ElementHelper.GetByRoleAndNameAsync(_page, AriaRole.Button, "Next");
                        await nextButton.ClickAsync();

            var createButton = await ElementHelper.GetByRoleAndNameAsync(_page, AriaRole.Button, "Create");
            await createButton.ClickAsync();







        }



    }
}
