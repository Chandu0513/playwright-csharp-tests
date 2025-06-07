using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic; // Make sure this is added for Dictionary
using Microsoft.Playwright;
using NUnit.Framework;

namespace PlaywrightNUnitFramework.Utils
{
    public class BaseTest
    {
        protected IPlaywright? Playwright;
        protected IBrowser? Browser;
        protected IBrowserContext? Context;
        protected IPage? Page;
        protected TestConfig? Config;

        public async Task InitializePlaywright(string browserName, string? authStoragePath = null)
        {
            Config = new TestConfig();
            Playwright = await Microsoft.Playwright.Playwright.CreateAsync();
            var isHeadless = Config.IsHeadless;

            Browser = browserName.ToLower() switch
            {
                "chromium" => await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
                {
                    Headless = isHeadless
                }),
                "firefox" => await Playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions
                {
                    Headless = isHeadless
                }),
                "webkit" => await Playwright.Webkit.LaunchAsync(new BrowserTypeLaunchOptions
                {
                    Headless = isHeadless
                }),
                _ => throw new ArgumentException($"Unsupported browser: {browserName}")
            };

            var contextOptions = new BrowserNewContextOptions
            {
                BaseURL = Config.BaseUrl,
                ViewportSize = null // allow native sizing
            };

            if (!string.IsNullOrEmpty(authStoragePath) && File.Exists(authStoragePath))
            {
                contextOptions.StorageStatePath = authStoragePath;
            }

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
        public async Task TearDown()
        {
            if (Context != null)
                await Context.CloseAsync();

            if (Browser != null)
                await Browser.CloseAsync();

            Playwright?.Dispose();
        }
    }
}
