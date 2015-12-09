using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SystemStatus.Agent.WindowsService;
using SystemStatus.Domain;

namespace SystemStatus.Agent
{
  

    public class Program
    {
        private static SystemStatusAgentService _service;

        public static void Main(string[] args)
        {
            try
            {
                _service = new SystemStatusAgentService();
                _service.DoStart();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
      
            Console.ReadLine();
        }
    }



}
