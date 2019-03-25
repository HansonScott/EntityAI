using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityAI
{
    public class TemperatureSensor: Sensor
    {
        public override string Name
        {
            get { return "Temperature"; }
        }
        public TemperatureSensor(SensorySystem sensorySystem): base(sensorySystem)
        {
        }

        internal override void CaptureInput(Entity entity)
        {
            throw new NotImplementedException();
        }
    }
}
