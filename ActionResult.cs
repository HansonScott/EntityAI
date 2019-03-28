using System.Text;

namespace EntityAI
{
    public class ActionResult
    {
        public string Description
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                if(ResultValue < 0) { sb.Append("-"); }

                if(ActionResultTypeObject is CoreAttribute)
                {
                    sb.Append((ActionResultTypeObject as CoreAttribute).Name);
                }
                else if(ActionResultTypeObject is Sensor)
                {
                    sb.Append((ActionResultTypeObject as Sensor).Name);
                }

                return sb.ToString();
            }
        }

        public object ActionResultTypeObject;

        public double ResultValue; // the amount of change to the resulting object
        public double ChanceToResult; // in some cases, there might not be a result value change, but there's a chance (breaking an item, for example)
    }
}