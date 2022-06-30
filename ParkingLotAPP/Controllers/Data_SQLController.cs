using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkingLotAPP.DAL;
using ParkingLotAPP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ParkingLotAPP.HELP;
using static ParkingLotAPP.Models.DataModel;

namespace ParkingLotAPP.Controllers
{
    [Route("api/[controller]")]
    
    public class Data_SQLController : Controller
    {
        /*  <目的>    登入後回傳可管理的停車場和公司名   </目的>
         *  <參數>    
         *            參數1 使用者帳號:  Account
         *            參數2 使用者密碼:  Password
         *  </參數>
         *  <路徑>    "/api/Data_SQL/Login"
         *  <回傳>    parkingGuid(停車場guid)    parkingNo(停車場編號)    parkingName(停車場名稱)  companyName(公司名稱  </回傳>*/
        [HttpPost("Login")]
        public ActionResult Login(string Account, string Password)
        {
            Manager_SQL manager_SQL = new Manager_SQL("192.168.1.47", "3306", "parkinglist", "root", "0912208000");
            LoginResponse response;
            var session1 = HttpContext.Session.GetObjectFromJson<Manager>("sessionManger");

            //已登入的回傳
            if (session1 != null && session1.Account == Account && session1.Password == Password)
            {
                response = new LoginResponse { Code = "201", ErrMsg = "已登入" };
            }
            else
            {
                //建立儲存session的物件
                Manager sessionManger = new Manager();
                HttpContext.Session.SetObjectAsJson("sessionManger", sessionManger);
                try
                {
                    var manager = manager_SQL.Login(Account, Password);
                    //登入成功
                    if (manager != null)
                    {
                        HttpContext.Session.SetObjectAsJson("sessionManger", manager);
                        var parkinglotlist = manager_SQL.GetParkingLotLists(manager.SysGuid);

                        response = new LoginResponse { Code = "200", ErrMsg = "", Data = parkinglotlist, CompanyName = manager.CompanyName };
                    }
                    //登入失敗
                    else
                    {
                        response = new LoginResponse { Code = "400", ErrMsg = "帳號密碼錯誤" };
                        Logout();
                    }
                }
                catch (Exception)
                {
                    response = new LoginResponse { Code = "403", ErrMsg = "連接資料庫逾時，請再試一次" };
                }
            }
            return Json(response);
        }



        /*  <目的>    取得所選停車場的授權資料   </目的>
         *  <參數>    
         *            參數1 停車場guid:  parkingGuid
         *  </參數>
         *  <路徑>    "/api/Data_SQL/AuthorizeData"
         *  <回傳>    parkingGuid(停車場guid)    parkingNo(停車場編號)    parkingName(停車場名稱)  companyName(公司名稱)  </回傳>*/
        [HttpGet("AuthorizeData")]
        public ActionResult GetAuthorizeData(string parkingGuid)
        {
            Manager_SQL manager_SQL = new Manager_SQL("192.168.1.47", "3306", "parkinglist", "root", "0912208000");
            Response response;
            var session1 = HttpContext.Session.GetObjectFromJson<Manager>("sessionManger");

            if (session1 != null && parkingGuid != null)
            {
                var authorizeData = manager_SQL.GetParkingLotInfo(parkingGuid);
                //SQL IP為UDP IP
                authorizeData.Led_info = manager_SQL.GetLedPort(authorizeData.SQLIP);
                authorizeData.Fence_info = manager_SQL.GetFencePort(authorizeData.SQLIP);

                HttpContext.Session.SetObjectAsJson("sessionParkInfo", authorizeData);
                response = new Response { Code = "200", ErrMsg = "", Data = authorizeData };
                return Json(response);
            }
            else
            {
                response = new Response { Code = "402", ErrMsg = "操作逾時，請重新登入", Data = null };
                return Json(response);
            }
        }



        /*  <目的>    登出並將SESSION清空   </目的>
         *  <路徑>    "/api/Data_SQL/Logout"
         *  <回傳>    訊息  </回傳>*/
        [HttpGet("Logout")]
        public ActionResult Logout()
        {
            Manager_SQL manager_SQL = new Manager_SQL("192.168.1.46", "3306", "parkinglist", "rotamoon", "rotamoon90692052");
            var session1 = HttpContext.Session.GetObjectFromJson<Manager>("sessionManger");
            if (session1 != null)
            {
                HttpContext.Session.Clear();
                Response response = new Response { Code = "200", ErrMsg = "", Data = "登出成功" };
                return Json(response);
            }
            else
            {
                Response response = new Response { Code = "403", ErrMsg = "尚未登入", Data = null };
                return Json(response);
            }
        }



        /*  <目的>    依照所選的頁面回傳5個停車場車輛的資訊   </目的>
         *  <參數>    
         *            參數1 停車場guid:  parkingGuid 
         *            參數2 頁數:  page
         *            參數3 搜尋時間: searchTime (選用) ,(格式 ex:2022年5月10號 10時11分12秒 => 20220510101112 可不完整)
         *            參數4 搜尋車牌: plateNum (選用) ,(格式無'-')
         *  </參數>
         *  <路徑>    "/api/Data_SQL/Logout"
         *  <回傳>    rid(車道) ymhdm(進場時間) platenum(車牌) tickno(票卡序號) jpg(圖片)   </回傳>
         *  <使用方式>  <img src="data:image/jpg;base64,jpg">    </使用方式> */
        [HttpGet("FtpFile")]
        public ActionResult FTP_GetFile(string parkingGuid, string page, string searchTime, string plateNum)
        {
            Response response;
            var getParkingLotInfo = Verify(parkingGuid);
            if (getParkingLotInfo != null)
            {
                int start = (int.Parse(page) - 1) * 5;
                
                ParkingLot_SQL parkingLot_SQL = new ParkingLot_SQL(getParkingLotInfo.SQLIP, getParkingLotInfo.SQLPort, getParkingLotInfo.SQLDBName, getParkingLotInfo.SQLAccount, getParkingLotInfo.SQLPassword);
                var carsinfo = parkingLot_SQL.GetFivejpg(start,searchTime,plateNum);
                
                

                ParkingLot_FTP parkingLot_FTP = new ParkingLot_FTP(getParkingLotInfo.FTPIP, getParkingLotInfo.FTPAccount, getParkingLotInfo.FTPPassword);
                carsinfo.ForEach(n=>n.JPG=parkingLot_FTP.FileDownloadAsync(n.JPG).Result);
                
                response = new Response { Code = "200", ErrMsg = "", Data = carsinfo };
            }
            else
            {
                response = new Response { Code = "402", ErrMsg = "操作逾時，請重新登入", Data = null };
            }
            return Json(response);
        }



        /*  <目的>    修改錯誤車牌   </目的>
         *  <參數>    
         *            參數1 停車場guid:  parkingGuid 
         *            參數2 原車牌:  n_PlateNum
         *            參數3 更改車牌: c_PlateNum   
         *  </參數>
         *  <路徑>    "/api/Data_SQL/ChangePlateNum"
         *  <回傳>    訊息   </回傳>*/
        [HttpPost("ChangePlateNum")]
        public ActionResult ChangePlateNum(string parkingGuid, string n_PlateNum, string c_PlateNum)
        {
            Response response;
            var getParkingLotInfo = Verify(parkingGuid);
            if (getParkingLotInfo != null)
            {
                ParkingLot_SQL parkingLot_SQL = new ParkingLot_SQL(getParkingLotInfo.SQLIP, getParkingLotInfo.SQLPort, getParkingLotInfo.SQLDBName, getParkingLotInfo.SQLAccount, getParkingLotInfo.SQLPassword);
                var x = parkingLot_SQL.ChangePlateNum(n_PlateNum, c_PlateNum);
                if (x != "From " + n_PlateNum + " To " + c_PlateNum + " update sucess")
                {
                    response = new Response { Code = "404", ErrMsg = x };
                }
                else
                {
                    var manager = HttpContext.Session.GetObjectFromJson<Manager>("sessionManger");
                    parkingLot_SQL.InsertLog(manager.Account, "修改車號" + n_PlateNum + " TO " + c_PlateNum);
                    response = new Response { Code = "200", ErrMsg = "", Data = x };
                }
            }
            else
            {
                response = new Response { Code = "402", ErrMsg = "操作逾時，請重新登入", Data = null };
            }
            return Json(response);
        }



        /*  <目的>    新增車牌   </目的>
         *  <參數>    
         *            參數1 停車場guid:  parkingGuid 
         *            參數2 車牌:  plateNum ,格式無'-'
         *            參數3 車道編號: rid,格式ex:001
         *            參數4 進場時間: ymhdm  ,(格式 ex:2022年5月10號 10時11分12秒 => 20220510101112) 
         *            參數5 費率別: tickclass ,(1,2,3)
         *  </參數>
         *  <路徑>    "/api/Data_SQL/InsertPlateNum"
         *  <回傳>    訊息   </回傳>*/
        [HttpPost("InsertPlateNum")]
        public ActionResult InsertPlateNum(string parkingGuid, string plateNum,string rid, string ymhdm,string tickno,string tickclass)
        {
            Response response;
            var getParkingLotInfo = Verify(parkingGuid);
            if (getParkingLotInfo != null)
            {
                ParkingLot_SQL parkingLot_SQL = new ParkingLot_SQL(getParkingLotInfo.SQLIP, getParkingLotInfo.SQLPort, getParkingLotInfo.SQLDBName, getParkingLotInfo.SQLAccount, getParkingLotInfo.SQLPassword);
                var x = parkingLot_SQL.InsertPlateNum(getParkingLotInfo.ParkingNo, plateNum,rid, ymhdm, tickno, tickclass);
                if (x == "Exist, Insert fail")
                {
                    response = new Response { Code = "404", ErrMsg = x };
                }
                else
                {
                    var manager = HttpContext.Session.GetObjectFromJson<Manager>("sessionManger");
                    parkingLot_SQL.InsertLog(manager.Account, "新增車號" + plateNum);
                    response = new Response { Code = "200", ErrMsg = "", Data = x };
                }
            }
            else
            {
                response = new Response { Code = "402", ErrMsg = "操作逾時，請重新登入" };
            }
            return Json(response);
        }


        
        /*  <目的>    依照所選的頁面回傳10個操作紀錄   </目的>
         *  <參數>    
         *            參數1 停車場guid:  parkingGuid 
         *            參數2 頁數:  page
         *            參數3 搜尋時間: searchTime (選用) ,(格式 ex:2022年5月10號 10時11分12秒 => 20220510101112 可不完整)
         *  </參數>
         *  <路徑>    "/api/Data_SQL/DBlog"
         *  <回傳>    manager(管理員名稱) log(修改資訊) time(修改時間)   </回傳>
         */
        [HttpGet("DBlog")]
        public ActionResult GetLogs(string parkingGuid , string page, string searchTime,string manager)
        {
            Response response;
            var getParkingLotInfo = Verify(parkingGuid);
            if (getParkingLotInfo != null)
            {
                int start = (int.Parse(page) - 1) * 5;
                ParkingLot_SQL parkingLot_SQL = new ParkingLot_SQL(getParkingLotInfo.SQLIP, getParkingLotInfo.SQLPort, getParkingLotInfo.SQLDBName, getParkingLotInfo.SQLAccount, getParkingLotInfo.SQLPassword);
                var x = parkingLot_SQL.GetLogs(start, searchTime,manager);
                response = new Response { Code = "200", ErrMsg = "", Data = x };
            }
            else
            {
                response = new Response { Code = "402", ErrMsg = "操作逾時，請重新登入" };
            }
            return Json(response);
        }



        /*  <目的>    取得新增車牌時預設的資料   </目的>
         *  <參數>    
         *            參數1 停車場guid:  parkingGuid 
         *            
         *  </參數>
         *  <路徑>    "/api/Data_SQL/DefaultInfo"
         *  <回傳>    parkno(停車場場號) tickno(最新票號+1) rid(車道編號)   </回傳>*/
        [HttpGet("DefaultInfo")]
        public ActionResult GetDefaultInfo(string parkingGuid)
        {
            Response response;
            var getParkingLotInfo = Verify(parkingGuid);
            if (getParkingLotInfo != null)
            {
                ParkingLot_SQL parkingLot_SQL = new ParkingLot_SQL(getParkingLotInfo.SQLIP, getParkingLotInfo.SQLPort, getParkingLotInfo.SQLDBName, getParkingLotInfo.SQLAccount, getParkingLotInfo.SQLPassword);
                var x = parkingLot_SQL.DefaultInfo(getParkingLotInfo.ParkingNo);
                response = new Response { Code = "200", ErrMsg = "", Data = x };
            }
            else
            {
                response = new Response { Code = "402", ErrMsg = "操作逾時，請重新登入" };
            }
            return Json(response);
        }



        /*  <目的>    取得所有柵欄資訊   </目的>
         *  <參數>    
         *            參數1 停車場guid:  parkingGuid 
         *            
         *  </參數>
         *  <路徑>    "/api/Data_SQL/GetFence"
         *  <回傳>    fence_no(柵欄編號) fence_port(無作用) remark(柵欄註記)   </回傳>*/
        [HttpGet("GetFence")]
        public ActionResult GetFence(string parkingGuid) 
        {
            Response response;
            var getParkingLotInfo = Verify(parkingGuid);
            if (getParkingLotInfo != null)
            {
                if (getParkingLotInfo.ParkingType == "CT")
                {
                    response = new Response { Code = "200", ErrMsg = "", Data = getParkingLotInfo.Fence_info.ToList() };
                }
                else
                {
                    ParkingLot_SQL parkingLot_SQL = new ParkingLot_SQL(getParkingLotInfo.SQLIP, getParkingLotInfo.SQLPort, getParkingLotInfo.SQLDBName, getParkingLotInfo.SQLAccount, getParkingLotInfo.SQLPassword);
                    response = new Response { Code = "200", ErrMsg = "", Data = parkingLot_SQL.GetAllFence() };
                }
            }
            else
            {
                response = new Response { Code = "402", ErrMsg = "操作逾時，請重新登入" };
            }
            return Json(response);
        }





        /*  <目的>    驗證SESSION 並取得停車場的授權資料   </目的>
         */
        public ParkingLotInfo Verify(string key)
        {
            var x = HttpContext.Session.GetObjectFromJson<ParkingLotInfo>("sessionParkInfo");
            if (x != null && x.ParkingGuid == key)
            {
                return HttpContext.Session.GetObjectFromJson<ParkingLotInfo>("sessionParkInfo");
            }
            else
            {
                return null;
            }
        }
    }
}
