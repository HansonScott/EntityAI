using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityAI
{
    public class SightSensor: Sensor
    {
        double SightDistance;

        public SightSensor(SensorySystem parentSystem): base(parentSystem)
        { 
        }

        internal override void CaptureInput(Entity entity)
        {
            entity.senses.SightsCurrentlySeen = GetAllSightsSeen(SightDistance, entity.CurrentEnvironment.Sights, entity.PositionCurrent, entity.CurrentEnvironment.SightDistance_Current);
        }


        private List<Sight> GetAllSightsSeen(double BaseSightDistance, List<Sight> sights, Position p, double SightDistance)
        {
            List<Sight> results = new List<Sight>();

            foreach (Sight s in sights)
            {

            }

            foreach (Sight s in sights)
            {
                if (s.IsSeen(BaseSightDistance, this.Effectiveness_Current, p, SightDistance)) ;
                {
                    results.Add(s);
                }
            }
            if (results.Count > 0)
            {
                return results;
            }
            else
            {
                return null;
            }
        }

    }
}
