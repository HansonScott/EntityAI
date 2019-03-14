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
        public List<EntityAction> Actions;

        public Solution()
        {
            Actions = new List<EntityAction>();
        }

        public static Solution FindSolutionForNeed(EntityNeed need)
        {
            // lookup from database?

            // create from known actions?

            // if none found, return null
            return null;
        }
    }
}