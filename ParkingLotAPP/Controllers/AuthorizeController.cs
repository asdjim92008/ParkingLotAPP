using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkingLotAPP.DAL;
using ParkingLotAPP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingLotAPP.Controllers
{
    [Route("api/[controller]")]
    
    public class AuthorizeController : Controller
    {
        Manager_SQL manager_SQL = new Manager_SQL("192.168.1.46", "3306", "parkinglist", "rotamoon", "rotamoon90692052");
        [HttpPost]
        public ActionResult Login(string Account,string Password)
        {
            DataModel.Response response;
            var manager = manager_SQL.Login(Account, Password);
            if (manager!=null)
            {
                response = new DataModel.Response { Code = "200", ErrMsg = "", Data = manager };
            }
            else
            {
                response = new DataModel.Response { Code = "400", ErrMsg = "帳號密碼錯誤", Data = manager };
            }
            
            return Json(response);
        }
    }
}
