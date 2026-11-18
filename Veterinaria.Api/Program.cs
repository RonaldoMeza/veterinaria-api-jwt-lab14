using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Veterinaria.Datos.Context;
using Veterinaria.Datos.Repositorios;
using Veterinaria.Datos.Repositorios.Interfaces;
using Veterinaria.Negocio.Mapeos;
using Veterinaria.Negocio.Configuracion;
using Veterinaria.Negocio.Servicios;
using Veterinaria.Negocio.Servicios.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddOpenApi(options =>
    options.AddDocumentTransformer((document, _, _) =>
    {
        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes = new Dictionary<string, IOpenApiSecurityScheme>
        {
            ["Bearer"] = new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT"
            }
        };
        document.Security =
        [
            new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("Bearer", document)] = []
            }
        ];
        return Task.CompletedTask;
    }));

var connectionString = builder.Configuration.GetConnectionString("Veterinaria")
    ?? throw new InvalidOperationException(
        "No se configuró la cadena de conexión 'ConnectionStrings:Veterinaria'. Use User Secrets o una variable de entorno.");

builder.Services.AddDbContext<VeterinariaContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddScoped<IPropietarioRepositorio, PropietarioRepositorio>();
builder.Services.AddScoped<IMascotaRepositorio, MascotaRepositorio>();
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
builder.Services.AddScoped<IPropietarioServicio, PropietarioServicio>();
builder.Services.AddScoped<IMascotaServicio, MascotaServicio>();
builder.Services.AddScoped<IAuthServicio, AuthServicio>();
builder.Services.AddAutoMapper(_ => { }, typeof(MappingProfile).Assembly);

var jwt = builder.Configuration.GetSection("Jwt").Get<JwtOpciones>()
    ?? throw new InvalidOperationException("No se configuró la sección Jwt.");
if (string.IsNullOrWhiteSpace(jwt.Clave) || jwt.Clave.Length < 32)
    throw new InvalidOperationException("Jwt:Clave debe configurarse mediante User Secrets y tener al menos 32 caracteres.");

builder.Services.AddSingleton(jwt);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwt.Emisor,
            ValidateAudience = true,
            ValidAudience = jwt.Audiencia,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Clave)),
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddAuthorization();

var allowedOrigin = builder.Configuration["Cors:AllowedOrigin"] ?? "http://localhost:3000";
builder.Services.AddCors(options => options.AddPolicy("Frontend", policy => policy
    .WithOrigins(allowedOrigin)
    .AllowAnyHeader()
    .AllowAnyMethod()));

var app = builder.Build();

app.MapOpenApi();
app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "Veterinaria API v1"));

// Render termina HTTPS en su proxy y se comunica por HTTP con el contenedor.
// Localmente se mantiene la redirección HTTPS del perfil de desarrollo.
if (!app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}
app.UseCors("Frontend");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
