using Microsoft.Playwright;

namespace PlaywrightTests.Common.Controller.Blade
{
    public interface IBlade
    {
        public Task<ILocator> GetBladeLocatorAsync();
        public Task ClickCloseAsync();
    }
}
