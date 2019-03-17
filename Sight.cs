using System;

namespace EntityAI
{
    public class Sight
    {
        EntityObject ThisObject;

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