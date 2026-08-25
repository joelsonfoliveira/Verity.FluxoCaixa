using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Verity - Fluxo de Caixa API",
        Version = "v1",
        Description = "API para gerenciamento de lançamentos e consulta de saldo diário."
    });
});

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Verity - Fluxo de Caixa - API v1");
});

app.UseHttpsRedirection();

app.MapControllers();

app.Run();