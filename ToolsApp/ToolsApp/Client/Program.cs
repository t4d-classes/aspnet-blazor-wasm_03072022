using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using Autofac;
using Autofac.Extensions.DependencyInjection;

using ToolsApp.Client;


var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.ConfigureContainer(new AutofacServiceProviderFactory(containerBuilder => {
  containerBuilder.RegisterType<ColorsData>().As<IColorsData>().SingleInstance();
  containerBuilder.RegisterType<CarsData>().As<ICarsData>().SingleInstance();
  containerBuilder.RegisterType<ScreenBlocker>().As<IScreenBlocker>().SingleInstance();
}));

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp =>
  new HttpClient {
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
  });

await builder.Build().RunAsync();
