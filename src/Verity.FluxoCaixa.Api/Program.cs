using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Verity.FluxoCaixa.Api.Middlewares;
using Verity.FluxoCaixa.Aplicacao.Interfaces;
using Verity.FluxoCaixa.Aplicacao.Servicos;
using Verity.FluxoCaixa.Infraestrutura.Persistencia;
using Verity.FluxoCaixa.Infraestrutura.Persistencia.Repositorios;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Title = "Verity - Fluxo de Caixa API",
            Version = "v1",
            Description = "API para gerenciamento de lançamentos e consulta de saldo diário.",
        }
    );
});

var connectionString =
    builder.Configuration.GetConnectionString("FluxoCaixa") ?? "Data Source=fluxo-caixa.db";

builder.Services.AddDbContext<FluxoCaixaDbContext>(options => options.UseSqlite(connectionString));

builder.Services.AddScoped<ILancamentoRepositorio, LancamentoRepositorio>();

builder.Services.AddScoped<LancamentoService>();
builder.Services.AddScoped<SaldoDiarioConsultaService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<FluxoCaixaDbContext>();
    dbContext.Database.Migrate();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSwagger();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Verity - Fluxo de Caixa - API v1");
});

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
