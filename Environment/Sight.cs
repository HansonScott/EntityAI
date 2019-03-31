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

        public EntityObject ThisObject = null;
        public Position Origin;
        public RecognitionFootPrint FootPrint;

        public string Description
        {
            get
            {
                if(ThisObject == null) { return "nothing"; }
                else
                {
                    return ThisObject.Description;
                }
            }
        }

        public Sight(EntityObject obj)
        {
            this.ThisObject = obj;
            this.Origin = obj.Position;

            // determine the footprint based on the object
            FootPrint = obj.Appearance;
        }

        internal bool IsSeen(double EntitySightDistance, double effectiveness_Current, Position p, double EnvironmentSightDistance)
        {
            // visibility of the object
            if(!ThisObject.Visibility)
            {
                // adjust for any invisible-piercing ability?

                return false;
            }
            else
            {
                // establish distance of the object from the position p
                double dist = p.DistanceFrom(ThisObject.Position);

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
}