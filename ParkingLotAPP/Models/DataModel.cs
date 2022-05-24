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
        public class SessionState 
        { 
            public string Account { get; set; }
            public string UserName { get; set; }
            
        }
        public class Manager
        {
            public string Account { get; set; }
            public string Password { get; set; }
            public string UserName { get; set; }
            public string CompanyName { get; set; }
            public string Permision { get; set; }
        }
    }
    

}
