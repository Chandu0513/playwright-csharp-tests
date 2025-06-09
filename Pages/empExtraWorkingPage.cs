using Microsoft.Playwright;
using PlaywrightNUnitFramework.Locators;
using PlaywrightNUnitFramework.Utils;

namespace PlaywrightNUnitFramework.Pages
{
    public class empExtraWorkingPage
    {
        private readonly IPage _page;
        private readonly TestConfig _config;




        public empExtraWorkingPage(IPage page, TestConfig config)
        {
            _page = page;
            _config = config;
        }

        public async Task Navigate(string baseUrl)
        {
            await _page.GotoAsync(baseUrl);
        }

        public async Task Login()
        {
            await _page.FillAsync(allLocators.EmailInput, _config.EmployeeEmail);
            await _page.FillAsync(allLocators.PasswordInput, _config.EmployeePassword);
            await _page.ClickAsync(allLocators.LoginButton);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }

        public async Task GetTitle()
        {
            var title = await _page.TitleAsync();
            Assert.That(title, Is.EqualTo("urBuddi"), "Title after login is incorrect.");
        }
        public async Task Reimbursement()
        {
            await _page.ClickAsync(allLocators.ReimbursementLocator);
            await _page.ClickAsync(allLocators.ApplyExtraWorkButtonLocator);

        }

        public async Task FillDetails()
        {
            // Fill the Hours input
            await _page.FillAsync(allLocators.HoursInputLocator, "8");

            // Select the Lead from the dropdown
            await _page.SelectOptionAsync(allLocators.LeadSelectLocator, new SelectOptionValue { Label = _config.AdminEmail });

            // Click the submit button
            await _page.ClickAsync(allLocators.ExtraworkSubmitButtonLocator);

            // Log to Extent Report
            string logMessage = "Extra work details filled and submitted.";
            Console.WriteLine(logMessage);  // Console log
            ExtentReportManager.LogInfo(logMessage);  // Extent Report log
        }

        private static Random random = new Random(); // Static Random object

        public async Task FillDate()
        {
            var now = DateTime.Now;
            var daysInMonth = DateTime.DaysInMonth(now.Year, now.Month);

            // Generate a random date within the current month
            int randomDay = random.Next(1, daysInMonth + 1);
            string randomDate = new DateTime(now.Year, now.Month, randomDay).ToString("yyyy-MM-dd");

            // Fill the extra work date field
            await _page.FillAsync(allLocators.ExtraworkDateInputLocator, randomDate);

            // Log the applied extra working day to console and Extent Report
            string logMessage = $"Extra working day applied for date: {randomDate}";
            Console.WriteLine(logMessage);  // Console log
            ExtentReportManager.LogInfo(logMessage);  // Log to Extent Report
        }





    }
}