using HotelManagment.Data;
using HotelManagment.Models;
using HotelManagment.Services;
using FluentEmail.MailKitSmtp;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<HotelManagmentDbContext>(
    options =>
    {
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("Default"));
    });

builder.Services.AddFluentEmail(builder.Configuration.GetSection("MailSettings").GetValue<string>("FromDefault"))
    .AddRazorRenderer()
    .AddMailKitSender(new SmtpClientOptions()
    {
        Server = builder.Configuration.GetSection("MailSettings").GetValue<string>("Server"),
        Port = builder.Configuration.GetSection("MailSettings").GetValue<int>("Port"),
        User = builder.Configuration.GetSection("MailSettings").GetValue<string>("User"),
        Password = builder.Configuration.GetSection("MailSettings").GetValue<string>("Password"),
        UseSsl = builder.Configuration.GetSection("MailSettings").GetValue<bool>("UseSsl"),
        RequiresAuthentication = builder.Configuration.GetSection("MailSettings").GetValue<bool>("RequiresAuthentication")
    });
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
})
    .AddEntityFrameworkStores<HotelManagmentDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;

})
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/Login";
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromDays(1);
        options.Cookie.Name = "Application";
    });

builder.Services.AddScoped<EmailServices>();
builder.Services.AddScoped<SeriLogServices>();
builder.Services.AddScoped<UserManager<ApplicationUser>>();
builder.Services.AddScoped<SignInManager<ApplicationUser>>();
builder.Services.AddScoped<RoleManager<ApplicationRole>>();
builder.Services.AddScoped<PrenotazioniServices>();
builder.Services.AddScoped<CamereServices>();

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

app.UseStatusCodePagesWithReExecute("/Error/{0}");
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
