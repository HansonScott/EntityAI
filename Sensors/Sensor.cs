using System;
using System.Collections.Generic;

namespace EntityAI
{
    /// <summary>
    /// A specific capability of the entity related to capturing sensory input
    /// </summary>
    public abstract class Sensor: EntityAttribute
    {
        private SensorySystem parentSystem;

        public double BaseSightDistance;

        /// <summary>
        /// 0 to 1.0 base, so 0.90 = 90% effectiveness (not the number 90)
        /// </summary>
        public double Effectiveness_Current;

        public Sensor(SensorySystem SensorSystem)
        {
            this.parentSystem = SensorSystem;
        }
        internal abstract void CaptureInput(Entity entity);
    }
}