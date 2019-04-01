using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityAI
{
    public class ResourceNeed : EntityNeed
    {
        public EntityResource Resource;

        public override string Name
        {
            get
            {
                if(Resource != null)
                {
                    return Resource.Description;
                }
                else return string.Empty;
            }
        }
        public ResourceNeed(EntityResource.ResourceType T, double Quantity, Position P)
        {
            Resource = new EntityResource(T, P);
            Resource.Quantity = Quantity;
        }
    }
}
