//using NotificationGate.models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace NotificationGate.Service
//{

//    public class LogService
//    {
//        private readonly ElasticsearchClient _client;

//        public LogService(ElasticsearchClient client)
//        {
//            _client = client;
//        }

//        public async Task Information(string message)
//        {
//            await SendLog(message, "Information", 2);
//        }

//        public async Task Warning(string message)
//        {
//            await SendLog(message, "Warning", 3);
//        }

//        public async Task Error(string message)
//        {
//            await SendLog(message, "Error", 4);
//        }

//        public async Task Critical(string message)
//        {
//            await SendLog(message, "Critical", 5);
//        }

//        private async Task SendLog(
//          string message,
//          string level,
//          int severity)
//        {
//            var log = new LogModel
//            {
//                Message = message,
//                Level = level,
//                Severity = severity,
//                Timestamp = DateTime.UtcNow
//            };

//            await _client.IndexAsync(
//                log,
//                i => i.Index("logs"));
//        }
//    }
//}
