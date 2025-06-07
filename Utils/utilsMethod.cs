using Microsoft.Playwright;
using System;
using System.Threading.Tasks;
using PlaywrightNUnitFramework.Locators;

namespace PlaywrightNUnitFramework.Pages
{
    public class utilsMethod
    {
        private readonly IPage _page;

        public utilsMethod(IPage page)
        {
            _page = page;
        }

        public async Task ApproveLeave(string employeeId)
        {
            await _page.ClickAsync(allLocators.LeaveManagement);
            await _page.ClickAsync(allLocators.RequestsButton);

            var leaveRowsLocator = _page.Locator(allLocators.LeftPinnedRows);


            try
            {
                await leaveRowsLocator.First.WaitForAsync(new LocatorWaitForOptions { Timeout = 10000 });
            }
            catch (TimeoutException)
            {
                Console.WriteLine("ℹ️ No leave requests to approve. Skipping approval step.");
                return;
            }

            var leaveRows = await leaveRowsLocator.AllAsync();

            foreach (var row in leaveRows)
            {
                var empCellElement = row.Locator(allLocators.EmployeeIdCell);
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



        public async Task ApproveExtraWorking(string employeeId)
        {
            // Step 1: Navigate to Reimbursement > Requests > Check Extra Work Requests
            await _page.ClickAsync(allLocators.ExtraWorkReimbursementTab);
            await _page.ClickAsync(allLocators.ExtraWorkRequestsButton);



            var leaveRowsLocator = _page.Locator(allLocators.ExtraWorkLeaveRows);

            // Step 2: Wait for at least one row
            try
            {
                await leaveRowsLocator.First.WaitForAsync(new LocatorWaitForOptions { Timeout = 10000 });
            }
            catch (TimeoutException)
            {
                Console.WriteLine("ℹ️ No extra work requests to approve. Skipping.");
                return;
            }

            var leaveRows = await leaveRowsLocator.AllAsync();

            foreach (var row in leaveRows)
            {
                var empIdCell = row.Locator(allLocators.ExtraWorkEmployeeIdCell);
                var empIdText = await empIdCell.InnerTextAsync();

                if (empIdText.Trim() == employeeId)
                {
                    var approveButton = row.Locator("button").Filter(new() { HasTextString = "Approve" });

                    try
                    {
                        await approveButton.WaitForAsync(new() { State = WaitForSelectorState.Visible, Timeout = 3000 });
                        await approveButton.ClickAsync();
                        Console.WriteLine($"✅ Approved extra work for employee ID: {employeeId}");
                    }
                    catch (TimeoutException)
                    {
                        Console.WriteLine($"⚠️ Approve button not clickable for employee ID: {employeeId}");
                    }

                    return;
                }
            }

            Console.WriteLine($"ℹ️ Employee ID {employeeId} not found in extra work requests.");
        }





    }
}
