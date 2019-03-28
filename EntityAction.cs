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
                entity.RaiseLog(new EntityLogging.EntityLog("starting: " + this.Description));

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
            entity.RaiseLog(new EntityLogging.EntityLog("continuing: " + this.Description));

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
                        // NOTE: this assumes we have the item and the target within reach...
                        if ((this.Item as EntityResource).IsConsumedOnUse())
                        {
                            entity.Inventory.RemoveResource((this.Item as EntityResource).RType);
                        }
                        else if ((this.Item as EntityResource).IsContainer())
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
                entity.RaiseLog(new EntityLogging.EntityLog("Unable to to perform action: " + this.ability.AType.ToString()));
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
                        entity.RaiseLog(new EntityLogging.EntityLog("Don't have needed target to perform action."));
                        this.ActionState = EntityActionState.Blocked;
                    }
                }
                else
                {
                    // if the target is supposed to be in the environment, check the senses for existance
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
                    entity.RaiseLog(new EntityLogging.EntityLog("Don't have needed " + (this.Item as EntityResource).RType.ToString() + " to perform action."));
                    this.ActionState = EntityActionState.Blocked;
                    return; // no need to go further
                }

                // quantity needed?
            }
            #endregion

            // if we get to here, then we can reset it.
            if(oldState == EntityActionState.Blocked)
            {
                this.ActionState = EntityActionState.New;
            }
        }
    }
}
