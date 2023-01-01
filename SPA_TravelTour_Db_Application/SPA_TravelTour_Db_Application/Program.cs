using Microsoft.EntityFrameworkCore;
using SPA_TravelTour_Db_Application.HostedService;
using SPA_TravelTour_Db_Application.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TravelTourDbContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("db")));
builder.Services.AddHostedService<DbSeederHostedService>();
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
var app = builder.Build();

app.UseStaticFiles();
app.MapDefaultControllerRoute();


app.Run();
