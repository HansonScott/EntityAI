using System;
using System.Text;

namespace EntityAI
{
    public class EntityObject
    {
        public Sound Sound;
        public Sight Appearance;
        public Position Position;
        public double Quantity = 1.0;

        public bool Visibility
        {
            get; set;
        }
        public string Name
        {
            get
            {
                if(Appearance != null && Appearance.FootPrint != Sight.RecognitionFootPrint.Unknown )
                {
                    return Appearance.Description;
                }
                else if(Sound != null && Sound.FootPrint != Sound.RecognitionFootPrint.Unknown)
                {
                    return Sound.Description;
                }
                else
                {
                    return "unknown";
                }
            }
        }
        public string Description
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                // appearance
                sb.Append("Looks like ");
                string a = Enum.GetName(typeof(Sight.RecognitionFootPrint), Appearance.FootPrint).ToLower();

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
                if(Sound.FootPrint != EntityAI.Sound.RecognitionFootPrint.Unknown)
                {
                    sb.Append(", and sounds like ");
                    string s = Enum.GetName(typeof(Sound.RecognitionFootPrint), Sound.FootPrint).ToLower();
                    sb.Append(s);

                }

                return sb.ToString();
            }
        }

        public EntityObject(Sight.RecognitionFootPrint appearance, Sound.RecognitionFootPrint sound, Position P)
        {
            this.Appearance = new Sight(appearance, P);
            this.Sound = new Sound(sound, 50, P);

            this.Position = P;

            // for now, just assume.
            this.Visibility = true;
        }
    }
}