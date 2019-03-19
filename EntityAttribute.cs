namespace EntityAI
{
    public class EntityAttribute
    {
        public enum ValueRelativeStatus
        {
            LowSD3 = 0,
            LowSD2 = 1,
            LowSD1 = 2,
            Mid = 3,
            HighSD1 = 4,
            HighSD2 = 5,
            HighSD3 = 6,
        }

        public double Importance;
        public double Efficiency; // %
        public double Effectiveness; // %?

        public ParameterRange param = new ParameterRange();

        public double Value_Mid;
        public double Value_HighSD1;
        public double Value_HighSD2;
        public double Value_HighSD3;
        public double Value_LowSD1;
        public double Value_LowSD2;
        public double Value_LowSD3;
        public double CurrentValue;

        internal ValueRelativeStatus GetRelativeValueStatus()
        {
            if (CurrentValue >= Value_HighSD3) { return ValueRelativeStatus.HighSD3; }
            else if (CurrentValue >= Value_HighSD2) { return ValueRelativeStatus.HighSD2; }
            else if (CurrentValue >= Value_HighSD1) { return ValueRelativeStatus.HighSD1; }
            else if (CurrentValue <= Value_LowSD3) { return ValueRelativeStatus.LowSD3; }
            else if (CurrentValue <= Value_LowSD2) { return ValueRelativeStatus.LowSD2; }
            else if (CurrentValue <= Value_LowSD1) { return ValueRelativeStatus.LowSD1; }
            else { return ValueRelativeStatus.Mid; }
        }
        internal bool IsInNeed(ValueRelativeStatus s)
        {
            return (s != ValueRelativeStatus.Mid);
        }

        internal bool HasOpportunity()
        {
            return (Efficiency < 100 || Effectiveness < 100);
        }
    }
}