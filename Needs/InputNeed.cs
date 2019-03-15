namespace EntityAI
{
    /// <summary>
    /// Represents an item in need of attention relating to sensory input
    /// </summary>
    public class InputNeed : EntityNeed
    {
        public Sensor SourceSensor;
        public Sound SourceSound;

        public InputNeed(Sensor s)
        {
            this.SourceSensor = s;
        }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        /// <param name="n"></param>
        public InputNeed(InputNeed source)
        {
            base.OriginationWhen = source.OriginationWhen;
            base.Urgency = source.Urgency;
            this.SourceSensor = source.SourceSensor;
        }

        public InputNeed(Sound S)
        {
            this.SourceSound = S;
        }
    }
}