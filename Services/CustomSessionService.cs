using Clothings_Store.Patterns;
using Newtonsoft.Json;
using System.Globalization;

namespace Clothings_Store.Services
{
    public class CustomSessionService<T> : ICustomSessionService<T> where T : class
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<CustomSessionService<T>> _logger;
        private string SESSION_KEY = "";
        public CustomSessionService(IHttpContextAccessor httpContextAccessor, ILogger<CustomSessionService<T>> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public List<T> GetSession(string sessionKey)
        {
            try
            {
                SESSION_KEY = sessionKey;
                var session = _httpContextAccessor.HttpContext!.Session;
                string jsonSession = session?.GetString(SESSION_KEY) ?? string.Empty;
                if (!string.IsNullOrEmpty(jsonSession))
                {
                    _logger.LogInformation("Get session." + sessionKey + " success");
                    return JsonConvert.DeserializeObject<List<T>>(jsonSession)!;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when get session " + sessionKey);
            }
            _logger.LogInformation("Create session." + sessionKey + " success");
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
