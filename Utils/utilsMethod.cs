using Microsoft.Playwright;
using System;
using System.Threading.Tasks;
using PlaywrightNUnitFramework.Locators;
using PlaywrightNUnitFramework.Utils;
using System.Text.RegularExpressions;

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

            var nextPageButton = _page.Locator("div[aria-label='Next Page']");
            var leaveRowsLocator = _page.Locator(allLocators.LeftPinnedRows);
            var horizontalScroll = _page.Locator("div.ag-body-horizontal-scroll-viewport");

            HashSet<string> visitedPages = new();

            while (true)
            {
                try
                {
                    await leaveRowsLocator.First.WaitForAsync(new LocatorWaitForOptions { Timeout = 10000 });
                }
                catch (TimeoutException)
                {
                    string noRequestsMessage = "❌ No leave requests found. Approval step failed.";
                    Console.WriteLine(noRequestsMessage);
                    ExtentReportManager.LogInfo(noRequestsMessage);
                    Assert.Fail(noRequestsMessage);
                }

                var leaveRows = await leaveRowsLocator.AllAsync();

                foreach (var row in leaveRows)
                {
                    var empCellElement = row.Locator(allLocators.EmployeeIdCell);
                    var empCell = await empCellElement.InnerTextAsync();

                    if (empCell.Trim() == employeeId)
                    {
                        var rowIndex = await row.GetAttributeAsync("row-index");
                        var approveButton = _page.Locator(allLocators.ApproveButton(rowIndex!));
                        await horizontalScroll.EvaluateAsync("el => el.scrollLeft = el.scrollWidth");

                        try
                        {
                            await approveButton.WaitForAsync(new LocatorWaitForOptions
                            {
                                State = WaitForSelectorState.Visible,
                                Timeout = 5000
                            });

                            await approveButton.ScrollIntoViewIfNeededAsync();
                            await approveButton.ClickAsync();

                            string successMessage = $"✅ Approved leave for employee ID: {employeeId}";
                            Console.WriteLine(successMessage);
                            ExtentReportManager.LogInfo(successMessage);
                            return;
                        }
                        catch (TimeoutException)
                        {
                            string failureMessage = $"❌ Approve button not clickable for employee ID: {employeeId}.";
                            Console.WriteLine(failureMessage);
                            ExtentReportManager.LogInfo(failureMessage);
                            Assert.Fail(failureMessage);
                        }
                    }
                }


                var nextDisabled = await nextPageButton.GetAttributeAsync("class");
                if (nextDisabled != null && nextDisabled.Contains("ag-disabled"))
                    break;


                var pageText = await _page.Locator("span[ref='lbCurrent']").InnerTextAsync();
                if (visitedPages.Contains(pageText))
                    break;

                visitedPages.Add(pageText);


                await nextPageButton.ClickAsync();
                await _page.WaitForTimeoutAsync(1000);
            }

            string notFoundMessage = $"❌ Employee ID {employeeId} not found in any leave request page.";
            Console.WriteLine(notFoundMessage);
            ExtentReportManager.LogInfo(notFoundMessage);
            Assert.Fail(notFoundMessage);
        }






        public async Task ApproveExtraWorking(string employeeId)
        {

            await _page.ClickAsync(allLocators.ExtraWorkReimbursementTab);
            await _page.ClickAsync(allLocators.ExtraWorkRequestsButton);

            var leaveRowsLocator = _page.Locator(allLocators.ExtraWorkLeaveRows);


            try
            {
                await leaveRowsLocator.First.WaitForAsync(new LocatorWaitForOptions { Timeout = 10000 });
            }
            catch (TimeoutException)
            {
                string noRequestsMessage = "❌ No extra work requests to approve. Approval step failed.";
                Console.WriteLine(noRequestsMessage);
                ExtentReportManager.LogInfo(noRequestsMessage);
                Assert.Fail(noRequestsMessage);
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

                        string successMessage = $"✅ Approved extra work for employee ID: {employeeId}";
                        Console.WriteLine(successMessage);
                        ExtentReportManager.LogInfo(successMessage);
                    }
                    catch (TimeoutException)
                    {
                        string failureMessage = $"❌ Approve button not clickable for employee ID: {employeeId}";
                        Console.WriteLine(failureMessage);
                        ExtentReportManager.LogInfo(failureMessage);
                        Assert.Fail(failureMessage);
                    }

                    return;
                }
            }


            string notFoundMessage = $"❌ Employee ID {employeeId} not found in extra work requests. Approval step failed.";
            Console.WriteLine(notFoundMessage);
            ExtentReportManager.LogInfo(notFoundMessage);
            Assert.Fail(notFoundMessage);
        }

        public static class ScreenshotHelper
        {
            // public static async Task<string> CaptureScreenshot(IPage page, string testName)
            // {
            //     string safeName = Regex.Replace(testName, "[^a-zA-Z0-9-_\\.]", "_");
            //     string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            //     string screenshotDir = Path.Combine(Directory.GetCurrentDirectory(), "Screenshots");
            //     Directory.CreateDirectory(screenshotDir);

            //     string screenshotPath = Path.Combine(screenshotDir, $"{safeName}_{timestamp}.png");
            //     await page.ScreenshotAsync(new PageScreenshotOptions { Path = screenshotPath, FullPage = true });

            //     return screenshotPath;
            // }

            public static async Task<string> CaptureScreenshot(IPage? page, string testName)
            {
                if (page == null)
                {
                    Console.WriteLine($"[ScreenshotHelper] Warning: Cannot capture screenshot. Page is null for test: {testName}");
                    return string.Empty;
                }

                string safeName = Regex.Replace(testName, "[^a-zA-Z0-9-_\\.]", "_");
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string screenshotDir = Path.Combine(Directory.GetCurrentDirectory(), "Screenshots");
                Directory.CreateDirectory(screenshotDir);

                string screenshotPath = Path.Combine(screenshotDir, $"{safeName}_{timestamp}.png");
                await page.ScreenshotAsync(new PageScreenshotOptions { Path = screenshotPath, FullPage = true });

                return screenshotPath;
            }

            public static void ClearOldScreenshots()
            {
                string screenshotDir = Path.Combine(Directory.GetCurrentDirectory(), "Screenshots");
                if (Directory.Exists(screenshotDir))
                    Directory.Delete(screenshotDir, true);
            }
        }
        public static async Task StopAndSaveTrace(IBrowserContext context, string? testName = null, string? browserName = null)
        {
            string safeTestName = Regex.Replace(testName ?? TestContext.CurrentContext.Test.Name, "[^a-zA-Z0-9-_\\.]", "_");
            string safeBrowserName = browserName ?? "unknown";

            string traceDir = Path.Combine(AppContext.BaseDirectory, "TestTraces");
            Directory.CreateDirectory(traceDir);

            string tracePath = Path.Combine(traceDir, $"{safeTestName}_{safeBrowserName}_trace.zip");

            try
            {
                await context.Tracing.StopAsync(new TracingStopOptions { Path = tracePath });
                Console.WriteLine($"✅ Trace saved to: {tracePath}");
                ExtentReportManager.LogInfo($"Trace saved to: {tracePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Trace saving failed: {ex.Message}");
                ExtentReportManager.LogInfo($"Trace saving failed: {ex.Message}");
            }
        }

    }
}
