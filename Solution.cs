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

                        // consume the water
                        // NOTE: split out to own lines, so we can add the cost to each action before adding it.
                        result.Actions.Add(new EntityAction(result, new Ability(Ability.AbilityType.Consume),
                                                                    new EntityResource(EntityResource.ResourceType.Water, CurrentEntity.PositionCurrent)));
                        return result;
                    default:
                        break;
                }
            }
            else if(need is ResourceNeed)
            {
                ResourceNeed rn = need as ResourceNeed;
                Solution result = new Solution();
                switch (rn.Resource.RType)
                {
                    case EntityResource.ResourceType.Container:
                    // gather the water in a container
                    // NOTE: split out to own lines, so we can add the cost to each action before adding it.
                    result.Actions.Add(new EntityAction(result, new Ability(Ability.AbilityType.Pick_Up),
                                                                new EntityResource(EntityResource.ResourceType.Container, CurrentEntity.PositionCurrent)));
                        return result;
                    case EntityResource.ResourceType.Water:
                        result.Actions.Add(new EntityAction(result, new Ability(Ability.AbilityType.Pick_Up),
                                                                    new EntityResource(EntityResource.ResourceType.Water, CurrentEntity.PositionCurrent),
                                                                    new EntityResource(EntityResource.ResourceType.Container, CurrentEntity.PositionCurrent)));
                        return result;
                    default:
                        break;
                }
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
    }
}
