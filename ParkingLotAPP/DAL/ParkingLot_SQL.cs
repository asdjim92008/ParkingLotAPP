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
        public List<DataModel.CarInfo> GetAlljpg()
        {
            try
            {
                using (var cn = new MySqlConnection(base._cnStr))
                {
                    string sql = $"select YMDHM,PLATENUM,TICKNO,JPGFILE as JPG from parkingpay order by YMDHM desc";
                    var list = cn.Query<DataModel.CarInfo>(sql).ToList();
                    return list;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /*public DataModel.CarInfo GetCarInfo(string JPGFILE)
        {
            try
            {
                using (var cn = new MySqlConnection(base._cnStr))
                {
                    var dynamicParams = new DynamicParameters();//←動態參數
                    dynamicParams.Add("JPGFILE", JPGFILE);
                    string sql = $"select YMDHM, PLATENUM, TICKNO from parkingpay where JPGFILE=@JPGFILE order by YMDHM";
                    
                    var list = cn.Query<DataModel.CarInfo>(sql, dynamicParams).ToList();
                    return list.FirstOrDefault() ;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }*/
        public string ChangePlateNum(string N_PlateNum,string C_PlateNum)
        {
            try
            {
                 using(var cn=new MySqlConnection(base._cnStr))
                {
                    var dynamicParams = new DynamicParameters();//←動態參數
                    dynamicParams.Add("N_PlateNum", N_PlateNum);
                    dynamicParams.Add("C_PlateNum", C_PlateNum);
                    string sql = $"select * from parkingpay where PLATENUM=@C_PlateNum";
                    var check = cn.Query(sql, dynamicParams);
                    if (check.Count() ==0)
                    {
                        sql = $"update parkingpay set PLATENUM =@C_PlateNum where PLATENUM=@N_PlateNum ";

                        var list = cn.Execute(sql, dynamicParams);
                        if (list == 0)
                        {
                            return "No PlateNumber";
                        }
                        return "From " + N_PlateNum + " To " + C_PlateNum + " update sucess";
                    }
                    return "The New Platenum has exist";
                    
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public string InsertPlateNum(string Pankno,string PlateNum,string YMHDM)
        {
            try
            {
                using(var cn=new MySqlConnection(base._cnStr))
                {
                    var dynamicParams = new DynamicParameters();//←動態參數
                    dynamicParams.Add("Pankno", Pankno);
                    dynamicParams.Add("PlateNum", PlateNum);
                    dynamicParams.Add("YMHDM", YMHDM);
                    string sql = $"insert ignore  into parkingpay( PANKNO, RID, PLATENUM, YMDHM) values (@Pankno,'001',@PlateNum,@YMHDM)";
                    var list = cn.Execute(sql,dynamicParams);
                    if (list == 0)
                    {
                        return "Repeat, Insert fail";
                    }
                    return "PlateNum " + PlateNum + " Insert success " ;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public void InsertLog(string Manager,string Log)
        {
            try
            {
                using(var cn= new MySqlConnection(base._cnStr))
                {
                    string now = DateTime.Now.ToString("yyyyMMdd");
                    now += DateTime.Now.ToString("HHmmss");
                    var dynamicParams = new DynamicParameters();
                    dynamicParams.Add("Manager",Manager);
                    dynamicParams.Add("Log", Log);
                    dynamicParams.Add("Time", now);
                    string sql = $"insert  into parkinglog(MANAGER, LOG, TIME) values (@Manager,@Log,@Time)";
                    var list = cn.Execute(sql, dynamicParams);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
