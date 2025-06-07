// using System;
// using System.Net.Http;
// using System.Net.Http.Json;
// using System.Threading.Tasks;
// using NUnit.Framework;

// namespace PlaywrightNUnitFramework.Tests
// {
//     public class LoginWithApi : PlaywrightNUnitFramework.Utils.BaseTest
//     {
//         public class LoginRequest
//         {
//             public string email { get; set; } = null!;
//             public string password { get; set; } = null!;
//             public string device_token { get; set; } = null!;
//             public string domain_name { get; set; } = null!;
//         }

//         public class LoginResponse
//         {
//             public string? token { get; set; }
//         }

//         private async Task<string> GetAuthToken()
//         {
//             using var client = new HttpClient();

//             var loginPayload = new LoginRequest
//             {
//                 email = "twl4admin@gmail.com",
//                 password = "twl4test",
//                 device_token = "f6kKS1pfbCvvllcKQ_mZ1u:APA91bEkNQUfMrUJbZ5KaAzggHIY49PHJsH3AsGq2HH_FTRuPJSL-dFxX4EdgtHoW3vbdssUDoz0dt0OcRd0mJjBZaQzI5DPaacO3VgWMeVXLY0lVsix6hc",
//                 domain_name = "optimworks"
//             };

//             var requestUrl = "https://dev-api.urbuddi.com/v1/authentication";
//             var response = await client.PostAsJsonAsync(requestUrl, loginPayload);
//             response.EnsureSuccessStatusCode();

//             var json = await response.Content.ReadFromJsonAsync<LoginResponse>();

//             if (json == null || string.IsNullOrEmpty(json.token))
//                 throw new Exception("Token was null or empty");

//             return json.token;
//         }

//         [SetUp]
//         public async Task SetupAsync()
//         {
//             // Initialize Playwright with desired browser, e.g. chromium
//             await InitializePlaywright("chromium");

//             // Get token from API
//             var token = await GetAuthToken();

//             // Inject token into localStorage BEFORE navigating
//             await Page!.AddInitScriptAsync($@"
//                 window.localStorage.setItem('loginDetails', '{token}');
//             ");
//         }

//         [TearDown]
//         public async Task TearDownAsync()
//         {
//             await Cleanup();
//         }

//      [Test]
// public async Task Test_AuthenticatedAccess()
// {
//     await Page!.GotoAsync("https://dev.urbuddi.com/");

//     var title = await Page.TitleAsync();
//     Console.WriteLine("Page Title: " + title);

//     Assert.That(title.ToLower(), Does.Contain("dashboard").Or.Contain("urbuddi"));
// }

//     }
// }
