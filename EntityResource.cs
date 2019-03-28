using System;

namespace EntityAI
{
    public class EntityResource
    {
        public enum ResourceType
        {
            Unknown = 0,
            Air = 1,
            Water = 2,

            Container = 10,
        }

        public double Quantity = 1.0;

        public ResourceType RType;

        public EntityResource(ResourceType t)
        {
            this.RType = t;
        }

        internal bool IsConsumedOnUse()
        {
            switch(this.RType)
            {
                case ResourceType.Air:
                case ResourceType.Water:
                    return true;
                default:
                    return false;
            }
        }

        internal bool IsContainer()
        {
            return (this.RType == ResourceType.Container);
        }
    }
}