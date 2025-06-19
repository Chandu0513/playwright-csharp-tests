using Microsoft.Playwright;
using PlaywrightNUnitFramework.Pages;
using PlaywrightNUnitFramework.Utils;


namespace PlaywrightNUnitFramework.Tests
{

    public class EmpLogin : BaseTest
    {
        public IPage? TestPage => Page;

        public async Task VerifyEmpLoginPageTitleAfterLogin(string browserName)
        {
            await InitializePlaywright(browserName);

            var emplogin = new empLoginPage(Page!, Config!);
            await emplogin.Navigate(Config!.BaseUrl);
            await emplogin.Login();
            await emplogin.GetTitle();
            await utilsMethod.StopAndSaveTrace(Context!, browserName: browserName);
        }


    }
}
