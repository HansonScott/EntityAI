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
            result.Inventory.AddResource(new EntityResource(EntityResource.ResourceType.Container));
            return result;
        }
        #endregion

        public bool RunSimulation()
        {
            if(this.CurrentEnvironment == null || this.Protagonist == null)
            {
                return false;
            }

            StartTime = DateTime.Now;
            if(EnvironmentThread.ThreadState == ThreadState.Stopped)
            {
                EnvironmentThread = new Thread(new ThreadStart(CurrentEnvironment.Run));
            }

            if (this.EnvironmentThread.ThreadState == ThreadState.Unstarted)
            {
                EnvironmentThread.Start();
            }

            if(ProtagonistThread.ThreadState == ThreadState.Stopped)
            {
                ProtagonistThread = new Thread(new ThreadStart(Protagonist.Run));
            }

            if (this.ProtagonistThread.ThreadState == ThreadState.Unstarted)
            {
                ProtagonistThread.Start();
            }
            return true;
        }

        public void StopSimulation()
        {
            CurrentEnvironment.ShutDown();
            Protagonist.ShutDown();
        }
    }
}
