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

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        "server=localhost;database=crud_db;user=root;password=",
        new MySqlServerVersion(new Version(10, 4, 32))
    )
);

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ProveedorService>();
builder.Services.AddScoped<ArticuloService>();
builder.Services.AddScoped<PedidoService>();
builder.Services.AddScoped<PedidoDetalleService>();
builder.Services.AddScoped<OrdenCompraService>();

var app = builder.Build();                 

if (app.Environment.IsDevelopment())       
{
    app.UseSwagger();     
    app.UseSwaggerUI();   
}

app.UseHttpsRedirection();                 

app.MapControllers();                      

app.Run();                                 
