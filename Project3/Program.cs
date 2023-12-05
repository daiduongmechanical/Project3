using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Project3.Data;
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
});

// add policy
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("admin"));
});

// add signalr
builder.Services.AddSignalR();

//add mail setting
builder.Services.AddScoped<IMailService, MailService>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddMiniProfiler(options =>
{
    // All of this is optional. You can simply call .AddMiniProfiler() for all defaults

    // (Optional) Path to use for profiler URLs, default is /mini-profiler-resources
    options.RouteBasePath = "/profiler";

    // (Optional) Control storage
    // (default is 30 minutes in MemoryCacheStorage)
    // Note: MiniProfiler will not work if a SizeLimit is set on MemoryCache!
    //   See: https://github.com/MiniProfiler/dotnet/issues/501 for details
    (options.Storage as MemoryCacheStorage).CacheDuration = TimeSpan.FromMinutes(60);

    // (Optional) Control which SQL formatter to use, InlineFormatter is the default
    options.SqlFormatter = new StackExchange.Profiling.SqlFormatters.InlineFormatter();

    // (Optional) You can disable "Connection Open()", "Connection Close()" (and async variant) tracking.
    // (defaults to true, and connection opening/closing is tracked)
    options.TrackConnectionOpenClose = true;

    // (Optional) Use something other than the "light" color scheme.
    // (defaults to "light")
    options.ColorScheme = StackExchange.Profiling.ColorScheme.Auto;

    // Optionally change the number of decimal places shown for millisecond timings.
    // (defaults to 2)
    options.PopupDecimalPlaces = 1;

    // The below are newer options, available in .NET Core 3.0 and above:

    // (Optional) You can disable MVC filter profiling
    // (defaults to true, and filters are profiled)
    options.EnableMvcFilterProfiling = true;
    // ...or only save filters that take over a certain millisecond duration (including their children)
    // (defaults to null, and all filters are profiled)
    // options.MvcFilterMinimumSaveMs = 1.0m;

    // (Optional) You can disable MVC view profiling
    // (defaults to true, and views are profiled)
    options.EnableMvcViewProfiling = true;
    // ...or only save views that take over a certain millisecond duration (including their children)
    // (defaults to null, and all views are profiled)
    // options.MvcViewMinimumSaveMs = 1.0m;

    // (Optional) listen to any errors that occur within MiniProfiler itself
    // options.OnInternalError = e => MyExceptionLogger(e);

    // (Optional - not recommended) You can enable a heavy debug mode with stacks and tooltips when using memory storage
    // It has a lot of overhead vs. normal profiling and should only be used with that in mind
    // (defaults to false, debug/heavy mode is off)
    //options.EnableDebugMode = true;
}).AddEntityFramework();

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
app.UseMiniProfiler();

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