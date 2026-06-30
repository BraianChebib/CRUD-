using CRUD.Data;
using CRUD.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    }); 

builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen();
builder.Services.AddControllersWithViews();

// Intentamos leer la conexión del archivo JSON
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Si estás en tu compu local y el JSON falla o viene vacío,
// le clavamos la conexión genérica sin contraseña a la fuerza.
if (string.IsNullOrEmpty(connectionString) || connectionString.Contains("root"))
{
    connectionString = "Server=localhost;Database=crud_db;Uid=root;Pwd=;";
}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(10, 4, 32)))
);

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ProveedorService>();
builder.Services.AddScoped<ArticuloService>();
builder.Services.AddScoped<PedidoService>();
builder.Services.AddScoped<PedidoDetalleService>();
builder.Services.AddScoped<OrdenCompraService>();
builder.Services.AddScoped<OrdenCompraDetalleService>();

builder.Services.AddAuthentication(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Acceso/Login"; 
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20); 
    });

var app = builder.Build();                 

if (app.Environment.IsDevelopment())       
{
    app.UseSwagger();     
    app.UseSwaggerUI();   
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Acceso}/{action=Login}/{id?}");

app.Run();                                 
