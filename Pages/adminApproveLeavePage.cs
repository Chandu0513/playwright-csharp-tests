using Microsoft.Playwright;
using PlaywrightNUnitFramework.Locators;
using PlaywrightNUnitFramework.Tests;
using PlaywrightNUnitFramework.Utils;

namespace PlaywrightNUnitFramework.Pages
{
    public class adminApproveLeavePage
    {
        private readonly IPage _page;
        private readonly TestConfig _config;



        public adminApproveLeavePage(IPage page, TestConfig config)
        {
            _page = page;
            _config = config;

        }

        public async Task Navigate()
        {
            await _page.GotoAsync(_config.BaseUrl);
        }

        public async Task Login()
        {
            await _page.FillAsync(allLocators.EmailInput, _config.AdminEmail);
            await _page.FillAsync(allLocators.PasswordInput, _config.AdminPassword);
            await _page.ClickAsync(allLocators.LoginButton);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }

        public async Task GetTitle()
        {
            var title = await _page.TitleAsync();
            var expectedTitle = "urBuddi";
            Assert.That(title, Is.EqualTo(expectedTitle), "Title after login is incorrect.");
        }

        public async Task ApproveLeave(string employeeId)
        {
            await _page.ClickAsync(allLocators.LeaveManagement);
            await _page.ClickAsync(allLocators.RequestsButton);
            var leaveRows = await _page.QuerySelectorAllAsync(allLocators.LeftPinnedRows);
            if (leaveRows == null || leaveRows.Count == 0)
            {
                Console.WriteLine("ℹ️ No leave requests to approve. Skipping approval step.");
                return;
            }

            foreach (var row in leaveRows)
            {
                var empCellElement = await row.QuerySelectorAsync(allLocators.EmployeeIdCell);
                if (empCellElement == null)
                    continue;
                var empCell = await empCellElement.InnerTextAsync();
                if (empCell.Trim() == employeeId)
                {
                    var rowIndex = await row.GetAttributeAsync("row-index");
                    await _page.EvaluateAsync($@"() => {{
                const viewport = document.querySelector('{allLocators.CenterColsViewport}');
                if (viewport) {{
                    viewport.scrollLeft = viewport.scrollWidth;
                }}
            }}");

                    await _page.WaitForTimeoutAsync(300);
                    var approveButton = _page.Locator(allLocators.ApproveButton(rowIndex!));
                    await approveButton.ScrollIntoViewIfNeededAsync();
                    await approveButton.ClickAsync();
                    Console.WriteLine($"✅ Approved leave for employee ID: {employeeId}");
                    return;
                }
            }

            Console.WriteLine($"ℹ️ Employee ID {employeeId} not found in leave requests. No approval needed.");
        }

    }
}
