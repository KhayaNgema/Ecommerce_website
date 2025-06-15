using Ecommerce_website.Data;
using Ecommerce_website.Helpers;
using Ecommerce_website.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient();
builder.Services.Configure<EncryptionConfiguration>(options =>
{
    options.Key = Convert.FromBase64String(builder.Configuration["AES_KEY"]);
    options.Iv = Convert.FromBase64String(builder.Configuration["AES_IV"]);
});

builder.Services.AddScoped<IEncryptionService, EncryptionService>();

builder.Services.AddHttpClient("NoSSLValidation")
    .ConfigurePrimaryHttpMessageHandler(() =>
        new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        });

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

var defaultCulture = new CultureInfo("en-US");

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(defaultCulture),
    SupportedCultures = new List<CultureInfo> { defaultCulture },
    SupportedUICultures = new List<CultureInfo> { defaultCulture }
});

#if DEBUG
GenerateAndDisplayAesKeys();
#endif

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Accounts}/{action=Login}/{id?}");

app.MapRazorPages();

app.Run();


static void GenerateAndDisplayAesKeys()
{
    using Aes aes = Aes.Create();
    aes.GenerateKey();
    aes.GenerateIV();

    string keyBase64 = Convert.ToBase64String(aes.Key);
    string ivBase64 = Convert.ToBase64String(aes.IV);

    File.WriteAllText("aes-keys.txt", $"AES Key (Base64): {keyBase64}\nAES IV (Base64): {ivBase64}");

    AppContext.SetSwitch("System.Drawing.EnableUnixSupport", true);
}
