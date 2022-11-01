using DomainBase;
using Orderpool.Api.Events;

namespace Orderpool.Api.Models.OrderWatcherAggregate
{
    public class OrderWatcher : Entity, IAggregateRoot
    {
        internal OrderWatcher()
        {
            Processses = new List<OrderProcess>();
        }

        internal OrderWatcher(int originOrderPK, Guid orderUuid)
            : this()
        {
            OriginOrderPK = originOrderPK;
            Handler = null;
            OrderUuid = orderUuid;
            Status = ProcessStatus.None;
            CreateTime = DateTime.Now;
        }

        public int OriginOrderPK { get; private set; }
        public string? Handler { get; private set; }
        public Guid OrderUuid { get; private set; }
        public ProcessStatus Status { get; private set; }
        public DateTime CreateTime { get; private set; }
        public DateTime? ProcessingStartTime { get; private set; }
        public List<OrderProcess> Processses { get; set; }

        public OrderWatcher PrepareOrder(string handler)
        {
            if (Status == ProcessStatus.None)
            {
                Handler = handler;
                Status = ProcessStatus.Standby;
            }

            return this;
        }

        public OrderWatcher StartProcess()
        {
            if (Status == ProcessStatus.Standby)
            {
                ProcessingStartTime = DateTime.Now;
                Status = ProcessStatus.Processing;
                AddDomainEvent(new OrderStartProcessingDomainEvent(this.Id));
            }

            return this;
        }

        public OrderWatcher EndProcess()
        {
            if (Status == ProcessStatus.Processing)
            {
                Status = ProcessStatus.Complete;
                AddDomainEvent(new FinishOrderProcessingDomainEvent(this.Id));
            }

            return this;
        }

        public OrderWatcher AddProcessResult(string result)
        {
            var process = new OrderProcess(this.Id, result, DateTime.Now);
            Processses.Add(process);

            return this;
        }
    }

    public enum ProcessStatus
    {
        None,
        Standby,
        Processing,
        Retry,
        Complete
    }
}
