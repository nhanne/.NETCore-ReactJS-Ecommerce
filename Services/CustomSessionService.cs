using Clothings_Store.Patterns;
using Newtonsoft.Json;
using System.Globalization;

namespace Clothings_Store.Services
{
    public class CustomSessionService<T> : ICustomSessionService<T> where T : class
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string SESSION_KEY = "";
        public CustomSessionService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public List<T> GetSession(string sessionKey)
        {
            SESSION_KEY = sessionKey;
            var session = _httpContextAccessor.HttpContext!.Session;
            string jsonSession = session?.GetString(SESSION_KEY) ?? string.Empty;
            if (!string.IsNullOrEmpty(jsonSession))
            {
                return JsonConvert.DeserializeObject<List<T>>(jsonSession)!;
            }
            return new List<T>();
        }
        public void SaveSession(List<T> listSession)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null && httpContext.Session != null)
            {
                var session = httpContext.Session;
                string listJSON = JsonConvert.SerializeObject(listSession);
                session.SetString(SESSION_KEY, listJSON);
            }
        }
        public void ClearSession(string sessionKey)
        {
            SESSION_KEY = sessionKey;
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null && httpContext.Session != null)
            {
                httpContext.Session.Remove(SESSION_KEY);
            }
        }
    }
}
