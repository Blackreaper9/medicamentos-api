using Microsoft.EntityFrameworkCore;
using MedicamentosAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// 🔥 CONFIGURAR CORS - PERMITIR TODOS LOS ORÍGENES
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()      // Permite cualquier origen (localhost, firebase, etc.)
              .AllowAnyMethod()      // Permite GET, POST, PUT, DELETE, etc.
              .AllowAnyHeader();     // Permite cualquier header
    });
});

// Configurar base de datos
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 🔥 USAR CORS - DEBE IR ANTES DE UseAuthorization
app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();