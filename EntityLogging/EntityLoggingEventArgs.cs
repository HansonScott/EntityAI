using System;

namespace EntityLogging
{
    public class EntityLoggingEventArgs: EventArgs
    {
        public EntityLog Log;

        public EntityLoggingEventArgs(EntityLog log)
        {
            this.Log = log;
        }
    }
}