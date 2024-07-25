using Bike.Api.ControleErros;
using BikeApi.Aplicacao.AluguelServico;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc(options =>
{
	options.Filters.Add(typeof(ExceptionFilter));
});

builder.Services.AddControllers(options =>
{
	options.Filters.Add(new ProducesAttribute("application/json"));
	options.Filters.Add(new ConsumesAttribute("application/json"));
});

builder.Services.AddEndpointsApiExplorer();

// Configure Swagger
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "Bike Aluguel API", Version = "v1" });

	var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");

	c.IncludeXmlComments(xmlPath);

	c.CustomSchemaIds(x => x.FullName);
});

builder.Services.AddScoped<IAluguelServico, AluguelServico>();
builder.Services.AddScoped<IIntegracaoExternoServico, IntegracaoExternoServico>();
builder.Services.AddScoped<IIntegracaoEquipamentoServico, IntegracaoEquipamentoServico>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Bike Aluguel API v1"));

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

/// <summary>
/// apenas uma "orelha" para poder colocar atribudo de excluir da cobertura
/// </summary>
[ExcludeFromCodeCoverage]
public partial class Program { }