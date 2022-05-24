using Dapper;
using MySql.Data.MySqlClient;
using ParkingLotAPP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingLotAPP.DAL
{
    public class Manager_SQL:Dapper_SQL
    {
        public Manager_SQL(string IP, string Port, string DBName, string acc, string pwd) : base(IP, Port, DBName, acc, pwd)
        {
        }
        public DataModel.Manager Login(string Account,string Password)
        {
            try
            {
                using(var cn = new MySqlConnection(base._cnStr))
                {
                    string sql = $"select Account,Password,UserName,CompanyName,Permision from sysappuser where Account='{Account}' and Password='{Password}'";
                    var list = cn.Query<DataModel.Manager>(sql).ToList();
                    return list.FirstOrDefault();
                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
