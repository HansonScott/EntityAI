namespace EntityAI
{
    public class ParameterRange
    {
        private CoreAttribute.CoreAttributeType CType;

        public ParameterRange(CoreAttribute.CoreAttributeType value)
        {
            this.CType = value;
        }

        public double Optimal { get; set; }
        public double Min_SD1 { get; set; }
        public double Min_SD2 { get; set; }
        public double Max_SD1 { get; set; }
        public double Max_SD2 { get; set; }
    }
}