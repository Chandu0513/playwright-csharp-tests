using Microsoft.Playwright;
using PlaywrightNUnitFramework.Locators;
using PlaywrightNUnitFramework.Utils;

namespace PlaywrightNUnitFramework.Pages
{
    public class adminLoginPage
    {
        private readonly IPage _page;
        private readonly TestConfig _config;
        


        public adminLoginPage(IPage page, TestConfig config)
        {
            _page = page;
            _config = config;
            
        }

        public async Task Navigate()
        {
            await _page.GotoAsync(_config.BaseUrl);
        }

        public async Task Login()
        {
            await _page.FillAsync(allLocators.EmailInput, _config.AdminEmail);
            await _page.FillAsync(allLocators.PasswordInput, _config.AdminPassword);
            await _page.ClickAsync(allLocators.LoginButton);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }

        public async Task GetTitle()
        {
            var title = await _page.TitleAsync();
            var expectedTitle = "urBuddi";
            Assert.That(title, Is.EqualTo(expectedTitle), "Title after login is incorrect.");
        }

        

    }
}
