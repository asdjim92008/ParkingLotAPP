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
                    var dynamicParams = new DynamicParameters();//←動態參數
                    dynamicParams.Add("Account", Account);
                    dynamicParams.Add("Password", Password);
                    string sql = $"select * from sysappuser where Account=@Account and Password=@Password";
                    return cn.Query<DataModel.Manager>(sql, dynamicParams).FirstOrDefault();

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
                    var dynamicParams = new DynamicParameters();//←動態參數
                    dynamicParams.Add("ParkingGuid", ParkingGuid);
                    string sql = $"SELECT * FROM parking where ParkingGuid=@ParkingGuid";
                    return cn.Query<DataModel.ParkingLotInfo>(sql, dynamicParams).FirstOrDefault();
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
                    var dynamicParams = new DynamicParameters();//←動態參數
                    dynamicParams.Add("SysGuid", SysGuid);
                    string sql = $"SELECT parking.ParkingGuid,parking.ParkingName,parking.ParkingNo " +
                        $"FROM (SELECT * FROM userparking WHERE SysGuid = @SysGuid)b " +
                        $"LEFT join parking ON parking.ParkingGuid = b.ParkingGuid";
                    return cn.Query<DataModel.ParkingLotList>(sql, dynamicParams).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        
    }
}
