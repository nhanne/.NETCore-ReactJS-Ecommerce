namespace Clothings_Store.Patterns
{
    public interface ICustomSessionService<T>
    {
        List<T> GetSession(string sessionKey);
        void ClearSession(string sessionKey);
        void SaveSession(List<T> listSession);
    }
}
