using Microsoft.Extensions.DependencyInjection;
using MiWebService.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

string connectionString = builder.Configuration.GetConnectionString("ConexionServidor");
builder.Services.AddScoped<EmpresasDatos>(_ => new EmpresasDatos(connectionString));
builder.Services.AddScoped<PersonaDatos>(_ => new PersonaDatos(connectionString));
builder.Services.AddSingleton<MemoriaPersonas>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.SetIsOriginAllowed(origin =>
        origin.StartsWith("http://localhost") ||
            origin.StartsWith("http://127.0.0.1") ||
            origin.StartsWith("http://192.168.15.135") ||
            origin.StartsWith("file://")
        )
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthorization();
app.MapControllers();

app.Run();