﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityAI
{
    public class TasteSensor: Sensor
    {
        public TasteSensor(SensorySystem sensorySystem): base(sensorySystem)
        {
        }

        internal override void CaptureInput(Entity entity)
        {
            throw new NotImplementedException();
        }
    }
}
