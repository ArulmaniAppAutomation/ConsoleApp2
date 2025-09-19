using Microsoft.Playwright;
using PlaywrightTests.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaywrightTests.Common.Controller.Blade
{
    public class BladeBase : BaseController
    {
        protected string BladeName { get; set; }
        public BladeBase(IPage? page, string? frameName, EnumHelper.Language language, ILocator? parentLocator = null, string bladeName = null) : base(page, frameName, language, parentLocator)
        {
            BladeName = bladeName;
        }
    }
}
