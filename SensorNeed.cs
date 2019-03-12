using System;

namespace EntityAI
{
    internal class SensorNeed: EntityNeed
    {
        public Sensor Sensor;

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