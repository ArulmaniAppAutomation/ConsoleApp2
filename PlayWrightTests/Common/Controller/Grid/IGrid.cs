using Microsoft.Playwright;
using System.Data;

namespace PlaywrightTests.Common.Controller.Grid
{
    public interface IGrid
    {
        public Task<List<(string? displayName, string? dataItemKey)>> GetDisplayedRowHeadersAsync();
        public Task DeleteRowByRowHeaderAsync(string RowHeader);
        public Task RemoveRowByRowHeaderAsync(string RowHeader);
        public Task ClearGridAllDataAsync(string columnName, string keyWord);
        public Task ClickRowHeaderToShowDetailAsync(string RowHeader); // click the first cell of the target row
        public Task ClickSpecialColumnToShowDetailAsync(string RowHeader, string ColumnName);
        public Task ClickColumnNameToSortASCAsync(string ColumnName);
        public Task ClickColumnNameToSortDESCAsync(string ColumnName);
        public Task<DataTable> GetGridAllDataAsync();
        public Task<List<string>> GetColumnsDataByColumnNameAsync(string columnName);
        public Task<string> GetGridContextTextAsync();
        public Task ClickContextMenuAsync(int nth=0);
        public Task<bool> WaitGridDataToLoadAsync();
    }
}
