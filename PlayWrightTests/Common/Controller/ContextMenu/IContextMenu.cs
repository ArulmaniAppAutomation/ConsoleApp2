using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaywrightTests.Common.Controller.ContextMenu
{
    internal interface IContextMenu
    {
        public Task CLickSpecialNameMenuItemAsync(string menuItemName);
        public Task ClickDeleteContextMenuItemAsync();
        public Task ClickDuplicateContextMenuItemAsync();
        public Task ClickExportJSONContextMenuItemAsync();
    }
}
