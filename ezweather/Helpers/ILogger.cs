namespace ezweather.Helpers
{
    public interface ILogger
    {
        void Write(string message);
        void Write(string message, params object[] args);
    }
}