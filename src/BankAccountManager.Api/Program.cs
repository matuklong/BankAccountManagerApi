using BankAccountManager.Api.Endpoints;
using BankAccountManager.Domain.Account.Interface;
using BankAccountManager.Domain.Account.ViewModel;
using BankAccountManager.Domain.Transaction.Interface;
using BankAccountManager.Domain.Transaction.ViewModel;
using BankAccountManager.Infrastructure.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using System.Security.Principal;

var builder = WebApplication.CreateBuilder(args);

// Docker run -e: Load configuration from env with Prefix BankAccountManager_.
// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-9.0#naming-of-environment-variables
builder.Configuration.AddEnvironmentVariables(prefix: "BankAccountManager_");

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var configuration = builder.Configuration;
builder.Services.AddApiDependecyInjection(configuration);

var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
var MyAllowSpecificOrigins = "Frontend";
if (allowedOrigins != null && allowedOrigins.Length > 0)
{
    Console.WriteLine($"Adding Cors {MyAllowSpecificOrigins}: {string.Join(';',allowedOrigins)}");

    builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: MyAllowSpecificOrigins,
                          builder =>
                          {
                              builder
                              .WithOrigins(allowedOrigins)
                              .AllowAnyHeader()
                              .AllowAnyMethod()
                              .AllowCredentials();
                          });
    });
}


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();


    app.UseHttpsRedirection();
}

if (allowedOrigins != null && allowedOrigins.Length > 0)
    app.UseCors(MyAllowSpecificOrigins);

// Aplication Endpoints
app.AddAccountEndpoints();
app.AddHealthCheckEndpoints();
app.AddTransactionEndpoints();
app.AddTransactionTypeEndpoints();

Console.WriteLine("Starting Bank Account Manager API with configuration: " + builder.Environment.EnvironmentName);

app.Run();

