using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityAI
{
    public class PainSensor: Sensor
    {
        public PainSensor(SensorySystem sensorySystem): base(sensorySystem)
        {
        }

        public override string Name
        {
            get { return "Pain"; }
        }

        internal override void CaptureInput(Entity entity)
        {
            throw new NotImplementedException();
        }
    }
}
