using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Net.NetworkInformation;

namespace AuditConsole
{
    class Program
    {
        static ServiceHost host = null;
        public static MonServer server = null;
        static CNetworkCardDesc NetworkCardForServerDesc = null;
        static void Main(string[] args)
        {
            foreach (NetworkInterface NetInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (NetInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
                    NetInterface.NetworkInterfaceType == NetworkInterfaceType.FastEthernetT ||
                    NetInterface.NetworkInterfaceType == NetworkInterfaceType.GigabitEthernet ||
                    NetInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                {
                    NetworkCardForServerDesc = new CNetworkCardDesc(NetInterface);
                }
            }
            server = new MonServer(NetworkCardForServerDesc.CardIPAddress.GetAddressBytes());

            CreateServiceHost();

            Console.ReadLine();
        }

        static void CreateServiceHost()
        {
            try
            {
                byte[] addr = NetworkCardForServerDesc.CardIPAddress.GetAddressBytes();
                string ipAddr = addr[0] + "." + addr[1] + "." + addr[2] + "." + addr[3];
                Uri address = new Uri("net.tcp://" + ipAddr + ":41993/IMonService/");

                NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);

                Type contract = typeof(IMonService);

                host = new ServiceHost(typeof(MonService));

                host.AddServiceEndpoint(contract, binding, address);

                host.Open();

                Console.WriteLine("Сервер запущен!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void CloseServiceHost()
        {
            try
            {
                if (host != null)
                {
                    host.Close();

                    Console.WriteLine("Сервер остановлен!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
