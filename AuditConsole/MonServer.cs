using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AuditConsole
{
    public class MonServer
    {
        private byte[] serverIP;
        EventLog eventLog;

        public MonServer(byte[] _serverIP)
        {
            serverIP = _serverIP;
            DeviceChangeNotifier.Start();
        }

        public byte[] GetServerIP()
        {
            return serverIP;
        }

        public FileSystemWatcher GetFileSystemWatcher()
        {
            return new FileSystemWatcher();
        }

        public EventLog[] GetEventLogs()
        {
            return EventLog.GetEventLogs();
        }

        public FolderBrowserDialog GetFolderBrowserDIalog()
        {
            return new FolderBrowserDialog();
        }

        public string CheckDevices()
        {
            string log = DeviceChangeNotifier.Log;
            DeviceChangeNotifier.Log = "";
            return log;
        }
    }
}
