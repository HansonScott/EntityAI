using System;
using System.Threading;

namespace EntityAI
{
    internal class SensorySystem
    {
        private Entity entity;

        public SensorySystem(Entity entity)
        {
            this.entity = entity;
        }

        internal ThreadStart Run()
        {
            // loop through all the senses of parent entity

            // capture any new inputs, save to database with timestamps

            throw new NotImplementedException();
        }
    }
}