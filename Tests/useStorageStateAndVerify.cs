using NUnit.Framework;
using PlaywrightNUnitFramework.Utils;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Playwright;
using PlaywrightNUnitFramework.Pages;

namespace PlaywrightNUnitFramework.Tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.None)]
    public class UseStorageStateAndVerify : BaseTest

    {
        private const string AuthStoragePath = "auth.json";

        public static IEnumerable<string> BrowserList()
        {
            var config = new TestConfig();
            return config.Browsers;
        }
         [Test, Ignore("This test is temporarily excluded.")]
        [TestCaseSource(nameof(BrowserList))]
        public async Task FetchHolidayCountInMultipleTabs(string browserName)
        {
            Assert.That(File.Exists(AuthStoragePath), "Storage state file not found. Run login test first.");

            // Initialize Playwright with storage state
            await InitializePlaywright(browserName, AuthStoragePath);

            // Tab 1: Fetch holiday data
            var tab1 = await Context!.NewPageAsync();
            await tab1.GotoAsync("https://dev.urbuddi.com/");
            await tab1.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var holidayElements = tab1.Locator(".holiday-display-card");
            int totalHolidays = await holidayElements.CountAsync();
            Console.WriteLine($"Total holidays: {totalHolidays}");

            for (int i = 0; i < totalHolidays; i++)
            {
                var nameLocator = holidayElements.Nth(i).Locator(".holiday-name");
                var dateLocator = holidayElements.Nth(i).Locator(".name-heading");

                string name = await nameLocator.InnerTextAsync();
                string date = await dateLocator.InnerTextAsync();
                Console.WriteLine($"Holiday {i + 1}: {name} on {date}");
            }

            var title1 = await tab1.TitleAsync();
            Assert.That(title1, Is.EqualTo("urBuddi"), "Tab 1 title mismatch.");

            
            var tab2 = await Context.NewPageAsync();
            await tab2.GotoAsync("https://dev.urbuddi.com/settings");
            var title2 = await tab2.TitleAsync();
            Console.WriteLine($"Tab 2 title: {title2}");

            // Tab 3 (Optional): Another page like reports
            var tab3 = await Context.NewPageAsync();
            await tab3.GotoAsync("https://dev.urbuddi.com/reports");
            var title3 = await tab3.TitleAsync();
            var adminPage = new adminLoginPage(Page!, Config!);
            await adminPage.GetTitle();

            // Optional delay for visual inspection
            await Task.Delay(5000);
        }
    }
}

