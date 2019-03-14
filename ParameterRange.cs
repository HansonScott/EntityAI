namespace EntityAI
{
    /// <summary>
    /// represents a detailed DTO for the value ranges for a core attribute
    /// </summary>
    public class ParameterRange
    {
        public double Optimal { get; set; }
        public double Min_SD1 { get; set; }
        public double Min_SD2 { get; set; }
        public double Max_SD1 { get; set; }
        public double Max_SD2 { get; set; }

        public ParameterRange()
        {
        }
    }
}