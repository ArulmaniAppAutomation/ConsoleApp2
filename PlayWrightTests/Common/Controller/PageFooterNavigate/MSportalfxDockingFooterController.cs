using Microsoft.Playwright;
using PlaywrightTests.Common.Helper;

namespace PlaywrightTests.Common.Controller.PageFooterNavigate
{
    public class MSportalfxDockingFooterController : BaseController
    {

        public MSportalfxDockingFooterController(IPage page, string? frameName, EnumHelper.Language language, ILocator? parentLocator = null) : base(page, frameName, language, parentLocator)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns> The button is Enable or not</returns>
        public async Task<bool> ClickSpecialNameButtonAsync(string name)
        {
          return  await ClickButtonAsync(name);
        }
        public async Task ClickNextButtonAsync()
        {
            await ClickButtonAsync("Next");
        }
        public async Task ClickSelectButtonAsync()
        {
            await ClickButtonAsync("Select");
        }
        private async Task<bool> ClickButtonAsync(string name)
        {
            var buttonLocator = await ControlHelper.GetLocatorByClassAndHasTextAsync(await GetFooterLocatorAsync(), "fxs-button fxt-button", name, 0);
            if (await buttonLocator.IsEnabledAsync())
            {
                await buttonLocator.ClickAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
        private async Task<ILocator> GetFooterLocatorAsync()
        {
            if (this.ParentLocator != null)
            {
                return await ControlHelper.GetLocatorByClassAsync(this.ParentLocator, "msportalfx-docking-footer", 0);
            }
            else
            {
                return await ControlHelper.GetLocatorByClassAsync(this.CurrentIPage, "msportalfx-docking-footer", 0, iFrameName: this.CurrentIFrameName);
            }
        }
    }
}
