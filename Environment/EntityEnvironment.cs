using System.Collections.Generic;

namespace EntityAI
{
    public class EntityEnvironment
    {
        public List<Sound> Sounds;
        public List<Sight> Sights;
        public double SightDistance_Current;
        private const double SightDistance_Default = 5000; // units of distance in meters

        public EntityEnvironment()
        {
            Sounds = new List<Sound>();
            Sights = new List<Sight>();
        }
    }
}