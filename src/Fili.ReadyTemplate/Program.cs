using Fili.ReadyTemplate;
using Fili.ReadyTemplate.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Content (templates, docs, the 2004 overview) is fetched from the site's own static files,
// so the client is rooted at the host's base address and works under any sub-path.
builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddLocalization();
builder.Services.AddScoped<CultureService>();
builder.Services.AddScoped<ContentLoader>();
builder.Services.AddSingleton<TemplateCatalog>();

var host = builder.Build();

// wwwroot/js/app.js already told the runtime which culture to start in; this only normalises
// anything the runtime chose that the site does not ship resources for.
host.Services.GetRequiredService<CultureService>().NormaliseStartupCulture();

await host.RunAsync();
