using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingLotAPP.Models
{
    public class DataModel
    {
        public class Response
        {
            public string Code { get; set; }
            public string ErrMsg { get; set; }
            public Object Data { get; set; }
        }
        public class LoginResponse
        {
            public string Code { get; set; }
            public string ErrMsg { get; set; }
            public Object Data { get; set; }
            public string CompanyName { get; set; }
        }
        public class Manager
        {
            public string SysGuid { get; set; }
            public string Account { get; set; }
            public string Password { get; set; }
            public string CompanyGuid { get; set; }
            public string UserName { get; set; }
            public string CompanyName { get; set; }
            public string Permision { get; set; }
        }
        public class ParkingLotInfo 
        { 
            public string ParkingGuid { get; set; }
            public string CompanyGuid { get; set; }
            public string ParkingName { get; set; }
            public string ParkingNo { get; set; }
            public string ParkingType { get; set; }
            public string SQLIP { get; set; }
            public string SQLPort { get; set; }
            public string SQLAPort { get; set; }
            public string SQLAPSPort { get; set; }
            public string SQLAPSPort2 { get; set; }
            public string SQLAPSPort3 { get; set; }
            public string SQLAPSPort4 { get; set; }
            public string SQLAccount { get; set; }
            public string SQLPassword { get; set; }
            public string SQLDBName { get; set; }
            public string FTPIP { get; set; }
            public string FTPAccount { get; set; }
            public string FTPPassword { get; set; }
            public string LedPort { get; set; }
            public string LedPort1 { get; set; }
            public string LedPort2 { get; set; }
            public string LedPort3 { get; set; }
            public string EntracePort { get; set; }
            public string ExitPort { get; set; }
            public string Status { get; set; }
        }
        public class ParkingLotList
        {
            public string ParkingGuid { get; set; }
            public string ParkingNo { get; set; }
            public string ParkingName { get; set; }
            
        }
        public class CarInfo
        {
            public string YMDHM { get; set; }
            public string PLATENUM { get; set; }
            public string TICKNO { get; set; }
            public string JPG { get; set; }

        }
        public class DBlog
        {
            public string MANAGER { get; set; }
            public string LOG { get; set; }
            public string TIME { get; set; }

        }
        

    }
    

}
