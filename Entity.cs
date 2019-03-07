﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EntityAI
{
    public class Entity
    {
        public bool Continue = true; // stay alive variable

        SensorySystem senses;
        List<Sensor> sensors; 
        List<CoreAttribute> coreAttributes;

        #region Constructor and Setup
        public Entity()
        {
            CreateSenses();
            CreateAttributes();
        }

        private void CreateSenses()
        {
            sensors = new List<Sensor>();

            // create/load all sensors

            throw new NotImplementedException();
        }

        private void CreateAttributes()
        {
            coreAttributes = new List<CoreAttribute>();

            // create/load all attributes

            throw new NotImplementedException();

        }
        #endregion

        public void Run()
        {
            senses = new SensorySystem(this);

            // start up sensory input thread
            Thread thread = new Thread(new ThreadStart(senses.Run()));
            thread.Start();

            // main loop
            while (Continue)
            {
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
            }
        }

        #region Top Level Functions
        private void RunSelfDiagnostics()
        {
            // evaluate core attributes within acceptable parameters
            EvaluateCoreAttributes();

            // evaluate sensory systems
            EvaluateSensorySystems();

            // check that all needs have solutions
        }
        private void RespondToSensoryInput()
        {
            // look for new sensory input
            LoadNewInputs();

            // run diagnosis on sensory input
            // create needs
            DiagnoseSensoryInputs();
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
            throw new NotImplementedException();
        }
        private void EvaluateSensorySystems()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region SensoryResponse
        private void LoadNewInputs()
        {
            throw new NotImplementedException();
        }
        private void DiagnoseSensoryInputs()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region EvaluateNeeds
        private void CompareNeedsToPriorities()
        {
            throw new NotImplementedException();
        }
        private void CreateSolutionsFromNeeds()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region PerformActions
        private void PrioritizeSolutions()
        {
            throw new NotImplementedException();
        }
        private void PlanActions()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Reflection
        // future...
        #endregion
    }
}
