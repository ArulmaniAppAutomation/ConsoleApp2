namespace PlaywrightTests.Common.Controller.SearchBox
{
    public interface ISearchBox
    {
        public Task SetSearchBoxValueAsync(string value);
        public Task ClearSearchBoxValueAsync();
    }
}
