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
            // EmpLogin test
            string empLoginTestName = $"EmpLogin - {browserName}";
            ExtentReportManager.CreateTest(empLoginTestName);
            var empLogin = new EmpLogin();
            try
            {
                await empLogin.VerifyEmpLoginPageTitleAfterLogin(browserName);
                ExtentReportManager.LogPass($"{empLoginTestName} passed");
            }
            catch (System.Exception ex)
            {
                ExtentReportManager.LogFail($"{empLoginTestName} failed: {ex.Message}");
                
            }

            // EmpApplyLeave test
            string empLeaveTestName = $"EmpApplyLeave - {browserName}";
            ExtentReportManager.CreateTest(empLeaveTestName);
            var empLeave = new EmpApplyLeave();
            try
            {
                await empLeave.VerifyEmpApplyLeave(browserName);
                ExtentReportManager.LogPass($"{empLeaveTestName} passed");
            }
            catch (System.Exception ex)
            {
                ExtentReportManager.LogFail($"{empLeaveTestName} failed: {ex.Message}");
                
            }

            // AdminLogin test
            string adminLoginTestName = $"AdminLogin - {browserName}";
            ExtentReportManager.CreateTest(adminLoginTestName);
            var admin = new adminLoginTest();
            try
            {
                await admin.AdminVerifyLoginPageTitleAfterLogin(browserName);
                ExtentReportManager.LogPass($"{adminLoginTestName} passed");
            }
            catch (System.Exception ex)
            {
                ExtentReportManager.LogFail($"{adminLoginTestName} failed: {ex.Message}");
                
            }

            // AdminHolidayList test
            string adminHolidayTestName = $"AdminHolidayList - {browserName}";
            ExtentReportManager.CreateTest(adminHolidayTestName);
            var adminHoliday = new AdminHolidayList();
            try
            {
                await adminHoliday.VerifyAdminHolidayList(browserName);
                ExtentReportManager.LogPass($"{adminHolidayTestName} passed");
            }
            catch (System.Exception ex)
            {
                ExtentReportManager.LogFail($"{adminHolidayTestName} failed: {ex.Message}");
                
            }

            // AdminApproveLeave test
            string adminApproveTestName = $"AdminApproveLeave - {browserName}";
            ExtentReportManager.CreateTest(adminApproveTestName);
            var adminApprove = new adminApproveLeave();
            try
            {
                await adminApprove.AdminVerifyApproveLeave(browserName);
                ExtentReportManager.LogPass($"{adminApproveTestName} passed");
            }
            catch (System.Exception ex)
            {
                ExtentReportManager.LogFail($"{adminApproveTestName} failed: {ex.Message}");
                
            }

            // EmpExtraWorking test
            string empExtraWorkingTestName = $"EmpExtraWorking - {browserName}";
            ExtentReportManager.CreateTest(empExtraWorkingTestName);
            var empExtraWorking = new empExtraWorking();
            try
            {
                await empExtraWorking.VerifyEmpApplyExtraWorking(browserName);
                ExtentReportManager.LogPass($"{empExtraWorkingTestName} passed");
            }
            catch (System.Exception ex)
            {
                ExtentReportManager.LogFail($"{empExtraWorkingTestName} failed: {ex.Message}");
                
            }

            // AdminApproveExtra test
            string adminApproveExtraTestName = $"AdminApproveExtra - {browserName}";
            ExtentReportManager.CreateTest(adminApproveExtraTestName);
            var adminApproveExtra = new adminApproveExtra();
            try
            {
                await adminApproveExtra.AdminVerifyApproveExtra(browserName);
                ExtentReportManager.LogPass($"{adminApproveExtraTestName} passed");
            }
            catch (System.Exception ex)
            {
                ExtentReportManager.LogFail($"{adminApproveExtraTestName} failed: {ex.Message}");
                
            }
        }
    }
}
