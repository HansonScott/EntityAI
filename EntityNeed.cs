using System;

namespace EntityAI
{
    public class EntityNeed
    {
        public DateTime OriginationWhen;
        public double Urgency;

        protected EntityNeed()
        {
            OriginationWhen = DateTime.Now;
        }
    }
}