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
        public delegate void LoggingHandler(object sender, EntityLogging.EntityLoggingEventArgs e);
        public event LoggingHandler OnLog;

        internal void RaiseLog(EntityLogging.EntityLog log)
        {
            // using this inline vs checking for null is more thread safe
            OnLog?.Invoke(this, new EntityLoggingEventArgs(log));
        }
        #endregion

        #region Fields and Properties
        public bool Continue = true; // stay alive variable

        List<CoreAttribute> coreAttributes;
        public SensorySystem senses;
        ActionSystem actions;

        int delay = 100;

        List<EntityNeed> CurrentNeeds;
        List<Solution> CurrentSolutions;

        List<EntityNeed> CurrentOpportunities;
        List<Solution> CurrentOpportunitySolutions;

        public EntityEnvironment CurrentEnvironment;

        public Position PositionCurrent;
        #endregion

        #region Constructor and Setup
        public Entity()
        {
            coreAttributes = new List<CoreAttribute>();
            senses = new SensorySystem(this);
            actions = new ActionSystem(this);
            CurrentNeeds = new List<EntityNeed>();
            CurrentSolutions = new List<Solution>();
            CurrentOpportunities = new List<EntityNeed>();
            CurrentOpportunitySolutions = new List<Solution>();
        }
        #endregion

        public void Run()
        {
            // start up sensory input thread
            Thread thread = new Thread(new ThreadStart(senses.Run));
            thread.Start();

            // main loop
            while (Continue)
            {
                DateTime start = DateTime.Now;

                // self diagnostics
                RunSelfDiagnostics();

                // respond to sensory input
                RespondToSensoryInput();

                // evalute Needs
                EvaluateNeeds();

                // perform actions
                PerformActions();

                // reflection and re-evaluate
                ReflectAndReevaluate();

                // adjust timing?
                //int delay = 100;

                DateTime end = DateTime.Now;

                // wait until next loop
                double waittime = Math.Max(0, (delay - (end-start).TotalMilliseconds));

                // slow down the loop
                Thread.Sleep((int)waittime);
            }
        }

        #region Top Level Functions
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
        private void PerformActions()
        {
            // prioritize and load solutions
            PrioritizeSolutions();

            // plan actions for solutions
            PlanActions();
        }
        private void ReflectAndReevaluate()
        {
            // review patterns, trends, look to create new solutions
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
                else if(c.HasOpportunity())
                {
                    CoreNeed need = new CoreNeed(c);

                    // check if we have the need already
                    CoreNeed existingNeed = GetCoreNeed(need.Attribute.CType);

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

        private CoreNeed GetCoreNeed(CoreAttribute.CoreAttributeType cType)
        {
            foreach (CoreNeed n in this.CurrentNeeds)
            {
                if (n.Attribute.CType == cType) { return n; }
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
            throw new NotImplementedException();
            //return null;
        }
        private void UpdateCurrentNeedsFromNewSensoryNeeds(List<EntityNeed> SensoryNeeds)
        {
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
                Solution S = Solution.FindSolutionForNeed(this.CurrentNeeds[i]);

                if(S != null)
                {
                    Solution existingSolution = GetExistingSolution(S);

                    if (existingSolution != null)
                    {
                        // compare and change/replace?
                        this.CurrentSolutions.Remove(existingSolution);
                    }

                    this.CurrentSolutions.Add(S);
                }
                else
                {
                    // we don't have a solution for this need, 
                    // what do we do next? (look for one, create one, etc.)
                }
            }

            for (int i = 0; i < this.CurrentOpportunities.Count; i++)
            {
                Solution S = Solution.FindSolutionForNeed(this.CurrentOpportunities[i]);

                if (S != null)
                {
                    Solution existingSolution = GetExistingSolution(S);

                    if (existingSolution != null)
                    {
                        // compare and change/replace?
                        this.CurrentOpportunitySolutions.Remove(existingSolution);
                    }

                    this.CurrentOpportunitySolutions.Add(S);
                }
                else
                {
                    // we don't have a solution for this need, 
                    // what do we do next? (look for one, create one, etc.)
                }
            }
        }
        private Solution GetExistingSolution(Solution s)
        {
            foreach(Solution existingSolution in this.CurrentSolutions)
            {
                // what attributes should we check on?
                if (existingSolution.Benefit == s.Benefit &&
                    existingSolution.NeedFulfilled == s.NeedFulfilled)
                {
                    return existingSolution;
                }
            }

            // if we get here, we didn't find a match
            return null;
        }
        #endregion

        #region PerformActions
        private void PrioritizeSolutions()
        {
            // look at urgency, ROI, etc.

            // change order to solutions in the list

        }
        private void PlanActions()
        {
            // for each solution
            foreach(Solution s in this.CurrentSolutions)
            {
                // future: strategy comes into play here, as some action combinations can be optimized, etc.
                foreach(EntityAction ea in s.Actions)
                {
                    // add queued actions to action thread according to queued solutions
                    this.actions.ActionQueue.Add(ea);
                }
            }
        }
        #endregion
    }
}
