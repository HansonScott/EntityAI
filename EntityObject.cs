using System;
using System.Text;

namespace EntityAI
{
    public class EntityObject
    {
        public Sound.RecognitionFootPrint Sound;
        public Sight.RecognitionFootPrint Appearance;
        public Position Position;
        public double Quantity = 1.0;

        public bool Visibility
        {
            get; set;
        }
        public string Description
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                // appearance
                sb.Append("Looks like ");
                string a = Enum.GetName(typeof(Sight.RecognitionFootPrint), Appearance).ToLower();

                // collective
                if (a == "water") // NOTE: future: get metadata from the enum item to see if it is a collective, or...
                {
                    // don't add a prefix...
                }
                // specific quantity
                else if(this.Quantity != 1.0)
                {
                    sb.Append(Quantity);
                }
                // an...
                else if(a.StartsWith("a") || a.StartsWith("e") ||
                   a.StartsWith("o") || a.StartsWith("u") ||
                   a.StartsWith("h"))
                {
                    sb.Append("an ");
                }
                else // a...
                {
                    sb.Append("a ");
                }

                sb.Append(a);

                // sound
                if(Sound != EntityAI.Sound.RecognitionFootPrint.Unknown)
                {
                    sb.Append(", and sounds like ");
                    string s = Enum.GetName(typeof(Sound.RecognitionFootPrint), Sound).ToLower();
                    sb.Append(s);

                }

                return sb.ToString();
            }
        }

        public EntityObject(Sight.RecognitionFootPrint appearance, Sound.RecognitionFootPrint sound, Position P)
        {
            this.Appearance = appearance;
            this.Sound = sound;

            this.Position = P;

            // for now, just assume.
            this.Visibility = true;
        }
    }
}