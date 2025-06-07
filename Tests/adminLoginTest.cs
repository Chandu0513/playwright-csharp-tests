using PlaywrightNUnitFramework.Pages;
using PlaywrightNUnitFramework.Utils;


namespace PlaywrightNUnitFramework.Tests
{

    public class adminLoginTest : BaseTest
    {

        public async Task AdminVerifyLoginPageTitleAfterLogin(string browserName)
        {
            await InitializePlaywright(browserName);
            var adminPage = new adminLoginPage(Page!, Config!);
            await adminPage.Navigate();
            await adminPage.Login();
            await adminPage.GetTitle();

        }
    }
}







