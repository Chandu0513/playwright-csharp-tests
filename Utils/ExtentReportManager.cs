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
        public static ExtentTest? CurrentTest => _currentTest;

        public static string ReportRootPath { get; private set; } = string.Empty;

        public static void InitReport()
        {

            var baseDir = AppContext.BaseDirectory;
            ReportRootPath = Path.Combine(baseDir, "TestReports");

            Directory.CreateDirectory(ReportRootPath);

            var reportPath = Path.Combine(ReportRootPath, "ExtentReport.html");
            var htmlReporter = new ExtentSparkReporter(reportPath);
            htmlReporter.Config.DocumentTitle = "Playwright Test Report";
            htmlReporter.Config.ReportName = "Execution Report";

            _extent = new ExtentReports();
            _extent.AttachReporter(htmlReporter);
        }


        public static void CreateTest(string testName)
        {
            _currentTest = _extent?.CreateTest(testName);
        }

        public static void LogPass(string message) => _currentTest?.Pass(message);

        public static void LogFail(string message) => _currentTest?.Fail(message);

        public static void LogInfo(string message)
        {
            if (CurrentTest != null)
                CurrentTest.Info(message);
        }

        public static void Flush() => _extent?.Flush();

        public static void AttachScreenshot(string screenshotPath)
        {
            if (File.Exists(screenshotPath) && _currentTest != null)
            {
                _currentTest.AddScreenCaptureFromPath(screenshotPath);
            }
        }

        public static void AttachVideo(string videoPath)
        {
            if (File.Exists(videoPath) && _currentTest != null)
            {
                string relativePath = Path.GetRelativePath(ReportRootPath, videoPath).Replace("\\", "/");

                string videoHtml = $@"
            <video width='640' height='360' controls>
                <source src='{relativePath}' type='video/webm'>
                Your browser does not support the video tag.
            </video>";

                _currentTest.Info("Test Video:<br>" + videoHtml);
            }
        }



    }

}

