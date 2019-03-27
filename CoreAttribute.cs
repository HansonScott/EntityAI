using System;
using System.Text;

namespace EntityAI
{
    /// <summary>
    /// Represents the many statuses and acceptable ranges of operation for the entity
    /// </summary>
    public class CoreAttribute: EntityAttribute
    {
        public enum CoreAttributeType
        {
            Unknown = 0,

            // physical health
            Air = 1,
            Water = 2,
            Nutrients = 3,
            Pain = 4,
            Disease = 5,
            Energy = 6,

            // situational optimization
            Safety = 10,
            Equipment = 11,
            Stockpiles = 12, // quantity of high value items (predictive)
            Preparations = 13, // items needed are prepared for use (predictive)

            // capabilities
            AbleToSense = 20,
            AbleToAct = 21,
        }

        private CoreAttributeType m_CType;
        public CoreAttributeType CType
        {
            get
            { return m_CType; }

            set
            {
                m_CType = value;
                param = new ParameterRange();

                // populate param from type (database lookup?)
            }
        }

        public string Description
        {
            get
            {
                StringBuilder sb = new StringBuilder(Enum.GetName(typeof(CoreAttributeType), CType));
                sb.Append(" has value of ");
                sb.Append(base.CurrentValue * 100);
                sb.Append("%");
                return sb.ToString();
            }
        }

        public string Name
        {
            get
            {
                return Enum.GetName(typeof(CoreAttributeType), this.CType);
            }
        }

        private Entity ParentEntity;

        public CoreAttribute(Entity ParentEntity, CoreAttributeType CType)
        {
            this.ParentEntity = ParentEntity;
            this.CType = CType;
            base.UpdateDelay = SetDelayByType(CType);
        }

        private TimeSpan SetDelayByType(CoreAttributeType cType)
        {
            // future, set this by database
            switch(cType)
            {
                case CoreAttributeType.Water:
                    return new TimeSpan(0, 0, 5);
                default:
                    return new TimeSpan(0, 0, 0);
            }
        }

        internal void UpdateForTiming()
        {
            // only update the values of the ones we have a delay for (?)
            if(UpdateDelay > new TimeSpan(0,0,0))
            {
                if (DateTime.Now > base.LastUpdate + base.UpdateDelay)
                {
                    this.CurrentValue -= .01; // 1%

                    LastUpdate = DateTime.Now;

                    this.ParentEntity.RaiseLog(new EntityLogging.EntityLog($"{this.Description}"));
                }
            }
        }

        internal static double GetEnergyDrainForDistanceMoved(Position positionCurrent, Position pTarget, double speed)
        {
            double dist = positionCurrent.DistanceFrom(pTarget);

            // apply formula, taking into account speed

            // theory: distance is the base, but the faster the speed, the more energy was used.
            double baseSpeed = Ability.BaseSpeed;

            double result = dist * (speed / baseSpeed);

            return result;
        }
    }
}