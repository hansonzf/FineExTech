using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Domain.AggregatesModel.CountingCargoPaperAggregate;

namespace Warehouse.Domain.Events
{
    public class StorehouseNotHaveEnoughCapabilityDomainEvent : INotification
    {
        public StorehouseNotHaveEnoughCapabilityDomainEvent(List<CountingCargoResult> cargoDetails)
        {
            CargoDetails = cargoDetails;
        }

        public List<CountingCargoResult> CargoDetails { get; private set; }
    }
}
