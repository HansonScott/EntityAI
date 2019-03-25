using System;
using System.Collections.Generic;

namespace EntityAI
{
    /// <summary>
    /// represents a complex set of actions that bring about resolution of a need.
    /// </summary>
    public class Solution
    {
        public EntityNeed NeedFulfilled;
        public SolutionBenefit Benefit;
        public SolutionCost Cost;
        public List<EntityAction> Actions;

        public Solution()
        {
            Actions = new List<EntityAction>();
        }

        public static Solution FindSolutionForNeed(EntityNeed need, Entity CurrentEntity)
        {
            // lookup from database?

            // create from known actions?

            // if none found, return null

            #region hard code the prototype - water
            Solution result = new Solution();
            result.NeedFulfilled = need;

            CoreNeed cn = (need as CoreNeed);
            Type t = cn.Attribute.CType.GetType();
            result.Benefit = new SolutionBenefit(t, cn.Attribute.CType, 0.3);

            // the cost of drinking water: energy
            result.Cost = new SolutionCost(typeof(EntityAI.CoreAttribute.CoreAttributeType), EntityAI.CoreAttribute.CoreAttributeType.Energy, 0.1);

            // the actions to achive it:

            // find water - a separate action?
            Position target = null;
            foreach(Sound s in CurrentEntity.senses.SoundsCurrentlyHeard)
            {
                if(s.FootPrint == Sound.RecognitionFootPrint.Water)
                {
                    target = s.Origin;
                    break;
                }
            }
            if(target == null)
            {
                foreach(Sight s in CurrentEntity.senses.SightsCurrentlySeen)
                {
                    if(s.FootPrint == Sight.RecognitionFootPrint.Water)
                    {
                        target = s.Origin;
                        break;
                    }
                }
            }

            // go to water
            result.Actions.Add(new EntityAction(new Ability(Ability.AbilityType.Walk), target, null));

            // gather the water in a container
            result.Actions.Add(new EntityAction(new Ability(Ability.AbilityType.Use), new EntityResource(EntityResource.ResourceType.Water) ,new EntityResource(EntityResource.ResourceType.Container)));

            // consume the water
            result.Actions.Add(new EntityAction(new Ability(Ability.AbilityType.Consume), new EntityResource(EntityResource.ResourceType.Water)));
            #endregion

            return result;
        }
    }
}