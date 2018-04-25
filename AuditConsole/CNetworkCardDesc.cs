using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace AuditConsole
{
	/// <summary>
	/// Класс, содержащий данные о сетевой карте, необходимые в программе. Данные берутся из класса NetworkInterface
	/// </summary>
	public class CNetworkCardDesc
	{
		#region Свойство Description. Аналог свойства NetworkInterface.Description. Только для чтения.
		string m_Description = null;
		/// <summary>
		/// Аналог свойства NetworkInterface.Description.
		/// </summary>
		public string Description
		{
			get { return m_Description; }
		}
		#endregion


		#region Свойство Id. Аналог свойства NetworkInterface.Id. Только для чтения.
		string m_Id = null;
		/// <summary>
		/// Аналог свойства NetworkInterface.Id.
		/// </summary>
		public string Id
		{
			get { return m_Id; }
		}
		#endregion


		#region Свойство IsConnected. Подключён ли провод к карте. Только для чтения.
		bool m_IsConnected = false;
		/// <summary>
		/// Подключён ли провод к карте. true, если NetworkInterface.OperationalStatus == OperationalStatus.Up
		/// </summary>
		public bool IsConnected
		{
			get { return m_IsConnected; }
		}
		#endregion


		#region Свойство HasDhcpServerAddresses. Пуста ли коллекция NetworkInterface.DhcpServerAddresses. Только для чтения.
		bool m_HasDhcpServerAddresses = false;
		/// <summary>
		/// Пуста ли коллекция NetworkInterface.DhcpServerAddresses
		/// </summary>
		public bool HasDhcpServerAddresses
		{
			get { return m_HasDhcpServerAddresses; }
		}
		#endregion


		#region Свойство CardIPAddress. IP адрес карты. Только для чтения.
		IPAddress m_CardIPAddress = null;
		/// <summary>
		/// IP адрес карты. Берётся первый из IPInterfaceProperties.UnicastAddresses
		/// </summary>
		public IPAddress CardIPAddress
		{
			get { return m_CardIPAddress; }
		}
		#endregion


		#region Свойство CardSubnetMask. IP адрес карты. Только для чтения.
		IPAddress m_CardSubnetMask = null;
		/// <summary>
		/// IP адрес карты. Берётся первый из IPInterfaceProperties.UnicastAddresses
		/// </summary>
		public IPAddress CardSubnetMask
		{
			get { return m_CardSubnetMask; }
		}
		#endregion


		#region Свойство IsDemoCardDesc.
		bool m_IsDemoCardDesc = false;
		/// <summary>
		/// Демонстрационный адаптор или нет
		/// </summary>
		public bool IsDemoCardDesc
		{
			get { return m_IsDemoCardDesc; }
		}
		#endregion


		#region Конструкторы
		/// <summary>
		/// Конструктор, позволяющий создать класс на основе данных о сетевом адапторе
		/// </summary>
		/// <param name="NetInterface"></param>
		public CNetworkCardDesc(NetworkInterface NetInterface)
		{
			m_Description = NetInterface.Description;
			m_Id = NetInterface.Id;
			m_IsConnected = NetInterface.OperationalStatus == OperationalStatus.Up;

			IPInterfaceProperties IpProps = NetInterface.GetIPProperties();

			m_HasDhcpServerAddresses = IpProps.DhcpServerAddresses.Count > 0;
			foreach (UnicastIPAddressInformation IPInfo in IpProps.UnicastAddresses)
			{
				if (IPInfo.Address.AddressFamily == AddressFamily.InterNetwork)
				{	/* Нам нужны только адреса IPv4 */
					m_CardIPAddress = IPInfo.Address;
					m_CardSubnetMask = IPInfo.IPv4Mask;
				}
			}
		}
		#endregion
	}
}
