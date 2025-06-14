using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using Ecommerce_website.Models;
using Ecommerce_website.ViewModels;

public class StoresController : Controller
{
    private readonly IHttpClientFactory _clientFactory;

    public StoresController(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    [HttpGet]
    public async Task<IActionResult> Stores()
    {
        var token = HttpContext.Session.GetString("JWToken");

        if (string.IsNullOrEmpty(token))
        {
            return RedirectToAction("Login", "Account"); 
        }

        var client = _clientFactory.CreateClient("NoSSLValidation");

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
    public async Task<IActionResult> NewStore(NewStoreViewModel viewModel)
    {
        return Ok();
    }
}
