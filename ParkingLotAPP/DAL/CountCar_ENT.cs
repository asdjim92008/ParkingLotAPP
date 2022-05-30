using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Text;

namespace ParkingLotAPP.DAL
{
    public class CountCar_ENT
    {
        SerialPort serialPort = new SerialPort();
    public CountCar_ENT(string PortName,int BaudRate,string Parity,int DataBit,string StopBit)
        {
            serialPort.PortName = PortName;
            serialPort.BaudRate = BaudRate;
            serialPort.Parity = serialPort.Parity;
            
            serialPort.StopBits = StopBits.One;
            serialPort.ReadTimeout = 5000;
            serialPort.WriteTimeout = 5000;
            if (!serialPort.IsOpen)
            {
                serialPort.Open();
            }
        }
        public string GetCarCount()
        {
            try
            {
                //取得數量命令 [007EDD]  共8位Byte
                if (serialPort.IsOpen)
                {
                    string send_data = "[007EDD]";
                    byte[] utfBytes = Encoding.UTF8.GetBytes(send_data);
                    
                    serialPort.Write(utfBytes,0,8);
                    
                    byte[] recieve_data = new byte[13];
                    var t=serialPort.BytesToRead;
                     var count = 0;
                    //接收計數板訊息
                     while (count<13)
                     {
                         var x=serialPort.ReadByte();
                         recieve_data[count] = (byte)x;
                         count++;
                     }

                    serialPort.Close();
                    var response = Encoding.UTF8.GetString(recieve_data);
                    return response.Substring(6,4);
                }
                else
                {
                    return "open fail";
                }
            }
            catch(InvalidOperationException ex)
            {
                throw ex;
            }
            
        }
        public string SetCarCount(string CarCount)
        {
            try
            {
                //設定數量命令 [007S1+0000DD]  共14位Byte
                if (serialPort.IsOpen)
                {
                    string send_data = "[007S1+";
                    send_data += CarCount.PadLeft(4, '0');
                    send_data += "DD]";

                    byte[] utfBytes = Encoding.UTF8.GetBytes(send_data);
                    serialPort.Write(utfBytes, 0, 14);

                    serialPort.Close();
                    return CarCount.PadLeft(4,'0');
                }
                else
                {
                    return "open fail";
                }
                
            }
            catch (InvalidOperationException ex)
            {
                throw ex;
            }
        }
        public string OpenEnter()
        {
            try
            {
                //開啟柵欄命令 [1A1]  共5位Byte
                if (serialPort.IsOpen)
                {
                    string send_data = "[1A1]";
                    byte[] utfBytes = Encoding.UTF8.GetBytes(send_data);
                    var recievedata = serialPort.ReadByte();
                    serialPort.Write(utfBytes, 0, 5);
                    serialPort.Close();
                    return  "success";
                }
                else
                {
                    return "open fail";
                }

            }
            catch (InvalidOperationException ex)
            {
                throw ex;
            }
        }
    }
}
