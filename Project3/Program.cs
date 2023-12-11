using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Project3.Data;
using Project3.Filter;
using Project3.Hubs;
using Project3.Mail;
using StackExchange.Profiling.Storage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<MyDbContext>(o =>
{
    o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

// add authozied use cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = "/login";
    options.AccessDeniedPath = "/unauthozied";
    options.ExpireTimeSpan = TimeSpan.FromDays(90);
});

// add athentication to action filter
builder.Services.AddMvc(options =>
{
    options.Filters.Add(new AuthorizeFilter());
    options.Filters.Add(typeof(ServiceActionFilter));
});

// add policy
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ManagerOnly", policy => policy.RequireRole("manager"));

    options.AddPolicy("AdminOrManager", policy =>
    {
        policy.RequireRole("admin", "manager");
    });
    options.AddPolicy("AdminOrWriterOrmanager", policy =>
    {
        policy.RequireRole("admin", "writer", "manager");
    });
});

// add signalr
builder.Services.AddSignalR();

//add mail setting
builder.Services.AddScoped<IMailService, MailService>();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapHub<ChatHub>("/chathub");
});

app.Run();