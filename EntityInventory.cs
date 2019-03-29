using System;
using System.Collections.Generic;

namespace EntityAI
{
    public class EntityInventory
    {
        public List<EntityResource> Items;
        public Entity entity;

        public EntityInventory(Entity entity)
        {
            this.Items = new List<EntityResource>();
            this.entity = entity;
        }
        public void AddResource(EntityResource res)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                EntityResource er = Items[i];
                if (er.RType == res.RType)
                {
                    er.Quantity += res.Quantity;
                    return;
                }
            }

            // if we get here, then we don't have this item type yet
            Items.Add(res);
        }
        public void RemoveResource(EntityResource.ResourceType RType)
        {
            for(int i = 0; i < Items.Count; i++)
            {
                EntityResource er = Items[i];
                if (er.RType == RType)
                {
                    if(er.Quantity > 1)
                    {
                        er.Quantity--;
                    }
                    else
                    {
                        Items.Remove(er);
                    }
                    break;
                }
            }
        }
        public bool HaveResource(EntityResource.ResourceType RType)
        {
            return HaveResource(RType, 1);
        }
        public bool HaveResource(EntityResource.ResourceType RType, int quantity)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                EntityResource er = Items[i];
                if (er.RType == RType)
                {
                    return (er.Quantity >= quantity);
                }
            }

            return false;
        }
    }
}