using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AuditConsole
{
    public class DeviceChangeNotifier : Form
    {
        public delegate void DeviceNotifyDelegate(Message msg);
        public static event DeviceNotifyDelegate DeviceNotify;
        private static DeviceChangeNotifier mInstance;

        //-----------------------------------------------------------------------------------
        // отслеживание изменения списка оборудования 
        //-----------------------------------------------------------------------------------

        const int WM_DEVICECHANGE = 0x0219;
        const int DBT_DEVICEARRIVAL = 0x8000;
        const int DBT_DEVICEREMOVECOMPLETE = 0x8004;
        const int DBT_DEVTYP_VOLUME = 0x00000002;

        static string log = "";

        public static string Log
        {
            get
            {
                return log;
            }
            set
            {
                log = value;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DevBroadcastVolume
        {
            public int Size;
            public int DeviceType;
            public int Reserved;
            public int Mask;
            public Int16 Flags;
        }
        public static void Start()
        {
            Thread t = new Thread(runForm);
            t.SetApartmentState(ApartmentState.STA);
            t.IsBackground = true;
            t.Start();
        }
        public static void Stop()
        {
            if (mInstance == null) throw new InvalidOperationException("Notifier not started");
            DeviceNotify = null;
            mInstance.Invoke(new MethodInvoker(mInstance.endForm));
        }
        private static void runForm()
        {
            Application.Run(new DeviceChangeNotifier());
        }

        private void endForm()
        {
            this.Close();
        }
        protected override void SetVisibleCore(bool value)
        {
            // Prevent window getting visible
            if (mInstance == null) CreateHandle();
            mInstance = this;
            value = false;
            base.SetVisibleCore(value);
        }
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_DEVICECHANGE)
            {
                int EventCode = m.WParam.ToInt32();
                switch (EventCode)
                {
                    case DBT_DEVICEARRIVAL:
                        {
                            int devType = Marshal.ReadInt32(m.LParam, 4);
                            if (devType == DBT_DEVTYP_VOLUME)
                            {
                                DevBroadcastVolume vol;
                                vol = (DevBroadcastVolume)Marshal.PtrToStructure(m.LParam,
                                   typeof(DevBroadcastVolume));
                                WriteLog(string.Format("Подключено новое устройство:  маска = {0}", vol.Mask));
                            }
                            break;
                        }
                    case DBT_DEVICEREMOVECOMPLETE:
                        {
                            int devType = Marshal.ReadInt32(m.LParam, 4);
                            if (devType == DBT_DEVTYP_VOLUME)
                            {
                                DevBroadcastVolume vol;
                                vol = (DevBroadcastVolume)Marshal.PtrToStructure(m.LParam,
                                   typeof(DevBroadcastVolume));
                                WriteLog(string.Format("Удалено устройство:  маска = {0}", vol.Mask));
                            }
                            break;
                        }
                }
            }
            base.WndProc(ref m);
        }

        void WriteLog(string s)
        {
            /*listBox1.Items.Add(s);
            listBox1.SelectedIndex = listBox1.Items.Count - 1;*/
            log += s;
        }
    }
}
