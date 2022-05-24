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
        [HttpPost]
        public ActionResult Login(string Account,string Password)
        {
            Response response;
            var session = HttpContext.Session.GetObjectFromJson<SessionState>("mySession");

            //已登入的回傳
            if (session!= null)
            {
                response = new Response { Code = "201", ErrMsg = "已登入", Data = session };
            }
            else
            {
                //建立儲存session的物件
                SessionState sessionState = new SessionState();
                HttpContext.Session.SetObjectAsJson("mySession", sessionState);
                session = HttpContext.Session.GetObjectFromJson<SessionState>("mySession");

                var manager = manager_SQL.Login(Account, Password);

                //登入成功
                if (manager != null)
                {
                    response = new Response { Code = "200", ErrMsg = "", Data = manager };
                    session.Account = manager.Account;
                    session.UserName = manager.UserName;
                    HttpContext.Session.SetObjectAsJson("mySession", session);
                }
                //登入失敗
                else
                {
                    response = new Response { Code = "400", ErrMsg = "帳號密碼錯誤", Data = manager };
                }
            }


            return Json(response);
        }
    }
}
