using System.Collections.Generic;

namespace EntityAI
{
    /// <summary>
    /// represents the system of the entity that performs actions either automatically, reactively, or planned
    /// </summary>
    public class ActionSystem
    {
        public enum ActionState
        {
            Waiting = 0,
            Acting = 1,
        }
        
        private Entity entity;
        public List<Ability> Abilities;
        public List<EntityAction> ActionQueue;
        EntityAction CurrentAction;

        private bool ShouldContinue;

        public ActionState CurrentState;
        
        public ActionSystem(Entity entity)
        {
            this.entity = entity;

            Abilities = new List<Ability>();
            ActionQueue = new List<EntityAction>();
            
            // default to a waiting state
            this.CurrentState = ActionState.Waiting;            
        }

        public void AddAction(EntityAction A)
        {
            ActionQueue.Add(A);
        }

        public void Run()
        {
            // when the function is first called, it can ba ssumed the loop should actually be ran
            ShouldContinue = true;

            while (ShouldContinue)
            {
                DateTime Start = DateTime.Now;

                // most likely, we will be in the middle of acting, so just continue
                if(this.CurrentState == ActionState.Acting)
                {
                    // perform the current action
                }
                // if we are not doing anything yet
                else if(this.CurrentState == ActionState.Waiting)
                {
                    EntityAction nextAction = null;
                    
                    this.entity.RaiseLog(new EntityLogging.EntityLog("Checking for actions..."));

                    // apply logic to logical next action in chain for a solution, or switch based on urgency, etc.
                    if(CurrentAction != null)
                    {
                        nextAction = CurrentAction.ParentSolution.GetNextAction(CurrentAction);

                        if(nextAction == null)
                        {
                            // then we may have finished the last action within the current solution, remove solution?
                        }
                    }
                    
                    if(nextAction == null)
                    {
                        // then there is no next logical action, so pick from the queue
                        // loop through all the actions to determine highest priority
                        foreach (Action a in this.ActionQueue)
                        {
                        }
                    }
                } // end if currently waiting

                DateTime End = DateTime.Now;

                // adjust timing?

                // wait until next loop
                double waittime = Math.Max(0, (LoopDelay - (End - Start).TotalMilliseconds));

                // slow down the loop
                Thread.Sleep((int)waittime);
            }
        }
        
        public void ShutDown()
        {
            ShouldContinue = false;
        }
    }
}
