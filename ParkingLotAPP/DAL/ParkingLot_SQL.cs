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
        public List<DataModel.CarInfo> GetFivejpg(int start,string searchTime,string plateNum)
        {
            try
            {
                using (var cn = new MySqlConnection(base._cnStr))
                {
                    var dynamicParams = new DynamicParameters();//←動態參數
                    dynamicParams.Add("START", start);
                    dynamicParams.Add("YMDHM", searchTime+"%");
                    dynamicParams.Add("PLATENUM", "%"+plateNum);
                    string sql;
                    switch (Judge(searchTime, plateNum))
                    {
                        case 0:     //無搜尋時間，無搜尋車牌
                            sql= $"select DISTINCT RID,YMDHM,PLATENUM,TICKNO,JPGFILE as JPG from parkingpay inner join " +
                            $"(select YMDHM from parkingpay order by YMDHM desc limit @START,5)b using (YMDHM) ORDER BY YMDHM desc";
                            break;
                        case 1:     //有搜尋時間，無搜尋車牌
                            sql = $"select DISTINCT RID,YMDHM,PLATENUM,TICKNO,JPGFILE as JPG from parkingpay inner join " +
                            $"(select YMDHM from parkingpay where YMDHM like @YMDHM order by YMDHM desc limit @START,5)b using (YMDHM) ORDER BY YMDHM desc";
                            break;
                        case 2:     //無搜尋時間，有搜尋車牌
                            sql= $"select DISTINCT RID,YMDHM,PLATENUM,TICKNO,JPGFILE as JPG from parkingpay inner join " +
                            $"(select YMDHM from parkingpay where PLATENUM like @PLATENUM order by YMDHM desc limit @START,5)b using (YMDHM) ORDER BY YMDHM desc";
                            break;
                        default:    //有搜尋時間，有搜尋車牌
                            sql = $"select DISTINCT RID,YMDHM,PLATENUM,TICKNO,JPGFILE as JPG from parkingpay inner join " +
                            $"(select YMDHM from parkingpay where PLATENUM like @PLATENUM and YMDHM like @YMDHM order by YMDHM desc limit @START,5)b using (YMDHM) ORDER BY YMDHM desc";
                            break;
                    }
                    var list = cn.Query<DataModel.CarInfo>(sql,dynamicParams).ToList();
                    return list;
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
                    string sql = $"select TICKNO from parkingpay order by TICKNO desc limit 1";
                    var temp = cn.Query<string>(sql).FirstOrDefault();
                    temp = (temp == null) ? "000001" : (int.Parse(temp)+1).ToString().PadLeft(6, '0');
                    var dynamicParams = new DynamicParameters();//←動態參數
                    dynamicParams.Add("Pankno", Pankno);
                    dynamicParams.Add("PlateNum", PlateNum);
                    dynamicParams.Add("YMHDM", YMHDM);
                    dynamicParams.Add("TICKNO", temp);
                    sql = $"insert ignore  into parkingpay( PANKNO, RID, PLATENUM, YMDHM, TICKNO) values (@Pankno,'001',@PlateNum,@YMHDM,@TICKNO)";
                    var list = cn.Execute(sql,dynamicParams);
                    if (list == 0)
                    {
                        return "Exist, Insert fail";
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
        public List<DataModel.DBlog> GetLogs(int start,string searchTime,string manager)
        {
            try
            {
                using (var cn = new MySqlConnection(base._cnStr))
                {
                    var dynamicParams = new DynamicParameters();//←動態參數
                    dynamicParams.Add("START", start);
                    dynamicParams.Add("TIME", searchTime+"%");
                    dynamicParams.Add("MANAGER", manager);
                    string sql;
                    switch (Judge(searchTime, manager))
                    {
                        case 0:     //無搜尋時間，無搜尋管理人
                            sql = $"select DISTINCT * from parkinglog inner join " +
                            $"(select TIME from parkinglog order by TIME desc limit @START,10)b using (TIME) ORDER BY TIME desc";
                            break;
                        case 1:     //有搜尋時間，無搜尋管理人
                            sql = $"select DISTINCT * from parkinglog inner join " +
                            $"(select TIME from parkinglog where TIME like @TIME order by TIME desc limit @START,10)b using (TIME) ORDER BY TIME desc";
                            break;
                        case 2:     //無搜尋時間，有搜尋管理人
                            sql = $"select DISTINCT * from parkinglog inner join " +
                            $"(select TIME from parkinglog where MANAGER = @MANAGER order by TIME desc limit @START,10)b using (TIME) ORDER BY TIME desc";
                            break;
                        default:    //有搜尋時間，有搜尋管理人
                            sql = $"select DISTINCT * from parkinglog inner join " +
                            $"(select TIME from parkinglog where MANAGER = @MANAGER and TIME like @TIME order by TIME desc limit @START,10)b using (TIME) ORDER BY TIME desc";
                            break;
                    }
                    
                    var list = cn.Query<DataModel.DBlog>(sql, dynamicParams).ToList();
                    return list;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        public int Judge(string s1,string s2)
        {
            if (s1 == null && s2 == null) return 0;
            else if (s1 != null && s2 == null) return 1;
            else if (s1 == null && s2 != null) return 2;
            else return 3;
        }
        
    }
}
