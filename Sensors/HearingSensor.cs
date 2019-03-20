using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityAI
{
    public class HearingSensor: Sensor
    {
        public HearingSensor(SensorySystem parentSystem): base(parentSystem)
        {

        }

        internal override void CaptureInput(Entity entity)
        {
            // sort of cheating in the code to send the environment information, but logical for an AI
            entity.senses.SoundsCurrentlyHeard = GetAllSoundsHeard(entity.CurrentEnvironment.Sounds, entity.PositionCurrent);
        }

        private List<Sound> GetAllSoundsHeard(List<Sound> sounds, Position p)
        {
            List<Sound> results = new List<Sound>();

            // capture ambient volume.
            double ambiantLoudness = 5; // 5 for entity noises such as breathing, heart beat, etc.

            // come back to this later, for now just use the default.
            //foreach (Sound s in sounds)
            //{
            //    ambiantLoudness = Math.Max(ambiantLoudness, s.Loudness);
            //}


            foreach (Sound s in sounds)
            {
                if (s.IsHeard(this.Effectiveness_Current, p, ambiantLoudness))
                {
                    base.parentSystem.entity.RaiseLog(new EntityLogging.EntityLog("I hear " + s.Description, System.Diagnostics.TraceLevel.Verbose));

                    results.Add(s);
                }
            }
            if (results.Count > 0)
            {
                return results;
            }
            else
            {
                return null;
            }
        }
    }
}
