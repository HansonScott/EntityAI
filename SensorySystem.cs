using System;
using System.Collections.Generic;
using System.Threading;
using static EntityAI.Sensor;

namespace EntityAI
{
    /// <summary>
    /// Represents the overall system responsible for gathering sensory input and generating items of interest
    /// </summary>
    public class SensorySystem
    {
        private Entity entity;
        public List<Sensor> sensors;
        private List<InputNeed> InputNeeds;

        public bool ShouldContinue = true;
        private int LoopDelay = 500;

        #region Constructor and Setup
        public SensorySystem(Entity entity)
        {
            this.entity = entity;

            CreateSensors();
            InputNeeds = new List<InputNeed>();
        }
        private void CreateSensors()
        {
            Array S_sensors = Enum.GetValues(typeof(SensorType));
            foreach(SensorType t in S_sensors)
            {
                this.sensors.Add(new Sensor(t));
            }
        }
        #endregion

        /// <summary>
        /// the parent thread function that loops to find inputs from sensors
        /// </summary>
        /// <returns></returns>
        public void Run()
        {
            while (ShouldContinue)
            {
                DateTime Start = DateTime.Now;

                // loop through all the senses of parent entity
                foreach (Sensor s in sensors)
                {
                    // capture any new inputs, add to list with timestamps
                    s.CaptureInput(this.entity);
                }

                DateTime End = DateTime.Now;

                // adjust timing?

                // wait until next loop
                double waittime = Math.Max(0, (LoopDelay - (End - Start).TotalMilliseconds));

                // slow down the loop
                Thread.Sleep((int)waittime);
            }
        }

        /// <summary>
        /// the accessor method to get the needs recently created.
        /// </summary>
        /// <returns></returns>
        public List<InputNeed> GetInputNeeds()
        {
            List<InputNeed> results = new List<InputNeed>();

            lock(this)
            {
                foreach (InputNeed n in this.InputNeeds)
                {
                    results.Add(new InputNeed(n));
                }

                this.InputNeeds.Clear();
            }

            return results;
        }
    }
}