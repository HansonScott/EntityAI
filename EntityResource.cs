namespace EntityAI
{
    internal class EntityResource
    {
        public enum ResourceType
        {
            Unknown = 0,
            Air = 1,
            Water = 2,

            Container = 10,
        }

        public double Quantity = 0.1;

        public ResourceType RType;

        public EntityResource(ResourceType t)
        {
            this.RType = t;
        }
    }
}