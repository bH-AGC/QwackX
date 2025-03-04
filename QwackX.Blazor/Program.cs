using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using QwackX.Blazor;
using QwackX.Blazor.Domain.Repositories;
using QwackX.Blazor.Domain.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("Default", client =>
{
    client.BaseAddress = new Uri("https://localhost:7295/");
});

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped<IAuthRepository, AuthService>();
builder.Services.AddScoped<IUserRepository, UserService>();
builder.Services.AddScoped<IPostRepository, PostService>();
builder.Services.AddScoped<IReplyRepository, ReplyService>();
builder.Services.AddScoped<ILikeRepository, LikeService>();
builder.Services.AddScoped<IPostViewRepository, PostViewService>();

await builder.Build().RunAsync();