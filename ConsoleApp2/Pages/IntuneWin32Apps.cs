using Account_Management.Framework;
using log4net;
using Microsoft.Playwright;
using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework.Utilities;
using PlaywrightTests.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Reflection;
using System.Text.Json.Serialization;
namespace Account_Management.Pages
{
    public class IntuneWin32Apps
    {

        public static IPage _page;
        private readonly string _portalUrl;
        public IntuneWin32Apps(IPage page, string portalUrl)
        {
            _page = page;
            _portalUrl = portalUrl;
        }
        public async Task Select_Win32AppAsyncWithData(RootObject testCase)
        {
            var appinfo_Name = testCase.AppInfo.Name;
            var appinfo_des = testCase.AppInfo.Description;
            var publisher = testCase.AppInfo.Publisher;



            var element = await ElementHelper.GetByClassAndHasTextAsync(_page, "fxc-dropdown-placeholder", "Select app type");
            element.ClickAsync();
            var win32app_button = await ElementHelper.GetByRoleAndNameAsync(_page, AriaRole.Treeitem, "Windows app (Win32)");
            await win32app_button.ClickAsync();
            var select_button = _page.Locator("//span[text()='Select']");
            await select_button.ClickAsync();
            var selectAppPackageFile = await ElementHelper.GetByButtonRoleAndNameAsync(_page, "Select app package file", true);
            await selectAppPackageFile.ClickAsync();
            // Click the "Select a file" button
            // Click the "Select a file" button
            // Locate the file input


            var fileInput = await ElementHelper.GetByClassAsync(_page, "fxs-async-fileupload-overlay");

            // 2. Upload the file using Playwright's SetInputFilesAsync
            await fileInput.SetInputFilesAsync("C:\\Users\\v-arulmani\\source\\repos\\ConsoleApp2\\Test_Apps\\SimpleMSI.intunewin");



            var okButton = await ElementHelper.GetByRoleAndNameAsync(_page, AriaRole.Button, "OK");
            await okButton.ClickAsync();

            // 1. Locate the input element
            var nameInput = await ElementHelper.GetByClassAndAriaLableAsync(_page, "azc-input", "Name");

            // 2. Clear the input
            await nameInput.FillAsync(""); // Clear any existing text

            // 3. Enter new value
            await nameInput.FillAsync(appinfo_Name);
            // 1. Locate the publisher input element
            var publisherInput = await ElementHelper.GetByClassAndAriaLableAsync(_page, "azc-input", "Publisher");

            // 2. Enter the publisher name
            await publisherInput.FillAsync(publisher);

            var nextButton = await ElementHelper.GetByRoleAndNameAsync(_page, AriaRole.Button, "Next");
            await nextButton.ClickAsync();


            var nextButton1 = await ElementHelper.GetByRoleAndNameAsync(_page, AriaRole.Button, "Next");
            await nextButton1.ClickAsync();
            await SetRequirementsFS(testCase);












        }


        public static async Task SetMinimun_operatingSystem()
        {
            try
            {
                Console.WriteLine("Trying to locate dropdown...");
                var dropdown = await _page.WaitForSelectorAsync("#form-label-id-45textbox", new()
                {
                    State = WaitForSelectorState.Visible,
                    Timeout = 5000
                });

                Console.WriteLine("Dropdown found, scrolling into view...");
                await dropdown.ScrollIntoViewIfNeededAsync();

                Console.WriteLine("Clicking dropdown...");
                await dropdown.ClickAsync();

                Console.WriteLine("Dropdown clicked.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occurred: " + ex.Message);
                await _page.ScreenshotAsync(new PageScreenshotOptions { Path = "dropdown_error.png" });
            }

            // Select the first option by its visible text
            var firstOption = await ElementHelper.GetByRoleAndHasTextAsync(_page, AriaRole.Treeitem, "Windows 10 1607");
            await firstOption.ClickAsync();





        }

        public static async Task SelectMultiDropDownItems()
        {

            try
            {
                var radioButton = await ElementHelper.GetByClassAndHasTextAsync(_page, "azc-optionPicker-item", "Yes. Specify the systems the app can be installed on.");
                if (await radioButton.IsVisibleAsync() && await radioButton.IsEnabledAsync())
                {
                    await radioButton.ClickAsync();
                }
                else
                {
                    Console.WriteLine("Radio button is not visible or not enabled.");
                }


                // Locate the checkbox by role and label text
                var checkbox = await ElementHelper.GetByRoleAndHasTextAsync(_page, AriaRole.Checkbox, "Install on x64 system");
                await checkbox.ClickAsync();
                await _page.ScreenshotAsync(new PageScreenshotOptions
                {
                    Path = "C:\\Users\\Screenshot\\Operating_System.png" // Path where the screenshot will be saved
                });
            }
            catch (Exception ex)
            {


            }
        }

        public static async Task Set_Inputitem(String Comparsion_Value, String input_Data)
        {


            try
            {

                var inputBox = await ElementHelper.GetInputByLabelTextAsync(_page, Comparsion_Value);
                await inputBox.FillAsync(input_Data); // Replace "1024" with your desired value




            }
            catch (Exception ex)
            {
                throw;
            }








        }

        public static async Task Add_ReqRule()
        {

            try
            {
                ILocator parentLocator = await ElementHelper.GetByClassAsync(_page, "fxt-tabs-inner");
                var addButton = await ElementHelper.GetByRoleAndAriaLabelAsync(parentLocator, AriaRole.Button, "Add", false);
                await addButton.Nth(1).ClickAsync();



            }

            catch (Exception ex)
            {
                throw;
            }




        }

        public static async Task Select_ReqType()
        {


            try
            {
                var dropdown = await ElementHelper.GetByClassAndHasTextAsync(_page, "fxc-dropdown-placeholder", "Select one");
                await dropdown.ClickAsync();
                try
                {
                    var scriptOption = await ElementHelper.GetByClassAndHasTextAsync(_page, "fxc-dropdown-option", "Script");
                    await scriptOption.Nth(1).ClickAsync();
                }

                catch (Exception ex)
                {
                    var scriptOption = await ElementHelper.GetByClassAndHasTextAsync(_page, "fxc-dropdown-option", "Script");
                    await scriptOption.Nth(1).ClickAsync();


                }
            }



            catch (Exception ex)
            {
                Console.Write("it is erroe");


            }




        }

        public static async Task File_Browser(String file_path,String Script_name)
        {


            try{

                // 1. Find the parent element by class
                var parentLocator = await ElementHelper.GetByClassAsync(_page, "fxs-async-fileupload", exact: false);

                // 2. Find the file input element by stable class substring within the parent
                var fileInputLocator = await ElementHelper.GetByClassAsync(parentLocator, "fxs-async-fileupload-overlay", exact: false);

                // 3. Upload the file
                await fileInputLocator.SetInputFilesAsync(file_path);



            }


            catch (Exception ex)
            {
             

            }


            // 1. Find the parent element by stable class substring
            

        }

        public static async Task SelectReq_RulesDropDown(String DateAndTime,String Select_output)
        {

            try
            {
                var dropdown = await ElementHelper.GetByComBoxRoleAndNameAsync(_page, DateAndTime);
                await dropdown.ClickAsync();

                // Open the dropdown first (if not already open)
                //  await dropdown.ClickAsync(); // dropdown is the element that opens the popup

                // Find all popups
                var option = await ElementHelper.GetByRoleAndNameAsync(_page, AriaRole.Treeitem, Select_output);
                await option.ClickAsync();



            }

            catch (Exception ex)
            {

            }
        }

        public static async Task SelectOperator_DropDown(String Operator,String equals)
        {


            // Locate the dropdown by its label "Operator"
            var operatorDropdown = await ElementHelper.GetByComBoxRoleAndNameAsync(_page, Operator);

            // Click the dropdown
            await operatorDropdown.ClickAsync();
            var option = await ElementHelper.GetByRoleAndNameAsync(_page, AriaRole.Treeitem, equals);
            await option.ClickAsync();



        }
        public static async Task SelectCheckBox(String CheckBoxName)
        {
            try
            {// Step 1: Get the radio group container by label
                var radioGroup = await ElementHelper.GetByLableAsync(_page, "Run script as 32-bit process on 64-bit clients");

                // Step 2: Find the "Yes" radio within that group
                var yesRadio = await ElementHelper.GetByRoleAndNameAsync(radioGroup, AriaRole.Radio, "Yes");

                // Step 3: Click the "Yes" radio
                await yesRadio.ClickAsync();


            }
            catch (Exception ex)
            {
            }
        }
        public static Dictionary<string, string> GetPropertyToUILabelMap<T>()
        {
            var result = new Dictionary<string, string>();

            var props = typeof(T).GetProperties();
            foreach (var prop in props)
            {
                var jsonAttr = prop.GetCustomAttribute<JsonPropertyNameAttribute>();
                string label = jsonAttr?.Name ?? prop.Name;
                result[prop.Name] = label;
            }

            return result;
        }







        public static async Task SetRequirementsFS(RootObject entity)
        {
            try
            {
                var reqInfo = entity.RequirementsInfo;
                if (reqInfo == null) return;

                var labelMap = GetPropertyToUILabelMap<RequirementsInfo>();

                var props = reqInfo.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (var prop in props)
                {
                    string propertyName = prop.Name;
                    string value = prop.GetValue(reqInfo)?.ToString();

                    if (string.IsNullOrEmpty(value))
                        continue;

                    string uiLabel = labelMap[propertyName]; // UI-friendly label from JsonPropertyName

                    if (propertyName == nameof(RequirementsInfo.OperatingSystemArchitecture))
                    {
                        var OSArch = value.Split(',');
                        await IntuneWin32Apps.SelectMultiDropDownItems();
                    }
                    else if (propertyName == nameof(RequirementsInfo.MinimumOperatingSystem))
                    {
                        await IntuneWin32Apps.SetMinimun_operatingSystem();
                    }
                    else
                    {
                        // This is the key fix:
                        await IntuneWin32Apps.Set_Inputitem(uiLabel, value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw; // You should log this or wrap it in a custom exception
            }

            // The RequirementRules section remains unchanged as it's already dictionary-based
            if (entity.RequirementRules != null && entity.RequirementRules.Count > 0)
            {
                foreach (var item in entity.RequirementRules)
                {
                    await IntuneWin32Apps.Add_ReqRule();
                    await IntuneWin32Apps.Select_ReqType();




                    foreach (var key in item.RequirementInfo.Keys)
                    {
                        if (key.Equals("Script file"))
                        {
                            string appFileFullPath = System.Text.RegularExpressions.Regex.Replace(Path.Combine(Environment.CurrentDirectory, item.RequirementInfo["Script file"].Replace(".\\", "")), "[+^%~()]", "{$0}");

                            await IntuneWin32Apps.File_Browser(appFileFullPath,key);

                        }
                        else if (key.Equals("Run this script using the logged on credentials") || key.Contains("Run script as 32-bit process on 64-bit clients") || key.Contains("Associated with a 32-bit app on 64-bit clients") || key.Contains("Enforce script signature check"))
                        {

                            await IntuneWin32Apps.SelectCheckBox(key);

                            //ClickExtensions.ClickLiOption(Framework, key, item.RequirementInfo[key]);
                        }

                        else if (key.Equals("Property") || key.Equals("Select output data type") || key.Equals("Registry key requirement"))
                                   {

                            await IntuneWin32Apps.SelectReq_RulesDropDown(key, item.RequirementInfo[key]);



                                    }


                        else if (key.Equals("Operator"))
                                   {

                            await IntuneWin32Apps.SelectOperator_DropDown(key, item.RequirementInfo[key]);

                            //SelectExtensions.SelectOperatorDropDown(Framework, item.RequirementInfo[key], "Operator");



                        }
                            

                    }
                    //                else if (key.Equals("Operator"))
                    //                {
                    //                    SelectExtensions.SelectOperatorDropDown(Framework, item.RequirementInfo[key], "Operator");
                    //                }
                    //                else if (key.Equals("Run this script using the logged on credentials") || key.Contains("Run script as 32-bit process on 64-bit clients") || key.Contains("Associated with a 32-bit app on 64-bit clients") || key.Contains("Enforce script signature check"))
                    //                {
                    //                    ClickExtensions.ClickLiOption(Framework, key, item.RequirementInfo[key]);
                    //                }
                    //                else
                    //                {
                    //                    if (key.Equals("Value") && item.RequirementInfo.Keys.Any(a => a == "Select output data type" || a == "Property" || a == "Registry key requirement")
                    //                        && item.RequirementInfo[item.RequirementInfo.Keys.Where(a => a == "Select output data type" || a == "Property" || a == "Registry key requirement").First()].Contains("Date"))
                    //                    {
                    //                        var dateValue = item.RequirementInfo[key].Split(',');
                    //                        var parent = Framework.FindElementByClassName("azc-dateTimePicker");
                    //                        IWebElement date = parent.FindElement(By.ClassName("azc-datePicker")).FindElement(By.TagName("input"));
                    //                        IWebElement time = parent.FindElement(By.ClassName("azc-timePicker")).FindElement(By.TagName("input"));
                    //                        SetExtensions.SetDateTimeClear(Framework, date);
                    //                        ClickExtensions.SendKeysToItem(Framework, dateValue[0], date);
                    //                        log.Info("set the value of date as: " + dateValue[0]);
                    //                        if (dateValue.Length > 1)
                    //                        {
                    //                            time.Click();
                    //                            SetExtensions.SetDateTimeClear(Framework, time);
                    //                            ClickExtensions.SendKeysToItem(Framework, dateValue[1], time);
                    //                            log.Info("set the value of time as: " + dateValue[1]);
                    //                        }
                    //                    }
                    //                    else
                    //                    {
                    //                        SetExtensions.SetItemTextInCurrentBlade(Framework, key, item.RequirementInfo[key]);
                    //                    }
                    //                }
                    //            }
                    //            ClickExtensions.PressActionButtonAndWaitForBlade(Framework, "OK", "");
                    //        }
                    //    }
                    //    ClickExtensions.PressActionButtonAndWaitForBlade(Framework, "Next", "");
                    //}
                    //   catch ()
                    //   {
                    //       Console.WriteLine("Exception in SetRequirementsFS: " );
                    //// Re-throw the exception after logging it
                    //   }















                }

            }
        }
    }
}








    








