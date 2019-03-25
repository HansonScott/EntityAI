using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityAI
{
    public class SmellSensor: Sensor
    {
        public override string Name
        {
            get { return "Smell"; }
        }
        public SmellSensor(SensorySystem sensorySystem): base(sensorySystem)
        {
        }

        internal override void CaptureInput(Entity entity)
        {
            throw new NotImplementedException();
        }
    }
}
