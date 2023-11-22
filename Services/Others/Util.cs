using System.Security.Cryptography;
using System.Text;

namespace Clothings_Store.Services.Others
{
    public class Util
    {
        public static String HmacSHA512(string key, String inputData)
        {
            var hash = new StringBuilder();
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputData);
            using (var hmac = new HMACSHA512(keyBytes))
            {
                byte[] hashValue = hmac.ComputeHash(inputBytes);
                foreach (var theByte in hashValue)
                {
                    hash.Append(theByte.ToString("x2"));
                }
            }
            return hash.ToString();
        }
        public static string GetIpAddress(IHttpContextAccessor httpContextAccessor)
        {
            string ipAddress;
            try
            {
                ipAddress = httpContextAccessor.HttpContext!.Connection.RemoteIpAddress!.ToString();

                string forwardedFor = httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"]!;
                if (!string.IsNullOrEmpty(forwardedFor) && forwardedFor.ToLower() != "unknown")
                {
                    // Lấy địa chỉ IP từ danh sách có thể chứa nhiều địa chỉ IP được ngăn cách bằng dấu phẩy
                    ipAddress = forwardedFor.Split(',')[0];
                }
            }
            catch (Exception ex)
            {
                ipAddress = "Invalid IP:" + ex.Message;
            }
            return ipAddress;
        }

    }
}
