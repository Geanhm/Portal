using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Portal.BlazorWasm;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Remova a linha que usa builder.HostEnvironment.BaseAddress
// Mantenha apenas esta, que é a que aponta para o seu Back-end
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.Configuration["ApiUrl"] ?? "http://localhost:5000")
});

await builder.Build().RunAsync();