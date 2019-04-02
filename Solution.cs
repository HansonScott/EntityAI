using System;
using System.Collections.Generic;
using System.Text;

namespace EntityAI
{
    /// <summary>
    /// represents a complex set of actions that bring about resolution of a need.
    /// </summary>
    public class Solution
    {
        public enum EntitySolutionState
        {
            created = 0,
            planned = 1,
            active = 2,
            blocked = 3,
            completed = 4,
        }

        public EntitySolutionState SolutionState;
        public List<EntityAction> Actions;

        public string Description
        {
            get
            {
                StringBuilder sb = new StringBuilder(Actions.Count);
                bool first = true;
                foreach(EntityAction ea in Actions)
                {
                    if(!first)
                    {
                        sb.Append(",");
                    }
                    sb.Append(ea.Description);
                    first = false;
                }

                return sb.ToString();
            }
        }

        public Solution()
        {
            Actions = new List<EntityAction>();
            SolutionState = EntitySolutionState.created;
        }

        public static Solution FindSolutionForNeed(EntityNeed need, Entity CurrentEntity)
        {
            // lookup from database?
            // create from known actions?
            // if none found, return null

            if(need is CoreNeed)
            {
                CoreNeed cn = need as CoreNeed;

                switch(cn.Attribute.CType)
                {
                    case CoreAttribute.CoreAttributeType.Water:
                        Solution result = new Solution();

                        EntityResource.ResourceType neededResourceType = EntityResource.ResourceType.Water; // DB lookup would be good here.

                        EntityResource neededRes = new EntityResource(neededResourceType, CurrentEntity.PositionCurrent);

                        // consume the water
                        // NOTE: split out to own lines, so we can add the cost to each action before adding it.
                        result.Actions.Add(new EntityAction(result, new Ability(Ability.AbilityType.Consume), neededRes));

                        if(!CurrentEntity.Inventory.HaveResource(neededResourceType))
                        {
                            if(neededRes.RequiresContainer())
                            {
                                result.Actions.Insert(0, new EntityAction(result, new Ability(Ability.AbilityType.Pick_Up),
                                                                            new EntityResource(neededResourceType, CurrentEntity.PositionCurrent),
                                                                            new EntityResource(EntityResource.ResourceType.Container, CurrentEntity.PositionCurrent)));
                            }
                            else
                            {
                                result.Actions.Insert(0, new EntityAction(result, new Ability(Ability.AbilityType.Pick_Up),
                                                                            new EntityResource(neededResourceType, CurrentEntity.PositionCurrent)));
                            }
                        }

                        return result;
                    default:
                        break;
                }
            }
            else if(need is ResourceNeed)
            {
                ResourceNeed rn = need as ResourceNeed;
                Solution result = new Solution();

                if(rn.Resource.RequiresContainer())
                {
                    result.Actions.Add(new EntityAction(result, new Ability(Ability.AbilityType.Pick_Up),
                                                                new EntityResource(rn.Resource.RType, CurrentEntity.PositionCurrent),
                                                                new EntityResource(EntityResource.ResourceType.Container, CurrentEntity.PositionCurrent)));
                }
                else
                {
                    result.Actions.Add(new EntityAction(result, new Ability(Ability.AbilityType.Pick_Up),
                                                                new EntityResource(EntityResource.ResourceType.Container, CurrentEntity.PositionCurrent)));
                }

                return result;
            }

            return null;
        }
        
        public EntityAction GetNextAction(EntityAction A)
        {
            if(this.Actions == null || this.Actions.Count == 0) { return null; }

            // otherwise, look through the list for the next index.
            int currentIndex = 0;
            for(int i = 0; i < this.Actions.Count; i++)
            {
                if (this.Actions[i] == A) { currentIndex = i; break; }
            }

            if (this.Actions.Count > currentIndex + 1) { return this.Actions[currentIndex + 1]; }

            return null;
        }

        internal int GetIndexOfAction(EntityAction ea)
        {
            for(int i = 0; i < this.Actions.Count; i++)
            {
                if(this.Actions[i] == ea) { return i; }
            }

            return this.Actions.Count;
        }
    }
}
