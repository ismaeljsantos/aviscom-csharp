using Aviscom.Data;
using Aviscom.Services;
using Aviscom.Services.Interfaces;
using Aviscom.Utils.JsonConverters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.Converters.Add(new UlidJsonConverter());
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

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Autorization",
        Description = "Introduza o seu token no campo abaixo.\n\nExemplo: \"12345abcdef...\"",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Conexão Banco de Dados
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AviscomContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddSingleton<IEncryptionService, EncryptionService>();
builder.Services.AddScoped<IEnderecoService, EnderecaoService>();
builder.Services.AddScoped<IContatoService, ContatoService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IFuncaoService, FuncaoService>();
builder.Services.AddScoped<ISetorService, SetorService>();
builder.Services.AddScoped<IUsuarioFuncaoService, UsuarioFuncaoService>();
builder.Services.AddScoped<IEmpresaService, EmpresaService>();
builder.Services.AddScoped<IInstituicaoService, InstituicaoService>();
builder.Services.AddScoped<IEscolaridadeService, EscolaridadeService>();

var key = Encoding.ASCII.GetBytes(builder.Configuration["JwtSettings:SecretKey"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // Em produção, mude para true
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        ValidateLifetime = true, // Verifica se o token não expirou
        ClockSkew = TimeSpan.Zero // Remove margem de tempo na expiração
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Administrador", policy =>
    policy.RequireRole("Administrador"));

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
