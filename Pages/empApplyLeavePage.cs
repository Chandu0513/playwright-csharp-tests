using Microsoft.Playwright;
using PlaywrightNUnitFramework.Locators;
using PlaywrightNUnitFramework.Utils;

namespace PlaywrightNUnitFramework.Pages
{
    public class empApplyLeavePage
    {
        private readonly IPage _page;
        private readonly TestConfig _config;




        public empApplyLeavePage(IPage page, TestConfig config)
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

        public async Task ApplyLeave()
        {
            await _page.ClickAsync(allLocators.LeaveManagement);
            await _page.ClickAsync(allLocators.ApplyLeaveButton);
        }

        private static Random random = new Random(); // Static Random object

    public async Task Leavedatepickup()
{
    DateTime startRange = new DateTime(2025, 7, 2);
    DateTime latestStart = new DateTime(2025, 7, 30);
    int range = (latestStart - startRange).Days;

    // Use static random object for generating random dates
    Random random = new Random();
    DateTime fromDate = startRange.AddDays(random.Next(range + 1));
    DateTime toDate = fromDate.AddDays(1);

    string fromDateStr = fromDate.ToString("yyyy-MM-dd");
    string toDateStr = toDate.ToString("yyyy-MM-dd");

    // Console log for applying leave dates
    string leaveDatesMessage = $"Applying leave from {fromDateStr} to {toDateStr}";
    Console.WriteLine(leaveDatesMessage);  // Print to console

    // Log the same information to Extent Report
    ExtentReportManager.LogInfo(leaveDatesMessage);  // Log to Extent Report

    // Fill the leave form with generated dates
    await _page.FillAsync(allLocators.FromDateInput, fromDateStr);
    await _page.FillAsync(allLocators.ToDateInput, toDateStr);

    // Optional: Wait for a small timeout for the actions to complete
    await _page.WaitForTimeoutAsync(200);
}



        public async Task FillLeaveDetails()
        {
            await _page.SelectOptionAsync(allLocators.LeadSelect, _config.AdminEmail);
            await _page.FillAsync(allLocators.SubjectInput, "Vacation leave request");
            await _page.FillAsync(allLocators.ReasonTextarea, "I would like to apply leave due to personal reasons.");
        }

        public async Task CheckboxSubmit()
        {
            var leaveRadio = _page.Locator(allLocators.LeaveTypeRadio);
            if (!await leaveRadio.IsCheckedAsync())
            {
                await leaveRadio.CheckAsync();
            }
        }

        public async Task SubmitLeave()
        {
            var submitButton = _page.Locator(allLocators.SubmitButton, new() { HasTextString = "Submit" });
            await submitButton.ClickAsync();
            await _page.WaitForTimeoutAsync(4000);
        }

        public async Task ClickOkOnLopWarningModal()
        {
            var okButton = _page.Locator(allLocators.LopModalOkButton);
            await okButton.ClickAsync();
        }
    }
}
