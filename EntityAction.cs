namespace EntityAI
{
    /// <summary>
    /// Represents a specific proactive change performed by the entity
    /// </summary>
    public class EntityAction
    {
        public Ability ability;
        public ActionCost Cost;
        public object Target = null;
        public object Item = null;
        public object Environment = null;

        public Solution ParentSolution = null;
        
        /// <summary>
        /// represents conducting an action with the assumption there is no need for an item or target
        /// </summary>
        /// <param name="ability"></param>
        public EntityAction(Ability ability)
        {
        }

        /// <summary>
        /// represents conducting an action with the assumption there is an item involved
        /// </summary>
        /// <param name="ability"></param>
        /// <param name="Item"></param>
        public EntityAction(Ability ability, object Item)
        {
        }

        /// <summary>
        ///  respresents conducting an action with an item on a target
        /// </summary>
        /// <param name="ability"></param>
        /// <param name="Target"></param>
        /// <param name="Item"></param>
        public EntityAction(Ability ability, object Target, object Item)
        {
        }
    }
}
