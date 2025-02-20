using Microsoft.Data.SqlClient;
using System.Data.Common;
using QwackX.Api.Domain.Repositories;
using QwackX.Api.Domain.Services;

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
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<DbConnection>(sp => new SqlConnection(configuration.GetConnectionString("Database")));
builder.Services.AddScoped<IUserRepository, UserServices>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(policyName);
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();