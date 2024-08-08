using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using Services.Services;
using Entities.Models;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Infrastructure.Hubs;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<QuizContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("defualtConnection"));
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options=> {

    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
    options.SignIn.RequireConfirmedEmail = true;
}).AddEntityFrameworkStores<QuizContext>().AddDefaultTokenProviders();

// Injection for Services
builder.Services.AddSignalR();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IQuizService,QuizService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ISubjectService, SubjectService>();
builder.Services.AddScoped<ISectionService, SectionService>();
builder.Services.AddScoped<IMaterialsService, MaterialsService>();
builder.Services.AddScoped<IChatService, ChatService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
        .AddCookie()
        .AddGoogle(options =>
        {
            options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
            options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
        });

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

app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

app.MapHub<QuizHub>("/QuizHub");
app.MapHub<ChatHub>("/ChatHub");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
