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
            /*serialPort.DataBits = DataBit;
            serialPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), StopBit, true);*/
            //serialPort.Parity = Parity.no;
            //Parity.None;
            serialPort.StopBits = StopBits.One;
            serialPort.ReadTimeout = 1000;
            serialPort.WriteTimeout = 1000;
            if (!serialPort.IsOpen)
            {
                serialPort.Open();
            }
        }
        public string GetCarCount()
        {
            try
            {
                
                if (serialPort.IsOpen)
                {
                    serialPort.Write("[007EDD]");
                    var response = serialPort.ReadByte();
                    return "11";
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
                
                string send_data = "[007S1+";
                send_data += CarCount.PadLeft(4, '0');
                send_data += "DD]";
                Encoding iso = Encoding.GetEncoding("ISO-8859-1");
                Encoding utf8 = Encoding.UTF8;
                byte[] utfBytes = utf8.GetBytes(send_data);
                byte[] isoBytes = Encoding.Convert(utf8, iso, utfBytes);
                string msg = iso.GetString(isoBytes);
                
                serialPort.Write(utfBytes,0,14);
                //serialPort.
                return "success";
            }
            catch (InvalidOperationException ex)
            {
                throw ex;
            }
            
        }
    }
}
