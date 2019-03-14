using System;

namespace EntityAI
{
    /// <summary>
    /// A specific capability of the entity related to capturing sensory input
    /// </summary>
    public class Sensor: EntityAttribute
    {
        public enum SensorType
        {
            Unknown = 0,

            See = 1,
            Feel = 2,
            Taste = 3,
            Smell = 4,
            Hear = 5,

            Temperature = 10,

            Pain = 20,
        }

        private SensorType m_SType;
        public SensorType SType
        {
            get
            {
                return m_SType;
            }
            set
            {
                this.m_SType = value;
            }
        }

        public double Effectiveness_Current;

        public Sensor(SensorType t)
        {
            this.SType = t;
        }

        internal InputNeed CaptureInput(Entity entity)
        {
            throw new NotImplementedException();
        }
    }
}