using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLogging
{
    public class EntityLog
    {
        public string Message;
        public System.Diagnostics.TraceLevel Severity;
        public DateTime When;

        public EntityLog(string Message) : this(Message, System.Diagnostics.TraceLevel.Verbose) { }
        public EntityLog(string Message, System.Diagnostics.TraceLevel Severity) : this(Message, Severity, DateTime.Now) { }
        public EntityLog(string Message, System.Diagnostics.TraceLevel Severity, DateTime When)
        {
            this.Message = Message;
            this.Severity = Severity;
            this.When = When;
        }
    }
}
