using Account_Management.Framework;
using log4net;
using Microsoft.Playwright;
using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework.Utilities;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using PlaywrightTests.Common.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;
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
            //await IntuneWin32Apps.SetDetectionRulesFS();











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

        public static async Task File_Browser(String file_path)
        {


            try
            {

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

        public static async Task SelectReq_RulesDropDown(String DateAndTime, String Select_output)
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

        public static async Task SelectOperator_DropDown(String Operator, String equals)
        {


            // Locate the dropdown by its label "Operator"
            var operatorDropdown = await ElementHelper.GetByComBoxRoleAndNameAsync(_page, Operator);

            // Click the dropdown
            await operatorDropdown.ClickAsync();
            // Assume 'page' is your IPage instance and the dropdown is already focused
            switch (equals)
            {
                case "Equals":
                    await _page.Keyboard.PressAsync("ArrowDown");
                    break;
                case "Not equal to":
                    await _page.Keyboard.PressAsync("ArrowDown");
                    await _page.Keyboard.PressAsync("ArrowDown");
                    break;
                case "Greater than or equal to":
                    await _page.Keyboard.PressAsync("ArrowDown");
                    await _page.Keyboard.PressAsync("ArrowDown");
                    await _page.Keyboard.PressAsync("ArrowDown");
                    break;
                case "Greater than":
                    await _page.Keyboard.PressAsync("ArrowDown");
                    await _page.Keyboard.PressAsync("ArrowDown");
                    await _page.Keyboard.PressAsync("ArrowDown");
                    await _page.Keyboard.PressAsync("ArrowDown");
                    break;
                case "Less than or equal to":
                    await _page.Keyboard.PressAsync("ArrowDown");
                    await _page.Keyboard.PressAsync("ArrowDown");
                    await _page.Keyboard.PressAsync("ArrowDown");
                    await _page.Keyboard.PressAsync("ArrowDown");
                    await _page.Keyboard.PressAsync("ArrowDown");
                    break;
                case "Less than":
                    await _page.Keyboard.PressAsync("ArrowDown");
                    await _page.Keyboard.PressAsync("ArrowDown");
                    await _page.Keyboard.PressAsync("ArrowDown");
                    await _page.Keyboard.PressAsync("ArrowDown");
                    await _page.Keyboard.PressAsync("ArrowDown");
                    await _page.Keyboard.PressAsync("ArrowDown");
                    break;
            }

            // Press Enter to select
            await _page.Keyboard.PressAsync("Enter");

            //var option = await ElementHelper.GetByRoleAndNameAsync(_page, AriaRole.Treeitem, equals);
            //await option.ClickAsync();



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

        public static async Task SendKeysToItemAsync(string keys, ILocator item)
        {
            // Wait for item to be visible and enabled
            await item.WaitForAsync(new() { State = WaitForSelectorState.Visible });
            await item.ScrollIntoViewIfNeededAsync();
            await item.ClickAsync();

            int count = 0;
            string value = await item.InputValueAsync() ?? string.Empty;

            while (!string.Equals(value, keys, StringComparison.OrdinalIgnoreCase) && count++ < 3)
            {
                await item.FillAsync(""); // Clear first
                await item.TypeAsync(keys); // More reliable than FillAsync in some frameworks
                await Task.Delay(200); // Small delay to let frontend handle the update
                value = await item.InputValueAsync() ?? string.Empty;

                Console.WriteLine($"Attempt {count}: Expected: {keys}, Actual: {value}");
            }

            // Optionally press Enter if needed
            // await item.PressAsync("Enter");
        }



        public static async Task SetDateTimeClear(ILocator date)
        {
            date.ClickAsync();

            char[] deleteTextSequenceChars = { '\u007F', '\u0008' };
            string deleteTextSequence = new string(deleteTextSequenceChars);
            int count = 0;
            while (count++ < 6)
            {
                await IntuneWin32Apps.SendKeysToItemAsync(deleteTextSequence, date);
            }





        }

        public static async Task SetItemTextInCurrentBladeAsync(string label, string textValue)
        {
            // Log the action
            Console.WriteLine($"Select value of the \"{label}\" as: {textValue}");

            if (_page == null)
                throw new ArgumentNullException(nameof(_page));

            // 1. Find all section controls using your helper
            var sectionControls = await ElementHelper.GetByClassAsync(_page, "fxc-section-control", exact: false, waitUntilElementExist: false);
            var sectionList = await sectionControls.AllAsync();

            // 2. Find the parent section whose label matches the given label using your helpers
            ILocator parent = null;
            foreach (var section in sectionList)
            {
                // Use your helper to find the label inside this section
                var labelLocator = await ElementHelper.GetByClassAsync(section, "azc-form-label", exact: false, waitUntilElementExist: false);
                if (await labelLocator.CountAsync() > 0)
                {
                    var labelText = await labelLocator.First.InnerTextAsync();
                    if (labelText.Trim().Equals(label, StringComparison.OrdinalIgnoreCase))
                    {
                        parent = section;
                        break;
                    }
                }
            }

            if (parent == null)
                throw new Exception($"Could not find section with label: {label}");

            // 3. Find the input inside the parent section using your helper
            var input = await ElementHelper.GetByClassAsync(parent, "azc-input", exact: false, waitUntilElementExist: false);

            // 4. Set the value using Playwright's FillAsync
            await SendKeysToItemAsync(textValue, input);
        }



        public static async Task PressActionButtonAndWaitForBladeAsync(IPage page, string actionName, string bladeName, string className = "fxs-button")
        {
            if (page == null)
                throw new ArgumentNullException(nameof(page));

            try
            {
                await ClickElementIfClassNameExistsAsync(page, className, actionName);

                // If a blade name is provided, wait for the blade to appear
                if (!string.IsNullOrEmpty(bladeName))
                {
                    Console.WriteLine($"Wait the {bladeName} blade loading");

                    // Use your helper to find all blades (assuming a blade has a unique class, e.g., "fxc-blade")
                    var blades = await ElementHelper.GetByClassAsync(page, "fxc-blade", exact: false, waitUntilElementExist: false);
                    var bladeList = await blades.AllAsync();

                    // Wait for the blade with the correct name to appear (timeout logic can be added as needed)
                    ILocator targetBlade = null;
                    foreach (var blade in bladeList)
                    {
                        var text = await blade.InnerTextAsync();
                        if (text.Trim().Equals(bladeName, StringComparison.OrdinalIgnoreCase))
                        {
                            targetBlade = blade;
                            break;
                        }
                    }

                    if (targetBlade != null)
                    {
                        Console.WriteLine($"{bladeName} blade load completed");
                        return;
                    }
                    else
                    {
                        throw new Exception($"Blade '{bladeName}' did not appear.");
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public static async Task ClickElementIfClassNameExistsAsync(IPage page, string className, string actionName)
        {
            int max = 300;
            bool clicked = false;

            while (max-- > 0)
            {
                try
                {
                    // Use your helper to get all elements with the given class
                    var elements = await ElementHelper.GetByClassAsync(page, className, exact: false, waitUntilElementExist: false);
                    var elementList = await elements.AllAsync();

                    ILocator target = null;

                    // Loop through elements in reverse to mimic .Last()
                    for (int i = elementList.Count - 1; i >= 0; i--)
                    {
                        var el = elementList[i];
                        var text = (await el.InnerTextAsync()).Trim();
                        var classAttr = await el.GetAttributeAsync("class") ?? "";

                        if (text.StartsWith(actionName, StringComparison.OrdinalIgnoreCase) && !classAttr.Contains("fxs-button-disabled"))
                        {
                            target = el;
                            break;
                        }
                    }

                    if (target != null)
                    {
                        Console.WriteLine("Click the element: " + await target.InnerTextAsync());
                        await target.ClickAsync();
                        clicked = true;
                        break;
                    }
                }
                catch
                {
                    // Ignore and retry
                    await Task.Delay(200);
                }
            }

            if (!clicked)
                throw new Exception($"The control {className} {actionName} cannot be displayed or enabled correctly");
        }


        public static async Task SelectSingleDropDownItemAsync(IPage page, string label, string valueToSelect)
        {
            Console.WriteLine($"[SelectSingleDropDown] Select value: \"{valueToSelect}\" from: \"{label}\"");

            if (page == null)
                throw new ArgumentNullException(nameof(page));

            // 1. Find all section controls (fxc-section-control)
            var sectionControls = await ElementHelper.GetByClassAsync(page, "fxc-section-control", exact: false, waitUntilElementExist: false);
            var sectionList = await sectionControls.AllAsync();

            ILocator parent = null;
            foreach (var section in sectionList)
            {
                var labelLocator = await ElementHelper.GetByClassAsync(section, "azc-form-label", exact: false, waitUntilElementExist: false);
                if (await labelLocator.CountAsync() > 0)
                {
                    var labelText = await labelLocator.First.InnerTextAsync();
                    if (labelText.Trim().Equals(label, StringComparison.OrdinalIgnoreCase))
                    {
                        parent = section;
                        break;
                    }
                }
            }

            if (parent == null)
                throw new Exception($"Could not find section with label: {label}");

            // 2. Find and click the dropdown input inside the parent (fxc-dropdown-input is robust for this UI)
            var dropdownInput = await ElementHelper.GetByClassAsync(parent, "fxc-dropdown-input", exact: false, waitUntilElementExist: false);
            await dropdownInput.ClickAsync();

            // 3. Wait for dropdown options to appear and select the correct one
            // Dropdown options have class: fxc-dropdown-option and role="treeitem"
            var options = await ElementHelper.GetByClassAsync(page, "fxc-dropdown-option", exact: false, waitUntilElementExist: false);
            var optionList = await options.AllAsync();

            ILocator dropDownItem = null;
            foreach (var item in optionList)
            {
                var text = (await item.InnerTextAsync()).Trim();
                if (text.Equals(valueToSelect, StringComparison.OrdinalIgnoreCase) && await item.IsVisibleAsync())
                {
                    dropDownItem = item;
                    break;
                }
            }

            if (dropDownItem == null)
                throw new Exception($"Dropdown item '{valueToSelect}' not found or not visible.");

            await dropDownItem.ClickAsync();

            // 4. Wait a moment for the selection to register (optional, mimics Selenium's Thread.Sleep)
            await Task.Delay(1000);

            // 5. Validate the selection
            var selectedText = (await dropdownInput.InnerTextAsync()).Trim();
            if (!selectedText.Equals(valueToSelect, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException($"{valueToSelect} does not appear to be selected in drop down. {selectedText} is displayed instead.");
            }

        }
        public static async Task ClickLiOptionAsync(IPage page, string parentLabel, string value)
        {
            Console.WriteLine($"Set the value of \"{parentLabel}\" as: \"{value}\"");

            // Find the parent element whose text starts with parentLabel
            var parent = await FindItemStartingWithAsync(page, parentLabel, "fxc-section-control");

            // Use your helper to find all <li> elements inside the parent
            var liElements = await ElementHelper.GetByLocatorAsync(parent, "li", isNeedSleep: false);
            var liList = await liElements.AllAsync();

            ILocator targetLi = null;
            foreach (var li in liList)
            {
                var text = (await li.InnerTextAsync()).Trim();
                if (text.Equals(value, StringComparison.OrdinalIgnoreCase))
                {
                    targetLi = li;
                    break;
                }
            }

            if (targetLi == null)
                throw new Exception($"List item '{value}' not found under parent '{parentLabel}'.");

            await targetLi.ClickAsync();
        }

        public static async Task ClickLinkAsync(IPage page, string ariaLabelText, string linkText)
        {
            if (page == null)
                throw new ArgumentNullException(nameof(page));

            // Use your helper to find all <a> elements with role="button" and the correct aria-label
            var links = await ElementHelper.GetByLocatorAsync(page, $"a[role='button'][aria-label='{ariaLabelText}']", isNeedSleep: false);
            var linkList = await links.AllAsync();

            ILocator targetLink = null;
            foreach (var link in linkList)
            {
                var text = (await link.InnerTextAsync()).Trim();
                if (text == linkText)
                {
                    targetLink = link;
                    break;
                }
            }

            if (targetLink == null)
                throw new Exception($"Link with aria-label '{ariaLabelText}' and text '{linkText}' not found.");

            await targetLink.ClickAsync();
        }

        public static async Task SetDependencyFSAsync(IPage page, string appName, List<DependencyEntity> dependencyEntityList)
        {
            // 1. Click the "+ Add" button using your helper
            await ClickLinkAsync(page, "Add", "+ Add");

            // 2. Find the dependency table (try both possible classes)
            ILocator table = null;
            var tableClasses = new[] { "msportalfx-docking-body", "azc-grid-container" };
            foreach (var tableClass in tableClasses)
            {
                var tables = await ElementHelper.GetByClassAsync(page, tableClass, exact: false, waitUntilElementExist: false);
                var tableList = await tables.AllAsync();
                foreach (var grid in tableList)
                {
                    var headers = await ElementHelper.GetByClassAsync(grid, "azc-grid-tableHeader", exact: false, waitUntilElementExist: false);
                    if (await headers.CountAsync() > 0)
                    {
                        var headerText = (await headers.First.InnerTextAsync()).Trim().ToUpper();
                        if (headerText.Contains("DEPENDENCY COUNT"))
                        {
                            table = grid;
                            break;
                        }
                    }
                }
                if (table != null) break;
            }
            if (table == null)
                throw new Exception("Dependency table not found.");

            // 3. For each dependency, select the row (search if needed)
            foreach (var dep in dependencyEntityList)
            {
                ILocator theRow = null;
                var rows = await ElementHelper.GetByClassAsync(table, "azc-grid-row", exact: false, waitUntilElementExist: false);
                var rowList = await rows.AllAsync();
                foreach (var row in rowList)
                {
                    var text = (await row.InnerTextAsync()).Trim();
                    if (text.Contains(dep.Name))
                    {
                        theRow = row;
                        break;
                    }
                }

                // If not found, try searching
                if (theRow == null)
                {
                    var searchInputs = await ElementHelper.GetByClassAsync(page, "azc-input", exact: false, waitUntilElementExist: false);
                    var inputList = await searchInputs.AllAsync();
                    ILocator searchBox = null;
                    foreach (var input in inputList)
                    {
                        var ariaLabel = await input.GetAttributeAsync("aria-label");
                        if (ariaLabel == "Select")
                        {
                            searchBox = input;
                            break;
                        }
                    }
                    if (searchBox != null)
                    {
                        await searchBox.FillAsync(dep.Name);
                        // Wait for the row to appear after search
                        rows = await ElementHelper.GetByClassAsync(page, "azc-grid-row", exact: false, waitUntilElementExist: false);
                        rowList = await rows.AllAsync();
                        foreach (var row in rowList)
                        {
                            var text = (await row.InnerTextAsync()).Trim();
                            if (text.Contains(dep.Name))
                            {
                                theRow = row;
                                break;
                            }
                        }
                        await searchBox.FillAsync(""); // Clear the search box
                    }
                }

                if (theRow == null)
                    throw new Exception($"Dependency row for '{dep.Name}' not found.");

                await theRow.ClickAsync();
            }

            IntuneWin32Apps.PressActionButtonAndWaitForBladeAsync(_page, "Select", "");

            foreach (var dependencyEntity in dependencyEntityList)
            {
                Console.WriteLine($"Set the Automatically install Value is \"{(dependencyEntity.AutomaticallyInstall ? "Yes" : "No")}\" of \"{dependencyEntity.Name}\"");

                // Find the row for the dependency app using your Playwright helper method
                var theRow = await SelectAndReturnRowAsync(page, dependencyEntity.Name);

                // Find all <li> elements inside the row using your helper
                var liElements = await ElementHelper.GetByLocatorAsync(theRow, "li", isNeedSleep: false);
                var liList = await liElements.AllAsync();

                // Find the <li> with the correct text ("Yes" or "No") and click it
                foreach (var li in liList)
                {
                    var text = (await li.InnerTextAsync()).Trim();
                    if (text.Equals(dependencyEntity.AutomaticallyInstall ? "Yes" : "No", StringComparison.OrdinalIgnoreCase))
                    {
                        await li.ClickAsync();
                        break;
                    }
                }
            }




        }


        public static async Task<ILocator> SelectAndReturnRowAsync(IPage page, string itemToSelect)
        {
            Console.WriteLine($"[SelectAndReturnRow] Select value: \"{itemToSelect}\"");

            if (page == null)
                throw new ArgumentNullException(nameof(page));

            for (int retries = 0; retries < 10; retries++)
            {
                try
                {
                    // Try new UI: fxc-gc-table
                    var tables = await ElementHelper.GetByClassAsync(page, "fxc-gc-table", exact: false, waitUntilElementExist: false);
                    var tableList = await tables.AllAsync();
                    if (tableList.Count > 0)
                    {
                        var rows = await ElementHelper.GetByClassAsync(tableList.Last(), "fxc-gc-row", exact: false, waitUntilElementExist: false);
                        var rowList = await rows.AllAsync();
                        foreach (var row in rowList)
                        {
                            var text = (await row.InnerTextAsync()).Trim();
                            if (text.Contains(itemToSelect))
                            {
                                return row;
                            }
                        }
                    }

                    // Try old UI: azc-grid-groupdata
                    tables = await ElementHelper.GetByClassAsync(page, "azc-grid-groupdata", exact: false, waitUntilElementExist: false);
                    tableList = await tables.AllAsync();
                    if (tableList.Count > 0)
                    {
                        var rows = await ElementHelper.GetByClassAsync(tableList.Last(), "azc-grid-row", exact: false, waitUntilElementExist: false);
                        var rowList = await rows.AllAsync();
                        foreach (var row in rowList)
                        {
                            var text = (await row.InnerTextAsync()).Trim();
                            if (text.Contains(itemToSelect))
                            {
                                return row;
                            }
                        }
                    }
                }
                catch
                {
                    // Ignore and retry
                }
                await Task.Delay(500);
            }

            throw new InvalidOperationException("Unable to find item in grid: " + itemToSelect);
        }

        public static async Task<ILocator> FindItemStartingWithAsync(IPage page, string itemName, string itemClass)
        {
            if (page == null)
                throw new ArgumentNullException(nameof(page));

            int maxCount = 120;
            ILocator foundItem = null;

            while (maxCount-- > 0)
            {
                // Use your helper to get all elements with the given class
                var elements = await ElementHelper.GetByClassAsync(page, itemClass, exact: false, waitUntilElementExist: false);
                var elementList = await elements.AllAsync();

                foreach (var item in elementList)
                {
                    var text = (await item.InnerTextAsync()).Trim();
                    if (text.StartsWith(itemName, StringComparison.OrdinalIgnoreCase))
                    {
                        foundItem = item;
                        break;
                    }
                }

                if (foundItem != null)
                    break;

                await Task.Delay(500);
            }

            if (foundItem == null)
                throw new InvalidOperationException($"Unable to find: {itemName} in {itemClass}");

            // Optional: Wait for UI stability
            await Task.Delay(1000);

            return foundItem;
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

                            await IntuneWin32Apps.File_Browser(appFileFullPath);

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


                        else
                        {
                            if (key.Equals("Value") && item.RequirementInfo.Keys.Any(a => a == "Select output data type" || a == "Property" || a == "Registry key requirement")
                                && item.RequirementInfo[item.RequirementInfo.Keys.Where(a => a == "Select output data type" || a == "Property" || a == "Registry key requirement").First()].Contains("Date"))
                            {
                                var dateValue = item.RequirementInfo[key].Split(',');
                                // Locate the stable parent element by class (using your helper)
                                var parent = await ElementHelper.GetByClassAsync(_page, "azc-dateTimePickerField", exact: false, waitUntilElementExist: false);


                                // Locate the date input inside the parent element
                                //var date = await ElementHelper.GetByClassAndHasTextAsync(parent, "azc-input", "MM/DD/YYYY");
                                var date = await ElementHelper.GetByClassAndPlaceholderAsync(_page, "azc-input", "MM/DD/YYYY");
                                // Locate the time input inside the parent element
                                var time = await ElementHelper.GetByClassAndHasTextAsync(parent, "azc-input", "h:mm:ss AM/PM");



                               await IntuneWin32Apps.SetDateTimeClear(date);
                              await  IntuneWin32Apps.SendKeysToItemAsync(dateValue[0], date);


                                if (dateValue.Length > 1)
                                {
                                    time.ClickAsync();
                                    await IntuneWin32Apps.SetDateTimeClear(time);
                                    await IntuneWin32Apps.SendKeysToItemAsync(dateValue[1], time);
                                }
                            }

                            else
                            {
                                await IntuneWin32Apps.SetItemTextInCurrentBladeAsync(key, item.RequirementInfo[key]);
                            }















                        }

                        await IntuneWin32Apps.PressActionButtonAndWaitForBladeAsync(_page, "OK", "");




















                    }

                }



                await IntuneWin32Apps.PressActionButtonAndWaitForBladeAsync(_page, "Next", "");


            }
        }




        public static async Task SetDetectionFS(RootObject entity)
        {
            try
            {


                IntuneWin32Apps.SelectSingleDropDownItemAsync(_page, "Rules format", entity.RulesFormat);

                if (entity.RulesFormat.ToLower() == "use a custom detection script")
                {
                    var item = entity.DetectionRules[0];
                    //Select Rules format
                    string appFileFullPath = System.Text.RegularExpressions.Regex.Replace(Path.Combine(Environment.CurrentDirectory, item.RuleInfo["Script file"].Replace(".\\", "")), "[+^%~()]", "{$0}");
                    IntuneWin32Apps.File_Browser(appFileFullPath);

                    //foreach (var ruleInfo in item.RuleInfo.Keys)
                    //{
                    //    if (ruleInfo != "Script file")
                    //        IntuneWin32Apps.ClickLiOption(_page, ruleInfo, item.RuleInfo[ruleInfo]);
                    //}

                }
                else
                {
                    foreach (var item in entity.DetectionRules)
                    {
                        //Select Rules format
                        IntuneWin32Apps.PressActionButtonAndWaitForBladeAsync(_page, "Add", "Detection rule", "fxc-dockedballoon");

                        //wait until the page loads


                        //Select rule type
                        IntuneWin32Apps.SelectSingleDropDownItemAsync(_page, "Rule type", item.RuleType.ToString());
                        foreach (var ruleInfo in item.RuleInfo.Keys)
                        {
                            if (ruleInfo.Equals("Detection method"))
                            {
                                IntuneWin32Apps.SelectSingleDropDownItemAsync(_page, "Detection method", item.RuleInfo[ruleInfo]);
                            }
                            else if (ruleInfo.Equals("Operator"))
                            {
                                IntuneWin32Apps.SelectSingleDropDownItemAsync(_page, item.RuleInfo[ruleInfo], "Operator");
                            }
                            else if (ruleInfo.Equals("MSI product version check") || ruleInfo.Contains("Associated with a 32-bit"))
                            {
                                IntuneWin32Apps.ClickLiOptionAsync(_page, ruleInfo, item.RuleInfo[ruleInfo]);
                            }
                            else
                            {
                                if (item.RuleInfo.Keys.Contains("Detection method") && item.RuleInfo["Detection method"].Contains("Date") && ruleInfo.Equals("Value"))
                                {
                                    var dateValue = item.RuleInfo[ruleInfo].Split(',');
                                    var parent = await ElementHelper.GetByClassAsync(_page, "azc-dateTimePickerField", exact: false, waitUntilElementExist: false);


                                    // Locate the date input inside the parent element
                                    var date = await ElementHelper.GetByClassAndPlaceholderAsync(_page, "azc-input", "MM/DD/YYYY");

                                    // Locate the time input inside the parent element
                                    var time = await ElementHelper.GetByClassAndHasTextAsync(parent, "azc-input", "h:mm:ss AM/PM");

                                    IntuneWin32Apps.SetDateTimeClear(date);
                                    IntuneWin32Apps.SendKeysToItemAsync(dateValue[0], date);
                                    if (dateValue.Length > 1)
                                    {
                                        time.ClickAsync();
                                        IntuneWin32Apps.SetDateTimeClear(time);
                                        IntuneWin32Apps.SendKeysToItemAsync(dateValue[1], time);
                                    }
                                }
                                else
                                {
                                    IntuneWin32Apps.SetItemTextInCurrentBladeAsync(ruleInfo, item.RuleInfo[ruleInfo]);
                                }
                            }
                        }
                        IntuneWin32Apps.PressActionButtonAndWaitForBladeAsync(_page, "OK", "", "ext-actionbar-button");
                    }
                }
                // ClickExtensions.PressActionButtonAndWaitForBlade(Framework, "OK", "", "ext-actionbar-button");
                IntuneWin32Apps.PressActionButtonAndWaitForBladeAsync(_page, "OK", "", "ext-actionbar-button");


            }

          
                    
                
            
            catch (Exception ex)
            {
                throw; // You should log this or wrap it in a custom exception
            }
            // The DetectionRules section remains unchanged as it's already dictionary-based











        }
    
    

        public static async Task SetDependenciesFS(RootObject entity)
        {

            if (entity.DependencyEntities != null && entity.DependencyEntities.Count > 0)
            {
                try
                {

                   // Framework.WaitElementLoaded("Software dependencies", "azc-formElementSubLabelContainer", "azc-formElementContainer");
                    IntuneWin32Apps.SetDependencyFSAsync(_page, entity.TestCaseName, entity.DependencyEntities);
                }
                catch (Exception ex)
                {
                   // appTestResult.ErrorMessage.AppendLine("Dependency app failed." + ex.Message);
                   // log.Error("Dependency app failed." + ex.Message);
                    throw ex;
                }
            }
            IntuneWin32Apps.PressActionButtonAndWaitForBladeAsync(_page, "Next", "");




        }







    }
}


















