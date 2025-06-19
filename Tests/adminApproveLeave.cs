using Microsoft.Playwright;
using PlaywrightNUnitFramework.Pages;
using PlaywrightNUnitFramework.Utils;


namespace PlaywrightNUnitFramework.Tests
{

    public class adminApproveLeave : BaseTest
    {

        public IPage? TestPage => Page;
        public async Task AdminVerifyApproveLeave(string browserName)
        {
            await InitializePlaywright(browserName);
            var adminPage = new adminLoginPage(Page!, Config!);
            var Utils = new utilsMethod(Page!);
            await adminPage.Navigate();
            await adminPage.Login();
            await adminPage.GetTitle();
            await Utils.ApproveLeave("EMPNEW123");
            await utilsMethod.StopAndSaveTrace(Context!, browserName: browserName);
        }
    }
}
