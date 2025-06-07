using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using Microsoft.Playwright;
using NUnit.Framework;
using System.Text.RegularExpressions;
using System.IO;

namespace PlaywrightNUnitFramework.Utils
{
    public class BaseTest
    {
        protected IPlaywright? Playwright;
        protected IBrowser? Browser;
        protected IBrowserContext? Context;
        protected IPage? Page;
        protected TestConfig? Config;

        [OneTimeSetUp]
        public void GlobalSetup()
        {
            ExtentReportManager.InitReport();
        }

        [SetUp]
        public void BeforeEachTest()
        {
            string testName = TestContext.CurrentContext.Test.Name;
            ExtentReportManager.CreateTest(testName);
        }

        // YOU will still call this inside each test
        public async Task InitializePlaywright(string browserName, string? authStoragePath = null)
        {
            Config = new TestConfig();
            Playwright = await Microsoft.Playwright.Playwright.CreateAsync();
            var isHeadless = Config.IsHeadless;

            Browser = browserName.ToLower() switch
            {
                "chromium" => await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = isHeadless }),
                "firefox" => await Playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions { Headless = isHeadless }),
                "webkit" => await Playwright.Webkit.LaunchAsync(new BrowserTypeLaunchOptions { Headless = isHeadless }),
                _ => throw new ArgumentException($"Unsupported browser: {browserName}")
            };

            var contextOptions = new BrowserNewContextOptions
            {
                BaseURL = Config.BaseUrl,
                ViewportSize = null
            };

            if (!string.IsNullOrEmpty(authStoragePath) && File.Exists(authStoragePath))
                contextOptions.StorageStatePath = authStoragePath;

            Context = await Browser.NewContextAsync(contextOptions);
            Page = await Context.NewPageAsync();
            Page.SetDefaultTimeout(Config.Timeout);

            if (browserName.ToLower() == "chromium" && !isHeadless)
            {
                var session = await Context.NewCDPSessionAsync(Page);
                var targetInfo = await session.SendAsync("Browser.getWindowForTarget");
                int windowId = targetInfo?.GetProperty("windowId")!.GetInt32() ?? 0;

                await session.SendAsync("Browser.setWindowBounds", new Dictionary<string, object>
                {
                    ["windowId"] = windowId,
                    ["bounds"] = new Dictionary<string, object>
                    {
                        ["windowState"] = "maximized"
                    }
                });
            }
        }

        [TearDown]
        public async Task AfterEachTest()
        {
            var context = TestContext.CurrentContext;
            string status = context.Result.Outcome.Status.ToString();
            string testName = context.Test.Name;

            if (context.Result.FailCount > 0)
            {
                ExtentReportManager.LogFail(context.Result.Message);

                if (Page != null)
                {
                    var screenshotPath = $"TestResults/Screenshots/{SanitizeFileName(testName)}.png";
                    Directory.CreateDirectory(Path.GetDirectoryName(screenshotPath)!);

                    var screenshot = await Page.ScreenshotAsync();
                    await File.WriteAllBytesAsync(screenshotPath, screenshot);
                    ExtentReportManager.AddScreenshot(screenshotPath);
                }
            }
            else
            {
                ExtentReportManager.LogPass("Test Passed");
            }

            if (Context != null) await Context.CloseAsync();
            if (Browser != null) await Browser.CloseAsync();
            Playwright?.Dispose();
        }

        [OneTimeTearDown]
        public void GlobalTearDown()
        {
            ExtentReportManager.Flush();
        }

        private string SanitizeFileName(string name)
        {
            return Regex.Replace(name, "[^a-zA-Z0-9-_\\.]", "_");
        }
    }
}
