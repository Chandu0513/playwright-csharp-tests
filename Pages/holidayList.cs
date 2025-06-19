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
            await _page.WaitForSelectorAsync(_holidayCards);
            var holidayElements = _page.Locator(_holidayCards);
            int totalHolidays = await holidayElements.CountAsync();
            string totalHolidaysMessage = $"Total holidays: {totalHolidays}";
            Console.WriteLine(totalHolidaysMessage);
            ExtentReportManager.LogInfo(totalHolidaysMessage);


            for (int i = 0; i < totalHolidays; i++)
            {
                var nameLocator = holidayElements.Nth(i).Locator(_holidayName);
                var dateLocator = holidayElements.Nth(i).Locator(_holidayDate);
                string name = await nameLocator.InnerTextAsync();
                string date = await dateLocator.InnerTextAsync();
                string holidayMessage = $"Holiday {i + 1}: {name} on {date}";
                Console.WriteLine(holidayMessage);
                ExtentReportManager.LogInfo(holidayMessage);
            }
        }


    }
}
