using BankAccountManager.Domain.Account.Interface;
using BankAccountManager.Domain.Account.ViewModel;
using BankAccountManager.Domain.Transaction.Interface;
using BankAccountManager.Domain.Transaction.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace BankAccountManager.Api.Endpoints;

public static class TransactionEndpoint
{
    public static IEndpointRouteBuilder AddTransactionEndpoints(this IEndpointRouteBuilder app)
    {


        app.MapGet("/transaction", async (
            [FromServices] ITransactionService transactionService, [FromServices] IAccountService accountService,
            [FromQuery] int accountId, [FromQuery] DateTime startTransactionDate) =>
        {
            var account = await accountService.GetById(accountId);
            if (account == null)
                return Results.NotFound();

            var transactionList = await transactionService.GetTransactions(account, startTransactionDate);

            return Results.Ok(transactionList);
        })
        .WithName("GetTransaction")
        .WithOpenApi();

        app.MapPost("/transaction", async (
            [FromServices] ITransactionService transactionService, [FromServices] IAccountService accountService,
            [FromBody] List<CreateTransactionRequestViewModel> createTransactionRequestViewModel) =>
        {
            var accountId = createTransactionRequestViewModel.FirstOrDefault()?.AccountId;
            if (accountId == null)
                return Results.BadRequest();

            var account = await accountService.GetById(accountId.Value);
            if (account == null)
                return Results.NotFound();

            var transactionList = await transactionService.CreateTransaction(account, createTransactionRequestViewModel);

            return Results.Ok(transactionList);
        })
        .WithName("PostTransaction")
        .WithOpenApi();

        app.MapPost("/transaction/{transactionId}/type/{transactionTypeId}", async (
            [FromServices] ITransactionService transactionService, [FromServices] IAccountService accountService,
            int transactionId, int transactionTypeId) =>
        {
            var transaction = await transactionService.UpdateTransactionType(transactionId, transactionTypeId);
            if (transaction == null)
                return Results.BadRequest();

            return Results.Ok(transaction);
        })
        .WithName("UpdateTransactionType")
        .WithOpenApi();

        app.MapDelete("/transaction", async (
            [FromServices] ITransactionService transactionService, [FromServices] IAccountService accountService,
            [FromQuery] int accountId, [FromQuery] int transactionId) =>
        {
            var account = await accountService.GetById(accountId);
            if (account == null)
                return Results.NotFound();

            await transactionService.DeleteTransaction(account, transactionId);

            return Results.Accepted();
        })
        .WithName("DeleteTransaction")
        .WithOpenApi();


        return app;
    }
}
