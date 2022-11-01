using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TempConsoleApp
{
    public class OrderWatcher
    {
        public long Id { get; set; }
        public int OriginOrderPK { get; set; }
        public string? Handler { get; set; }
        public Guid OrderUuid { get; set; }
        public ProcessStatus Status { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? ProcessingStartTime { get; set; }

        private static long _id = 0;
        private static int _originPK = 0;
        public static Faker<OrderWatcher> FakerData { get; } =
            new Faker<OrderWatcher>()
                .RuleFor(p => p.Id, f => _id++)
                .RuleFor(p => p.OriginOrderPK, f => _originPK++)
                .RuleFor(p => p.Handler, f => "723b2abd-ddba-41e8-a342-212a71d947b2")
                .RuleFor(p => p.OrderUuid, f => Guid.NewGuid())
                .RuleFor(p => p.Status, f => ProcessStatus.None)
                .RuleFor(p => p.CreateTime, f => DateTime.Now);
    }

    public class OrderProcess
    {
        public long Id { get; set; }
        public long WatcherId { get; set; }
        public string Result { get; set; }
        public DateTime ProcessTime { get; set; }
    }

    public class RemoteOrder
    {
        public long Id { get; set; }
        public int OriginOrderPK { get; set; }
        public Guid OrderUuid { get; set; }
        public string ContentOfOrder { get; set; }
        public DateTime CreatedTime { get; set; }

        private static long _id = 0;
        private static int _originPK = 0;
        public static Faker<RemoteOrder> FakerData { get; } =
            new Faker<RemoteOrder>()
                .RuleFor(p => p.Id, f => _id++)
                .RuleFor(p => p.OriginOrderPK, f => _originPK++)
                .RuleFor(p => p.OrderUuid, f => Guid.NewGuid())
                .RuleFor(p => p.ContentOfOrder, f => "{}")
                .RuleFor(p => p.CreatedTime, f => DateTime.Now);
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
