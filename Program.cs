using Clothings_Store.Data;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<StoreContext>(options =>
               options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDistributedMemoryCache(); // cache
// session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian session tự động hết hạn
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


var app = builder.Build();

// Configure the HTTP request pipeline. (Middleware)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    //: Cấu hình HTTP Strict Transport Security (HSTS) để tăng cường bảo mật bằng cách
    // yêu cầu trình duyệt kết nối qua HTTPS trong một khoảng thời gian nhất định.
    app.UseHsts();
}

app.UseHttpsRedirection(); // Chuyển hướng yêu cầu từ HTTP sang HTTPS nếu chúng được gửi qua HTTP.

app.UseStaticFiles(); // Cho phép phục vụ các tệp tĩnh như CSS, JavaScript và hình ảnh từ thư mục tĩnh trong dự án.

app.UseRouting(); // Ứng dụng định tuyến yêu cầu HTTP đến các điểm cuối tương ứng (controllers và actions) dựa trên địa chỉ URL.

app.UseSession(); // Middleware sử dụng session

app.UseAuthorization(); // Sử dụng để xác thực và ủy quyền người dùng trong ứng dụng.

// Cấu hình định tuyến cho các controller trong ứng dụng.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

