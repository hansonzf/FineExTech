using DomainBase;
using Shippment.Domain.AggregateModels.EquipmentAggregate;
using Shippment.Domain.AggregateModels.LocationAggregate;
using Shippment.Domain.AggregateModels.RouterAggregate;
using Shippment.Domain.Events;

namespace Shippment.Domain.AggregateModels.ScheduleAggregate
{
    public class TransportSchedule : Entity, IAggregateRoot
    {
        protected TransportSchedule() { }

        public TransportSchedule(Leg routeLeg, DateTime estimateSetoutTime, float transportInterval = 0)
        {
            RouteId = routeLeg.RouteId;
            ScheduleNumber = DateTime.Now.ToString("yyyyMMddhhmmssffff");
            From = routeLeg.From;
            To = routeLeg.To;
            Efficiency = new TimeManagement(estimateSetoutTime, transportInterval);
            Status = ScheduleStatus.Created;
            Type = ScheduleType.Transport;
            //Equipment = equipment;
            //Method = Equipment.Type switch
            //{
            //    EquipmentType.Vehicle => TransportMethod.ByRoad,
            //    EquipmentType.Trackor => TransportMethod.ByRoad,
            //    EquipmentType.Ship => TransportMethod.BySea,
            //    EquipmentType.AirCraft => TransportMethod.ByAir,
            //    _ => throw new NotSupportedException()
            //};

            //AddDomainEvent(new MakingScheduleDomainEvent(RouteId, Equipment, From, To, Efficiency));
        }


        public long RouteId { get; protected set; }
        public string ScheduleNumber { get; protected set; }
        public EquipmentDescription? Equipment { get; protected set; }
        public LocationDescription From { get; protected set; }
        public LocationDescription To { get; protected set; }
        public TimeManagement Efficiency { get; protected set; }
        public TransportMethod Method { get; protected set; }
        public ScheduleType Type { get; protected set; }
        public ScheduleStatus Status { get; protected set; }


        public virtual bool PrepareSchedule()
        {
            if (Status != ScheduleStatus.Created)
                return false;

            var evt = new PrepareScheduleDomainEvent
            { };
            AddDomainEvent(evt);
            Status = ScheduleStatus.Standby;

            return true;
        }

        public virtual bool Execute(LocationDescription departureLocation)
        {
            if (departureLocation is null)
                return false;

            if (Status != ScheduleStatus.Standby)
                throw new InvalidOperationException();

            if (departureLocation.LocationId == From.LocationId)
            {
                var efficiency = new TimeManagement(
                    Efficiency.EstimateSetoutTime,
                    Efficiency.EstimateTransportInterval,
                    DateTime.Now);
                Efficiency = efficiency;
                Status = ScheduleStatus.Executed;

                AddDomainEvent(new ScheduleExecutedDomainEvent());

                return true;
            }

            return false;
        }

        public virtual bool Cancel()
        {
            if (Status != ScheduleStatus.Standby)
                return false;

            AddDomainEvent(new CancelScheduleDomainEvent());

            return true;
        }

        public bool ChangeEstimateSetoutTime(DateTime newSetoutTime)
        {
            if (Status != ScheduleStatus.Standby)
                return false;

            DateTime beforeEstimate = Efficiency.EstimateSetoutTime;
            double tripInterval = Efficiency.EstimateTransportInterval;
            Efficiency = new TimeManagement(newSetoutTime, tripInterval);

            AddDomainEvent(new ScheduleSetoutTimeChangedDomainEvent());

            return true;
        }

        public bool DelayEstimateSetoutTimeByHours(double hours)
        {
            DateTime setoutTime = Efficiency.EstimateSetoutTime;
            DateTime newSetoutTime = setoutTime.AddHours(hours);

            return ChangeEstimateSetoutTime(newSetoutTime);
        }

        public bool AheadEstimateSetoutTimeByHours(double hours)
        {
            DateTime setoutTime = Efficiency.EstimateSetoutTime;
            DateTime newSetoutTime = setoutTime.AddHours(0 - hours);

            return ChangeEstimateSetoutTime(newSetoutTime);
        }

        public virtual bool CheckIn(LocationDescription destinationLocation)
        {
            if (Status != ScheduleStatus.Executed)
                throw new InvalidOperationException();

            if (To.LocationId == destinationLocation.LocationId)
            {
                Status = ScheduleStatus.Completed;
                var evt = new ArrivedTransportDestinationDomainEvent
                { 
                    ScheduleId = Id,
                    ScheduleType = Type,
                    Setout = From,
                    Destination = To
                };
                AddDomainEvent(evt);
                return true;
            }

            return false;
        }

        public bool ChangeScheduleDestination(LocationDescription destination)
        {
            if (Status != ScheduleStatus.Standby)
                return false;

            To = destination;

            return true;
        }
    }
}
