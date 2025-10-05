using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account_Management.CommonBase
{
    public class WindowsUtils: BaseCommonUtils
    {


        public WindowsUtils(IPage page, string env) : base(page, env)
        {


        }
        public new async Task GoToMainPageAsync()
        {

            await GoToHomePageAsync();
            siteBar.ClickAppsAsync();
            await siteBarMenu.ClickWindowsAsync();









        }


    }
}
