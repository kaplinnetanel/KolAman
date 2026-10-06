using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationGate.models
{

    public class LogModel
    {
        public string Message { get; set; } = "";
        public string Level { get; set; } = "";
        public int Severity { get; set; }
        public DateTime Timestamp { get; set; }
    }

   
}
