using NUnit.Framework;
using PlaywrightNUnitFramework.Tests;
using PlaywrightNUnitFramework.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;
using static PlaywrightNUnitFramework.Pages.utilsMethod;

namespace PlaywrightNUnitFramework
{
    [TestFixture]
    [Parallelizable(ParallelScope.Self)]
    public class testRunner
    {
        public static IEnumerable<string> BrowserList()
        {
            var config = new TestConfig();
            return config.Browsers;
        }

        [OneTimeSetUp]
        public void InitReport()
        {
            ExtentReportManager.InitReport();
            ScreenshotHelper.ClearOldScreenshots();
        }

        [OneTimeTearDown]
        public void TearDownReport()
        {
            ExtentReportManager.Flush();
        }

        // [Test, Order(1)]
        // [TestCaseSource(nameof(BrowserList))]
        // public async Task EmpLoginTest(string browserName)
        // {
        //     string testName = $"EmpLogin - {browserName}";
        //     ExtentReportManager.CreateTest(testName);
        //     var empLogin = new EmpLogin();
        //     try
        //     {
        //         await empLogin.VerifyEmpLoginPageTitleAfterLogin(browserName);
        //         ExtentReportManager.LogPass($"{testName} passed");
        //     }
        //     catch (System.Exception ex)
        //     {
        //         var screenshotPath = await ScreenshotHelper.CaptureScreenshot(empLogin.TestPage!, testName);
        //         ExtentReportManager.LogFail($"{testName} failed: {ex.Message}");
        //         ExtentReportManager.AttachScreenshot(screenshotPath);
        //         throw;
        //     }
        // }

        [Test, Order(2)]
        [TestCaseSource(nameof(BrowserList))]
        public async Task EmpApplyLeaveTest(string browserName)
        {
            string testName = $"EmpApplyLeave - {browserName}";
            ExtentReportManager.CreateTest(testName);
            var empLeave = new EmpApplyLeave();
            try
            {
                await empLeave.VerifyEmpApplyLeave(browserName);
                ExtentReportManager.LogPass($"{testName} passed");
            }
            catch (System.Exception ex)
            {
                var screenshotPath = await ScreenshotHelper.CaptureScreenshot(empLeave.TestPage!, testName);
                ExtentReportManager.LogFail($"{testName} failed: {ex.Message}");
                ExtentReportManager.AttachScreenshot(screenshotPath);
                throw;
            }
        }

        // [Test, Order(3)]
        // [TestCaseSource(nameof(BrowserList))]
        // public async Task AdminLoginTest(string browserName)
        // {
        //     string testName = $"AdminLogin - {browserName}";
        //     ExtentReportManager.CreateTest(testName);
        //     var admin = new adminLoginTest();
        //     try
        //     {
        //         await admin.AdminVerifyLoginPageTitleAfterLogin(browserName);
        //         ExtentReportManager.LogPass($"{testName} passed");
        //     }
        //     catch (System.Exception ex)
        //     {
        //         var screenshotPath = await ScreenshotHelper.CaptureScreenshot(admin.TestPage!, testName);
        //         ExtentReportManager.LogFail($"{testName} failed: {ex.Message}");
        //         ExtentReportManager.AttachScreenshot(screenshotPath);
        //         throw;
        //     }
        // }

        // [Test, Order(4)]
        // [TestCaseSource(nameof(BrowserList))]
        // public async Task AdminHolidayListTest(string browserName)
        // {
        //     string testName = $"AdminHolidayList - {browserName}";
        //     ExtentReportManager.CreateTest(testName);
        //     var adminHoliday = new AdminHolidayList();
        //     try
        //     {
        //         await adminHoliday.VerifyAdminHolidayList(browserName);
        //         ExtentReportManager.LogPass($"{testName} passed");
        //     }
        //     catch (System.Exception ex)
        //     {
        //         var screenshotPath = await ScreenshotHelper.CaptureScreenshot(adminHoliday.TestPage!, testName);
        //         ExtentReportManager.LogFail($"{testName} failed: {ex.Message}");
        //         ExtentReportManager.AttachScreenshot(screenshotPath);
        //         throw;
        //     }
        // }

        // [Test, Order(5)]
        // [TestCaseSource(nameof(BrowserList))]
        // public async Task AdminApproveLeaveTest(string browserName)
        // {
        //     string testName = $"AdminApproveLeave - {browserName}";
        //     ExtentReportManager.CreateTest(testName);
        //     var adminApprove = new adminApproveLeave();
        //     try
        //     {
        //         await adminApprove.AdminVerifyApproveLeave(browserName);
        //         ExtentReportManager.LogPass($"{testName} passed");
        //     }
        //     catch (System.Exception ex)
        //     {
        //         var screenshotPath = await ScreenshotHelper.CaptureScreenshot(adminApprove.TestPage!, testName);
        //         ExtentReportManager.LogFail($"{testName} failed: {ex.Message}");
        //         ExtentReportManager.AttachScreenshot(screenshotPath);
        //         throw;
        //     }
        // }

        // [Test, Order(6)]
        // [TestCaseSource(nameof(BrowserList))]
        // public async Task EmpExtraWorkingTest(string browserName)
        // {
        //     string testName = $"EmpExtraWorking - {browserName}";
        //     ExtentReportManager.CreateTest(testName);
        //     var empExtraWorking = new empExtraWorking();
        //     try
        //     {
        //         await empExtraWorking.VerifyEmpApplyExtraWorking(browserName);
        //         ExtentReportManager.LogPass($"{testName} passed");
        //     }
        //     catch (System.Exception ex)
        //     {
        //         var screenshotPath = await ScreenshotHelper.CaptureScreenshot(empExtraWorking.TestPage!, testName);
        //         ExtentReportManager.LogFail($"{testName} failed: {ex.Message}");
        //         ExtentReportManager.AttachScreenshot(screenshotPath);
        //         throw;
        //     }
        // }

        // [Test, Order(7)]
        // [TestCaseSource(nameof(BrowserList))]
        // public async Task AdminApproveExtraTest(string browserName)
        // {
        //     string testName = $"AdminApproveExtra - {browserName}";
        //     ExtentReportManager.CreateTest(testName);
        //     var adminApproveExtra = new adminApproveExtra();
        //     try
        //     {
        //         await adminApproveExtra.AdminVerifyApproveExtra(browserName);
        //         ExtentReportManager.LogPass($"{testName} passed");
        //     }
        //     catch (System.Exception ex)
        //     {
        //         var screenshotPath = await ScreenshotHelper.CaptureScreenshot(adminApproveExtra.TestPage!, testName);
        //         ExtentReportManager.LogFail($"{testName} failed: {ex.Message}");
        //         ExtentReportManager.AttachScreenshot(screenshotPath);
        //         throw;
        //     }
        // }
    }
}
