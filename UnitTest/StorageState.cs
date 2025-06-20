using NUnit.Framework;
using PlaywrightNUnitFramework.Pages;
using PlaywrightNUnitFramework.Utils;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Playwright;
using System.Data.SqlTypes;

namespace PlaywrightNUnitFramework.StorageState
{
    [TestFixture]
    [Parallelizable(ParallelScope.Self)]
    public class LoginAndVerifyWithStorageState : BaseTest
    {
        private const string AuthStoragePath = "auth.json";

        public static IEnumerable<string> BrowserList()
        {
            var config = new TestConfig();
            return config.Browsers;
        }

        [Test, Order(1), Ignore("Excluded from this run")]
        [TestCaseSource(nameof(BrowserList))]
        public async Task LoginAndSaveStorageState(string browserName)
        {
            if (File.Exists(AuthStoragePath))
            {
                File.Delete(AuthStoragePath); 
            }

            await InitializePlaywright(browserName); 

           var loginPage = new StorageStateLogin(Page!, Config!);

            await loginPage.Navigate(Config!.BaseUrl);
            await loginPage.Login("twl4admin@gmail.com", "twl4test");

            await Page!.WaitForURLAsync("https://dev.urbuddi.com/");
            await Context!.StorageStateAsync(new BrowserContextStorageStateOptions
            {
                Path = AuthStoragePath
            });

            await AfterEachTest();
        }

        //storagestates

       [Test, Order(2),  Ignore("Excluded from this run")]
        [TestCaseSource(nameof(BrowserList))]
        public async Task OpenEmployeePageUsingStoredLogin(string browserName)
        {
            Assert.That(File.Exists(AuthStoragePath), "Storage state file not found. Run login test first.");

            await InitializePlaywright(browserName, AuthStoragePath);

            await Page!.GotoAsync("https://dev.urbuddi.com/");
            await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var holidayElements = Page.Locator(".holiday-display-card");

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

            var actualTitle = await Page.TitleAsync();
            var expectedTitle = "urBuddi";

            Assert.That(actualTitle, Is.EqualTo(expectedTitle), "Redirect after using storage state failed.");

            await AfterEachTest();
        }
    }
}
