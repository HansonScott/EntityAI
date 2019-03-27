using System;
using System.Collections.Generic;
using System.Threading;

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
        private double LoopDelay = 1000;

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

                #region Acting
                // most likely, we will be in the middle of acting, so just continue
                if(this.CurrentState == ActionState.Acting)
                {
                    // perform the current action
                    PerformAction(CurrentAction);
                }
                #endregion
                #region Waiting
                // if we are not doing anything yet
                else if(this.CurrentState == ActionState.Waiting)
                {
                    EntityAction nextAction = null;
                    
                    this.entity.RaiseLog(new EntityLogging.EntityLog("Checking for actions..."));

                    // apply logic to logical next action in chain for a solution, or switch based on urgency, etc.
                    if(CurrentAction != null && CurrentAction.ActionState != EntityAction.EntityActionState.Blocked)
                    {
                        if(CurrentAction.ActionState == EntityAction.EntityActionState.Complete)
                        {
                            nextAction = CurrentAction.ParentSolution.GetNextAction(CurrentAction);

                            if (nextAction == null)
                            {
                                // then we may have finished the last action within the current solution, remove solution?
                                this.entity.CurrentSolutions.Remove(CurrentAction.ParentSolution);
                            }
                        }
                    }
                    else // no current action, or action is blocked - this would be our entry point.
                    {
                        // get the top of the stack
                        if(this.entity.actions.ActionQueue.Count > 0)
                        {
                            // find the right one to pick from the list, by priority or logical solution?
                            // for now, just get the next one.
                            foreach(EntityAction ea in this.entity.actions.ActionQueue)
                            {
                                if (ea.ActionState != EntityAction.EntityActionState.Blocked)
                                {
                                    nextAction = this.entity.actions.ActionQueue[0];
                                    break;
                                }
                            }
                        }
                        else // no actions in the queue
                        {
                            this.entity.RaiseLog(new EntityLogging.EntityLog("no actions in the queue, idling..."));                            
                        }
                    } // end no current actions

                    if (nextAction == null)
                    {
                        // then there is no next logical action, so pick from the queue
                        // loop through all the actions to determine highest priority
                        foreach (EntityAction a in this.ActionQueue)
                        {
                            // compare priorities?

                            // just assign this one.
                            nextAction = a;
                            break; // just default to the first item.
                        }
                    }

                    // we've found something to do, so clear the past action, then start doing the next action.
                    if(CurrentAction != null) { this.ActionQueue.Remove(CurrentAction); }
                    if(nextAction != null)
                    {
                        this.entity.RaiseLog(new EntityLogging.EntityLog("starting to act on " + nextAction.Description));
                        CurrentAction = nextAction;
                        this.CurrentState = ActionState.Acting;
                    }

                } // end if currently waiting
                #endregion

                DateTime End = DateTime.Now;

                // adjust timing?

                // wait until next loop
                double waittime = Math.Max(0, (LoopDelay - (End - Start).TotalMilliseconds));

                // slow down the loop
                Thread.Sleep((int)waittime);
            }
        }

        private void PerformAction(EntityAction currentAction)
        {
            switch(currentAction.ActionState)
            {
                case EntityAction.EntityActionState.New:
                    currentAction.Start(this.entity);
                    break;
                case EntityAction.EntityActionState.Active:
                    currentAction.Update(this.entity);
                    break;
                case EntityAction.EntityActionState.Blocked:
                    entity.RaiseLog(new EntityLogging.EntityLog("Cannot perform action, it is blocked."));
                    this.CurrentState = ActionState.Waiting;
                    break;
                case EntityAction.EntityActionState.Complete:
                    this.CurrentState = ActionState.Waiting;
                    break;
                default:
                    break;
            }
        }

        public void ShutDown()
        {
            ShouldContinue = false;
        }

        internal double GetAbilityValue(Ability.AbilityType aType)
        {
            foreach(Ability a in Abilities)
            {
                if(a.AType == aType)
                {
                    return a.CurrentValue;
                }
            }

            return 0;
        }
    }
}
