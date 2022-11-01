using DomainBase;

namespace Orderpool.Api.Models.OrderWatcherAggregate
{
    public class OrderProcess : Entity
    {
        internal OrderProcess()
        { }

        internal OrderProcess(long watcherId, string result, DateTime processTime)
        {
            WatcherId = watcherId;
            Result = result;
            ProcessTime = processTime;
        }

        public long WatcherId { get; private set; }
        public string Result { get; set; }
        public DateTime ProcessTime { get; set; }
    }
}
