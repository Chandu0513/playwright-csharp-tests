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

            // Start Tracing here ✅
            await Context.Tracing.StartAsync(new TracingStartOptions
            {
                Screenshots = true,
                Snapshots = true,
                Sources = true
            });

            // Maximize window if Chromium & not headless
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
    string testName = testContext.Test.Name;
    string sanitizedTestName = SanitizeFileName(testName);

    if (testContext.Result.FailCount > 0)
    {
        ExtentReportManager.LogFail(testContext.Result.Message ?? "Test Failed");

        // Save trace on failure
        if (Context != null)
        {
            string traceDir = Path.Combine(ExtentReportManager.ReportRootPath, "Traces");
            Directory.CreateDirectory(traceDir);
            string tracePath = Path.Combine(traceDir, $"{sanitizedTestName}.zip");

            await Context.Tracing.StopAsync(new TracingStopOptions
            {
                Path = tracePath
            });

            ExtentReportManager.LogInfo($"Trace saved: <a href='file:///{tracePath}'>Download Trace</a>");
        }

        // Take screenshot on failure
        if (Page != null)
        {
            string screenshotDir = Path.Combine(ExtentReportManager.ReportRootPath, "Screenshots");
            Directory.CreateDirectory(screenshotDir);
            string screenshotPath = Path.Combine(screenshotDir, $"{sanitizedTestName}.png");

            var screenshot = await Page.ScreenshotAsync();
            await File.WriteAllBytesAsync(screenshotPath, screenshot);
            ExtentReportManager.AddScreenshot(screenshotPath);
        }
    }
    else
    {
        ExtentReportManager.LogPass("Test Passed");

        if (Context != null)
        {
            await Context.Tracing.StopAsync(); // discard trace on pass
        }
    }

    if (Context != null) await Context.CloseAsync();
    if (Browser != null) await Browser.CloseAsync();
    Playwright?.Dispose();
}


        private string SanitizeFileName(string name)
        {
            return Regex.Replace(name, "[^a-zA-Z0-9-_\\.]", "_");
        }
    }
}
