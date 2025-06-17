using dotenv.net;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PlaywrightNUnitFramework.Utils
{
    public class TestConfig
    {
        public List<string> Browsers { get; }
        public string BaseUrl { get; }
        public int Timeout { get; }
        public bool IsHeadless { get; }
        public string AdminEmail { get; }
        public string AdminPassword { get; }
        public string EmployeeEmail { get; }
        public string EmployeePassword { get; }

        public TestConfig()
        {
            DotEnv.Load();

            var browserEnv = Environment.GetEnvironmentVariable("BROWSERS");
            var baseUrlEnv = Environment.GetEnvironmentVariable("BASE_URL");
            var timeoutEnv = Environment.GetEnvironmentVariable("DEFAULT_TIMEOUT");
            var headlessEnv = Environment.GetEnvironmentVariable("HEADLESS");
            var adminEmailEnv = Environment.GetEnvironmentVariable("ADMIN_EMAIL");
            var adminPasswordEnv = Environment.GetEnvironmentVariable("ADMIN_PASSWORD");
            var employeeEmailEnv = Environment.GetEnvironmentVariable("EMP_EMAIL");
            var employeePasswordEnv = Environment.GetEnvironmentVariable("EMP_PASSWORD");

            
            if (string.IsNullOrWhiteSpace(browserEnv)) throw new Exception("Missing BROWSERS in .env file.");
            if (string.IsNullOrWhiteSpace(baseUrlEnv)) throw new Exception("Missing BASE_URL in .env file.");
            if (string.IsNullOrWhiteSpace(timeoutEnv)) throw new Exception("Missing DEFAULT_TIMEOUT in .env file.");
            if (string.IsNullOrWhiteSpace(adminEmailEnv)) throw new Exception("Missing ADMIN_EMAIL in .env file.");
            if (string.IsNullOrWhiteSpace(adminPasswordEnv)) throw new Exception("Missing ADMIN_PASSWORD in .env file.");
            if (string.IsNullOrWhiteSpace(employeeEmailEnv)) throw new Exception("Missing EMP_EMAIL in .env file.");
            if (string.IsNullOrWhiteSpace(employeePasswordEnv)) throw new Exception("Missing EMP_PASSWORD in .env file.");

            
            Browsers = browserEnv.Split(',').Select(b => b.Trim()).ToList();
            BaseUrl = baseUrlEnv;

            if (!int.TryParse(timeoutEnv, out var timeout))
                throw new Exception("DEFAULT_TIMEOUT must be a valid integer.");
            Timeout = timeout;

            IsHeadless = !string.Equals(headlessEnv, "false", StringComparison.OrdinalIgnoreCase);

            AdminEmail = adminEmailEnv;
            AdminPassword = adminPasswordEnv;
            EmployeeEmail = employeeEmailEnv;
            EmployeePassword = employeePasswordEnv;
        }
    }
}
