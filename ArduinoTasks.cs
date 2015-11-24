using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication
{
    public class ArduinoTasks
    {
        public List<Task> CreateTasks()
        {
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

                    WaitHandle waitHandle = new AutoResetEvent(false);

                    tasks.Add(Task.Run(() =>
                    {
                        var port = rfidReader.GetPort();
                        var cmdbServer = new Endpoint();

                        while (port.IsOpen)
                        {
                            var id = findUID(port, 0, "");

                            if (id != "empty")
                            {
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
        
        private string findUID(SerialPort port, int position, string bufferString){
            char[] buffer = new char[1];
            while(!bufferString.Contains("Card UID: ")){
                port.Read(buffer, position, 1);
                
                bufferString += new string(buffer);
                return findUID(port, position++, bufferString);
            }
            char[] uid = new char[11];
            port.Read(uid, position, 11);
            return new string(uid);
        }
    }
}