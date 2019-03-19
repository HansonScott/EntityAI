using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EntityAI;

namespace EntitySimulator
{
    public class Simulator
    {
        public EntityAI.EntityEnvironment CurrentEnvironment;
        Thread EnvironmentThread;
        
        public EntityAI.Entity Protagonist;
        Thread ProtagonistThread;

        private DateTime StartTime;

        public TimeSpan RunTime
        {
            get
            {
                return DateTime.Now - StartTime;
            }
        }

        #region Constructor and Setup
        public Simulator()
        {
            CurrentEnvironment = SetupEnvironment();
            EnvironmentThread = new Thread(new ThreadStart(CurrentEnvironment.Run));

            Protagonist = SetupProtagonist();
            ProtagonistThread = new Thread(new ThreadStart(Protagonist.Run));
        }

        private EntityEnvironment SetupEnvironment()
        {
            EntityAI.EntityEnvironment result = new EntityEnvironment();
            // set any variables?

            return result;
        }
        private Entity SetupProtagonist()
        {
            EntityAI.Entity result = new EntityAI.Entity();

            result.CurrentEnvironment = CurrentEnvironment;
            result.PositionCurrent = new EntityAI.Position();

            return result;
        }
        #endregion

        public void RunSimulation()
        {
            StartTime = DateTime.Now;
            EnvironmentThread.Start();
            ProtagonistThread.Start();
        }

        public void StopSimulation()
        {
            CurrentEnvironment.ShutDown();
            Protagonist.ShutDown();
        }
    }
}
