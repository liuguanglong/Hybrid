using Blazored.LocalStorage;
using Hybrid.CQRS;
using Hybrid.Web;
using Hybrid.Web.Auth;
using Hybrid.Web.PlugIn;
using Hybrid.Web.Service;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddMudServices();

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>(
    provider => new CustomAuthStateProvider(
        provider.GetRequiredService<ILocalStorageService>(),
        provider.GetRequiredService<Supabase.Client>(),
        provider.GetRequiredService<ILogger<CustomAuthStateProvider>>()
    )
    );
builder.Services.AddAuthorizationCore();

String url = builder.Configuration["SupaBase:Url"];
String urlGraphql = builder.Configuration["SupaBase:GraphqlUrl"];
String urlRestful = builder.Configuration["SupaBase:RestfulServiceUrl"];
String key = builder.Configuration["SupaBase:Key"];

builder.Services.AddScoped<Supabase.Client>(
    provider => new Supabase.Client(
        url,
        key,
        new Supabase.SupabaseOptions
        {
            AutoRefreshToken = true,
            AutoConnectRealtime = true,
            SessionHandler = new CustomSupabaseSessionHandler(
                provider.GetRequiredService<ILocalStorageService>(),
                provider.GetRequiredService<ILogger<CustomSupabaseSessionHandler>>()
            )
        }
    )
);

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped(i =>
                new CommandService(urlGraphql, urlRestful, key)
            );

builder.Services.AddScoped<IPackageRepository, PackageRepository>();
builder.Services.AddScoped<Interop>();
builder.Services.AddSingleton<PluginStateService>(new PluginStateService());

String urlHyridServer = builder.Configuration["HybridServer:Url"];
builder.Services.AddHttpClient("HybridServer", httpClient =>
{
    httpClient.BaseAddress = new Uri(urlHyridServer);
    httpClient.DefaultRequestHeaders.Add(
        HeaderNames.Accept, "application/json");
});

builder.Services.AddScoped<UserService>();

await builder.Build().RunAsync();
