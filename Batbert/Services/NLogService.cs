using Batbert.Interfaces;
using NLog;

namespace Batbert.Services
{
    public class NLogService<T> : ILogger<T>
    {
        private Logger _instance;

        public Logger Instance => _instance ??= LogManager.GetCurrentClassLogger();

        public void Trace(string msg)
        {
            Instance.Trace("[{0}]\t {1}", typeof(T).Name, msg);
        }

        public void Information(string msg)
        {
            Instance.Info("[{0}]\t {1}", typeof(T).Name, msg);
        }

        public void Debug(string msg)
        {
            Instance.Debug("[{0}]\t {1}", typeof(T).Name, msg);
        }

        public void Warning(string msg)
        {
            Instance.Warn("[{0}]\t {1}", typeof(T).Name, msg);
        }

        public void Error(string msg)
        {
            Instance.Error("[{0}]\t {1}", typeof(T).Name, msg);
        }
        public void Fatal(string msg)
        {
            Instance.Fatal("[{0}]\t {1}", typeof(T).Name, msg);
        }

    }
}
