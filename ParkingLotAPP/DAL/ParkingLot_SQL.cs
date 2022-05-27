using Dapper;
using MySql.Data.MySqlClient;
using ParkingLotAPP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingLotAPP.DAL
{
    public class ParkingLot_SQL : Dapper_SQL
    {
        public ParkingLot_SQL(string IP, string Port, string DBName, string acc, string pwd) : base(IP, Port, DBName, acc, pwd)
        {
            
        }
        public List<string> GetAlljpg()
        {
            try
            {
                using (var cn = new MySqlConnection(base._cnStr))
                {
                    string sql = $"select JPGFILE from parkingpay";
                    var list = cn.Query<string>(sql).ToList();
                    return list;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataModel.CarInfo GetCarInfo(string JPGFILE)
        {
            try
            {
                using (var cn = new MySqlConnection(base._cnStr))
                {
                    string sql = $"select YMDHM, PLATENUM, TICKNO from parkingpay where JPGFILE='{JPGFILE}' order by YMDHM";
                    var list = cn.Query<DataModel.CarInfo>(sql).ToList();
                    return list.FirstOrDefault() ;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string ChangePlateNum(string N_PlateNum,string C_PlateNum)
        {
            try
            {
                 using(var cn=new MySqlConnection(base._cnStr))
                {
                    string sql = $"update parkingpay set PLATENUM ='{C_PlateNum}' where PLATENUM='{N_PlateNum}'";
                    
                    var list =cn.Execute(sql);
                    if (list==0)
                    {
                        return "No PlateNiumber";
                    }
                    return "From "+N_PlateNum+" To "+C_PlateNum+" update sucess";
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
