namespace EntityAI
{
    /// <summary>
    /// Represents an item in need of attention relating to sensory input
    /// </summary>
    public class InputNeed : EntityNeed
    {
        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="n"></param>
        public InputNeed(InputNeed source)
        {
            base.OriginationWhen = source.OriginationWhen;
            base.Urgency = source.Urgency;
        }
    }
}