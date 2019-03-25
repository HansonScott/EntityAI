using System;

namespace EntityAI
{
    /// <summary>
    /// Represents a need relating to the core attributes
    /// </summary>
    public class CoreNeed: EntityNeed
    {
        public CoreAttribute Attribute;

        public double Change;

        public override string Name
        {
            get { return this.Attribute.Name; }
        }

        public CoreNeed (CoreAttribute A): base()
        {
            this.Attribute = A;
            SetUrgencyAndChange();
        }

        private void SetUrgencyAndChange()
        {
            CoreAttribute.ValueRelativeStatus s = Attribute.GetRelativeValueStatus();

            switch(s)
            {
                case CoreAttribute.ValueRelativeStatus.HighSD3:
                    Urgency = 3;
                    Change = -3;
                    break;
                case CoreAttribute.ValueRelativeStatus.HighSD2:
                    Urgency = 2;
                    Change = -2;
                    break;
                case CoreAttribute.ValueRelativeStatus.HighSD1:
                    Urgency = 1;
                    Change = -1;
                    break;
                case CoreAttribute.ValueRelativeStatus.LowSD3:
                    Urgency = 3;
                    Change = 3;
                    break;
                case CoreAttribute.ValueRelativeStatus.LowSD2:
                    Urgency = 2;
                    Change = 2;
                    break;
                case CoreAttribute.ValueRelativeStatus.LowSD1:
                    Urgency = 1;
                    Change = 1;
                    break;
                default:
                    break;
            }
        }
    }
}