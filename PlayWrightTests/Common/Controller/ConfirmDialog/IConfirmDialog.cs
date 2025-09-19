using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaywrightTests.Common.Controller.ConfirmDialog
{
    internal interface IConfirmDialog
    {
        public Task<ILocator> GetDialogButtonLocatorByNameAsync(string buttonName);
        public Task ClickDialogYesButtonAsync();
        public Task ClickDialogNoButtonAsync();
        public Task ClickDialogDeleteButtonAsync();
        public Task ClickDialogCancelButtonAsync();
        public Task ClickDialogOKButtonAsync();
    }
}
