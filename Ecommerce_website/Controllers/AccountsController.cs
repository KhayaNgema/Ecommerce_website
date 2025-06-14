using Ecommerce_website.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

public class AccountsController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AccountsController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
    {
        _httpClientFactory = httpClientFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var client = _httpClientFactory.CreateClient("NoSSLValidation");

        var payload = new
        {
            emailOrPhone = model.EmailOrPhone,
            password = model.Password,
            rememberMe = model.RememberMe
        };

        var json = JsonSerializer.Serialize(payload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.PostAsync("https://localhost:7135/api/Auth/login", content);

        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        var resultString = await response.Content.ReadAsStringAsync();

        var result = JsonSerializer.Deserialize<LoginResponse>(resultString, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (result == null || string.IsNullOrEmpty(result.Token))
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        HttpContext.Session.SetString("JWToken", result.Token);

        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", result.Token);

        var userResponse = await client.GetAsync("https://localhost:7135/api/CurrentUser/currentUser");

        if (!userResponse.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "Failed to retrieve user information.");
            return View(model);
        }

        var userJson = await userResponse.Content.ReadAsStringAsync();

        var user = JsonSerializer.Deserialize<CurrentUserResponse>(userJson, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Failed to retrieve user information.");
            return View(model);
        }

        // Store user info json in session
        HttpContext.Session.SetString("CurrentUser", userJson);

        // Now just redirect without SignInAsync
        return RedirectToAction("Index", "Home");
    }


}
