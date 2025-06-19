using Microsoft.Playwright;
using PlaywrightNUnitFramework.Pages;
using PlaywrightNUnitFramework.Utils;


namespace PlaywrightNUnitFramework.Tests
{

    public class adminApproveExtra : BaseTest
    {

        public IPage? TestPage => Page;
        public async Task AdminVerifyApproveExtra(string browserName)
        {
            await InitializePlaywright(browserName);
            var adminPage = new adminApproveExtraPage(Page!, Config!);
            var Utils = new utilsMethod(Page!);
            await adminPage.Navigate();
            await adminPage.Login();
            await adminPage.GetTitle();
            await Utils.ApproveExtraWorking("EMPNEW123");
            await utilsMethod.StopAndSaveTrace(Context!, browserName: browserName);
            // string tracePath = Path.Combine(AppContext.BaseDirectory, "my-trace.zip");
            // await Context!.Tracing.StopAsync(new TracingStopOptions { Path = tracePath });

            // Console.WriteLine($"Trace saved to: {tracePath}");
        }
    }
}
