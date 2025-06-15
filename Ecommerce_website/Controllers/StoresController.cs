using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using Ecommerce_website.Models;
using Ecommerce_website.ViewModels;

public class StoresController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public StoresController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
    {
        _httpClientFactory = httpClientFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet]
    public async Task<IActionResult> StoreOwners()
    {
        var token = HttpContext.Session.GetString("JWToken");

        if (string.IsNullOrEmpty(token))
        {
            return RedirectToAction("Login", "Account");
        }

        var client = _httpClientFactory.CreateClient("NoSSLValidation");

        var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7135/api/Stores/storeOwners");
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await client.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "Unable to retrieve store owners from the API.");
            return View(new List<StoreOwnerResponse>());
        }

        var json = await response.Content.ReadAsStringAsync();

        var storeOwners = JsonSerializer.Deserialize<List<StoreOwnerResponse>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return View(storeOwners);
    }

    [HttpGet]
    public async Task<IActionResult> Stores()
    {
        var token = HttpContext.Session.GetString("JWToken");

        if (string.IsNullOrEmpty(token))
        {
            return RedirectToAction("Login", "Account"); 
        }

        var client = _httpClientFactory.CreateClient("NoSSLValidation");

        var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7135/api/Stores/stores");
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await client.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(string.Empty, "Unable to retrieve stores from the API.");
            return View(new List<StoreResponse>());
        }

        var json = await response.Content.ReadAsStringAsync();

        var stores = JsonSerializer.Deserialize<List<StoreResponse>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return View(stores);
    }

    [HttpGet]
    public async Task<IActionResult> NewStore()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> NewStore(NewStoreViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return View(viewModel);

        var token = HttpContext.Session.GetString("JWToken");

        if (string.IsNullOrWhiteSpace(token))
        {
            ModelState.AddModelError("", "Authentication token is missing. Please log in again.");
            return View(viewModel);
        }

        var client = _httpClientFactory.CreateClient("NoSSLValidation");

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using var form = new MultipartFormDataContent();

        form.Add(new StringContent(viewModel.StoreName ?? ""), "StoreName");
        form.Add(new StringContent(viewModel.StoreCode ?? ""), "StoreCode");
        form.Add(new StringContent(viewModel.StoreType.ToString()), "StoreType");
        form.Add(new StringContent(viewModel.Street ?? ""), "Street");
        form.Add(new StringContent(viewModel.City ?? ""), "City");
        form.Add(new StringContent(viewModel.PostalCode ?? ""), "PostalCode");
        form.Add(new StringContent(viewModel.Province.ToString()), "Province");
        form.Add(new StringContent(viewModel.Country ?? ""), "Country");
        form.Add(new StringContent(viewModel.Longitude?.ToString() ?? "0"), "Longitude");
        form.Add(new StringContent(viewModel.Latitude?.ToString() ?? "0"), "Latitude");

        if (viewModel.StoreLogo != null && viewModel.StoreLogo.Length > 0)
        {
            var logoContent = new StreamContent(viewModel.StoreLogo.OpenReadStream());
            logoContent.Headers.ContentType = new MediaTypeHeaderValue(viewModel.StoreLogo.ContentType);
            form.Add(logoContent, "StoreLogo", viewModel.StoreLogo.FileName);
        }

        if (viewModel.SignedContract != null && viewModel.SignedContract.Length > 0)
        {
            var contractContent = new StreamContent(viewModel.SignedContract.OpenReadStream());
            contractContent.Headers.ContentType = new MediaTypeHeaderValue(viewModel.SignedContract.ContentType);
            form.Add(contractContent, "SignedContract", viewModel.SignedContract.FileName);
        }

        var response = await client.PostAsync("https://localhost:7135/api/Stores/newStore", form);

        if (response.IsSuccessStatusCode)
        {
            TempData["Message"] = $"You have successfully added {viewModel.StoreName} with store code {viewModel.StoreCode} into your system.";
            return RedirectToAction("Stores", "Stores");
        }

        var errorContent = await response.Content.ReadAsStringAsync();
        ModelState.AddModelError(string.Empty, "API Error: " + errorContent);

        return View(viewModel);
    }


    [HttpGet]
    public IActionResult NewStoreOwner()
    {
        var model = new NewStoreOwnerViewModel
        {
            ReturnUrl = Url.Action("StoreOwners", "Stores")
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> NewStoreOwner(NewStoreOwnerViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var client = _httpClientFactory.CreateClient();

        using var content = new MultipartFormDataContent();

        content.Add(new StringContent(viewModel.FirstName ?? ""), "FirstName");
        content.Add(new StringContent(viewModel.LastName ?? ""), "LastName");
        content.Add(new StringContent(viewModel.Email ?? ""), "Email");
        content.Add(new StringContent(viewModel.PhoneNumber ?? ""), "PhoneNumber");
        content.Add(new StringContent(viewModel.DateOfBirth.ToString("yyyy-MM-dd")), "DateOfBirth");
        content.Add(new StringContent(viewModel.Gender.ToString()), "Gender");
        content.Add(new StringContent(viewModel.Ethnicity.ToString()), "Ethnicity");
        content.Add(new StringContent(viewModel.HomeLanguage.ToString()), "HomeLanguage");

        content.Add(new StringContent(viewModel.StreetNumber ?? ""), "StreetNumber");
        content.Add(new StringContent(viewModel.City_town ?? ""), "City_town");
        content.Add(new StringContent(viewModel.Province.ToString()), "Province");
        content.Add(new StringContent(viewModel.Zip_code ?? ""), "Zip_code");
        content.Add(new StringContent(viewModel.Country ?? ""), "Country");


        if (viewModel.ProfilePicture != null && viewModel.ProfilePicture.Length > 0)
        {
            var streamContent = new StreamContent(viewModel.ProfilePicture.OpenReadStream());
            streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(viewModel.ProfilePicture.ContentType);
            content.Add(streamContent, "ProfilePicture", viewModel.ProfilePicture.FileName);
        }

        var response = await client.PostAsync("https://localhost:7135/Stores/newStoreOwner", content); 

        if (response.IsSuccessStatusCode)
        {
            TempData["Message"] = "Store owner onboarded successfully.";
            return RedirectToAction("StoreOwners", "Stores");
        }
        else
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, "Failed to onboard store owner. Details: " + responseContent);
            return View(viewModel);
        }
    }

}
