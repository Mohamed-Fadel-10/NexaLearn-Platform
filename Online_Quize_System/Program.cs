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
using Services.Unit_Of_Work;
using Online_Quize_System.Seed_Data;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<QuizContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("defualtConnection"));
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => {
    options.Password.RequireDigit = true; 
    options.Password.RequireNonAlphanumeric = true; 
    options.Password.RequiredLength = 8;
    options.SignIn.RequireConfirmedEmail = true; 
})
.AddEntityFrameworkStores<QuizContext>() 
.AddDefaultTokenProviders();


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
builder.Services.AddScoped<ITextSimilarityService, TextSimilarityService>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IQuestionsService, QuestionsService>();

builder.Services.Configure<EmailSetting>(builder.Configuration.GetSection("EmailConfiguration"));


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
// Call Seeding Method 

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        SeedData.SeedRolesAndAdminAsync(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding roles and users.");
    }
}

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
