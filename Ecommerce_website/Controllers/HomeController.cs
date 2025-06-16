using Ecommerce_website.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using System.Security.Claims;
using System.Net.Http.Headers;
using Ecommerce_website.Interfaces;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IEncryptionService _encryptionService;

    public HomeController(ILogger<HomeController> logger,
         IEncryptionService encryptionService,
         IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _encryptionService = encryptionService;
    }

    public async Task<IActionResult> StoreDashboard(string storeId)
    {
        var token = HttpContext.Session.GetString("JWToken");

        if (string.IsNullOrEmpty(token))
            return RedirectToAction("Login", "Account");

        var client = _httpClientFactory.CreateClient("NoSSLValidation");

        var request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:7135/api/Stores/storeDetails?storeId={storeId}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await client.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "Failed to load store details.");
            return View(new StoreDetails()); 
        }

        var json = await response.Content.ReadAsStringAsync();

        var storeDetails = JsonSerializer.Deserialize<StoreDetails>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        ViewBag.StoreName = storeDetails.StoreName;
        ViewBag.StoreLogo = storeDetails.StoreLogo;

        return View(storeDetails);
    }


    public async Task<IActionResult> Index(string tab)
    {
        var userJson = HttpContext.Session.GetString("CurrentUser");
        if (!string.IsNullOrEmpty(userJson))
        {
            var user = JsonSerializer.Deserialize<CurrentUserResponse>(userJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (user != null)
            {

                ViewBag.ActiveTab = string.IsNullOrEmpty(tab) ? "sportnews" : tab;
                return RedirectToAction("Home");
            }
        }


        var token = HttpContext.Session.GetString("JWToken");
        if (string.IsNullOrEmpty(token))
        {
            ViewBag.ActiveTab = string.IsNullOrEmpty(tab) ? "sportnews" : tab;
            return View();
        }

        // If you want, you can call API to validate token and get user here,
        // but since you store user in session, that may be redundant.

        // For now, redirect to Login if no user info:
        return RedirectToAction("Login", "Accounts");
    }


    public async Task<IActionResult> Home()
    {
        var userJson = HttpContext.Session.GetString("CurrentUser");

        if (string.IsNullOrEmpty(userJson))
        {
            return RedirectToAction("Login", "Accounts");
        }

        var user = JsonSerializer.Deserialize<CurrentUserResponse>(userJson, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (user == null)
        {
            return RedirectToAction("Login", "Accounts");
        }

        ViewBag.LoggedInUser = $"{user.FirstName} {user.LastName}".Trim();

        var roles = user.Roles ?? new List<string>();

        if (roles.Contains("System Administrator"))
            return View("AdminDashboard");
        if (roles.Contains("Store Owner"))
            return View("StoreOwnerDashboard");
        if (roles.Contains("Paramedic"))
            return View("ParamedicDashboard");
        if (roles.Contains("Kitchen Staff"))
            return View("KitchenStaffDashboard");
        if (roles.Contains("Pharmacist"))
            return View("PharmacistDashboard");
        if (roles.Contains("Receptionist"))
            return View("ReceptionistDashboard");

        return View("PatientDashboard");
    }



    private async Task<CurrentUserResponse?> GetCurrentUserFromApi(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            Console.WriteLine(JsonSerializer.Serialize(new
            {
                Error = "JWT token is null or empty",
                Timestamp = DateTime.UtcNow
            }));
            return null;
        }

        var client = _httpClientFactory.CreateClient("NoSSLValidation");

        try
        {
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync("https://localhost:7135/api/CurrentUser/currentUser");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(JsonSerializer.Serialize(new
                {
                    Error = "Failed to retrieve user from API",
                    StatusCode = response.StatusCode,
                    Timestamp = DateTime.UtcNow
                }));
                return null;
            }

            var jsonString = await response.Content.ReadAsStringAsync();

            var user = JsonSerializer.Deserialize<CurrentUserResponse>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Console.WriteLine(JsonSerializer.Serialize(new
            {
                Message = "User successfully retrieved from API",
                User = user,
                Timestamp = DateTime.UtcNow
            }));

            return user;
        }
        catch (Exception ex)
        {
            Console.WriteLine(JsonSerializer.Serialize(new
            {
                Error = "Exception occurred while fetching user",
                Exception = ex.Message,
                Timestamp = DateTime.UtcNow
            }));
            return null;
        }
    }



    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
