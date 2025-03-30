using BankAccountManager.Domain.Account.Interface;
using BankAccountManager.Domain.Account.ViewModel;
using BankAccountManager.Domain.Transaction.Interface;
using BankAccountManager.Domain.Transaction.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace BankAccountManager.Api.Endpoints;

public static class TransactionTypeEndpoint
{
    public static IEndpointRouteBuilder AddTransactionTypeEndpoints(this IEndpointRouteBuilder app, string endpointPrefix)
    {

        app.MapGet(endpointPrefix + "/transaction-type", async (
            [FromServices] ITransactionTypeService transactionTypeService,
            [FromQuery] int? transactionTypeId) =>
        {
            var transactionTypeList = await transactionTypeService.GetAll();

            return Results.Ok(transactionTypeList);
        })
        .WithName("GetTransactionType")
        .WithOpenApi();

        app.MapPost(endpointPrefix + "/transaction-type", async (
            [FromServices] ITransactionTypeService transactionTypeService,
            [FromBody] CreateTransactionTypeRequestViewModel createTransactionTypeRequestViewModel) =>
        {
            if (string.IsNullOrWhiteSpace(createTransactionTypeRequestViewModel?.TransactionTypeString))
                return Results.BadRequest();

            var transactionSuccess = await transactionTypeService.AddType(createTransactionTypeRequestViewModel.TransactionTypeString);

            return Results.Ok(new { transactionSuccess = transactionSuccess });
        })
        .WithName("PostTransactionType")
        .WithOpenApi();

        app.MapPut(endpointPrefix + "/transaction-type/{transactionTypeId}", async (
            [FromServices] ITransactionTypeService transactionTypeService,
            [FromBody] CreateTransactionTypeRequestViewModel createTransactionTypeRequestViewModel,
            [FromRoute] int? transactionTypeId) =>
        {
            if (string.IsNullOrWhiteSpace(createTransactionTypeRequestViewModel?.TransactionTypeString))
                return Results.BadRequest();

            if (transactionTypeId == 0 || transactionTypeId == null)
                return Results.BadRequest();

            var transactionSuccess = await transactionTypeService.EditType(transactionTypeId.Value, createTransactionTypeRequestViewModel.TransactionTypeString);

            if (!transactionSuccess)
                return Results.NotFound();

            return Results.Ok(new { transactionSuccess = transactionSuccess });
        })
        .WithName("PutTransactionType")
        .WithOpenApi();


        return app;
    }
}
