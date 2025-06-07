using PlaywrightNUnitFramework.Pages;
using PlaywrightNUnitFramework.Utils;


namespace PlaywrightNUnitFramework.Tests
{
    
    public class EmpLogin : BaseTest
    {
       
     
        public async Task VerifyEmpLoginPageTitleAfterLogin(string browserName)
        {
            await InitializePlaywright(browserName);
            var emplogin = new empLoginPage(Page!, Config!);
            await emplogin.Navigate(Config!.BaseUrl);
            await emplogin.Login();
            await emplogin.GetTitle();
        }

       
    }
}
