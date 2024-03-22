using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;
using TimeSlice.WebApp.Components;
using TimeSlice.WebApp.Providers;
using TimeSlice.WebApp.Services.Auth;
using TimeSlice.WebApp.Services.Base;
using TimeSlice.WebApp.Services.Timebox;

var builder = WebApplication.CreateBuilder( args );

builder.AddServiceDefaults();
builder.Services.AddHttpClient<ApiService>( client => client.BaseAddress = new( "http://apiservice" ) );

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<ApiAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>( q => q.GetRequiredService<ApiAuthenticationStateProvider>() );
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddMudServices();

builder.Services.AddScoped<ITimeboxService, TimeboxService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler( "/Error", createScopeForErrors: true );
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies( typeof( TimeSlice.WebApp.Client._Imports ).Assembly );

app.Run();
