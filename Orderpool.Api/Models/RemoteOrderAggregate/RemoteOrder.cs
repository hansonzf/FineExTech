using DomainBase;

namespace Orderpool.Api.Models.RemoteOrderAggregate
{
    public class RemoteOrder : Entity, IAggregateRoot
    {
        internal RemoteOrder()
        { }

        internal RemoteOrder(int originOrderPK, Guid orderUuid, string contentOfOrder)
        {
            OriginOrderPK = originOrderPK;
            OrderUuid = orderUuid;
            ContentOfOrder = contentOfOrder;
            CreatedTime = DateTime.Now;
        }

        public int OriginOrderPK { get; private set; }
        public Guid OrderUuid { get; private set; }
        public string ContentOfOrder { get; private set; }
        public DateTime CreatedTime { get; private set; }

        public Guid Bind()
        {
            var orderUuid = Guid.NewGuid();
            OrderUuid = orderUuid;

            return orderUuid;
        }
    }
}
