using Dapper;
using MySql.Data.MySqlClient;
using ParkingLotAPP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ParkingLotAPP.Models.DataModel;

namespace ParkingLotAPP.DAL
{
    public class ParkingLot_SQL : Dapper_SQL
    {
        public ParkingLot_SQL(string IP, string Port, string DBName, string acc, string pwd) : base(IP, Port, DBName, acc, pwd)
        {
            
        }
        public List<CarInfo> GetFivejpg(int start,string searchTime,string plateNum)
        {
            try
            {
                using (var cn = new MySqlConnection(base._cnStr))
                {
                    var dynamicParams = new DynamicParameters();//←動態參數
                    dynamicParams.Add("START", start);
                    dynamicParams.Add("YMDHM", searchTime+"%");
                    dynamicParams.Add("PLATENUM", "%"+plateNum + "%");
                    string sql = Judge(searchTime, plateNum) switch
                    {
                        //無搜尋時間，無搜尋車牌
                        0 => $"select DISTINCT RID,YMDHM,PLATENUM,TICKNO,JPGFILE as JPG from parkingpay inner join " +
                                                    $"(select YMDHM from parkingpay order by YMDHM desc limit @START,5)b using (YMDHM) ORDER BY YMDHM desc",
                        //有搜尋時間，無搜尋車牌
                        1 => $"select DISTINCT RID,YMDHM,PLATENUM,TICKNO,JPGFILE as JPG from parkingpay inner join " +
                                                    $"(select YMDHM from parkingpay where YMDHM like @YMDHM order by YMDHM desc limit @START,5)b using (YMDHM) ORDER BY YMDHM desc",
                        //無搜尋時間，有搜尋車牌
                        2 => $"select DISTINCT RID,YMDHM,PLATENUM,TICKNO,JPGFILE as JPG from parkingpay inner join " +
                                                    $"(select YMDHM from parkingpay where PLATENUM like @PLATENUM order by YMDHM desc limit @START,5)b using (YMDHM) ORDER BY YMDHM desc",
                        //有搜尋時間，有搜尋車牌
                        _ => $"select DISTINCT RID,YMDHM,PLATENUM,TICKNO,JPGFILE as JPG from parkingpay inner join " +
                                                    $"(select YMDHM from parkingpay where PLATENUM like @PLATENUM and YMDHM like @YMDHM order by YMDHM desc limit @START,5)b using (YMDHM) ORDER BY YMDHM desc",
                    };
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
        public string InsertPlateNum(string Pankno,string PlateNum,string Rid,string YMHDM,string TickNo,string TickClass)
        {
            try
            {
                using(var cn=new MySqlConnection(base._cnStr))
                {
                    Rid = (Rid == null) ? "001" : Rid;
                    var dynamicParams = new DynamicParameters();//←動態參數
                    dynamicParams.Add("Pankno", Pankno);
                    dynamicParams.Add("PlateNum", PlateNum);
                    dynamicParams.Add("Rid", Rid);
                    dynamicParams.Add("YMHDM", YMHDM);
                    dynamicParams.Add("TICKCLASS", TickClass);
                    dynamicParams.Add("TICKNO", TickNo);
                    string sql = $"insert ignore  into parkingpay( PANKNO, RID, PLATENUM, YMDHM, TICKNO,TICKCLASS) values (@Pankno,@Rid,@PlateNum,@YMHDM,@TICKNO,@TICKCLASS)";
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
        public DefaultInfo DefaultInfo(string pankno)
        {
            try
            {
                using (var cn = new MySqlConnection(base._cnStr))
                {
                    string sql = $"select TICKNO from parkingpay where TICKNO like '17%' order by TICKNO desc limit 1";
                    var tickno = cn.Query<string>(sql).FirstOrDefault();
                    tickno = (tickno == null) ? "170001" : (int.Parse(tickno) + 1).ToString().PadLeft(6, '0');
                    return new DefaultInfo { PANKNO = pankno, RID = "001", TICKNO = tickno };
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
                    
        }
        public List<Fence> GetAllFence() 
        {
            try
            {
                using (var cn = new MySqlConnection(base._cnStr))
                {
                    string sql = $"select rid as fence_no,remark from doortab order by rid asc";
                    List<Fence> fences = cn.Query<Fence>(sql).ToList();
                    return fences;
                }
            }
            catch (Exception ex)
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
        public List<DBlog> GetLogs(int start,string searchTime,string manager)
        {
            try
            {
                using (var cn = new MySqlConnection(base._cnStr))
                {
                    var dynamicParams = new DynamicParameters();//←動態參數
                    dynamicParams.Add("START", start);
                    dynamicParams.Add("TIME", searchTime+"%");
                    dynamicParams.Add("MANAGER", "%" + manager + "%");
                    string sql = Judge(searchTime, manager) switch
                    {
                        //無搜尋時間，無搜尋管理人
                        0 => $"select DISTINCT * from parkinglog inner join " +
                                                    $"(select TIME from parkinglog order by TIME desc limit @START,10)b using (TIME) ORDER BY TIME desc",
                        //有搜尋時間，無搜尋管理人
                        1 => $"select DISTINCT * from parkinglog inner join " +
                                                    $"(select TIME from parkinglog where TIME like @TIME order by TIME desc limit @START,10)b using (TIME) ORDER BY TIME desc",
                        //無搜尋時間，有搜尋管理人
                        2 => $"select DISTINCT * from parkinglog inner join " +
                                                    $"(select TIME from parkinglog where MANAGER like @MANAGER order by TIME desc limit @START,10)b using (TIME) ORDER BY TIME desc",
                        //有搜尋時間，有搜尋管理人
                        _ => $"select DISTINCT * from parkinglog inner join " +
                                                    $"(select TIME from parkinglog where MANAGER like @MANAGER and TIME like @TIME order by TIME desc limit @START,10)b using (TIME) ORDER BY TIME desc",
                    };
                    var list = cn.Query<DataModel.DBlog>(sql, dynamicParams).ToList();
                    return list;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        public string OpenFence(string rid)
        {
            try
            {
                using (var cn = new MySqlConnection(base._cnStr))
                {
                    var dynamicParams = new DynamicParameters();//←動態參數
                    dynamicParams.Add("rid", rid);
                    string sql = $"update doortab set remoteopen ='Y' where rid=@rid ";
                    var check = cn.Execute(sql, dynamicParams);
                    if (check == 1)
                    {
                        return "open sucess";
                    }
                    else
                    {
                        return "open fail";
                    }
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
