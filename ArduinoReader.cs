using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleApplication {
	    public class ArduinoReader {
        public List<Task> CreateTasks() {
            var rfidReaders = new List<Arduino>();
            rfidReaders.Add(new Arduino1());
            rfidReaders.Add(new Arduino2());

            List<Task> tasks = new List<Task>();

            rfidReaders.ForEach((rfidReader) =>
            {
                try
                {
                    rfidReader.OpenPort();
                    System.Console.WriteLine($"'{rfidReader.Location}' found");

                    tasks.Add(Task.Run(() =>
                    {
                        while (true)
                        {
                            var readLine = rfidReader.ReadInput();
                            
                            var id = (readLine.StartsWith("Card UID: ")) ? readLine.Substring(10) : null;
                            if (!string.IsNullOrWhiteSpace(id))
                            {
                                System.Console.WriteLine(id);
                                System.Console.WriteLine("Checkin of '" + id + "' at '" + rfidReader.Location + "'");
                                var cmdbServer = new Endpoint();
                                cmdbServer.UpdateDevice(id, rfidReader.Location);
                            }
                        }
                    }));
                }
                catch (System.IO.IOException)
                {
                    System.Console.WriteLine($"'{rfidReader.Location}' not found");
                }
            });
            
            return tasks;
        }
    }
}