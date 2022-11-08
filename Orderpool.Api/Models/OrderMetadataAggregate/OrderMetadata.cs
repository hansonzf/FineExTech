using DomainBase;

namespace Orderpool.Api.Models.OrderMetadataAggregate
{
    public class OrderMetadata : Entity, IAggregateRoot
    {
        protected OrderMetadata()
        { }

        public OrderMetadata(Order remoteOrder)
        { 
            
        }
    }
}
