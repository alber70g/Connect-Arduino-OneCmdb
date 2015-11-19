using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO.Ports;
using RestSharp;
using System.Net;

namespace ConsoleApplication
{
    public class Program
    {
        public void Main(string[] args)
        {
            var endpoint = new Endpoint();
            endpoint.UpdateDevice("6D 49 D3 A5", "Location1");

            // var tasks = new ArduinoReader().CreateTasks();
            // Task.WaitAll(tasks.ToArray());
        }
    }
}
