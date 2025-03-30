using BankAccountManager.Domain.Account.Interface;
using BankAccountManager.Domain.Account.Model;
using BankAccountManager.Domain.Account.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Update;

namespace BankAccountManager.Api.Endpoints;

public static class AccountEndpoint
{
    public static IEndpointRouteBuilder AddAccountEndpoints(this IEndpointRouteBuilder app, string endpointPrefix)
    {

        app.MapGet(endpointPrefix + "/account", async ([FromServices] IAccountService accountService,
            [FromQuery] int? accountId) =>
        {
            if (accountId != null)
            {
                var accountItem = await accountService.GetById(accountId.Value);

                if (accountItem == null)
                    return Results.NotFound();
                else
                    return Results.Ok(new List<AccountModel>() { accountItem });
            }
            else
            {
                var accountList = await accountService.GetActiveAccounts();

                if (!accountList.Any())
                    return Results.NotFound();

                return Results.Ok(accountList);
            }
        })
        .WithName("GetAccountList")
        .WithOpenApi();

        app.MapPost(endpointPrefix + "/account", async ([FromServices] IAccountService accountService, [FromBody] CreateAccountViewModel createAccountViewModel) =>
        {

            var account = await accountService.CreateAccount(createAccountViewModel);

            return Results.Ok(account);
        })
        .WithName("PostAccount")
        .WithOpenApi();

        return app;
    }
}
