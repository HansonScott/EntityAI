using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityAI
{
    public class HearingSensor: Sensor
    {
        public override string Name
        {
            get { return "Hearing"; }
        }

        public HearingSensor(SensorySystem parentSystem): base(parentSystem)
        {

        }

        internal override void CaptureInput(Entity entity)
        {
            // sort of cheating in the code to send the environment information, but logical for an AI
            entity.senses.SoundsCurrentlyHeard.AddRange(GetAllNewSoundsHeard(entity.CurrentEnvironment.Sounds, entity.PositionCurrent));
        }

        private List<Sound> GetAllNewSoundsHeard(List<Sound> sounds, Position p)
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

                    // now, before adding, check that we don't already have it.
                    if (!SoundCurrentlyHeard(s))
                    {
                        base.parentSystem.entity.RaiseLog(new EntityLogging.EntityLog("I hear something: " + s.Description, System.Diagnostics.TraceLevel.Verbose));
                        results.Add(s);
                    }
                }
            }

            return results;
        }

        private bool SoundCurrentlyHeard(Sound s)
        {
            foreach(Sound cs in this.parentSystem.SoundsCurrentlyHeard)
            {
                if (cs.Description == s.Description &&
                    cs.Origin == s.Origin)
                    return true;
            }

            return false;
        }
    }
}
