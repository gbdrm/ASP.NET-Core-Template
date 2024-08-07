using AspNetMvcTemplate.Data;
using AspNetMvcTemplate.Data.Models;
using AspNetMvcTemplate.Data.Seed;
using AspNetMvcTemplate.Temp;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var hostingProvider = Environment.GetEnvironmentVariable("HOSTING_PROVIDER");

bool isRender = hostingProvider?.ToLower() == "render.com";
#if DEBUG
isRender = true;
#endif

var connectionString = builder.Configuration.GetConnectionString("PostgresConnection");

// fallback to DefaultConnection
if (connectionString == null)
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// change to something simpler and it should be the same DB for local and prod
var fake = true;
if (fake)
{
    builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));
}
else
{
    builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));
}

builder.Services.AddIdentity<AppUser, IdentityRole<long>>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole<long>>()
    .AddUserManager<UserManager<AppUser>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddHealthChecks();
builder.Services.AddCoreAdmin("Admin");
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddTransient<IEmailSender, InMemoryEmailSender>(); // Replace with your implementation

var app = builder.Build();
app.Logger.LogInformation("Application starting up");

if(!fake)
using (var scope = app.Services.CreateScope())
{
    app.Logger.LogInformation("Seeding initial data.");
    var services = scope.ServiceProvider;
    var seeder = new DefaultDataSeeder(services);
    await seeder.SeedAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}

if (isRender)
{
    app.Logger.LogInformation("Deployed on render.com, no need in https");
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

    app.UseHttpsRedirection();
}

// TODO: move later to dev env
app.UseDeveloperExceptionPage();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.MapHealthChecks("/health");

app.Run();
