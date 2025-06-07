using NUnit.Framework;
using PlaywrightNUnitFramework.Tests;
using PlaywrightNUnitFramework.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlaywrightNUnitFramework
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
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
        }

        [OneTimeTearDown]
        public void TearDownReport()
        {
            ExtentReportManager.Flush();
        }

        [Test]
        [TestCaseSource(nameof(BrowserList))]
        public async Task RunTestsInOrder(string browserName)
        {
            // Create a test in ExtentReports for this run
            string testName = $"EmpLogin - {browserName}";
            ExtentReportManager.CreateTest(testName);

            var empLogin = new EmpLogin();
            try
            {
                await empLogin.VerifyEmpLoginPageTitleAfterLogin(browserName);
                ExtentReportManager.LogPass($"{testName} passed");
            }
            catch (System.Exception ex)
            {
                ExtentReportManager.LogFail($"{testName} failed: {ex.Message}");
                throw; // rethrow so NUnit marks it failed
            }

            // var empLeave = new EmpApplyLeave();
            // await empLeave.VerifyEmpApplyLeave(browserName);

            // var admin = new adminLoginTest();
            // await admin.AdminVerifyLoginPageTitleAfterLogin(browserName);

            // var adminHoliday = new AdminHolidayList();
            // await adminHoliday.VerifyAdminHolidayList(browserName);

            // var adminApprove = new adminApproveLeave();
            // await adminApprove.AdminVerifyApproveLeave(browserName);

            // var empExtraWorking = new empExtraWorking();
            // await empExtraWorking.VerifyEmpApplyExtraWorking(browserName);

            // var adminApproveExtra = new adminApproveExtra();
            // await adminApproveExtra.AdminVerifyApproveExtra(browserName);
        }
    }
}
