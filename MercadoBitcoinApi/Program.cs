using MercadoBitcoinApi.Services;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using MercadoBitcoinApi.Services.Interfaces;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Mercado Bitcoin API Integration",
        Version = "v1",
        Description = "API para integração com a API do Mercado Bitcoin - Consulta de contas e posições de ativos"
    });

    // Incluir comentários XML
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

// Configuração das credenciais da API do Mercado Bitcoin
var tapiId = builder.Configuration["MercadoBitcoin:TapiId"] 
    ?? Environment.GetEnvironmentVariable("TAPI_ID") 
    ?? throw new InvalidOperationException("TAPI_ID não configurado. Configure via appsettings.json ou variável de ambiente.");

var tapiSecret = builder.Configuration["MercadoBitcoin:TapiSecret"] 
    ?? Environment.GetEnvironmentVariable("TAPI_SECRET") 
    ?? throw new InvalidOperationException("TAPI_SECRET não configurado. Configure via appsettings.json ou variável de ambiente.");

var http = new HttpClient
{
    BaseAddress = new Uri("https://api.mercadobitcoin.net")
};

builder.Services.AddHttpClient<IMercadoBitcoinService, MercadoBitcoinService>(client =>
{
    client.BaseAddress = new Uri("https://api.mercadobitcoin.net");
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
});
builder.Services.AddSingleton<IAuthenticationService>(sp => new AuthenticationService(http, tapiId, tapiSecret));

var app = builder.Build();

// Configure the HTTP request pipeline.
// IMPORTANTE: CORS deve ser configurado antes de outros middlewares
app.UseCors();

// Servir arquivos estáticos (wwwroot)
app.UseStaticFiles();

// Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mercado Bitcoin API Integration v1");
    c.RoutePrefix = "swagger"; // Swagger UI em /swagger
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
