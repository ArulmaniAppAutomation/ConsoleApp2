//using Account_Management.Framework.ElementHelper;
using Microsoft.Playwright;
using Newtonsoft.Json.Linq;
using PlaywrightTests.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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


        public async Task SelectAppAsync(string? iFrameName = "AppList.ReactView", string value= "Android store app", string? category = null,int nth=0, bool doubleCheck = false)
        {
            ILocator dropDownObject;
            IFrameLocator? frameLocator = null;

            try
            {
                // Wait for the iframe to appear
                await _page.WaitForSelectorAsync($"iframe[name='{iFrameName}']", new() { Timeout = 10000 });
                frameLocator = _page.FrameLocator($"iframe[name='{iFrameName}']");
            }
            catch
            {
                Console.WriteLine("⚠️ Iframe not found, continuing with main page.");
            }

            // Use appropriate locator
            if (frameLocator != null)
            {
                dropDownObject = await ElementHelper.GetByComBoxRoleAndNameAsync(frameLocator, comBoxName);
            }
            else
            {
                dropDownObject = await ElementHelper.GetByComBoxRoleAndNameAsync(_page, comBoxName);
            }


            await dropDownObject.Nth(nth).HoverAsync();
            await dropDownObject.Nth(nth).ClickAsync();
            if (doubleCheck)
            {
                Thread.Sleep(1000);
                var expandStatus = await dropDownObject.Nth(nth).GetAttributeAsync("aria-expanded");
                
                if (expandStatus == "false")
                {
                    await dropDownObject.Nth(nth).HoverAsync();
                    await dropDownObject.Nth(nth).ClickAsync();
                }

                ILocator categoryObject;

                if (!(frameLocator == null))
                {
                    
                    categoryObject = await ElementHelper.GetByAriaLabelAsync(frameLocator, category);
                }
                else
                {
                    categoryObject = await ElementHelper.GetByAriaLabelAsync(_page, category);
                }
                var appTypeLocators = categoryObject.Locator("~ div");
                var optionObject = (await appTypeLocators.AllAsync()).First(t => t.InnerTextAsync().Result == value);
                await optionObject.Nth(0).ClickAsync();



            }






        }



    }
}
