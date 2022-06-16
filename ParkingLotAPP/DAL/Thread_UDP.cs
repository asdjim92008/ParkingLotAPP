using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParkingLotAPP.DAL
{
    public class Thread_UDP
    {
        UdpClient uc;
        IPEndPoint ipep;
        public Thread_UDP(string port)
        {
            
            uc = new UdpClient(int.Parse(port));
            ipep = new IPEndPoint(IPAddress.Parse("192.168.1.30"), int.Parse(port));
            uc.Client.ReceiveTimeout = 3000;
            uc.Client.SendTimeout = 3000;
        }
        public string SetCarCount(string CarCount)
        {
            CarCount = CarCount.PadLeft(4, '0');
            var b = Encoding.UTF8.GetBytes("[007S1+" + CarCount + "DD]");
            uc.Send(b, b.Length, ipep);
            string response;
            try
            {
                response = Encoding.UTF8.GetString(uc.Receive(ref ipep));
            }
            catch(Exception)
            {
                response = null;
            }
            uc.Close();
            return (response == "(007S11B)") ? CarCount : "connect fail";
        }
        public string GetCarCount() 
        {
            var b = Encoding.UTF8.GetBytes("[007EDD]");
            uc.Send(b, b.Length, ipep);
            string response;
            try
            {
                response = Encoding.UTF8.GetString(uc.Receive(ref ipep));
            }
            catch (Exception)
            {
                response = null;
            }
            uc.Close();
            return (response != null) ? response.Substring(6, 4) : "connect fail";
        }
        public string OpenFence() 
        {
            //未完成的柵欄指令
            var b = Encoding.UTF8.GetBytes("[1A1]");
            uc.Send(b, b.Length, ipep);
            string response;
            try
            {
                response = Encoding.UTF8.GetString(uc.Receive(ref ipep));
            }
            catch (Exception)
            {
                response = null;
            }
            uc.Close();
            return (response != null) ? response : "connect fail";
        }
    }
}
