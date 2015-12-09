using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemStatus.Domain.ViewModels
{
    public class CreateAppEventModel
    {
        public DateTime EventTime { get; set; }
        public string Message { get; set; }
        public int AppID { get; set; }
        public AppStatus AppStatus { get; set; }
        public int FromAppEventHookID { get; set; }
        public decimal? Value { get; set; }
    }
}
