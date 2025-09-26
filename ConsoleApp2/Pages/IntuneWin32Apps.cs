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
using System.Text;
using System.Threading.Tasks;

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
            SetRequirementsFS(testCase);












        }

        public static async Task SelectMultiDropDownItems()
        {

            var specify_SystemsButton = await ElementHelper.GetByClassAndHasTextAsync(_page, "azc-optionPicker-item", "Yes. Specify the systems the app can be installed on.");
            await specify_SystemsButton.ClickAsync();
            var x86_Button = await ElementHelper.GetByClassAndAriaLableAsync(_page, "azc-input", "Install on x86 system");
            await x86_Button.ClickAsync();
            var checkBox = await ElementHelper.GetByClassAndAriaLableAsync(_page, "azc-input", "Install on x64 system");
            await checkBox.ClickAsync();

        }

        public static async Task SetRequirementsFS(RootObject entity)
        {

           


            try
            {
                var log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


                var reqInfo = entity.RequirementsInfo;
                if (reqInfo != null)
                {
                    // Use reflection to iterate through properties
                    var props = reqInfo.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

                    foreach (var prop in props)
                    {
                        string key = prop.Name; // e.g. "OperatingSystemArchitecture"
                        string value = prop.GetValue(reqInfo)?.ToString();

                        if (string.IsNullOrEmpty(value))
                            continue;

                        if (key == "OperatingSystemArchitecture")
                        {
                            var OSArch = value.Split(',');

                            IntuneWin32Apps.SelectMultiDropDownItems();


                        }
                    }

                }

            }





            catch (Exception ex)
            {

                throw;
            }
            } 
        }
    } 

                        //                else if (key == "MinimumOperatingSystem")
                        //                {
                        //                    SelectExtensions.SelectSingleDropDownItem(Framework, "Minimum operating system", value);
                        //                }
                        //                else
                        //                {
                        //                    SetExtensions.SetItemTextInCurrentBlade(Framework, key, value);
                        //                }
                        //            }
                        //        }

//        // The RequirementRules section remains unchanged as it's already dictionary-based
//        if (win32AppEntity.RequirementRules != null && win32AppEntity.RequirementRules.Count > 0)
//        {
//            foreach (var item in win32AppEntity.RequirementRules)
//            {
//                ClickExtensions.ClickLink(Framework, "Add", "+ Add");

//                //wait until the page loads
//                Framework.WaitElementLoaded("Requirement type", "fxc-section-control", "azc-input");

//                //Select Requirement type
//                SelectExtensions.SelectSingleDropDownItem(Framework, "Requirement type", item.RequirementType.ToString());

//                foreach (var key in item.RequirementInfo.Keys)
//                {
//                    if (key.Equals("Script file"))
//                    {
//                        string appFileFullPath = System.Text.RegularExpressions.Regex.Replace(Path.Combine(Environment.CurrentDirectory, item.RequirementInfo["Script file"].Replace(".\\", "")), "[+^%~()]", "{$0}");
//                        ClickExtensions.FileBrowse(Framework, appFileFullPath);
//                    }
//                    else if (key.Equals("Property") || key.Equals("Select output data type") || key.Equals("Registry key requirement"))
//                    {
//                        SelectExtensions.SelectSingleDropDownItem(Framework, key, item.RequirementInfo[key]);
//                    }
//                    else if (key.Equals("Operator"))
//                    {
//                        SelectExtensions.SelectOperatorDropDown(Framework, item.RequirementInfo[key], "Operator");
//                    }
//                    else if (key.Equals("Run this script using the logged on credentials") || key.Contains("Run script as 32-bit process on 64-bit clients") || key.Contains("Associated with a 32-bit app on 64-bit clients") || key.Contains("Enforce script signature check"))
//                    {
//                        ClickExtensions.ClickLiOption(Framework, key, item.RequirementInfo[key]);
//                    }
//                    else
//                    {
//                        if (key.Equals("Value") && item.RequirementInfo.Keys.Any(a => a == "Select output data type" || a == "Property" || a == "Registry key requirement")
//                            && item.RequirementInfo[item.RequirementInfo.Keys.Where(a => a == "Select output data type" || a == "Property" || a == "Registry key requirement").First()].Contains("Date"))
//                        {
//                            var dateValue = item.RequirementInfo[key].Split(',');
//                            var parent = Framework.FindElementByClassName("azc-dateTimePicker");
//                            IWebElement date = parent.FindElement(By.ClassName("azc-datePicker")).FindElement(By.TagName("input"));
//                            IWebElement time = parent.FindElement(By.ClassName("azc-timePicker")).FindElement(By.TagName("input"));
//                            SetExtensions.SetDateTimeClear(Framework, date);
//                            ClickExtensions.SendKeysToItem(Framework, dateValue[0], date);
//                            log.Info("set the value of date as: " + dateValue[0]);
//                            if (dateValue.Length > 1)
//                            {
//                                time.Click();
//                                SetExtensions.SetDateTimeClear(Framework, time);
//                                ClickExtensions.SendKeysToItem(Framework, dateValue[1], time);
//                                log.Info("set the value of time as: " + dateValue[1]);
//                            }
//                        }
//                        else
//                        {
//                            SetExtensions.SetItemTextInCurrentBlade(Framework, key, item.RequirementInfo[key]);
//                        }
//                    }
//                }
//                ClickExtensions.PressActionButtonAndWaitForBlade(Framework, "OK", "");
//            }
//        }
//        ClickExtensions.PressActionButtonAndWaitForBlade(Framework, "Next", "");
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine("Exception in SetRequirementsFS: " + ex.Message);
//        throw; // Re-throw the exception after logging it
//    }
//}







