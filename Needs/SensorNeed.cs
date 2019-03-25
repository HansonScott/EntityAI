using System;

namespace EntityAI
{
    /// <summary>
    /// Represents a need relating to the sensors themselves
    /// </summary>
    internal class SensorNeed: EntityNeed
    {
        public Sensor Sensor;

        public override string Name
        {
            get { return this.Sensor.Name; }
        }

        public SensorNeed(Sensor s): base()
        {
            this.Sensor = s;
            SetUrgency();
        }

        private void SetUrgency()
        {
            double diffPercent = this.Sensor.Effectiveness_Current / 1;

            if (diffPercent < .3) { base.Urgency = 3; }
            else if (diffPercent < .6) { base.Urgency = 2; }
            else if (diffPercent < .8) { base.Urgency = 1; }
            else { base.Urgency = 0; }
        }
    }
}