namespace Clothings_Store.Interface
{
    public interface ICustomSessionService<T>
    {
        List<T> GetSession(string sessionKey);
        void ClearSession(string sessionKey);
        void SaveSession(List<T> listSession);
    }
}
