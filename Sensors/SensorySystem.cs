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
        #region Fields and Properties
        internal Entity entity;
        public List<Sensor> sensors;

        public bool ShouldContinue = true;
        private int LoopDelay = 500;

        public List<Sound> SoundsCurrentlyHeard;
        public List<Sight> SightsCurrentlySeen;

        public SightSensor Sensor_Sight;
        public HearingSensor Sensor_Hearing;
        public PainSensor Sensor_Pain;
        public SmellSensor Sensor_Smell;
        public TasteSensor Sensor_Taste;
        public TemperatureSensor Sensor_Temp;
        public TouchSensor Sensor_Touch;
        #endregion

        #region Constructor and Setup
        public SensorySystem(Entity entity)
        {
            this.entity = entity;

            CreateSensors();
        }
        private void CreateSensors()
        {
            this.sensors = new List<Sensor>();

            Sensor_Sight = new SightSensor(this);
            this.sensors.Add(Sensor_Sight);
            Sensor_Hearing = new HearingSensor(this);
            this.sensors.Add(Sensor_Hearing);
            //this.Sensor_Pain = new PainSensor(this);
            //this.sensors.Add(Sensor_Pain);
            //this.Sensor_Smell = new SmellSensor(this);
            //this.sensors.Add(Sensor_Smell);
            //this.Sensor_Taste = new TasteSensor(this);
            //this.sensors.Add(Sensor_Taste);
            //this.Sensor_Temp = new TemperatureSensor(this);
            //this.sensors.Add(Sensor_Temp);
            //this.Sensor_Touch = new TouchSensor(this);
            //this.sensors.Add(Sensor_Touch);

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
    }
}