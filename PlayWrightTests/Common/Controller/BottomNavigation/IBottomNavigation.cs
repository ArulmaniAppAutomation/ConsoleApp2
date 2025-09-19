namespace PlaywrightTests.Common.Controller.BottomNavigation
{
    public interface IBottomNavigation
    {
        public Task ClickBottomNavigationSpecialNameButtonAsync(string buttonName);
        public Task VerifyBottomNavigationSpecialNameButtonStatusAsync(string buttonName, string status);
    }
}
