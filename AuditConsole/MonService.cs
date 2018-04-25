using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace AuditConsole
{
    public class MonService : IMonService
    {
        private MonServer server;

        public MonService()
        {
            server = Program.server;
        }

        public EventLogEntryCollection GetEventLogEntries(string logType)
        {
            EventLog ev = new EventLog(logType, System.Environment.MachineName);
            return ev.Entries;
        }

        public byte[] GetServerIP()
        {
            return server.GetServerIP();
        }

        public string GetServerName()
        {
            return Environment.MachineName;
        }

        public bool TestConnection()
        {
            return true;
        }

        public FileSystemWatcher GetFileSystemWatcher()
        {
            return server.GetFileSystemWatcher();
        }

        public EventLog[] GetEventLogs()
        {
            return server.GetEventLogs();
        }

        public FolderBrowserDialog GetFolderBrowserDIalog()
        {
            return server.GetFolderBrowserDIalog();
        }

        public string CheckDevices()
        {
            return server.CheckDevices();
        }
    }
}
