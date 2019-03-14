using System.Collections.Generic;

namespace EntityAI
{
    /// <summary>
    /// represents the system of the entity that performs actions either automatically, reactively, or planned
    /// </summary>
    public class ActionSystem
    {
        private Entity entity;
        public List<Ability> Abilities;
        public List<EntityAction> ActionQueue;

        public ActionSystem(Entity entity)
        {
            this.entity = entity;

            Abilities = new List<Ability>();
            ActionQueue = new List<EntityAction>();
        }

        public void AddAction(EntityAction A)
        {
            ActionQueue.Add(A);
        }

        public void Run()
        {
            // go through list of queued actions

            // perform them
        }
    }
}