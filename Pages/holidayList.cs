using Microsoft.Playwright;
using PlaywrightNUnitFramework.Utils;
using System;
using System.Threading.Tasks;

namespace PlaywrightNUnitFramework.Pages
{
    public class holidayList
    {
        private readonly IPage _page;

        private readonly string _holidayCards = ".holiday-display-card";
        private readonly string _holidayName = ".holiday-name";
        private readonly string _holidayDate = ".name-heading";

        public holidayList(IPage page)
        {
            _page = page;
        }

        public async Task FetchHolidayList()
{
    // Wait for the holiday elements to be visible on the page
    await _page.WaitForSelectorAsync(_holidayCards);

    // Get all holiday elements
    var holidayElements = _page.Locator(_holidayCards);
    int totalHolidays = await holidayElements.CountAsync();

    // Log the total number of holidays to both the console and the Extent Report
    string totalHolidaysMessage = $"Total holidays: {totalHolidays}";
    Console.WriteLine(totalHolidaysMessage);  // Console Output
    ExtentReportManager.LogInfo(totalHolidaysMessage);  // Extent Report Output

    // Loop through the holiday elements and extract their details
    for (int i = 0; i < totalHolidays; i++)
    {
        var nameLocator = holidayElements.Nth(i).Locator(_holidayName);
        var dateLocator = holidayElements.Nth(i).Locator(_holidayDate);

        // Get the holiday name and date from the page
        string name = await nameLocator.InnerTextAsync();
        string date = await dateLocator.InnerTextAsync();

        // Log the holiday name and date to both the console and the Extent Report
        string holidayMessage = $"Holiday {i + 1}: {name} on {date}";
        Console.WriteLine(holidayMessage);  // Console Output
        ExtentReportManager.LogInfo(holidayMessage);  // Extent Report Output
    }
}


    }
}
