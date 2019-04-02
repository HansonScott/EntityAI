using EntityLogging;
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
            this.LoopDelay = entity.LoopDelay;
            Abilities = PopulateAbilities();
            ActionQueue = new List<EntityAction>();
            
            // default to a waiting state
            this.CurrentState = ActionState.Waiting;            
        }

        private List<Ability> PopulateAbilities()
        {
            List<Ability>  result = new List<Ability>();
            string[] ATypes = Enum.GetNames(typeof(Ability.AbilityType));
            foreach (string a in ATypes)
            {
                result.Add(new Ability((Ability.AbilityType)Enum.Parse(typeof(Ability.AbilityType), a)));
            }
            return result;
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

                EvaluateAnyBlockedActions();

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
                    
                    this.entity.RaiseLog("Checking for actions...");

                    // apply logic to logical next action in chain for a solution, or switch based on urgency, etc.
                    if(CurrentAction != null && CurrentAction.ActionState != EntityAction.EntityActionState.Blocked)
                    {
                        if(CurrentAction.ActionState == EntityAction.EntityActionState.Complete)
                        {
                            if(CurrentAction.ParentSolution != null)
                            {
                                nextAction = CurrentAction.ParentSolution.GetNextAction(CurrentAction);
                            }

                            if (nextAction == null)
                            {
                                // then we may have finished the last action within the current solution, remove solution?
                                CurrentAction.ParentSolution.SolutionState = Solution.EntitySolutionState.completed;
                                this.entity.RaiseLog("Completed solution: " + CurrentAction.ParentSolution.Description);
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
                            this.entity.RaiseLog("no actions in the queue, idling...");
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
                            if(a != CurrentAction && a.ActionState != EntityAction.EntityActionState.Blocked)
                            {
                                nextAction = a;
                                break; // just default to the first item.
                            }
                        }
                    }

                    // if we get to here, then we're done with this action, remove it from the queue
                    if (CurrentAction != null)
                    {
                        this.ActionQueue.Remove(CurrentAction);
                        CurrentAction = null;
                    }

                    // if we've found something to do, then start doing the next action.
                    if (nextAction != null)
                    {
                        this.entity.RaiseLog("next planned action: " + nextAction.Description);
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

        internal bool HaveBlockedActions()
        {
            foreach(EntityAction ea in this.ActionQueue)
            {
                if(ea.ActionState == EntityAction.EntityActionState.Blocked) { return true; }
            }

            return false;
        }

        private void EvaluateAnyBlockedActions()
        {
            for(int i = 0; i < this.ActionQueue.Count; i++)
            {
                EntityAction ea = ActionQueue[i];

                if (ea.ActionState == EntityAction.EntityActionState.Blocked)
                {
                    this.entity.RaiseLog($"evaluating a blocked action: {ea.Description}...");

                    // actions are blocked for a few reasons, check the assumptions of each reason.
                    ea.EvaluateForBlockedStatus(this.entity);
                }
            }
        }

        private void PerformAction(EntityAction currentAction)
        {
            switch(currentAction.ActionState)
            {
                case EntityAction.EntityActionState.New:
                    this.CurrentState = ActionState.Acting;
                    currentAction.Start(this.entity);
                    break;
                case EntityAction.EntityActionState.Active:
                    this.CurrentState = ActionState.Acting;
                    currentAction.Update(this.entity);
                    break;
                case EntityAction.EntityActionState.Blocked:
                    entity.RaiseLog($"Cannot {currentAction.Description}, it is blocked.");
                    this.CurrentState = ActionState.Waiting;
                    break;
                case EntityAction.EntityActionState.Complete:
                    this.CurrentState = ActionState.Waiting;
                    break;
                default:
                    break;
            }
        }

        internal int GetIndexOfAction(EntityAction ea)
        {
            for(int i = 0; i < this.ActionQueue.Count; i++)
            {
                if(this.ActionQueue[i] == ea) { return i; }
            }

            return this.ActionQueue.Count;
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

        public void EvaluateBlockedActions()
        {
            if (!HaveBlockedActions()) { return; }

            if(CurrentAction?.ActionState == EntityAction.EntityActionState.Blocked)
            {
                this.entity.RaiseLog("current action is blocked, discontinuing current action.");
                // then stop doing this action
                CurrentAction = null;

                // in fact, stop doing any actions just at the moment, to allow for re-evaluating.
                this.CurrentState = ActionState.Waiting;
            }

                for (int i = 0; i < ActionQueue.Count; i++)
                {
                    EntityAction ea = ActionQueue[i];

                    if (ea.ActionState == EntityAction.EntityActionState.Blocked)
                    {
                        ResolveBlockedAction(ea);
                    }
                }
        }

        private void ResolveBlockedAction(EntityAction ea)
        {
            this.entity.RaiseLog("Looking to resolve blocked action: " + ea.Description);

            // figure out what type of need we have.
            // ability = abilityNeed
            // target or item == resourceNeed
            #region Check for ability
            double val = GetAbilityValue(ea.ability.AType);

            // if the value of this ability is 0, the entity cannot perform this.
            if (val == 0)
            {
                this.entity.CurrentNeeds.Add(new AbilityNeed(ea.ability));
            }
            #endregion

            #region Check Target
            if (ea.Target != null &&
                ea.Target is EntityResource)
            {
                EntityResource ear = (ea.Target as EntityResource);
                // how do we track if a target resource needs to be in the inventory?
                // for example to chop a tree, it might just need to be a nearby target, not an inventory item...

                // if the target is supposed to be in the inventory, check the inventory
                if (!this.entity.Inventory.HaveResource(ear.RType))
                {
                    // we don't have it in our inventory, see if we have it available from our senses...
                    Position target = null;
                    foreach (Sound s in this.entity.senses.SoundsCurrentlyHeard)
                    {
                        if (s.FootPrint == ear.Sound.FootPrint)
                        {
                            target = s.Origin;
                            break;
                        }
                    }
                    if (target == null)
                    {
                        foreach (Sight s in this.entity.senses.SightsCurrentlySeen)
                        {
                            if (s.FootPrint == ear.Appearance.FootPrint)
                            {
                                target = s.Origin;
                                break;
                            }
                        }
                    }

                    if (target == null)// we don't know of the resource within the environment
                    {
                        this.entity.RaiseLog("cannot find " + ear.RType.ToString());
                        return;
                    }

                    this.entity.RaiseLog("found the needed target resource, seeing if we need to travel to it, or have a container...");

                    // if we're too far away from our needed resource, add a walk action
                    if(target.DistanceFrom(this.entity.PositionCurrent) > 5)
                    {
                        this.entity.RaiseLog("the target resource that we need is too far to reach, need to walk to it.");
                        EntityAction newAction = (new EntityAction(ea.ParentSolution, new Ability(Ability.AbilityType.Walk), target, null));

                        int aqi = GetIndexOfAction(ea);
                        // insert this new action into the same slot in the action queue
                        ActionQueue.Insert(aqi, newAction);

                        // add this new action to the blocked action's solution
                        // by inerting at this index, this new action will go right before the blocked one
                        int i = ea.ParentSolution.GetIndexOfAction(ea);
                        //ea.ParentSolution.Actions.Insert(i, newAction);

                        ea.ActionState = EntityAction.EntityActionState.New;
                    }
                    else if(ea.ability.AType == Ability.AbilityType.Consume)
                    {
                        this.entity.RaiseLog("the target resource is within reach, I need to pick it up before I can consume it.");
                        EntityAction newAction = null;
                        if(ear.RequiresContainer())
                        {
                            newAction = (new EntityAction(ea.ParentSolution, new Ability(Ability.AbilityType.Pick_Up), ear, new EntityResource(EntityResource.ResourceType.Container, target)));
                        }
                        else
                        {
                            newAction = (new EntityAction(ea.ParentSolution, new Ability(Ability.AbilityType.Pick_Up), ear, null));
                        }

                        int aqi = GetIndexOfAction(ea);
                        // insert this new action into the same slot in the action queue
                        ActionQueue.Insert(aqi, newAction);

                        // add this new action to the blocked action's solution
                        // by inerting at this index, this new action will go right before the blocked one
                        int i = ea.ParentSolution.GetIndexOfAction(ea);
                        //ea.ParentSolution.Actions.Insert(i, newAction);

                        ea.ActionState = EntityAction.EntityActionState.New;
                    }

                    // if we need to pick it up, but it requires a container, then make sure to add an action to do so.
                    if (ear.RequiresContainer() &&
                        !this.entity.Inventory.HaveResource(EntityResource.ResourceType.Container))
                    {
                        this.entity.RaiseLog("the target resource that we need requires a container, and we don't have one in our inventory, so adding a need.");
                        ResourceNeed n = new ResourceNeed(EntityResource.ResourceType.Container, 1, null);

                        if (!NeedExists(n))
                        {
                            // default to adding this need to the top of the list.
                            this.entity.CurrentNeeds.Insert(0,n);
                        }
                    }

                } // end if not in inventory
            }
            else { } // check other types of targets than resources (unreachable position, such as target doesn't exist?)
            #endregion

            #region Check Item
            if (ea.Item != null &&
                ea.Item is EntityResource)
            {
                this.entity.RaiseLog("blocked action has an entity resource item needed, checking to see if we have it or can get to it.");

                EntityResource eai = (ea.Item as EntityResource);
                if (!this.entity.Inventory.HaveResource(eai.RType))
                {
                    // we don't have it in our inventory, see if we have it available from our senses...
                    Position target = null;
                    foreach (Sound s in this.entity.senses.SoundsCurrentlyHeard)
                    {
                        if (s.FootPrint == eai.Sound.FootPrint)
                        {
                            target = s.Origin;
                            break;
                        }
                    }
                    if (target == null)
                    {
                        foreach (Sight s in this.entity.senses.SightsCurrentlySeen)
                        {
                            if (s.FootPrint == eai.Appearance.FootPrint)
                            {
                                target = s.Origin;
                                break;
                            }
                        }
                    }

                    if (target == null)// we don't know of the resource within the environment
                    {
                        this.entity.RaiseLog("I don't have it, nor know where to find: " + eai.Name);
                        return;
                    }


                    this.entity.RaiseLog($"found {eai.Name}, so adding an action to walk to it.");
                    EntityAction newAction = (new EntityAction(ea.ParentSolution, new Ability(Ability.AbilityType.Walk), target, null));

                    int aqi = GetIndexOfAction(ea);
                    // insert this new action into the same slot in the action queue
                    ActionQueue.Insert(aqi, newAction);

                    //ea.ParentSolution.Actions.Insert(0, (new EntityAction(ea.ParentSolution, new Ability(Ability.AbilityType.Walk), target, null)));
                    ea.ActionState = EntityAction.EntityActionState.New;
                    // reset the state of the solution to reload to the action queue
                    //ea.ParentSolution.SolutionState = Solution.EntitySolutionState.created;
                }
            }
            #endregion
        }

        private bool NeedExists(ResourceNeed rn)
        {
            foreach(EntityNeed en in this.entity.CurrentNeeds)
            {
                if(!(en is ResourceNeed)) { continue; }
                ResourceNeed existingRN = en as ResourceNeed;
                
                if(existingRN.Resource.RType == rn.Resource.RType) { return true; }
            }

            return false;
        }
    }
}
