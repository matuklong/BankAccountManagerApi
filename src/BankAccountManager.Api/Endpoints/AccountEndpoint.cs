using BankAccountManager.Domain.Account.Interface;
using BankAccountManager.Domain.Account.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace BankAccountManager.Api.Endpoints;

public static class AccountEndpoint
{
    public static IEndpointRouteBuilder AddAccountEndpoints(this IEndpointRouteBuilder app)
    {

        app.MapGet("/account", async ([FromServices] IAccountService accountService) =>
        {

            var accountList = await accountService.GetActiveAccounts();

            if (!accountList.Any())
                return Results.NotFound();

            return Results.Ok(accountList);
        })
        .WithName("GetAccount")
        .WithOpenApi();

        app.MapPost("/account", async ([FromServices] IAccountService accountService, [FromBody] CreateAccountViewModel createAccountViewModel) =>
        {

            var account = await accountService.CreateAccount(createAccountViewModel);

            return Results.Ok(account);
        })
        .WithName("PostAccount")
        .WithOpenApi();

        return app;
    }
}
