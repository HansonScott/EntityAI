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
            throw new NotImplementedException();
        }
    }
}