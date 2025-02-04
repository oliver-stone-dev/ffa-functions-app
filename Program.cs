using ffa_functions_app;
using ffa_functions_app.Models;
using ffa_functions_app.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Configuration;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices((hostContext, services) =>
    {
        var environment = hostContext.HostingEnvironment;

        if (environment.IsDevelopment())
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost")
                        .AllowAnyHeader()
                        .AllowAnyMethod();

                    policy.SetIsOriginAllowed(origin => new Uri(origin).Host == "filmfriendlyairports.com")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
        }
        else
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.SetIsOriginAllowed(origin => new Uri(origin).Host == "filmfriendlyairports.com")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });
        }

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(Environment.GetEnvironmentVariable("SqlConnectionString")));

        services.AddIdentity<Account, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<IAirportService, AirportService>();
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<IReviewService, ReviewService>();
        services.AddTransient<ITokenProviderService,TokenProviderService>();

        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

    })
    .Build();

host.Run();