using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder( args );


// Using a persistent volume mount requires a stable password rather than the default generated one.
var sqlpw = builder.Configuration["sqlpassword"];
if (builder.Environment.IsDevelopment() && string.IsNullOrEmpty( sqlpw ))
{
    throw new InvalidOperationException( """
        A password for the local SQL Server container is not configured.
        Add one to the AppHost project's user secrets with the key 'sqlpassword', e.g. dotnet user-secrets set sqlpassword <password>
        """ );
}

// To have a persistent volume mount across container instances, it must be named (VolumeMountType.Named).
var sqlDatabase = builder.AddSqlServerContainer( "sql", sqlpw, 1400 )
    .WithVolumeMount( "VolumeMount.sql.data", "/var/opt/mssql", VolumeMountType.Named )
    .AddDatabase( "timeSliceSql" );


var cache = builder.AddRedis( "cache" );

var apiService = builder.AddProject<Projects.TimeSlice_ApiService>( "apiservice" )
    .WithReference( sqlDatabase );

builder.AddProject<Projects.TimeSlice_WebApp>( "webapp" )
    .WithReference( cache )
    .WithReference( apiService );

builder.AddProject<Projects.TimeSlice_WebApp_Client>( "webapp-client" )
    .WithReference( apiService );

builder.Build().Run();
