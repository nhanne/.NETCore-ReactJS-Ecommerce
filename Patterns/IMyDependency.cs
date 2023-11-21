namespace Clothings_Store.Patterns
{
    public interface IMyDependency
    {
        void WriteMessage(string message);
    }
    public class MyDependency : IMyDependency
    {
        public void WriteMessage(string message)
        {
            Console.WriteLine($"\tMyDependency.WriteMessage Message: {message}");
        }
    }
}
