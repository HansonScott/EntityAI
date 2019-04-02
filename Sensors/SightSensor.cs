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
            entity.senses.SightsCurrentlySeen.AddRange(GetAllNewSightsSeen(SightDistance, entity.CurrentEnvironment.Objects, entity.PositionCurrent, entity.CurrentEnvironment.SightDistance_Current));
        }


        private List<Sight> GetAllNewSightsSeen(double BaseSightDistance, List<EntityObject> objects, Position p, double SightDistance)
        {
            List<Sight> results = new List<Sight>();

            foreach (EntityObject obj in objects)
            {
                Sight s = obj.Appearance;
                if (s.IsSeen(BaseSightDistance, this.Effectiveness_Current, p, SightDistance))
                {
                    if(!SightCurrentlySeen(s))
                    {
                        // log that the entity sees something
                        base.parentSystem.entity.RaiseLog("I see something: " + s.Description);

                        results.Add(s);
                    }
                }
            }

            return results;
        }

        private bool SightCurrentlySeen(Sight s)
        {
            foreach(Sight sght in this.parentSystem.SightsCurrentlySeen)
            {
                if(sght.Description == s.Description &&
                    sght.Origin == s.Origin)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
