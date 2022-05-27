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
    
    public class AuthorizeController : Controller
    {
        Manager_SQL manager_SQL = new Manager_SQL("192.168.1.46", "3306", "parkinglist", "rotamoon", "rotamoon90692052");

        
        /*  <目的>    登入後回傳可管理的停車場和公司名   </目的>
         *  <參數>    
         *            參數1 使用者帳號:  Account
         *            參數2 使用者密碼:  Password
         *  </參數>
         *  <路徑>    "/api/Authorize"
         *  <回傳>    parkingGuid(停車場guid)    parkingNo(停車場編號)    parkingName(停車場名稱)  companyName(公司名稱  </回傳>*/
        [HttpPost]
        public ActionResult Login(string Account,string Password)
        {
            LoginResponse response;
            var session1 = HttpContext.Session.GetObjectFromJson<Manager>("sessionManger");

            //已登入的回傳
            if (session1!=null && session1.Account==Account && session1.Password==Password)
            {
                response = new LoginResponse { Code = "201", ErrMsg = "已登入" };
            }
            else
            {
                //建立儲存session的物件
                Manager sessionManger = new Manager();
                HttpContext.Session.SetObjectAsJson("sessionManger", sessionManger);

                var manager = manager_SQL.Login(Account, Password);

                //登入成功
                if (manager != null)
                {
                    session1 = manager;
                    HttpContext.Session.SetObjectAsJson("sessionManger", session1);
                    var parkinglotlist = manager_SQL.GetParkingLotLists(session1.SysGuid);

                    response = new LoginResponse { Code = "200", ErrMsg = "", Data = parkinglotlist ,CompanyName=session1.CompanyName};
                }
                //登入失敗
                else
                {
                    response = new LoginResponse { Code = "400", ErrMsg = "帳號密碼錯誤" };
                    Logout();
                }
            }

            return Json(response);
        }
        
        /*  <目的>    取得所選停車場的授權資料   </目的>
         *  <參數>    
         *            參數1 停車場guid:  parkingGuid
         *  </參數>
         *  <路徑>    "/api/Authorize"
         *  <回傳>    parkingGuid(停車場guid)    parkingNo(停車場編號)    parkingName(停車場名稱)  companyName(公司名稱)  </回傳>*/
        [HttpGet]
        public ActionResult GetAuthorizeData(string parkingGuid)
        {
            Response response;
            var session1 = HttpContext.Session.GetObjectFromJson<Manager>("sessionManger");

            if (session1 != null && parkingGuid != null)
            {
                var authorizeData = manager_SQL.GetParkingLotInfo(parkingGuid);
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
         *  <路徑>    "/api/Authorize/logout"
         *  <回傳>    訊息  </回傳>*/
        [HttpGet("logout")]
        public ActionResult Logout()
        {
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
        

    }
}
