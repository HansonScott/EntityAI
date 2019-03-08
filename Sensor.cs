﻿namespace EntityAI
{
    public class Sensor
    {
        public enum SensorType
        {
            Unknown = 0,

            See = 1,
            Feel = 2,
            Taste = 3,
            Smell = 4,
            Hear = 5,
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

        public Sensor(SensorType t)
        {
            this.SType = t;
        }
    }
}