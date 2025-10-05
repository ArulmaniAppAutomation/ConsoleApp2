using Microsoft.Playwright;
using PlaywrightTests.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account_Management.CommonBase
{
    public class SelectAppTypeUtils: BaseCommonUtils
    {
        public SelectAppTypeUtils(IPage page, string env) : base(page, env)
        {
        }

        public async Task SelectAppTypeAsync(string? appType, string? typeCategory = null)
        {
            if (string.IsNullOrEmpty(appType))
            {
               // throw new CustomLogException($"appType is null, please set the right value!");
            }
            if (!string.IsNullOrEmpty(typeCategory))
            {
                await ControlHelper.SetComBoxRoleTreeItemRoleValueAsync(_page, "App Type", typeCategory, appType, 0, iFrameName: null);
            }
            else
            {
                await ControlHelper.SetComBoxRoleTreeItemRoleValueAsync(_page, "App Type", appType, 0, iFrameName: null);
            }
            await ClickSelectBtnAsync();
        }






    }



}
