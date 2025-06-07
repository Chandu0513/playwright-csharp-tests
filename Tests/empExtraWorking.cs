using PlaywrightNUnitFramework.Pages;
using PlaywrightNUnitFramework.Utils;


namespace PlaywrightNUnitFramework.Tests
{
   
   
    public class empExtraWorking : BaseTest
    {
        
        public async Task VerifyEmpApplyExtraWorking(string browserName)
        {
            await InitializePlaywright(browserName);
            var config = new TestConfig();
            var emplogin = new empExtraWorkingPage(Page!, config);
            await emplogin.Navigate(Config!.BaseUrl);
            await emplogin.Login();
            await emplogin.Reimbursement();
            await emplogin.FillDate();
            await emplogin.FillDetails();
           


        }
    }
}
