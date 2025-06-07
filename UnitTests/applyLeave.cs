using NUnit.Framework;
using PlaywrightNUnitFramework.Pages;
using PlaywrightNUnitFramework.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace PlaywrightNUnitFramework.Tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.None)] // Enforces order when needed
    public class ApplyLeave : BaseTest
    {
        public static IEnumerable<string> BrowserList()
        {
            var config = new TestConfig();
            return config.Browsers;
        }

         [Test, Ignore("This test is temporarily excluded.")]
        [TestCaseSource(nameof(BrowserList))]
        [Obsolete]
        public async Task VerifyEmpLoginPageTitleAfterLogin(string browserName)
        {
            await InitializePlaywright(browserName);


            var config = new TestConfig();
            var adminPage = new adminLoginPage(Page!, config);
            var emplogin = new empApplyLeavePage(Page!, config);

            await emplogin.Navigate(Config!.BaseUrl);
            await emplogin.Login();
            await emplogin.ApplyLeave();
            await emplogin.ClickOkOnLopWarningModal();

            // Fill 'From' date
            await Page!.FillAsync("input#fromDate", "2025-06-08");
            await Page.Locator("input#fromDate").PressAsync("Tab"); // trigger blur/change event

            // Fill 'To' date
            await Page.FillAsync("input#toDate", "2025-06-09");
            await Page.Locator("input#toDate").PressAsync("Tab");

            // Select Lead from dropdown
            await Page.SelectOptionAsync("select[name='lead']", "twl4admin@gmail.com");

            // Fill Subject
            await Page.FillAsync("input[name='subject']", "Annual Leave");

            // Fill Reason for Leave
            await Page.FillAsync("textarea[name='reason']", "Family event");

            // Select the radio button 'Leave'
            await Page.CheckAsync("input#leave");

            // Locate the Submit button
            var submitButton = Page.Locator("button[type='submit']", new() { HasTextString = "Submit" });


            // Perform a double click on Submit button
            await submitButton.ClickAsync();
            Thread.Sleep(4000);


        }
    }
}
