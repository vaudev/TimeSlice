var builder = DistributedApplication.CreateBuilder( args );

var cache = builder.AddRedis( "cache" );

var apiService = builder.AddProject<Projects.TimeSlice_ApiService>( "apiservice" );

//builder.AddProject<Projects.TimeSlice_Web>( "webfrontend" )
//    .WithReference( cache )
//    .WithReference( apiService );


builder.AddProject<Projects.TimeSlice_WebApp>( "webapp" )
    .WithReference( cache )
    .WithReference( apiService );

builder.AddProject<Projects.TimeSlice_WebApp_Client>( "webapp-client" )
    .WithReference( apiService );

builder.Build().Run();
