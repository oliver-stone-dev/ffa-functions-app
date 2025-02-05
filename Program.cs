using ffa_functions_app;
using ffa_functions_app.Models;
using ffa_functions_app.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices((hostContext, services) =>
    {
        var environment = hostContext.HostingEnvironment;

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(Environment.GetEnvironmentVariable("SqlConnectionString")));

        services.AddIdentity<Account, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<IAirportService, AirportService>();
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<IReviewService, ReviewService>();
        services.AddTransient<ITokenProviderService,TokenProviderService>();
        services.AddTransient<ITokenAuthenticationService, TokenAuthenticationService>();

        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

    })
    .Build();

host.Run();