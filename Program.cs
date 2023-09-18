using Clothings_Store.Data;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<StoreContext>(options =>
               options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    //: Đây là middleware để cấu hình HTTP Strict Transport Security (HSTS) để tăng cường bảo mật bằng cách yêu cầu trình duyệt kết nối qua HTTPS trong một khoảng thời gian nhất định.
    app.UseHsts();
}
// Middleware này sử dụng để tự động chuyển hướng yêu cầu từ HTTP sang HTTPS nếu chúng được gửi qua HTTP.
app.UseHttpsRedirection();
// Đây là middleware cho phép phục vụ các tệp tĩnh như CSS, JavaScript và hình ảnh từ thư mục tĩnh trong dự án.
app.UseStaticFiles();
// Middleware này làm cho ứng dụng định tuyến yêu cầu HTTP đến các điểm cuối tương ứng (controllers và actions) dựa trên địa chỉ URL.
app.UseRouting();
// Middleware này sử dụng để xác thực và ủy quyền người dùng trong ứng dụng.
app.UseAuthorization();
// Đây là cấu hình định tuyến cho các controller trong ứng dụng.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

