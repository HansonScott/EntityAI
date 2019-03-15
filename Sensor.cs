using System;
using System.Collections.Generic;

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

        public List<Sound> SoundsCurrentlyHeard;

        /// <summary>
        /// 0 to 1.0 base, so 0.90 = 90% effectiveness (not the number 90)
        /// </summary>
        public double Effectiveness_Current;

        public Sensor(SensorType t)
        {
            this.SType = t;
        }

        internal void CaptureInput(Entity entity)
        {
            // this is where, depending on the type, we need to capture the data (seen items, noises heard, etc.)
            switch (this.SType)
            {
                case SensorType.Feel:
                    // more difficult, add later
                    break;
                case SensorType.Hear:
                    // sort of cheating in the code to send the environment information, but logical for an AI
                    SoundsCurrentlyHeard = GetAllSoundsHeard(entity.CurrentEnvironment.Sounds, entity.PositionCurrent);
                    break;
                case SensorType.Pain:
                    // body pain
                    break;
                case SensorType.See:
                    // environment objects
                    break;
                case SensorType.Smell:
                    // environment smells
                    break;
                case SensorType.Taste:
                    // as an intransitive verb, the taste of the air/environment.  As transitive, the taste of the target.
                    break;
                case SensorType.Temperature:
                    // environment temperature
                    // body temparature
                    break;
                case SensorType.Unknown:
                default:
                    break;
            }
        }

        #region Sounds
        private List<Sound> GetAllSoundsHeard(List<Sound> sounds, Position p)
        {
            List<Sound> results = new List<Sound>();

            // capture ambient volume.
            double ambiantLoudness = 0;
            foreach (Sound s in sounds)
            {
                ambiantLoudness = Math.Max(ambiantLoudness, s.Loudness);
            }


            foreach (Sound s in sounds)
            {
                if (s.IsHeard(this.Effectiveness_Current, p, ambiantLoudness));
                {
                    results.Add(s);     
                }
            }
            if(results.Count > 0)
            {
                return results;
            }
            else
            {
                return null;
            }
        }
        #endregion
    }
}