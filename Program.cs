using System.Threading.Tasks;

namespace ConsoleApplication
{
    public class Program
    {
        public void Main(string[] args)
        {
            // var endpoint = new Endpoint();
            // endpoint.UpdateDevice("6D 49 D3 A5", "Location1");

            var tasks = new ArduinoTasks().CreateTasks();
            Task.WaitAll(tasks.ToArray());
        }
    }
}
