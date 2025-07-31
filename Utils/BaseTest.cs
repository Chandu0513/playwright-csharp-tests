using AventStack.ExtentReports;
using Microsoft.Playwright;
using NUnit.Framework;
using System.Text.RegularExpressions;
using System.IO;
using PlaywrightNUnitFramework.Utils;

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

            // string safeTestName = Regex.Replace(TestContext.CurrentContext.Test.Name, "[^a-zA-Z0-9-_\\.]", "_");
            // var videoPath = Path.Combine(AppContext.BaseDirectory, "videos", safeTestName);
            // Directory.CreateDirectory(videoPath);
            string safeTestName = Regex.Replace(TestContext.CurrentContext.Test.Name, "[^a-zA-Z0-9-_\\.]", "_");
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string randomString = Path.GetRandomFileName().Replace(".", "").Substring(0, 6); // 6-char random string

            string folderName = $"{safeTestName}_{timestamp}_{randomString}";
            string videoPath = Path.Combine(AppContext.BaseDirectory, "videos", folderName);
            Directory.CreateDirectory(videoPath);
            var contextOptions = new BrowserNewContextOptions
            {
                BaseURL = Config.BaseUrl,
                ViewportSize = null,
                RecordVideoDir = videoPath,
                RecordVideoSize = new RecordVideoSize { Width = 1280, Height = 720 }
            };

            if (!string.IsNullOrEmpty(authStoragePath) && File.Exists(authStoragePath))
                contextOptions.StorageStatePath = authStoragePath;

            Context = await Browser.NewContextAsync(contextOptions);
            Page = await Context.NewPageAsync();
            Page.SetDefaultTimeout(Config.Timeout);


            await Context.Tracing.StartAsync(new TracingStartOptions
            {
                Screenshots = true,
                Snapshots = true,
                Sources = true
            });


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
            var testContext = TestContext.CurrentContext;

            if (testContext.Result.FailCount > 0)
                ExtentReportManager.LogFail(testContext.Result.Message ?? "Test Failed");
            else
                ExtentReportManager.LogPass("Test Passed");

            if (Context != null) await Context.CloseAsync();
            if (Browser != null) await Browser.CloseAsync();
            Playwright?.Dispose();
        }





    }
}
