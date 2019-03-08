using System;

namespace EntityAI
{
    public class CoreAttribute
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
        public enum ValueRelativeStatus
        {
            LowSD3 = 0,
            LowSD2 = 1,
            LowSD1 = 2,
            Mid = 3,
            HighSD1 = 4,
            HighSD2 = 5,
            HighSD3 = 6,
        }

        public double Urgency;
        public double Importance;

        public double Efficiency; //?
        public double Effectiveness; //?

        public ParameterRange param = new ParameterRange(CoreAttributeType.Unknown);

        public double CurrentValue;

        private CoreAttributeType m_CType;
        public CoreAttributeType CType
        {
            get
            { return m_CType; }

            set
            {
                m_CType = value;
                param = new ParameterRange(value);
            }
        }

        public CoreAttribute()
        {

        }

        internal ValueRelativeStatus GetRelativeValueStatus()
        {
            throw new NotImplementedException();
        }
    }
}