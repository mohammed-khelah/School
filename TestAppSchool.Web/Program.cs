using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TestAppSchool.Data;
using TestAppSchool.Data.Models;
using TestAppSchool.Infrastructure.AutoMapper;
using TestAppSchool.Infrastructure.Middlewares;
using TestAppSchool.Infrastructure.Services.Email;
using TestAppSchool.Infrastructure.Services.Students;
using TestAppSchool.Infrastructure.Services.Users;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//Add DataBase
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<TestAppSchoolDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Edit on identity User
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 8;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.SignIn.RequireConfirmedEmail = false;
}).AddEntityFrameworkStores<TestAppSchoolDbContext>().AddDefaultTokenProviders().AddDefaultUI();

builder.Services.AddRazorPages();

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(MapperProfile).Assembly);

// Add Services
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IStudentService, StudentService>();
builder.Services.AddTransient<IEmailService, EmailService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler(opts => opts.UseMiddleware<ExceptionHandler>());
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseExceptionHandler(opts => opts.UseMiddleware<ExceptionHandler>());

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

//app.SeedDb().Run();
app.Run();
