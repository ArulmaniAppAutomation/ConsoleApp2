using Microsoft.Playwright;
using PlaywrightTests.Common.Helper;

namespace PlaywrightTests.Common.Controller.Grid
{
    public class GridPager_FXS : BaseController
    {

        private ILocator GridPagerLoctor
        {
            get
            {
                return GetGridPagerLocatorAsync().Result;
            }
        }
        private string ButtonClass = "fxs-button fxt-button fxs-inner-solid-border";
        private string PreviousButton = "< Previous";
        private string NextButton = "Next >";
        public GridPager_FXS(IPage? page, string? frameName, EnumHelper.Language language, ILocator? parentLocator = null) : base(page, frameName,language, parentLocator)
        {
        }
        public async Task<int> GetCurrentPageNumAsync()
        {
            int pageNum = 0;
            var combox = await ControlHelper.GetLocatorByRoleAndHasTextAsync(GridPagerLoctor, AriaRole.Combobox, "");
            var currentPageNum = await combox.InnerTextAsync();
            int.TryParse(currentPageNum, out pageNum);
            return pageNum;
        }
        public async Task<int> GetMaxPageNumAsync()
        {
            int pageNum = 0;
            await ControlHelper.ClickComBoxRoleAsync(GridPagerLoctor, 0);
            var maxPageNum = await ControlHelper.GetByRoleAndHasTextAsync(GridPagerLoctor, AriaRole.Treeitem, "", -1);
            var maxPageNumText = await maxPageNum.InnerTextAsync();
            int.TryParse(maxPageNumText, out pageNum);
            return pageNum;
        }
        public async Task ClickPreviousButtonAsync()
        {
            await ControlHelper.ClickByClassAndHasTextAsync(GridPagerLoctor, ButtonClass, PreviousButton, 0);
        }
        public async Task ClickNextPageButtonAsync()
        {
            await ControlHelper.ClickByClassAndHasTextAsync(GridPagerLoctor, ButtonClass, NextButton, 0);
        }
        /// <summary>
        /// Get the status of the previous button
        /// </summary>
        /// <returns></returns>
        public async Task<bool> GetPreviousButtonIsEnabledAsync()
        {
            var button = await ControlHelper.GetLocatorByClassAndHasTextAsync(GridPagerLoctor, ButtonClass, PreviousButton, 0);
            var status = await button.GetAttributeAsync("aria-disabled");
            return status == "false";
        }
        public async Task<bool> GetNextButtonIsEnabledAsync()
        {
            var button = await ControlHelper.GetLocatorByClassAndHasTextAsync(GridPagerLoctor, ButtonClass, NextButton, 0);
            var status = await button.GetAttributeAsync("aria-disabled");
            return status == "false";
        }
        #region private methods
        private async Task<ILocator> GetGridPagerLocatorAsync()
        {
            return await ControlHelper.GetByLocatorAsync(this.CurrentIPage, "[data-bind='pcControl: gridPager'][data-formelement='pcControl: gridPager']", 0, iFrameName: this.CurrentIFrameName);
        }

        #endregion
    }
}
