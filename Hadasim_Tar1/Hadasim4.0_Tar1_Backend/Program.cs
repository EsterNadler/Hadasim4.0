using BL;
using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//****
var connectionString = builder.Configuration.GetConnectionString("HmoConnectionString");
builder.Services.AddDbContext<HmoContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped<PatinetBL>();
builder.Services.AddScoped<IllnessBL>();
builder.Services.AddScoped<VaccinBL>();
builder.Services.AddScoped<VaccinsManufacturerBL>();
//****




var app = builder.Build();

//***********************

app.UseStaticFiles();

//***********************

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthorization();
app.MapControllers();



app.Run();
