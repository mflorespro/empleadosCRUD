using EmpleadosWeb.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


// Inyecta la cadena de conexión desde appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Registra DbHelper como singleton
builder.Services.AddSingleton<DbHelper>(provider =>
    new DbHelper(connectionString));

// Registra EmpleadoData como servicio transitorio
builder.Services.AddTransient<EmpleadoData>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    return new EmpleadoData(connectionString);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();


// Ruta por defecto
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Empleados}/{action=Index}/{id?}");

app.Run();
