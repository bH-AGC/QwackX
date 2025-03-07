using Microsoft.Data.SqlClient;
using System.Data.Common;
using System.Resources;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QwackX.Api.Domain.Repositories;
using QwackX.Api.Domain.Services;
using QwackX.Api.Infrastructure;
using QwackX.Api.Properties;
using QwackX.Api.Services;
using ToolsSecurity;

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

builder.Services.AddSingleton<IRsaService>(sp => new RsaService(Resources.keys));

builder.Services.AddSingleton<SecurityInfo>(sp =>
{
    var rsaService = sp.GetRequiredService<IRsaService>();
    var securityInfoService = SecurityInfoService.Create(rsaService);
    return securityInfoService.SecurityInfo;
});

SecurityInfo securityInfo = builder.Services.BuildServiceProvider().GetRequiredService<SecurityInfo>();

builder.Services.AddScoped<DbConnection>(sp => new SqlConnection(string.Format(configuration.GetConnectionString("Database")!, securityInfo.Login, securityInfo.Passwd)));

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["Issuer"],
            ValidAudience = configuration["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(securityInfo.SecretKey)),
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


builder.Services.AddScoped<ITokenRepository, TokenService>();
builder.Services.AddScoped<IAuthRepository, AuthService>();
builder.Services.AddScoped<IUserRepository, UserService>();
builder.Services.AddScoped<IPostRepository, PostService>();
builder.Services.AddScoped<IReplyRepository, ReplyService>();
builder.Services.AddScoped<ILikeRepository, LikeService>();

builder.Services.AddScoped<IPostViewRepository, PostViewService>();
builder.Services.AddSingleton<PostViewCache>();

// Enregistrer PostViewSyncService comme Singleton standard
builder.Services.AddSingleton<PostViewSyncService>();

// Enregistrer PostViewSyncService comme HostedService (pour qu'il démarre avec l'application)
builder.Services.AddHostedService(provider => provider.GetRequiredService<PostViewSyncService>());


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

app.MapControllers();

app.Run();