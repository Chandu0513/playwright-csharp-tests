using NUnit.Framework;
using PlaywrightNUnitFramework.Pages;
using PlaywrightNUnitFramework.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace PlaywrightNUnitFramework.Tests
{
   
    public class EmpApplyLeave : BaseTest
    {

              
        public async Task VerifyEmpApplyLeave(string browserName)
        {
            await InitializePlaywright(browserName);
            var config = new TestConfig();
            var emplogin = new empApplyLeavePage(Page!, config);
            await emplogin.Navigate(Config!.BaseUrl);
            await emplogin.Login();
            await emplogin.GetTitle();
            await emplogin.ApplyLeave();
           // await emplogin.ClickOkOnLopWarningModal();
            await emplogin.Leavedatepickup();
            await emplogin.FillLeaveDetails();
            await emplogin.CheckboxSubmit();
            await emplogin.SubmitLeave();


        }
    }
}
