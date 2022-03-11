using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using Autofac;
using Autofac.Extensions.DependencyInjection;

using ToolsApp.Client;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.ConfigureContainer(new AutofacServiceProviderFactory(containerBuilder => {
  containerBuilder.RegisterType<ColorsData>().As<IColorsData>().SingleInstance();
  containerBuilder.RegisterType<CarsData>().As<ICarsData>().SingleInstance();
  containerBuilder.RegisterType<ScreenBlocker>().As<IScreenBlocker>().SingleInstance();
}));

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient(
  "ToolsApp.ServerAPI",
  client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

builder.Services.AddScoped(
  sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("ToolsApp.ServerAPI"));

builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
    options.ProviderOptions.DefaultAccessTokenScopes.Add("api://e737479a-9e3b-4809-a5c7-8764df51e920/API.Access");
    options.ProviderOptions.LoginMode = "redirect";
});

await builder.Build().RunAsync();
