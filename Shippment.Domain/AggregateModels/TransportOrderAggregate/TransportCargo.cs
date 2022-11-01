using DomainBase;

namespace Shippment.Domain.AggregateModels.TransportOrderAggregate
{
    public class TransportCargo : Entity
    {
        internal TransportCargo(long orderId, string barCode, Cargo cargoInfo)
        {
            OrderId = orderId;
            BarCode = barCode;
            CargoInfo = cargoInfo;
        }
    
        public long OrderId { get; protected set; }
        public string BarCode { get; protected set; }
        public Cargo CargoInfo { get; protected set; }
    }
}


// Barcode为什么不定义在CargoInfo里面，有如下理由：
//  1，barcode不是货物的概念，其主要作用是为了系统中可以标识一件货物
//  2，barcode一般是由单独的服务分发，便于扫描枪扫码
// 所以TransportCargo的构造函数需要接收外部传进来的barcode
