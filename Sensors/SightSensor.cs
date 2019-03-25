using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityAI
{
    public class SightSensor: Sensor
    {
        public const double SIGHT_DISTANCE_DEFAULT = 500;
        double SightDistance = SIGHT_DISTANCE_DEFAULT;
        public override string Name
        {
            get { return "Sight"; }
        }

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
                if (s.IsSeen(BaseSightDistance, this.Effectiveness_Current, p, SightDistance))
                {
                    // log that the entity sees something
                    base.parentSystem.entity.RaiseLog(new EntityLogging.EntityLog("I see " + s.Description, System.Diagnostics.TraceLevel.Verbose));

                    results.Add(s);
                }
            }

            return results;
        }
    }
}
