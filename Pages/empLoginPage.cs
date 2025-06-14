using Microsoft.Playwright;
using PlaywrightNUnitFramework.Locators;
using PlaywrightNUnitFramework.Utils;

namespace PlaywrightNUnitFramework.Pages
{
    public class empLoginPage
    {
        private readonly IPage _page;
        private readonly TestConfig _config;




        public empLoginPage(IPage page, TestConfig config)
        {
            _page = page;
            _config = config;
        }

        public async Task Navigate(string baseUrl)
        {
            await _page.GotoAsync(baseUrl);
        }

        public async Task Login()
        {
            await _page.FillAsync(allLocators.EmailInput, _config.EmployeeEmail);
            await _page.FillAsync(allLocators.PasswordInput, _config.EmployeePassword);
            await _page.ClickAsync(allLocators.LoginButton);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }

        public async Task GetTitle()
        {
            var title = await _page.TitleAsync();
            Assert.That(title, Is.EqualTo("urBuddi"), "Title after login is incorrect.");
        }


    }
}
