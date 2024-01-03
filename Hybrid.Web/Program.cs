using Blazored.LocalStorage;
using Hybrid.CQRS;
using Hybrid.Web;
using Hybrid.Web.Auth;
using Hybrid.Web.PlugIn;
using Hybrid.Web.Service;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
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

var url = "https://gssfvpfhwmrwdqjlncij.supabase.co";
String urlGraphql = "https://gssfvpfhwmrwdqjlncij.supabase.co/graphql/v1";
String urlRestful = "https://gssfvpfhwmrwdqjlncij.supabase.co/rest/v1/rpc/";
String key = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6Imdzc2Z2cGZod21yd2RxamxuY2lqIiwicm9sZSI6ImFub24iLCJpYXQiOjE2OTY0NDU1OTMsImV4cCI6MjAxMjAyMTU5M30.oelS4Uht9MrQ2daG-KR8yERstAgL9K0JI7y_FR_F2_E";

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

await builder.Build().RunAsync();
