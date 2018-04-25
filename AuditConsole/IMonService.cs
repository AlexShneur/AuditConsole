using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace AuditConsole
{
    [ServiceContract]
    public interface IMonService
    {
        [OperationContract]
        bool TestConnection();
        [OperationContract]
        string GetServerName();
        [OperationContract]
        byte[] GetServerIP();
        [OperationContract]
        EventLog[] GetEventLogs();
        [OperationContract]
        EventLogEntryCollection GetEventLogEntries(string logType);
        [OperationContract]
        FileSystemWatcher GetFileSystemWatcher();
        [OperationContract]
        FolderBrowserDialog GetFolderBrowserDIalog();
        [OperationContract]
        string CheckDevices();
    }
}
