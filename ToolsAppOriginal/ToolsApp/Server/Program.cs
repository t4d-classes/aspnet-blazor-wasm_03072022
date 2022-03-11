using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using System.Reflection;

using Autofac;
using Autofac.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Swashbuckle.AspNetCore.SwaggerGen;

using ToolsApp.Core.Interfaces.Data;
using ToolsApp.Data;
using ToolsApp.Server.Exceptions;

Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

try
{
  var devOrigins = "devOrigins";

  var builder = WebApplication.CreateBuilder(args);

  builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);

  builder.Host.UseSerilog();
  builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

  builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
  {
    containerBuilder
      .RegisterType<DataContext>()
      .SingleInstance();

    if (builder.Configuration["ColorsData"] == "sql")
    {
      containerBuilder
        .RegisterType<ColorsSqlServerData>() // concrete
        .As<IColorsData>() // contract
        .InstancePerLifetimeScope(); // per http request
    }
    else
    {
      containerBuilder
        .RegisterType<ColorsInMemoryData>() // concrete
        .As<IColorsData>() // contract
        .SingleInstance(); // single instance
    }

    if (builder.Configuration["CarsData"] == "sql")
    {
      containerBuilder
        .RegisterType<CarsSqlServerData>() // concrete
        .As<ICarsData>() // contract
        .InstancePerLifetimeScope(); // per http request
    }
    else
    {
      containerBuilder
        .RegisterType<CarsInMemoryData>() // concrete
        .As<ICarsData>() // contract
        .SingleInstance(); // single instance
    }

  });

  builder.Services.AddCors(options =>
  {
    options.AddPolicy(name: devOrigins,
                      builder =>
                      {
                        builder
                          .WithOrigins("https://localhost:7282")
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                      });
  });

  builder.Services.AddControllersWithViews(options => {
    options.Filters.Add<HttpResponseExceptionFilter>();
  });
  builder.Services.AddRazorPages();

  builder.Services.AddApiVersioning(config =>
  {
    config.DefaultApiVersion = new ApiVersion(1, 0);
    config.AssumeDefaultVersionWhenUnspecified = true;
    config.ReportApiVersions = true;
  });

  builder.Services.AddVersionedApiExplorer(options =>
  {
      // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
      // note: the specified format code will format the version as "'v'major[.minor][-status]"
      options.GroupNameFormat = "'v'VVV";

      // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
      // can also be used to control the format of the API version in route templates
      options.SubstituteApiVersionInUrl = true;
  });


  builder.Services.AddEndpointsApiExplorer();

  builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
  builder.Services.AddSwaggerGen(options =>
  {
      // add a custom operation filter which sets default values
      // TODO
      options.OperationFilter<SwaggerDefaultValues>();

       
      var fileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
      var filePath = Path.Combine(AppContext.BaseDirectory, fileName);
      options.IncludeXmlComments(filePath);      
  });

  var app = builder.Build();

  app.UseSerilogRequestLogging();

  // Configure the HTTP request pipeline.
  if (app.Environment.IsDevelopment())
  {
    app.UseSwagger(options => { options.RouteTemplate = "api-docs/{documentName}/docs.json"; });
    app.UseSwaggerUI(options =>
    {
      var provider = app.Services.GetService<IApiVersionDescriptionProvider>();
      if (provider is not null) {
        options.RoutePrefix = "api-docs";
        foreach (var description in provider.ApiVersionDescriptions) {
          options.SwaggerEndpoint($"/api-docs/{description.GroupName}/docs.json", description.GroupName.ToUpperInvariant());
        }
      }
    });
    app.UseWebAssemblyDebugging();
    app.UseCors(devOrigins);
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

  Log.Information("Starting web host");
  app.Run();

}
catch (Exception exc)
{
  Log.Fatal(exc, "Host terminated unexpectedly");
}
finally
{
  Log.CloseAndFlush();
}

