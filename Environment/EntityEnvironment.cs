using EntityLogging;
using System;
using System.Collections.Generic;
using System.Threading;

namespace EntityAI
{
    public class EntityEnvironment
    {
        #region Custom Logging Events
        public delegate void LoggingHandler(object sender, EntityLogging.EntityLoggingEventArgs e);
        public event LoggingHandler OnLog;
        public event LoggingHandler OnTick;

        internal void RaiseLog(EntityLogging.EntityLog log)
        {
            // using this inline vs checking for null is more thread safe
            OnLog?.Invoke(this, new EntityLoggingEventArgs(log));
        }
        internal void Tick()
        {
            OnTick?.Invoke(this, new EntityLoggingEventArgs(new EntityLog("Tick")));
        }
        #endregion

        public List<EntityObject> Objects;

        public double SightDistance_Current;
        private const double SightDistance_Default = 500; // units of distance in meters

        private bool Continue = true;
        int delay = 1000; // 1 per second draw loop

        public EntityEnvironment()
        {
            Objects = new List<EntityObject>();

            SightDistance_Current = SightDistance_Default;
        }

        public void Run()
        {
            // reset the default, if the function is called, it is assummed to want to actually run.
            Continue = true;

            RaiseLog(new EntityLog("Starting environment..."));

            while(Continue)
            {
                DateTime start = DateTime.Now;

                // do stuff
                


                // adjust timing?
                //int delay = 100;

                DateTime end = DateTime.Now;

                // wait until next loop
                double waittime = Math.Max(0, (delay - (end - start).TotalMilliseconds));

                // tell anyone listening that we have finished a loop
                Tick();

                // slow down the loop
                Thread.Sleep((int)waittime);
            }

            RaiseLog(new EntityLog("Shutting down environment..."));
        }

        public void ShutDown()
        {
            this.Continue = false;
        }

        internal Position FindObject(Entity entity, EntityResource ear)
        {
            Position target = null;
            foreach (Sound s in entity.senses.SoundsCurrentlyHeard)
            {
                if (s.FootPrint == ear.Sound.FootPrint)
                {
                    // find a closer one, or just the first?
                    target = s.Origin;
                    break;
                }
            }
            if (target == null)
            {
                foreach (Sight s in entity.senses.SightsCurrentlySeen)
                {
                    if (s.FootPrint == ear.Appearance.FootPrint)
                    {
                        // find a closer one, or just the first?
                        target = s.Origin;
                        break;
                    }
                }
            }

            return target;
        }
    }
}