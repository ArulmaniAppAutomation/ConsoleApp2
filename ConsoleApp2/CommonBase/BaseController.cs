using Account_Management.Helper;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account_Management.CommonBase
{
    public class BaseController
    {

        protected IPage? CurrentIPage;
        protected string? CurrentIFrameName;
        protected ILocator? ParentLocator;
        protected EnumHelper.Language CurrentLanguage;
        protected string NoDataText;
        public BaseController(IPage? page, string? frameName, EnumHelper.Language language, ILocator? parentLocator = null, string noDataText = null)
        {
            CurrentIPage = page;
            CurrentIFrameName = frameName;
            ParentLocator = parentLocator;
            CurrentLanguage = language;
            NoDataText = noDataText;
        }

        public void SleepLong()
        {
            Thread.Sleep(3 * 1000);
        }
        public void SleepMiddle()
        {
            Thread.Sleep(2 * 1000);
        }
        public void SleepShort()
        {
            Thread.Sleep(1 * 1000);
        }
        public void Sleep(int seconds)
        {
            Thread.Sleep(seconds * 1000);
        }
    }
}

