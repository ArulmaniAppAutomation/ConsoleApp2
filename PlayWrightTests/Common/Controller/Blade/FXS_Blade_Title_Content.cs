using Microsoft.Playwright;
using PlaywrightTests.Common.Helper;

namespace PlaywrightTests.Common.Controller.Blade
{
    public class FXS_Blade_Title_Content : BladeBase, IBlade
    {
        private ILocator bladeLocator { get { return GetBladeLocatorAsync().Result; } }
        private ILocator BladeTitleLocator { get { return GetBladeTitleLocatorAsync().Result; } }
        public FXS_Blade_Title_Content(IPage? page, string? frameName, EnumHelper.Language language, ILocator? parentLocator = null,string bladeName=null) : base(page, frameName, language, parentLocator,bladeName)
        {
        }

        public async Task<string?> GetBladeTitleAsync()
        {
            return await ControlHelper.GetTextByClassAsync(BladeTitleLocator, "fxs-blade-title-titleText msportalfx-tooltip-overflow", 0);
        }
        public async Task ClickCloseAsync()
        {
            await ControlHelper.ClickByClassAsync(BladeTitleLocator, "fxs-blade-button fxs-blade-close", 0);
        }      
        private async Task<ILocator> GetBladeTitleLocatorAsync()
        {
            return await ControlHelper.GetLocatorByClassAsync(bladeLocator, "fxs-blade-title-content",-1);
        }
        public async Task<ILocator> GetBladeLocatorAsync()
        {
            ILocator bladeHeaderLocator;
            if (this.ParentLocator != null)
            {
                bladeHeaderLocator=await ControlHelper.GetLocatorByClassAndHasTextAsync(this.ParentLocator, "fxs-blade-header", this.BladeName, 0);
            }
            else
            {
                bladeHeaderLocator = await ControlHelper.GetLocatorByClassAndHasTextAsync(this.CurrentIPage, "fxs-blade-header", this.BladeName, 0, this.CurrentIFrameName);
            }
            return await ControlHelper.GetParentLocatorBySonLocatorAsync(bladeHeaderLocator, 1);
        }
    }
}
