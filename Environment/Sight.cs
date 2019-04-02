using System;

namespace EntityAI
{
    public class Sight
    {
        public enum RecognitionFootPrint // all the possible things an entity can see - database?
        {
            Unknown = 0,

            Water = 1,
            Wind = 2,
            Fire = 3,

            Footfall = 10,

            Animal_Call = 20,

            Container = 30,
        }

        public Position Origin;
        public RecognitionFootPrint FootPrint;

        public string Description
        {
            get
            {
                return FootPrint.ToString();
            }
        }

        public Sight(Sight.RecognitionFootPrint rfp, Position p)
        {
            this.Origin = p;
            this.FootPrint = rfp;
        }

        internal bool IsSeen(double EntitySightDistance, double effectiveness_Current, Position p, double EnvironmentSightDistance)
        {
            // establish distance of the object from the position p
            double dist = p.DistanceFrom(this.Origin);

            // if out of environment range, we're done
            if (dist > EnvironmentSightDistance) { return false; }
            else // the object is within the environment's sight distance
            {
                // compare distance with the seeing ability of the entity


                return true;
            }
        }
    }
}