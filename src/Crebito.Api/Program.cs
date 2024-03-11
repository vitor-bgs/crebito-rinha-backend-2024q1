using Crebito.Domain.Queries;
using Crebito.Domain.Services;
using Crebito.Domain.Services.Dtos;
using Crebito.Common.ErrorNotifications;

using Crebito.Api.EndpointFilters;
using Crebito.Api.DependencyInjection;

using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using Crebito.Api;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.SnakeCaseLower;
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddScoped<ErrorNotificationService>();


// Dapper
builder.Services.AddDapperRepository(builder.Configuration);


builder.Services.AddScoped<ProcessarTransacaoService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapPost(
    "/clientes/{id}/transacoes",
    async (
        int id,
        [FromBody] ProcessarTransacaoRequest? request,
        ProcessarTransacaoService service) =>
    {
        ProcessarTransacaoResponse? result = null;

        var resilience = CrebitoResilienceFactory.Create();
        await resilience.ExecuteAsync(async (cancellationToken) =>
        {
            try
            {
                result = await service.ProcessarTransacao(id, request);
            }
            catch (NpgsqlException ex)
            {
                throw;
            }
        });

        return result;
    })
    .AddEndpointFilter<ErrorNotificationFilter>();

app.MapGet(
    "/clientes/{id}/extrato",
    async (int id, IObterExtratoQueryService service) =>
    {
        ObterExtratoResponse? result = null;

        var resilience = CrebitoResilienceFactory.Create();
        await resilience.ExecuteAsync(async (cancellationToken) =>
        {
            result = await service.ObterExtrato(id);
        });

        return result;
    })
    .AddEndpointFilter<ErrorNotificationFilter>();

app.Run();