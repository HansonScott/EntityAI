using System;
using System.Collections.Generic;
using System.Threading;
using static EntityAI.Sensor;

namespace EntityAI
{
    public class SensorySystem
    {

        private Entity entity;
        public List<Sensor> sensors;

        public SensorySystem(Entity entity)
        {
            this.entity = entity;

            CreateSensors();
        }

        private void CreateSensors()
        {
            Array S_sensors = Enum.GetValues(typeof(SensorType));
            foreach(SensorType t in S_sensors)
            {
                this.sensors.Add(new Sensor(t));
            }
        }

        internal ThreadStart Run()
        {
            // loop through all the senses of parent entity

            // capture any new inputs, save to database with timestamps

            throw new NotImplementedException();
        }
    }
}