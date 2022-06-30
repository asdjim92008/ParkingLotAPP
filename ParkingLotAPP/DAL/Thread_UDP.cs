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
        public Thread_UDP(string ip,string port)
        {
            
            uc = new UdpClient(int.Parse(port));
            ipep = new IPEndPoint(IPAddress.Parse(ip), int.Parse(port));
            uc.Client.ReceiveTimeout = 3000;
            uc.Client.SendTimeout = 3000;
        }
        
        public string SetCarCount(string CarCount)
        {
            CarCount = CarCount.PadLeft(4, '0');
            var b = Encoding.UTF8.GetBytes("[007S1+" + CarCount + "DD]");
            try
            {
                uc.Send(b, b.Length, ipep);
                uc.Close();
                return "設定車位數 " + CarCount + " 訊號已送出";
            }
            catch (Exception)
            {
                uc.Close();
                return "connect fail";
            }
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
        public string OpenFence(string num) 
        {
            //未完成的柵欄指令
            /*
            [0016312B]  入口開門
            [0016322C]  入口關門
            [0026312C]  出口開門
            [0026322D]  出口關門
            */
            char x = (char)('A' + int.Parse(num));
            string cmd = "[" + num + "6312" + x + "]";
            var b = Encoding.UTF8.GetBytes(cmd);
            try
            {
                uc.Send(b, b.Length, ipep);
                uc.Close();
                return "開啟訊號已送出";
            }
            catch (Exception)
            {
                uc.Close();
                return "connect fail";
            }
            
        }
    }
}
