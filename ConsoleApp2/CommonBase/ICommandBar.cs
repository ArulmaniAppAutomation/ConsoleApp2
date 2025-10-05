using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account_Management.CommonBase
{
    public interface ICommandBar

    {

        public Task ClickCommandBarSpecialNameButtonAsync(string buttonName);
        public Task ClickCommandBarCreatePolicyButtonAsync();
        public Task ClickCommandBarAddButtonAsync();
        public Task ClickCommandBarCreateButtonAsync();
        public Task ClickCommandBarRefreshButtonAsync();
        public Task ClickCommandBarFilterButtonAsync();
        public Task<string> ClickCommandBarExportButtonAsync();
        public Task ClickCommandBarColumnsButtonAsync();
        public Task ClickCommandBarSyncAsync();
        public Task ClickCommandBarImportAsync();
        public Task ClickCommandBarAssignUserAsync();
        public Task ClickCommandBarDeleteAsync();
        public Task ClickCommandBarBulkDeviceActionsButtonAsync();
        public Task ClickCommandBarOverFlowAsync();






    }
}
