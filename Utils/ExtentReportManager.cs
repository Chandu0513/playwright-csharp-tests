using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using System;
using System.IO;

namespace PlaywrightNUnitFramework.Utils
{
    public static class ExtentReportManager
    {
        private static ExtentReports? _extent;
        private static ExtentTest? _currentTest;

        public static void InitReport()
        {

            var dir = @"C:\Users\Admin\PlaywrightNUnitFramework\TestReports";
            Directory.CreateDirectory(dir);

            var htmlReporter = new ExtentSparkReporter(Path.Combine(dir, "ExtentReport.html"));
            htmlReporter.Config.DocumentTitle = "Playwright Test Report";
            htmlReporter.Config.ReportName = "Execution Report";

            _extent = new ExtentReports();
            _extent.AttachReporter(htmlReporter);
        }

        public static void CreateTest(string testName)
        {
            _currentTest = _extent?.CreateTest(testName);
        }

        public static void LogPass(string message)
        {
            _currentTest?.Pass(message);
        }

        public static void LogFail(string message)
        {
            _currentTest?.Fail(message);
        }

        public static void AddScreenshot(string screenshotPath)
        {
            _currentTest?.AddScreenCaptureFromPath(screenshotPath);
        }

        public static void Flush()
        {
            _extent?.Flush();
        }
    }
}
