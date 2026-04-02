using Microsoft.EntityFrameworkCore;
using WebBusinessPromilexApp.Models;
using WebBusinessPromilexApp.Services;
using WebBusinessPromilexApp.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlServerOptionsAction: sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);
        }));

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(4);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = "PromilexAdminSession";
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<EmployeeService>();
builder.Services.AddScoped<MessageService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<PromotionService>();
builder.Services.AddScoped<SupplierService>();
builder.Services.AddScoped<InventoryLogService>();
builder.Services.AddScoped<AdminService>();

builder.Services.AddScoped<AdminOnlyAttribute>();

var app = builder.Build();

// Inicjalizacja bazy danych i tworzenie konta admina, jeśli nie istnieje
/*
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();

    context.Database.EnsureCreated();

    var adminUser = context.Employees.FirstOrDefault(e => e.Username == "admin");
    var hasher = new Microsoft.AspNetCore.Identity.PasswordHasher<Employee>();

    if (adminUser == null)
    {
        // 1. Admina w ogóle nie ma - tworzymy nowego
        adminUser = new Employee
        {
            Username = "admin",
            FirstName = "Główny",
            LastName = "Administrator",
            Role = "Admin",
            PasswordHash = hasher.HashPassword(null!, "Admin123!")
        };
        context.Employees.Add(adminUser);
        context.SaveChanges();
    }
    else
    {
        // 2. Admin istnieje, sprawdzamy czy jego hasło jest zepsute (nie jest hashem)
        // Hashe z PasswordHasher zawsze zaczynają się od "AQAAAA" (wersja V3)
        if (adminUser.PasswordHash == null || !adminUser.PasswordHash.StartsWith("AQAAAA"))
        {
            adminUser.PasswordHash = hasher.HashPassword(adminUser, "Admin123!");
            context.SaveChanges();
        }
    }
}*/
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
