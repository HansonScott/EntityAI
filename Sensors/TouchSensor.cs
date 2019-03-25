using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityAI
{
    public class TouchSensor : Sensor
    {
        public override string Name
        {
            get { return "Touch"; }
        }
        public TouchSensor(SensorySystem parentSystem) : base(parentSystem) { }
        internal override void CaptureInput(Entity entity)
        {
            throw new NotImplementedException();
        }
    }
}
