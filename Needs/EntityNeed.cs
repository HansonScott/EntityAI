using System;

namespace EntityAI
{
    /// <summary>
    ///  represents the parent class for all types of needs
    /// </summary>
    public abstract class EntityNeed
    {
        public DateTime OriginationWhen;
        public double Urgency;

        protected EntityNeed()
        {
            OriginationWhen = DateTime.Now;
        }

        public abstract string Name { get; }
    }
}