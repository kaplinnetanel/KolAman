using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationGate.models;

public class Alert
{
    public string alert_id { get; set; } = string.Empty;
    public string source { get; set; } = string.Empty; 
  
    public string title { get; set;  } = string.Empty; 
    public string content { get; set; } = string.Empty;
    [RegularExpression("^(Low|Medium|High|Critical)$")]
    public string  priority { get; set; } = string.Empty;
    [RegularExpression("^(`UNCLASSIFIED|RESTRICTED|SECRET|TOP_SECRET)$")]
    public string classification { get; set; } = string.Empty;
    public int lat { get; set; }
    public int lon { get; set; }
    public DateTime timestamp { get; set; }
    public string status { get; set; } = "WAITING"; 

}
