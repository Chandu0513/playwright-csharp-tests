using PlaywrightNUnitFramework.Tests;
using PlaywrightNUnitFramework.Utils;


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

        [Test]
        [TestCaseSource(nameof(BrowserList))]
        public async Task RunTestsInOrder(string browserName)
        {

            var empLogin = new EmpLogin();
            await empLogin.VerifyEmpLoginPageTitleAfterLogin(browserName);

            var empLeave = new EmpApplyLeave();
            await empLeave.VerifyEmpApplyLeave(browserName);

            var admin = new adminLoginTest();
            await admin.AdminVerifyLoginPageTitleAfterLogin(browserName);

            var adminHoliday = new AdminHolidayList();
            await adminHoliday.VerifyAdminHolidayList(browserName);

            var adminApprove = new adminApproveLeave();
            await adminApprove.AdminVerifyApproveLeave(browserName);

            var empExtraWorking = new empExtraWorking();
            await empExtraWorking.VerifyEmpApplyExtraWorking(browserName);

            var adminApproveExtra = new adminApproveExtra();
            await adminApproveExtra.AdminVerifyApproveExtra(browserName);
        }
    }
}
