using Microsoft.EntityFrameworkCore;
using WebBusinessPromilexApp.Models;
using WebBusinessPromilexApp.Services;
using WebBusinessPromilexApp.Filters;

var builder = WebApplication.CreateBuilder(args);

// 1. Dodanie obsługi Kontrolerów i Widoków
builder.Services.AddControllersWithViews();

// 2. Konfiguracja bazy danych SQL Server
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

// 3. Konfiguracja SESJI
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(4);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = "PromilexAdminSession";
});

builder.Services.AddHttpContextAccessor();

// 4. REJESTRACJA SERWISÓW (Dependency Injection)
// Rejestrujemy wszystkie klasy z folderu Services, aby kontrolery mogły z nich korzystać
builder.Services.AddScoped<AuthenticationService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<EmployeeService>();
builder.Services.AddScoped<MessageService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<PromotionService>();
builder.Services.AddScoped<SupplierService>();
builder.Services.AddScoped<InventoryService>();
builder.Services.AddScoped<InventoryLogService>();
builder.Services.AddScoped<AdminService>(); // Jeśli to nazwa klasy, a nie interfejsu

// 5. Rejestracja Filtrów
builder.Services.AddScoped<AdminOnlyAttribute>();

var app = builder.Build();

// --- KONFIGURACJA POTOKU (Middleware) ---

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// KLUCZOWA KOLEJNOŚĆ: Sesja musi być PRZED Autoryzacją
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();