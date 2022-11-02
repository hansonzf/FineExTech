using DomainBase;
using Shippment.Domain.AggregateModels.EquipmentAggregate;
using Shippment.Domain.AggregateModels.LocationAggregate;
using Shippment.Domain.AggregateModels.TransportOrderAggregate;
using Shippment.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shippment.Domain.AggregateModels.ScheduleAggregate
{
    public class PickupSchedule : TransportSchedule
    {
        public PickupInformation PickupInfo { get; protected set; }

        protected PickupSchedule()
            : base()
        {
            Type = ScheduleType.Pickup;
        }

        public PickupSchedule(EquipmentDescription equipment, DateTime estimateSetoutTime, LocationDescription from, PickupDescription pickupDescription)
            : this()
        {
            PickupInfo = PickupInformation.CopyFrom(pickupDescription);
            ScheduleNumber = DateTime.Now.ToString("yyyyMMddhhmmssffff");
            Equipment = equipment;
            From = from;
            To = new LocationDescription(0, pickupDescription.DetailAddress);
            Efficiency = new TimeManagement(estimateSetoutTime);
            Status = ScheduleStatus.Created;

            //AddDomainEvent(new MakingScheduleDomainEvent(RouteId, Equipment, From, To, Efficiency));
        }

        public override bool Execute(LocationDescription departureLocation)
        {
            if (!PickupInfo.Meaningful())
                return false;

            return base.Execute(departureLocation);
        }

        public override bool CheckIn(LocationDescription destinationLocation)
        {
            return base.CheckIn(destinationLocation);
        }
    }

    public class PickupInformation : ValueObject
    {
        private readonly DateTime minDate = new DateTime(1753, 1, 1);
        public string PickupCode { get; private set; }
        public string DetailAddress { get; private set; }
        public string ContactName { get; private set; }
        public string Phone { get; private set; }
        public DateTime PickupTime { get; private set; }
        public string Remark { get; private set; }

        private PickupInformation()
        { }

        public PickupInformation(string pickupCode, string detailAddress, string contactName, string phone, DateTime pickupTime, string remark)
        {
            PickupCode = pickupCode;
            DetailAddress = detailAddress;
            ContactName = contactName;
            Phone = phone;
            PickupTime = pickupTime;
            Remark = remark;
        }

        public bool GeneratePickupCode()
        {
            PickupCode = new Random(1).Next(0, 9999).ToString("0000");
            return !string.IsNullOrEmpty(PickupCode);
        }

        public bool Meaningful()
        {
            bool result = true;
            result &= !string.IsNullOrEmpty(ContactName);
            result &= !string.IsNullOrEmpty(Phone);
            result &= !string.IsNullOrEmpty(DetailAddress);
            result &= !string.IsNullOrEmpty(PickupCode);
            result &= PickupTime != minDate;

            return result;
        }

        public static PickupInformation CopyFrom(PickupDescription pickupDescription)
        {
            if (pickupDescription is null)
                return null;

            var info = new PickupInformation
            {
                PickupCode = pickupDescription.PickupCode,
                DetailAddress = pickupDescription.DetailAddress,
                ContactName = pickupDescription.ContactName,
                Phone = pickupDescription.Phone,
                PickupTime = pickupDescription.PickupTime.HasValue ? pickupDescription.PickupTime.Value : new DateTime(1753, 1, 1),
                Remark = pickupDescription.Remark
            };

            return info;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return PickupCode;
            yield return DetailAddress;
            yield return ContactName;
            yield return Phone;
            yield return PickupTime;
            yield return Remark;
        }
    }
}
