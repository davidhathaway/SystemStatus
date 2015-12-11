using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemStatus.Domain.ViewModels
{
    public class AppEventCollectionViewModel
    {
        public int MinValue { get; set; }
        public int MaxValue { get; set; }
        public int AppID { get; set; }
        public IEnumerable<AppEventViewModel> Events { get; set; }
    }
    public class AppEventViewModel
    {
        public int AppEventID { get; set; }
        public DateTime EventTime { get; set; }
        public string Message { get; set; }
        public AppStatus AppStatus { get; set; }
        public decimal? Value { get; set; }
        
    }
}
