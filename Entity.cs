using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EntityLogging;

namespace EntityAI
{
    /// <summary>
    /// Represents the parent object governing the many systems
    /// </summary>
    public class Entity
    {
        #region Custom Logging Events
        public delegate void LoggingHandler(object sender, EntityLoggingEventArgs e);
        public event LoggingHandler OnLog;

        internal void RaiseLog(string message)
        {
            RaiseLog(new EntityLog(message));
        }
        internal void RaiseLog(EntityLog log)
        {
            // using this inline vs checking for null is more thread safe
            OnLog?.Invoke(this, new EntityLoggingEventArgs(log));
        }
        #endregion

        #region Fields and Properties
        private bool Continue = true; // stay alive variable

        public List<CoreAttribute> coreAttributes;
        public SensorySystem senses;
        Thread SensoryThread;

        public ActionSystem actions;
        Thread ActionThread;

        // core entity loop delay, needs to be pretty fast for reacting to things.
        public int LoopDelay = 1000;

        public List<EntityNeed> CurrentNeeds = new List<EntityNeed>();
        public List<Solution> CurrentSolutions = new List<Solution>();

        public List<EntityNeed> CurrentOpportunities = new List<EntityNeed>();
        public List<Solution> CurrentOpportunitySolutions = new List<Solution>();

        public EntityEnvironment CurrentEnvironment;

        public Position PositionCurrent;
        public EntityInventory Inventory;
        #endregion

        #region Constructor and Setup
        public Entity()
        {
            coreAttributes = PopulateCoreAttributes();
            senses = new SensorySystem(this);
            actions = new ActionSystem(this);
            CurrentNeeds = new List<EntityNeed>();
            CurrentSolutions = new List<Solution>();
            CurrentOpportunities = new List<EntityNeed>();
            CurrentOpportunitySolutions = new List<Solution>();
            Inventory = new EntityInventory(this);
        }
        private List<CoreAttribute> PopulateCoreAttributes()
        {
            List<CoreAttribute> results = new List<CoreAttribute>();
            string[] CTypes = Enum.GetNames(typeof(CoreAttribute.CoreAttributeType));
            foreach (string cat in CTypes)
            {
                results.Add(new CoreAttribute(this, (CoreAttribute.CoreAttributeType)Enum.Parse(typeof(CoreAttribute.CoreAttributeType), cat)));
            }
            return results;
        }
        #endregion

        public void Run()
        {
            // if the function run is called, it is assumed the loop should actually run...
            Continue = true;

            RaiseLog("Hello.");

            // start up sensory input thread
            SensoryThread = new Thread(new ThreadStart(senses.Run));
            RaiseLog("I am starting up my senses...");
            SensoryThread.Start();

            ActionThread = new Thread(new ThreadStart(actions.Run));
            RaiseLog("I am starting up my actions...");
            ActionThread.Start();
            
            // main loop
            RaiseLog("I am starting my continuous loop.");
            while (Continue)
            {
                DateTime start = DateTime.Now;

                UpdateTickers();

                // self diagnostics
                RunSelfDiagnostics();

                // respond to sensory input
                RespondToSensoryInput();

                // evalute Needs
                EvaluateNeeds();

                // Evaluate blocked actions
                this.actions.EvaluateBlockedActions();

                // plan actions
                PlanActions();

                // reflection and re-evaluate
                ReflectAndReevaluate();

                // adjust timing?
                //int delay = 100;

                DateTime end = DateTime.Now;

                // wait until next loop
                double waittime = Math.Max(0, (LoopDelay - (end-start).TotalMilliseconds));

                // slow down the loop
                Thread.Sleep((int)waittime);
            }

            RaiseLog("Goodbye.");
        }

        public void ShutDown()
        {
            this.senses.ShutDown();
            this.actions.ShutDown();
            this.Continue = false;
        }

        #region Top Level Functions
        private void UpdateTickers()
        {
            // update timing-based attributes
            UpdateCoreAttributesForTiming();
        }
        private void RunSelfDiagnostics()
        {
            // evaluate core attributes within acceptable parameters
            EvaluateCoreAttributes();

            // evaluate sensory systems
            EvaluateSensorySystems();

            // evaluate abilities
            EvaluateAbilities();
        }

        private void RespondToSensoryInput()
        {
            // look for sensory input that could be considered a threat or an opportunity
            List<EntityNeed> needs = new List<EntityNeed>();

            // create actual entity needs to be dealt with
            needs = EvaluateSensoryInputForNeeds();

            // verify needs against current ones
            UpdateCurrentNeedsFromNewSensoryNeeds(needs);
        }
        private void EvaluateNeeds()
        {
            // evaluate needs against priorities
            CompareNeedsToPriorities();

            // create solution
            CreateSolutionsFromNeeds();
        }

        private void ReflectAndReevaluate()
        {
            // review patterns, trends, look to create new solutions
        }
        #endregion

        #region Tickers
        private void UpdateCoreAttributesForTiming()
        {
            foreach(CoreAttribute c in this.coreAttributes)
            {
                c.UpdateForTiming();
            }
        }
        #endregion

        #region Diagnostics
        private void EvaluateCoreAttributes()
        {
            // go through each attribute
            for(int i = 0; i < this.coreAttributes.Count; i++)
            {
                // check for parameters
                CoreAttribute c = this.coreAttributes[i];
                EntityAttribute.ValueRelativeStatus s = c.GetRelativeValueStatus();
                if(c.IsInNeed(s))
                {
                    CoreNeed need = new CoreNeed(c);

                    // check if we have the need already
                    CoreNeed existingNeed = GetCoreNeed(need.Attribute.CType);

                    if (existingNeed != null)
                    {
                        // check and adjust the urgency of the need
                        if(existingNeed.Urgency < need.Urgency)
                        {
                            // replace need?
                            // update source DateTime?
                            // update anything else?

                            RaiseLog("I have a core attribute need with a higher urgency than previously: " + need.Attribute.Description);
                            CurrentNeeds.Remove(existingNeed);
                            this.CurrentNeeds.Add(need);
                        }
                        else // we already have something this urgent
                        {
                            // do nothing.
                        }
                    }
                    else
                    {
                        RaiseLog("I have a new core attribute need: " + need.Attribute.Description);
                        this.CurrentNeeds.Add(need);
                    }

                } // end if in need
                else if(c.HasOpportunity())
                {
                    CoreNeed need = new CoreNeed(c);

                    // check if we have the need already
                    CoreNeed existingNeed = GetCoreOpportunity(need.Attribute.CType);

                    if (existingNeed != null)
                    {
                        // check and adjust the urgency of the need
                        if (existingNeed.Urgency < need.Urgency)
                        {
                            // replace need?
                            // update source DateTime?
                            // update anything else?

                            CurrentOpportunities.Remove(existingNeed);
                            CurrentOpportunities.Add(need);
                        }
                        else // we already have something this urgent
                        {
                            // do nothing.
                        }
                    }
                    else
                    {
                        CurrentOpportunities.Add(need);
                    }
                }
            } // end foreach attribute
        }

        private CoreNeed GetCoreOpportunity(CoreAttribute.CoreAttributeType cType)
        {
            for (int i = 0; i < this.CurrentOpportunities.Count; i++)
            {
                if (this.CurrentOpportunities[i] is CoreNeed)
                {
                    if ((this.CurrentOpportunities[i] as CoreNeed).Attribute.CType == cType) { return (this.CurrentOpportunities[i] as CoreNeed); }
                }
            }

            return null;
        }

        private CoreNeed GetCoreNeed(CoreAttribute.CoreAttributeType cType)
        {
            for(int i = 0; i < this.CurrentNeeds.Count; i++)
            {
                if(this.CurrentNeeds[i] is CoreNeed)
                {
                    if((this.CurrentNeeds[i] as CoreNeed).Attribute.CType == cType) { return (this.CurrentNeeds[i] as CoreNeed); }
                }
            }

            return null;
        }

        private void EvaluateSensorySystems()
        {
            // go through each attribute
            for (int i = 0; i < this.senses.sensors.Count; i++)
            {
                // check for parameters
                Sensor s = this.senses.sensors[i];
                EntityAttribute.ValueRelativeStatus vrs = s.GetRelativeValueStatus();
                if (s.IsInNeed(vrs))
                {
                    this.CurrentNeeds.Add(new SensorNeed(s));

                } // end if in need
                else if(s.HasOpportunity())
                {
                    this.CurrentOpportunities.Add(new SensorNeed(s));
                }
            } // end foreach attribute
        }

        private void EvaluateAbilities()
        {
            foreach(Ability A in this.actions.Abilities)
            {
                EntityAttribute.ValueRelativeStatus vrs = A.GetRelativeValueStatus();
                if (A.IsInNeed(vrs))
                {
                    AbilityNeed need = new AbilityNeed(A);

                    // check if we have the need already
                    AbilityNeed existingNeed = GetAbilityNeed(A.AType);

                    if (existingNeed != null)
                    {
                        // check and adjust the urgency of the need
                        if (existingNeed.Urgency < need.Urgency)
                        {
                            // replace need?
                            // update source DateTime?
                            // update anything else?

                            CurrentNeeds.Remove(existingNeed);
                            this.CurrentNeeds.Add(need);
                        }
                        else // we already have something this urgent
                        {
                            // do nothing.
                        }

                    }
                    else
                    {
                        this.CurrentNeeds.Add(need);
                    }

                } // end if in need            
            }
        }

        private AbilityNeed GetAbilityNeed(Ability.AbilityType aType)
        {
            foreach (AbilityNeed n in this.CurrentNeeds)
            {
                if (n.Ability.AType == aType) { return n; }
            }

            return null;
        }
        #endregion

        #region SensoryResponse
        private List<EntityNeed> EvaluateSensoryInputForNeeds()
        {
            //this.senses.SightsCurrentlySeen;
            //this.senses.SoundsCurrentlyHeard;

            // evaluate any sensory input as a threat, and therefore a need to respond.

            //throw new NotImplementedException();
            
            return null;
        }
        private void UpdateCurrentNeedsFromNewSensoryNeeds(List<EntityNeed> SensoryNeeds)
        {
            if (SensoryNeeds == null) { return; }

            // evaluate sensory input needs compared to current needs
            foreach(InputNeed n in SensoryNeeds)
            {
                // if this need is important, add it to our current needs.
                EntityNeed existingNeed = GetExistingNeed(n);
                if(existingNeed == null)
                {
                    this.CurrentNeeds.Add(n);
                }
                else
                {
                    // replace or change need?
                    // for now, just skip
                }
            }
        }
        private EntityNeed GetExistingNeed(InputNeed n)
        {
            foreach(EntityNeed existingNeed in this.CurrentNeeds)
            {
                if(existingNeed.GetType() == n.GetType())
                {
                    if(((InputNeed)existingNeed).SourceSensor == n.SourceSensor)
                    {
                        // any other details we need to check?
                        return existingNeed;
                    }
                }
            }

            // if we get here, then we didn't find a match
            return null;
        }
        #endregion

        #region EvaluateNeeds
        private void CompareNeedsToPriorities()
        {
            // look at all the needs, and priortize
        }
        private void CreateSolutionsFromNeeds()
        {
            for(int i = 0; i < this.CurrentNeeds.Count; i++)
            {
                Solution S = Solution.FindSolutionForNeed(this.CurrentNeeds[i], this);

                if(S != null)
                {
                    Solution existingSolution = GetExistingSolution(S);

                    if (existingSolution == null)
                    {
                        RaiseLog("Found a solution for need: " + this.CurrentNeeds[i].Name);
                        this.CurrentSolutions.Insert(0,S);
                        this.CurrentNeeds.RemoveAt(i--); // remove this from the list, and continue on
                    }
                }
                else
                {
                    // we don't have a solution for this need, 
                    // what do we do next? (look for one, create one, etc.)
                }
            }

            for (int i = 0; i < this.CurrentOpportunities.Count; i++)
            {
                Solution S = Solution.FindSolutionForNeed(this.CurrentOpportunities[i], this);

                if (S != null)
                {
                    Solution existingSolution = GetExistingOpportunitySolution(S);

                    if (existingSolution == null)
                    {
                        RaiseLog("Found a solution for opportunity: " + this.CurrentOpportunities[i].Name);
                        this.CurrentOpportunitySolutions.Add(S);
                    }
                }
                else
                {
                    // we don't have a solution for this need, 
                    // what do we do next? (look for one, create one, etc.)
                }
            }
        }

        private Solution GetExistingOpportunitySolution(Solution s)
        {
            foreach (Solution existingSolution in this.CurrentOpportunitySolutions)
            {
                // what attributes should we check on?
                if (existingSolution.Description == s.Description)
                {
                    return existingSolution;
                }
            }
            return null;
        }
        private Solution GetExistingSolution(Solution s)
        {
            foreach(Solution existingSolution in this.CurrentSolutions)
            {
                // what attributes should we check on?
                if (existingSolution.Description == s.Description)
                {
                    return existingSolution;
                }
            }

            // if we get here, we didn't find a match
            return null;
        }
        #endregion

        #region PerformActions
        private void PlanActions()
        {
            // for each solution
            foreach(Solution s in this.CurrentSolutions)
            {
                if(s.SolutionState == Solution.EntitySolutionState.created)
                {
                    // future: strategy comes into play here, as some action combinations can be optimized, etc.
                    // for now, just add them linearly.
                    for(int i = 0; i < s.Actions.Count; i++)
                    {
                        EntityAction ea = s.Actions[i];

                        // add queued actions to action thread according to queued solutions
                        this.actions.ActionQueue.Insert(i++, ea);
                    }

                    // change solution state from create the planned
                    s.SolutionState = Solution.EntitySolutionState.planned;
                }
            }

            foreach(Solution s in this.CurrentOpportunitySolutions)
            {
                if (s.SolutionState == Solution.EntitySolutionState.created)
                {
                    // future: strategy comes into play here, as some action combinations can be optimized, etc.
                    // for now, just add them linearly.
                    foreach (EntityAction ea in s.Actions)
                    {
                        // add queued actions to action thread according to queued solutions
                        // NOTE: because this is only opportunities, add them to the end of the queue.
                        this.actions.ActionQueue.Add(ea);
                    }

                    s.SolutionState = Solution.EntitySolutionState.planned;
                }
            }
        }
        #endregion
    }
}
