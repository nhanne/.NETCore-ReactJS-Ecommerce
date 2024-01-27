using Clothings_Store.Data;
using Clothings_Store.Interface;
using Clothings_Store.Models.Database;
using Clothings_Store.Models.Others;
using Clothings_Store.Repositories;
using Clothings_Store.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using System.Formats.Asn1;
using Microsoft.OpenApi.Models;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions();// Add services to the container.
var mvcBuilder = builder.Services.AddControllersWithViews();
var razorBuilder = builder.Services.AddRazorPages();
if (builder.Environment.IsDevelopment())
{
    razorBuilder.AddRazorRuntimeCompilation();
    mvcBuilder.AddRazorRuntimeCompilation();
};
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});
// How to connect StoreContext to MS SQL Server
builder.Services.AddDbContext<StoreContext>(options =>
               options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnection")),ServiceLifetime.Scoped);
builder.Services.AddDatabaseDeveloperPageExceptionFilter();  // How to throw e when error connected database

// Session distributed cache SQL Server
builder.Services.AddDistributedSqlServerCache(options =>
{
    options.ConnectionString = "Data Source=NHAN;Initial Catalog=ClothingsStore;Trusted_Connection=SSPI;Encrypt=false;TrustServerCertificate=true";
    //options.ConnectionString = "Data Source=SQL8006.site4now.net;Initial Catalog=db_aa2078_dbstore;User ID=db_aa2078_dbstore_admin;Password=UcB3266SbE.R5ry";
    options.SchemaName = "dbo";
    options.TableName = "OrderInfoSession";
});
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = "StoreSession";
});
// ASP.NET Identity
builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<StoreContext>()
                .AddDefaultTokenProviders();
builder.Services.Configure<SecurityStampValidatorOptions>(o =>
                   o.ValidationInterval = TimeSpan.FromMinutes(1));
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
    options.Cookie.Name = "StoreCookie";
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});
// External Login Configuration
builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{
    IConfigurationSection googleAuthNSection = builder.Configuration.GetSection("Authentication:Google");
    googleOptions.ClientId = googleAuthNSection["ClientId"]!;
    googleOptions.ClientSecret = googleAuthNSection["ClientSecret"]!;
    googleOptions.CallbackPath = "/loginGoogle";
}).AddFacebook(facebookOptions =>
{
    IConfigurationSection facebookAuthNSection = builder.Configuration.GetSection("Authentication:Facebook");
    facebookOptions.AppId = facebookAuthNSection["AppId"]!;
    facebookOptions.AppSecret = facebookAuthNSection["AppSecret"]!;
    facebookOptions.CallbackPath = "/loginFacebook";
});
// Service DI 
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddScoped<IEmailSender, SendMailService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped(typeof(ICustomSessionService<>), typeof(CustomSessionService<>));

// Repository 
builder.Services.AddScoped<IRepository<Category>, CategoryRepository>();
builder.Services.AddScoped<IRepository<Order>, OrderRepository>();
builder.Services.AddScoped<IRepository<OrderDetail>, OrderDetailRepository>();
builder.Services.AddScoped<IRepository<Product>, ProductRepository>();
builder.Services.AddScoped<IRepository<Customer>, CustomerRepository>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Online Payment
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.Configure<VnPayConfig>(builder.Configuration.GetSection("OnlinePayment:Vnpay"));
builder.Services.Configure<MoMoConfig>(builder.Configuration.GetSection("OnlinePayment:Momo"));
//
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IdentityErrorDescriber, AppIdentityErrorDescriber>();

var app = builder.Build();
// Configure the HTTP request pipeline. (Middleware)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.Use(async (context, next) =>
{
    if (context.Request.IsHttps)
    {
        context.Request.Scheme = "https";
    }
    await next();

    if (context.Response.StatusCode == 404 && !context.Response.HasStarted)
    {
        context.Request.Path = "/"; 
        await next();
    }
});
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});
app.Run();

