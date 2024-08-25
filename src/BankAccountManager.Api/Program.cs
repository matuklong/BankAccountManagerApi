using BankAccountManager.Domain.Account.Interface;
using BankAccountManager.Domain.Account.ViewModel;
using BankAccountManager.Domain.Transaction.Interface;
using BankAccountManager.Domain.Transaction.ViewModel;
using BankAccountManager.Infrastructure.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using System.Security.Principal;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var configuration = builder.Configuration;
builder.Services.AddApiDependecyInjection(configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


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

app.Run();

