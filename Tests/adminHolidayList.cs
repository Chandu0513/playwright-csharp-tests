using Microsoft.Playwright;
using PlaywrightNUnitFramework.Pages;
using PlaywrightNUnitFramework.Utils;



namespace PlaywrightNUnitFramework.Tests
{

    public class AdminHolidayList : BaseTest
    {

        public IPage? TestPage => Page;
        public async Task VerifyAdminHolidayList(string browserName)
        {
            await InitializePlaywright(browserName);
            var adminPage = new adminLoginPage(Page!, Config!);
            var holiday = new holidayList(Page!);
            await adminPage.Navigate();
            await adminPage.Login();
            await adminPage.GetTitle();
            await holiday.FetchHolidayList();

        }
    }
}
