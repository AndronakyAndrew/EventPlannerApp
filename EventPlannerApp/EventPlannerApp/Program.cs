using EventPlannerApp.Data;
using EventPlannerApp.Models;
using EventPlannerApp.Repository;
using EventPlannerApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Настройка DbContext
builder.Services.AddDbContext<EventPlannerContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//Настройка Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{

    //Отключение двухфакторной аутентификации 
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;

    //Настройка пароля 
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 4;
    options.Password.RequiredUniqueChars = 1;
})
    .AddEntityFrameworkStores<EventPlannerContext>()
    .AddDefaultTokenProviders();

//Подключение сервисов
builder.Services.AddScoped<EventRepository>();
builder.Services.AddScoped<EventService>();
builder.Services.AddScoped<BudgetItemsRepository>();
builder.Services.AddScoped<BudgetItemsService>();
builder.Services.AddScoped<GuestsRepository>();
builder.Services.AddScoped<GuestsService>();
builder.Services.AddScoped<ScheduleItemsRepository>();
builder.Services.AddScoped<ScheduleItemsService>();

//Настройка конфигурации cookie
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
