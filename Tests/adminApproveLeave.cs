using PlaywrightNUnitFramework.Pages;
using PlaywrightNUnitFramework.Utils;


namespace PlaywrightNUnitFramework.Tests
{
    
    public class adminApproveLeave : BaseTest
    {
    
        
        public async Task AdminVerifyApproveLeave(string browserName)
        {
            await InitializePlaywright(browserName);
            var adminPage = new adminLoginPage(Page!, Config!);
            var Utils = new utilsMethod(Page!);
            await adminPage.Navigate();      
            await adminPage.Login();
            await adminPage.GetTitle();
            await Utils.ApproveLeave("EMPD321");
        }
    }
}
