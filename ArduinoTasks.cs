using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

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
                    System.Console.WriteLine($"'{rfidReader.Alias}' found");

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
                                System.Console.WriteLine("id found " + id);
                                
                                id = "ID" + id.Replace(" ", "");
                                
                                System.Console.WriteLine("new id " + id);
                                // get item by id
                                var derivedFrom = cmdbServer.GetDerivedFrom(id);
                                System.Console.WriteLine($"'{derivedFrom}'");
                                // if location
                                if (derivedFrom == "Location".Trim()){
                                    System.Console.WriteLine("it is location");
                                    // update reader location
                                    cmdbServer.UpdateReaderLocation(rfidReader.Alias, id);
                                } else {
                                    System.Console.WriteLine("it is asset");
                                    // if asset
                                    // get location from reader
                                    var location = cmdbServer.GetAliasLocation(rfidReader.Alias);
                                    
                                    cmdbServer.UpdateDevice(id, location);
                                }
                            }
                        
                }
                    }));
                }
                catch (System.IO.IOException)
                {
                    System.Console.WriteLine($"'{rfidReader.Alias}' not found");
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