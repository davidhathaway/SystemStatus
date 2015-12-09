using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemStatus.Domain
{
    public class CommandResult<T>
    {
        public T Command { get; set; }
      
        public bool Success { get { return Errors.Count == 0; } }
      
        public Dictionary<string, List<string>> Errors { get; set; }
      
        public CommandResult()
        {
            Errors = new Dictionary<string, List<string>>();
        }

        public void AddError(string key, string error)
        {
            if (Errors.ContainsKey(key))
            {
                Errors[key].Add(error);
            }
            else
            {
                Errors.Add(key, new List<string>() { error });
            }
        }
    }
}
