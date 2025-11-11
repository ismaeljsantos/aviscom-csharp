using Aviscom.Data;
using Aviscom.Services;
using Aviscom.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    }
);
builder.Services.AddEndpointsApiExplorer();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Ensina o Swagger a tratar Ulid como uma string com formato "ulid"
    options.MapType<NUlid.Ulid>(() => new OpenApiSchema
    {
        Type = "string",
        Format = "ulid",
        Example = new Microsoft.OpenApi.Any.OpenApiString(NUlid.Ulid.NewUlid().ToString())
    });

    // Faz o mesmo para o Ulid anulável (se você for usar)
    options.MapType<NUlid.Ulid?>(() => new OpenApiSchema
    {
        Type = "string",
        Format = "ulid",
        Nullable = true,
        Example = new Microsoft.OpenApi.Any.OpenApiString(NUlid.Ulid.NewUlid().ToString())
    });
});

// Conexão Banco de Dados
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AviscomContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddSingleton<IEncryptionService, EncryptionService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
