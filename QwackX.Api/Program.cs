using Microsoft.Data.SqlClient;
using System.Data.Common;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QwackX.Api.Domain.Repositories;
using QwackX.Api.Domain.Services;
using QwackX.Api.Infrastructure;

string policyName = "PoliceCorse";

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddCors(options => options.AddPolicy(policyName,
    (o) =>
    {
        o.AllowAnyOrigin();
        o.AllowAnyHeader();
        o.AllowAnyMethod();
    }));

builder.Services.AddControllers();


Console.WriteLine("KEYS " + configuration["JwtSettings:SecretKey"]);

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // options.Events = new JwtBearerEvents
        // {
        //     OnAuthenticationFailed = context =>
        //     {
        //         // Log détaillé de l'exception d'authentification
        //         Console.WriteLine($"❌ Authentification échouée : {context.Exception.Message}");
        //         return Task.CompletedTask;
        //     },
        //     OnTokenValidated = context =>
        //     {
        //         // Log pour voir si le token a été validé avec succès
        //         Console.WriteLine("✅ Token validé avec succès !");
        //         return Task.CompletedTask;
        //     }
        // };

        
        // options.SaveToken = true;
        // options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["Issuer"],
            ValidAudience = configuration["Audience"],
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.Default.GetBytes(configuration["JwtSettings:SecretKey"])),
        };
    });


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
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
            Array.Empty<string>()
        }
    });
});


builder.Services.AddScoped<DbConnection>(sp => new SqlConnection(configuration.GetConnectionString("Database")));
builder.Services.AddScoped<ITokenRepository, TokenService>();
builder.Services.AddScoped<IAuthRepository, AuthService>();
builder.Services.AddScoped<IUserRepository, UserService>();

builder.Logging.AddConsole();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(policyName);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    var token = context.Request.Headers["Authorization"].ToString();
    Console.WriteLine($"🔍 Token reçu: {token}");

    await next();
});

// app.Use(async (context, next) =>
// {
//     await next();
//
//     if (context.Response.StatusCode == 401)
//     {
//         context.Response.ContentType = "application/json";
//         await context.Response.WriteAsync("{\"message\": \"Token invalide ou manquant\"}");
//     }
// });

// app.Use(async (context, next) =>
// {
//     // Test: ajouter un token d'authentification pour chaque requête
//     context.Request.Headers["Authorization"] = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJJZCI6IjI5IiwiVXNlcm5hbWUiOiJzdHJpbmciLCJFbWFpbCI6InVzZXJAZXhhbXBsZS5jb20iLCJDcmVhdGVkQXQiOiIwMDAxLTAxLTAxIDAwOjAwOjAwIiwianRpIjoiODYxOWNjZGMtNDdhNC00MjFmLThkODItZTkyNjEyYzc4MzI5IiwiaXNzIjoiUXdhY2tYLkFwaSIsImF1ZCI6IlF3YWNrWC5CbGF6b3IifQ.hjBzJycOiS1V7EBPu56zvlPr3ysI5rXJrgp_QawINEE";
//     
//     await next();
// });

app.MapControllers();

app.Run();