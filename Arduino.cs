
using System.IO.Ports;
namespace ConsoleApplication
{
    public abstract class Arduino
    {
        public int BaudRate { get { return 115200; } }
        public abstract string PortName { get; }
        public abstract string Alias { get; }

        private SerialPort _port;

        public Arduino()
        {
        }

        public void OpenPort()
        {
            _port = new SerialPort();
            _port.BaudRate = this.BaudRate;
            _port.PortName = this.PortName;
            
            _port.Open();
        }
        public SerialPort GetPort()
        {
            return _port;
        }
    }

    public class Arduino1 : Arduino
    {
        public override string PortName { get { return "/dev/cu.usbmodem641"; } }
        public override string Alias { get { return "Reader1"; } }
    }


    public class Arduino2 : Arduino
    {
        public override string PortName { get { return "/dev/cu.usbmodem411"; } }
        public override string Alias { get { return "Reader2"; } }
    }
}