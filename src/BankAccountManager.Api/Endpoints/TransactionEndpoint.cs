using BankAccountManager.Api.Dto;
using BankAccountManager.Domain.Account.Interface;
using BankAccountManager.Domain.Account.ViewModel;
using BankAccountManager.Domain.Transaction.Interface;
using BankAccountManager.Domain.Transaction.ViewModel;
using BankAccountManager.Infrastructure.Csv;
using Microsoft.AspNetCore.Mvc;

namespace BankAccountManager.Api.Endpoints;

public static class TransactionEndpoint
{
    public static IEndpointRouteBuilder AddTransactionEndpoints(this IEndpointRouteBuilder app, string endpointPrefix)
    {


        app.MapGet(endpointPrefix + "/transaction", async (
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

        app.MapPost(endpointPrefix + "/transaction", async (
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

        app.MapPost(endpointPrefix + "/transaction/{transactionId}/type/{transactionTypeId}", async (
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

        app.MapDelete(endpointPrefix + "/transaction", async (
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


        app.MapPost(endpointPrefix + "/transaction/parse-file", async (
            [FromServices] ITransactionService transactionService, [FromServices] IAccountService accountService,
            [FromForm] TransactionFileUploadDto transactionFileUploadDto) =>
        {
            if (transactionFileUploadDto == null)
                return Results.BadRequest();

            if (transactionFileUploadDto.FileUpload == null)
                return Results.BadRequest();

            var accountId = transactionFileUploadDto?.AccountId;
            if (accountId == null)
                return Results.BadRequest();

            var account = await accountService.GetById(accountId.Value);
            if (account == null)
                return Results.NotFound();

            await using var stream = transactionFileUploadDto.FileUpload.OpenReadStream();
            var responseList = await transactionService.ParseCsvFile(account, stream);

            return Results.Ok(responseList);
        })
        .WithName("UploadFileParse")
        .DisableAntiforgery()
        .WithOpenApi();


        app.MapPost(endpointPrefix + "/transaction/upload-file", async (
            [FromServices] ITransactionService transactionService, [FromServices] IAccountService accountService,
            [FromForm] TransactionFileUploadDto transactionFileUploadDto) =>
        {
            if (transactionFileUploadDto == null)
                return Results.BadRequest();

            if (transactionFileUploadDto.FileUpload == null)
                return Results.BadRequest();

            var accountId = transactionFileUploadDto?.AccountId;
            if (accountId == null)
                return Results.BadRequest();

            var account = await accountService.GetById(accountId.Value);
            if (account == null)
                return Results.NotFound();

            await using var stream = transactionFileUploadDto.FileUpload.OpenReadStream();
            var responseList = await transactionService.ProcessCsvFile(account, stream);

            return Results.Ok(responseList);
        })
        .WithName("UploadFileProcess")
        .DisableAntiforgery()
        .WithOpenApi();



        app.MapPost(endpointPrefix + "/transaction/reprocess-undefined-types", async (
            [FromServices] ITransactionService transactionService, [FromServices] IAccountService accountService,
            [FromBody] TransactionReprocesUndefinedTypesDto transactionReprocesUndefinedTypesDto) =>
        {
            if (transactionReprocesUndefinedTypesDto?.AccountId == null || transactionReprocesUndefinedTypesDto?.startTransactionDate == null)
                return Results.BadRequest();

            var account = await accountService.GetById(transactionReprocesUndefinedTypesDto.AccountId);
            if (account == null)
                return Results.NotFound();

            var result = await transactionService.ReprocessUndefinedTypes(account, transactionReprocesUndefinedTypesDto.startTransactionDate);
            if (!result)
                return Results.BadRequest();

            return Results.Ok();
        })
        .WithName("ReprocessUndefinedTypes")
        .WithOpenApi();

        return app;
    }
}
