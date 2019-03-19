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
                sb.Append(" has effectiveness of ");
                sb.Append(base.Effectiveness * 100);
                sb.Append("%, and an effectiveness of ");
                sb.Append(base.Effectiveness * 100);
                sb.Append("%");
                return sb.ToString();
            }
        }

        public CoreAttribute(CoreAttributeType CType)
        {
            this.CType = CType;
        }
    }
}