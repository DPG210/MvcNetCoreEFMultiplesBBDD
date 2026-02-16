using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MvcNetCoreEFMultiplesBBDD.Data;
using MvcNetCoreEFMultiplesBBDD.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//string conecctionString = builder.Configuration.GetConnectionString("SqlHospital");
//string conecctionString = builder.Configuration.GetConnectionString("OracleHospital");
string conecctionString = builder.Configuration.GetConnectionString("MySQLHospital");
//builder.Services.AddDbContext<EmpleadosContext>
////    (options => options.UseSqlServer(conecctionString));
//builder.Services.AddDbContext<EmpleadosContext>
//    (options => options.UseOracle(conecctionString));
builder.Services.AddTransient<IRepositoryEmpleados, RepositoryEmpleadosMySQL>();
builder.Services.AddDbContext<EmpleadosContext>
    (options => options.UseMySQL(conecctionString));
//builder.Services.AddTransient<IRepositoryEmpleados, RepositoryEmpleadosMySQL>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
