using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TimeSlice.WasmClient;
using TimeSlice.WebApp.Services.Base;

var builder = WebAssemblyHostBuilder.CreateDefault( args );
builder.RootComponents.Add<App>( "#app" );
builder.RootComponents.Add<HeadOutlet>( "head::after" );

builder.Services.AddHttpClient<ApiService>( client => client.BaseAddress = new( "https://localhost:5546" ) );
//builder.Services.AddScoped( sp => new HttpClient { BaseAddress = new Uri( "https://localhost:5546" ) } );

await builder.Build().RunAsync();
