using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EntityAI
{
    /// <summary>
    /// Represents the parent object governing the many systems
    /// </summary>
    public class Entity
    {
        public bool Continue = true; // stay alive variable

        List<CoreAttribute> coreAttributes;
        SensorySystem senses;
        ActionSystem actions;

        int delay = 100;

        List<EntityNeed> CurrentNeeds;
        List<Solution> CurrentSolutions;

        List<EntityNeed> CurrentOpportunities;
        List<Solution> CurrentOpportunitySolutions;

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
            // look for new sensory input
            List<InputNeed> needs = this.senses.GetInputNeeds();

            // run diagnosis on sensory input
            // create actual entity needs to be dealt with
            DiagnoseSensoryInputs(needs);
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
                    SensorNeed need = new SensorNeed(s);

                    // check if we have the need already
                    SensorNeed existingNeed = GetSensorNeed(s.SType);

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
                else if(s.HasOpportunity())
                {
                    SensorNeed need = new SensorNeed(s);

                    // check if we have the need already
                    SensorNeed existingNeed = GetSensorNeed(s.SType);

                    if (existingNeed != null)
                    {
                        // check and adjust the urgency of the need
                        if (existingNeed.Urgency < need.Urgency)
                        {
                            // replace need?
                            // update source DateTime?
                            // update anything else?

                            this.CurrentOpportunities.Remove(existingNeed);
                            this.CurrentOpportunities.Add(need);
                        }
                        else // we already have something this urgent
                        {
                            // do nothing.
                        }

                    }
                    else
                    {
                        this.CurrentOpportunities.Add(need);
                    }
                }
            } // end foreach attribute
        }

        private SensorNeed GetSensorNeed(Sensor.SensorType sType)
        {
            foreach (SensorNeed n in this.CurrentNeeds)
            {
                if (n.Sensor.SType == sType) { return n; }
            }

            return null;
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
        private void DiagnoseSensoryInputs(List<InputNeed> needs)
        {
            // evaluate sensory input needs compared to current needs
            foreach(InputNeed n in needs)
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

        #region
        #endregion

        #region PerformActions
        private void PrioritizeSolutions()
        {
            // look at urgency, ROI, etc.

            // change order to solutions in the list
            throw new NotImplementedException();
        }
        private void PlanActions()
        {
            // strategy comes into play here, as some action combinations can be optimized, etc.

            // add queued actions to action thread according to queued solutions
        }
        #endregion

        #region Reflection
        // future...
        #endregion
    }
}
