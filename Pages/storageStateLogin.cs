using Microsoft.Playwright;
using PlaywrightNUnitFramework.Locators;
using PlaywrightNUnitFramework.Utils;

namespace PlaywrightNUnitFramework.Pages
{
    public class StorageStateLogin
    {
        private readonly IPage _page;
        private readonly TestConfig _config;
         private readonly string _emailInput = "#userEmail";
        private readonly string _passwordInput = "#userPassword";
        private readonly string _loginButton = "button[type='submit']";




        public StorageStateLogin(IPage page, TestConfig config)
        {
            _page = page;
            _config = config;
        }

          
        public async Task Navigate(string baseUrl)
        {
            await _page.GotoAsync(baseUrl);
        }

        public async Task Login(string email, string password)
        {
            await _page.FillAsync(_emailInput, email);
            await _page.FillAsync(_passwordInput, password);
            await _page.ClickAsync(_loginButton);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }

        public async Task<string> GetTitle()
        {
            return await _page.TitleAsync();
        }




    }
}



       

       

        