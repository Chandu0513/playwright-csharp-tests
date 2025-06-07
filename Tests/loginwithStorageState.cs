using NUnit.Framework;
using PlaywrightNUnitFramework.Pages;
using PlaywrightNUnitFramework.Utils;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace PlaywrightNUnitFramework.Tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.None)]
    public class LoginWithStorageState : BaseTest
    {
        private const string AuthStoragePath = "auth.json";

        public static IEnumerable<string> BrowserList()
        {
            var config = new TestConfig();
            return config.Browsers;
        }

        [Test, Ignore("This test is temporarily excluded.")]
        [TestCaseSource(nameof(BrowserList))]
        public async Task LoginAndSaveStorageState(string browserName)
        {
            if (File.Exists(AuthStoragePath))
            {
                File.Delete(AuthStoragePath);
            }

            await InitializePlaywright(browserName);

            var config = new TestConfig();
            var adminPage = new adminLoginPage(Page!, config);
            await adminPage.Login();

            await Page!.WaitForURLAsync("https://dev.urbuddi.com/");


            await Context!.StorageStateAsync(new Microsoft.Playwright.BrowserContextStorageStateOptions
            {
                Path = AuthStoragePath
            });


            await adminPage.GetTitle();





        }
    }
}
