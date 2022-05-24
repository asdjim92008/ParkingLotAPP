using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingLotAPP.DAL
{
    public class Dapper_SQL
    {
        protected string _cnStr { get; set; }
        protected Dapper_SQL(string IP, string Port, string DBName, string acc, string pwd)
        {
            _cnStr = $"Server={IP};Port={Port};Database={DBName};Uid={acc};Pwd={pwd};Connection Timeout=3"; ;

        }
    }
}
