namespace Batbert.Interfaces
{
    public interface ILogger<T>
    {
        void Trace(string msg);
        void Information(string msg);
        void Debug(string msg);
        void Warning(string msg);
        void Error(string msg);
        void Fatal(string msg);
    }
}
