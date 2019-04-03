using System;
using System.Collections.Generic;
using System.Text;

namespace EntityAI
{
    /// <summary>
    /// Represents a specific proactive change performed by the entity
    /// </summary>
    public class EntityAction
    {
        public Ability ability;

        public List<ActionResult> Results;

        public object Target = null;
        public object Item = null;
        public object Environment = null;

        public Solution ParentSolution = null;
        public DateTime ActionStartedWhen;
        public TimeSpan DurationElapsed;
        public TimeSpan DurationRequired;

        public enum EntityActionState
        {
            New = 0,
            Active = 1,
            Blocked = 10,
            Remove = 15,
            Complete = 20,
        }
        public EntityActionState ActionState = EntityActionState.New;

        public string Description
        {
            get
            {
                // verb
                StringBuilder result = new StringBuilder(this.ability.AType.ToString());

                if(this.Target != null)
                {
                    result.Append(" ");

                    if (this.Target is Position)
                    {
                        result.Append("to a different position.");
                    }
                    else if(this.Target is EntityResource)
                    {
                        result.Append((this.Target as EntityResource).RType.ToString());
                    }
                }

                if(this.Item != null)
                {
                    result.Append(", using ");

                    if(this.Item is EntityResource)
                    {
                        result.Append((this.Item as EntityResource).RType.ToString());
                    }
                }

                if(this.Results != null && this.Results.Count > 0)
                {
                    result.Append(" with results of ");
                    bool first = true;
                    foreach(ActionResult ar in this.Results)
                    {
                        if(!first)
                        {
                            result.Append(", ");
                        }
                        result.Append(ar.Description);
                        first = false;
                    }
                }

                return result.ToString();
            }
        }

        /// <summary>
        /// represents conducting an action with the assumption there is no need for an item or target
        /// </summary>
        /// <param name="ability"></param>
        public EntityAction(Solution parent, Ability ability): this(parent, ability, null, null){}

        /// <summary>
        /// represents conducting an action with the assumption there is an item involved
        /// </summary>
        /// <param name="ability"></param>
        /// <param name="Item"></param>
        public EntityAction(Solution parent, Ability ability, object Item): this(parent, ability, Item, null){}

        /// <summary>
        ///  respresents conducting an action with an item on a target
        /// </summary>
        /// <param name="ability"></param>
        /// <param name="Target"></param>
        /// <param name="Item"></param>
        public EntityAction(Solution parent, Ability ability, object Target, object Item)
        {
            this.ParentSolution = parent;
            this.ability = ability;
            this.Target = Target;
            this.Item = Item;
        }

        public void Start(Entity entity)
        {
            EvaluateForBlockedStatus(entity);

            if(this.ActionState == EntityActionState.Blocked) { return; }
            else
            {
                entity.RaiseLog("starting: " + this.Description);

                if(this.ParentSolution != null)
                {
                    if(this.ParentSolution.SolutionState == Solution.EntitySolutionState.planned)
                    {
                        this.ParentSolution.SolutionState = Solution.EntitySolutionState.active;
                    }
                }

                this.ActionState = EntityActionState.Active;

                // proceed
                Update(entity);
            }
        }

        public void Update(Entity entity)
        {
            entity.RaiseLog("continuing: " + this.Description);

            switch (ability.AType)
            {
                //case Ability.AbilityType.Carry:
                //    break;
                case Ability.AbilityType.Consume:
                    // if we have waited long enough...
                    if (DateTime.Now > ActionStartedWhen + DurationRequired)
                    {
                        // result: apply effect of the consumption based on the item consumed.
                        switch((this.Target as EntityResource).RType)
                        {
                            case EntityResource.ResourceType.Water:
                                foreach(CoreAttribute ca in entity.coreAttributes)
                                {
                                    // primary result - adjust need for consumption
                                    if (ca.CType == CoreAttribute.CoreAttributeType.Water)
                                    {
                                        ca.CurrentValue += 0.3; // amount drunk is variable? - should this be a rate over time, or a set unit of quanity and result?
                                    }

                                    // secondary result - deplete energy
                                    if(ca.CType == CoreAttribute.CoreAttributeType.Energy)
                                    {
                                        ca.CurrentValue -= 0.01;
                                    }
                                }
                                break;
                            default:
                                break;
                        }

                        // result: remove resource from hands/container
                        entity.Inventory.RemoveResource((this.Target as EntityResource).RType);


                        // report completion
                        this.ActionState = EntityActionState.Complete;
                    }
                    else
                    {
                        // let the timespan continue to elapse. - update progress bar or something?
                    }
                    break;
                //case Ability.AbilityType.Crouch:
                //    break;
                //case Ability.AbilityType.Duck:
                //    break;
                //case Ability.AbilityType.Sit:
                //    break;
                //case Ability.AbilityType.Stand:
                //    break;
                //case Ability.AbilityType.Jump:
                //    break;
                //case Ability.AbilityType.Lie_Down:
                //    break;
                //case Ability.AbilityType.Doff:
                //    break;
                //case Ability.AbilityType.Don:
                //    break;
                //case Ability.AbilityType.Drop:
                //    break;
                case Ability.AbilityType.Pick_Up:
                    // if we've spent long enough doing this, then accomplish it
                    if (DateTime.Now > ActionStartedWhen + DurationRequired)
                    {
                        if (this.Target is EntityResource)
                        {
                            if (!(this.Target as EntityResource).RequiresContainer() || entity.Inventory.HaveResource(EntityResource.ResourceType.Container))
                            {
                                double distanceToTarget = (this.Target as EntityResource).Position.DistanceFrom(entity.PositionCurrent);
                                if (distanceToTarget > 5)
                                {
                                    entity.RaiseLog("can't pick up the resource, it is too far away.");
                                    this.ActionState = EntityActionState.Blocked;
                                    return;
                                }

                                // NOTE: this does not nest the target into the container item then into the inventory, 
                                // for now, just add the target to the inventory directly, and assume the container is in use.
                                entity.Inventory.AddResource(this.Target as EntityResource);
                                this.ActionState = EntityActionState.Complete;

                                // remove this item from the environment? - decrement quantity?
                                entity.CurrentEnvironment.Objects.Remove(this.Target as EntityResource);

                                return;
                            }
                            else // does require a container
                            {
                                if (!entity.Inventory.HaveResource(EntityResource.ResourceType.Container))
                                {
                                    entity.RaiseLog("this action requires a container, but we don't have one in our inventory, setting this action to blocked status.");
                                    this.ActionState = EntityActionState.Blocked;
                                    return;
                                }
                            }
                        }
                        else // the target is not a resource
                        {
                            entity.RaiseLog("I don't know how to pick up something that is not a resource...");
                            throw new NotImplementedException();
                        }
                        // NOTE: this assumes we have the item and the target within reach...
                        //if ((this.Item as EntityResource).IsConsumedOnUse())
                        //{
                        //    entity.Inventory.RemoveResource((this.Item as EntityResource).RType);
                        //}
                        //else if ((this.Item as EntityResource).IsContainer())
                        //{
                        //    // NOTE: this does not nest the target into the container item then into the inventory, 
                        //    // for now, just add the target to the inventory directly, and assume the container is in use.
                        //    entity.Inventory.AddResource(this.Target as EntityResource);
                        //}

                        //this.ActionState = EntityActionState.Complete;
                    }
                    else // we're still doing it and are not done yet.
                    {
                        // update a status or progress?
                    }
                    break;
                //case Ability.AbilityType.Place:
                //    break;
                //case Ability.AbilityType.Throw:
                //    break;
                case Ability.AbilityType.Use:
                    // if we've spent long enough doing this, then accomplish it
                    if (DateTime.Now > ActionStartedWhen + DurationRequired)
                    {
                        // NOTE: this assumes we have the item and the target within reach...

                        if((this.Item as EntityResource).IsConsumedOnUse())
                        {
                            entity.Inventory.RemoveResource((this.Item as EntityResource).RType);
                        }
                        else if((this.Item as EntityResource).IsContainer())
                        {
                            // NOTE: this does not nest the target into the container item then into the inventory, 
                            // for now, just add the target to the inventory directly, and assume the container is in use.
                            entity.Inventory.AddResource(this.Target as EntityResource);
                        }

                        this.ActionState = EntityActionState.Complete;
                    }
                    else // we're still doing it and are not done yet.
                    {
                         // update a status or progress?
                    }
                        break;
                //case Ability.AbilityType.Greet:
                //    break;
                //case Ability.AbilityType.Speak_Deceive:
                //    break;
                //case Ability.AbilityType.Speak_Inquire:
                //    break;
                //case Ability.AbilityType.Speak_Intimidate:
                //    break;
                //case Ability.AbilityType.Speak_Persuade:
                //    break;
                //case Ability.AbilityType.Speak_Statement:
                //    break;
                //case Ability.AbilityType.Use_Sensor:
                //    break;
                //case Ability.AbilityType.Sleep:
                //    break;
                //case Ability.AbilityType.Wake:
                //    break;
                case Ability.AbilityType.Walk:
                case Ability.AbilityType.Run:
                    // determine if we have reached the desired destination
                    Position pTarget = (this.Target as Position);
                    double dist = entity.PositionCurrent.DistanceFrom(pTarget);
                    if(dist < 5.0)
                    {
                        this.ActionState = EntityActionState.Complete;
                    }
                    else // we're still pretty far away
                    {
                        double speed = entity.actions.GetAbilityValue(ability.AType);
                        Position newPos = entity.PositionCurrent.GetNewPositionForSpeedToTarget(pTarget, speed);

                        // apply result: energy drain
                        foreach(CoreAttribute ca in entity.coreAttributes)
                        {
                            if(ca.CType == CoreAttribute.CoreAttributeType.Energy)
                            {
                                ca.CurrentValue -= CoreAttribute.GetEnergyDrainForDistanceMoved(entity.PositionCurrent, newPos, speed);
                            }
                        }

                        // apply result: move position towards targe
                        entity.PositionCurrent = newPos;
                    }
                    break;
                default:
                    break; // do nothing.
            }
        }

        internal void EvaluateForBlockedStatus(Entity entity)
        {
            EntityAction.EntityActionState oldState = this.ActionState;

            #region Check for ability
            double val = entity.actions.GetAbilityValue(this.ability.AType);
            if(val == 0)
            {
                entity.RaiseLog("Unable to to perform action: " + this.ability.AType.ToString());
                this.ActionState = EntityActionState.Blocked;
                return;
            }
            #endregion

            #region Check Target
            if (this.Target != null &&
                this.Target is EntityResource)
            {
                // how do we track if a target resource needs to be in the inventory?
                // for example to chop a tree, it might just need to be a nearby target, not an inventory item...

                if (this.ability.AType != Ability.AbilityType.Pick_Up)
                {
                    // if the target is supposed to be in the inventory, check the inventory
                    if (!entity.Inventory.HaveResource((this.Target as EntityResource).RType))
                    {
                        entity.RaiseLog("Don't have needed target to perform action.");
                        this.ActionState = EntityActionState.Blocked;
                        return;
                    }
                }
                else // pick up the target
                {
                    EntityResource ear = (this.Target as EntityResource);

                    // if the target is supposed to be in the environment, check the senses for existance
                    // we don't have it in our inventory, see if we have it available from our senses...
                    Position target = entity.CurrentEnvironment.FindObject(entity, ear);

                    if (target == null)// we don't know of the resource within the environment
                    {
                        entity.RaiseLog("cannot find " + ear.RType.ToString());
                        this.ActionState = EntityActionState.Blocked;
                        return;
                    }

                    entity.RaiseLog("found the needed target resource, seeing if we need to travel to it, or have a container...");

                    // if we're too far away from our needed resource, add a walk action
                    if (target.DistanceFrom(entity.PositionCurrent) > 5)
                    {
                        entity.RaiseLog("the target resource that we need is too far to reach, need to walk to it.");
                        EntityAction newAction = (new EntityAction(this.ParentSolution, new Ability(Ability.AbilityType.Walk), target, null));

                        int aqi = GetIndexOfAction(entity, this);
                        // insert this new action into the same slot in the action queue
                        entity.actions.InsertAction(aqi, newAction);

                        // add this new action to the blocked action's solution
                        // by inerting at this index, this new action will go right before the blocked one
                        int i = this.ParentSolution.GetIndexOfAction(this);
                        //ea.ParentSolution.Actions.Insert(i, newAction);

                        ActionState = EntityAction.EntityActionState.New;
                    }

                    // if we need to pick it up, but it requires a container, then make sure to add an action to do so.
                    if (ear.RequiresContainer() &&
                        !entity.Inventory.HaveResource(EntityResource.ResourceType.Container))
                    {
                        entity.RaiseLog("the target resource that we need requires a container, and we don't have one in our inventory, so adding a need.");
                        ResourceNeed n = new ResourceNeed(EntityResource.ResourceType.Container, 1, null);

                        if (!NeedExists(entity, n))
                        {
                            // default to adding this need to the top of the list.
                            entity.CurrentNeeds.Insert(0, n);
                        }
                    }
                    return;
                }

            }
            else { } // check other types of targets than resources (unreachable position?)
            #endregion

            #region Check Item
            if (this.Item != null &&
                this.Item is EntityResource)
            {
                if (!entity.Inventory.HaveResource((this.Item as EntityResource).RType))
                {
                    entity.RaiseLog($"Don't have needed {(this.Item as EntityResource).RType.ToString()} to perform action.");
                    this.ActionState = EntityActionState.Blocked;
                    return; // no need to go further
                }

                // quantity needed?
            }
            #endregion

            // if we get to here, then we can reset it.
            if(oldState == EntityActionState.Blocked)
            {
                entity.RaiseLog("it seems I have the ability, the target, and the item to perform this action, so setting it back to new");
                this.ActionState = EntityActionState.New;
            }
        }
        internal int GetIndexOfAction(Entity entity, EntityAction ea)
        {
            for (int i = 0; i < entity.actions.ActionQueue.Count; i++)
            {
                if (entity.actions.ActionQueue[i] == ea) { return i; }
            }

            return entity.actions.ActionQueue.Count;
        }
        private bool NeedExists(Entity entity, ResourceNeed rn)
        {
            foreach (EntityNeed en in entity.CurrentNeeds)
            {
                if (!(en is ResourceNeed)) { continue; }
                ResourceNeed existingRN = en as ResourceNeed;

                if (existingRN.Resource.RType == rn.Resource.RType) { return true; }
            }

            return false;
        }
    }
}
