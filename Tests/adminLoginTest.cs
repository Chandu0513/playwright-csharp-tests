using Microsoft.Playwright;
using PlaywrightNUnitFramework.Pages;
using PlaywrightNUnitFramework.Utils;


namespace PlaywrightNUnitFramework.Tests
{

    public class adminLoginTest : BaseTest
    {

        public IPage? TestPage => Page;
        public async Task AdminVerifyLoginPageTitleAfterLogin(string browserName)
        {
            await InitializePlaywright(browserName);
            var adminPage = new adminLoginPage(Page!, Config!);
            await adminPage.Navigate();
            await adminPage.Login();
            await adminPage.GetTitle();
            await utilsMethod.StopAndSaveTrace(Context!, browserName: browserName);

        }
    }
}







