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
                    string sql = $"select * from sysappuser where Account='{Account}' and Password='{Password}'";
                    var list = cn.Query<DataModel.Manager>(sql).ToList();
                    return list.FirstOrDefault();
                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataModel.ParkingLotInfo GetParkingLotInfo(string ParkingGuid) 
        {
            try
            {
                using (var cn = new MySqlConnection(base._cnStr))
                {
                    string sql = $"SELECT * FROM parking where ParkingGuid='{ParkingGuid}'";
                        
                    var list = cn.Query<DataModel.ParkingLotInfo>(sql).ToList();
                    return list.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<DataModel.ParkingLotList> GetParkingLotLists(string SysGuid)
        {
            try
            {
                using (var cn = new MySqlConnection(base._cnStr))
                {
                    string sql = $"SELECT parking.ParkingGuid,parking.ParkingName FROM (SELECT * FROM userparking " +
                        $"WHERE SysGuid = '{SysGuid}')b " +
                        $"LEFT join parking ON parking.ParkingGuid = b.ParkingGuid";
                    var list = cn.Query<DataModel.ParkingLotList>(sql).ToList();
                    return list;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        
    }
}
