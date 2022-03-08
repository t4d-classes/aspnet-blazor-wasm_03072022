using Microsoft.AspNetCore.ResponseCompression;
using System.Reflection;

using Autofac;
using Autofac.Extensions.DependencyInjection;

using ToolsApp.Core.Interfaces.Data;
using ToolsApp.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
  containerBuilder
    .RegisterType<PrimaryColorsInMemoryData>() // concrete
    .As<IColorsData>() // contract
    .SingleInstance(); // single instance
  containerBuilder
    .RegisterType<CarsInMemoryData>() // concrete
    .As<ICarsData>() // contract
    .SingleInstance(); // single instance
});

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {

  var fileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
  var filePath = Path.Combine(AppContext.BaseDirectory, fileName);
  options.IncludeXmlComments(filePath);

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
  app.UseWebAssemblyDebugging();
}
else
{
  app.UseExceptionHandler("/Error");
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
