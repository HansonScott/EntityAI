namespace EntityAI
{
    /// <summary>
    /// represents the quantified negative impact of performing an action
    /// </summary>
    public class ActionCost
    {
        public enum ActionCostType
        {
            Unknown  = 0,
            Time = 1,
            Energy = 2,
            Sensory_Degradation = 3,
            Ability_Degradation = 4,
        }

        public ActionCostType CType;
        public object ActionCostTypeCategory;

        public double CostValue; // the amount of change to the cost type
        public double ChangeToCost; // in some cases, there might not be a cost, but there's a chance (breaking an item, for example)
    }
}