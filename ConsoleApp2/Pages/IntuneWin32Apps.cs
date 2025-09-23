using Account_Management.Framework;
using Microsoft.Playwright;
using PlaywrightTests.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
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

            var element = await ElementHelper.GetByClassAndHasTextAsync(_page, "fxc-dropdown-placeholder", "Select app type");
            element.ClickAsync();
            var win32app_button = await ElementHelper.GetByRoleAndNameAsync(_page, AriaRole.Treeitem, "Windows app (Win32)");
            await win32app_button.ClickAsync();
            var select_button = _page.Locator("//span[text()='Select']");
            await select_button.ClickAsync();
            var selectAppPackageFile = await ElementHelper.GetByButtonRoleAndNameAsync(_page, "Select app package file", true);
            await selectAppPackageFile.ClickAsync();
            // Click the "Select a file" button
            var selectFileButton = await ElementHelper.GetByAttributeTitleAsync(_page, "Select a file");
            await selectFileButton.ClickAsync();

            // Attach the file
            var fileInput = await ElementHelper.GetByClassAsync(_page, "fxs-async-fileupload-overlay");
            await fileInput.SetInputFilesAsync("C:\\Users\\v-arulmani\\source\\repos\\ConsoleApp2\\Test_Apps\\SimpleMSI.intunewin");





        }



    }
}
