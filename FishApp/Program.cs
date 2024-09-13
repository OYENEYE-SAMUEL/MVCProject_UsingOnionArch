using Application.Configuration;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Services;
using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//builder.Services.AddDbContext<FishContext>(config => config.UseMySQL(builder.Configuration.GetConnectionString("FishString")));
builder.Services.AddDbContext<FishContext>(config => config.UseMySQL("server = localhost; user = root; database = fishcontextdb; password = Pa$$word"));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(cofig =>
{
    cofig.LoginPath = "/User/Login";
    cofig.LogoutPath = "/User/Logout";
    cofig.Cookie.Name = "FishApp";
    cofig.ExpireTimeSpan = TimeSpan.FromDays(7);
    cofig.AccessDeniedPath = "/User/Login";
});

builder.Services.Configure<FileConfiguration>(builder.Configuration.GetSection("FileStorage"));

builder.Services.AddScoped<IPondRepository, PondRepository>();
builder.Services.AddScoped<IStaffRepository, StaffRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IFishRepository, FishRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUserRespository, UserRepository>();
builder.Services.AddScoped<ICurrentUser, CurrentUser>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IFishPondRepository, FishPondRepository>();
builder.Services.AddScoped<IOrderFishRepository, OrderFishRespository>();
builder.Services.AddScoped<IFileUploadRepository, FileUploadRepository>();

builder.Services.AddScoped<IPondService, PondService>();
builder.Services.AddScoped<IStaffService, StaffService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IFishService, FishService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
