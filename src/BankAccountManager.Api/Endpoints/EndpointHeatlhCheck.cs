using BankAccountManager.Domain.Account.Interface;
using BankAccountManager.Domain.Account.Model;
using BankAccountManager.Domain.Account.ViewModel;
using Microsoft.AspNetCore.Mvc;
using BankAccountManager.Infrastructure.Database;

namespace BankAccountManager.Api.Endpoints;

public static class HealthCheckEndpoint
{
    public static IEndpointRouteBuilder AddHealthCheckEndpoints(this IEndpointRouteBuilder app)
    {

        app.MapGet("/healthcheck", async ([FromServices] HealthCheckRepository healthCheckRepository, CancellationToken cancellationToken) =>
        {
            // wait 2 seconds for the healthcheck to complete
            var healthCheckResult = await healthCheckRepository.CheckConnectivity(2, cancellationToken);
            var result = new 
            { 
                dateTime = DateTime.Now,
                databaseConnection = healthCheckResult 
            };

            if (healthCheckResult)
                return Results.Ok(result);
            else
                return Results.InternalServerError(result);
        })
        .WithName("healthcheck")
        .WithOpenApi();

        return app;
    }
}
