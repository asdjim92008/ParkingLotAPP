using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkingLotAPP.DAL;
using ParkingLotAPP.HELP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ParkingLotAPP.Models.DataModel;

namespace ParkingLotAPP.Controllers
{
    [Route("api/[controller]")]
    
    public class CountCarController : Controller
    {
        //  用途:取得計數板的車輛數量
        //  路徑 "/api/CountCar/Get?parkingGuid=停車場guid"
        [HttpGet("Get")]
        public ActionResult GetCarCount(string parkingGuid)
        {
            var getParkingLotInfo = Verify(parkingGuid);

            if (getParkingLotInfo!=null)
            {
                CountCar_ENT countCar_ENT = new CountCar_ENT(getParkingLotInfo.LedPort, 115200, "0", 8, "1");
                var cnt_msg = countCar_ENT.GetCarCount();
                if (cnt_msg == "open fail")
                {
                    return Json(new Response { Code = "404", ErrMsg = cnt_msg });
                }
                else
                {
                    return Json(new Response { Code = "200", ErrMsg = "", Data = cnt_msg });
                }
            }
            else
            {
                return Json(new Response { Code = "402", ErrMsg = "操作逾時，請重新登入", Data = null });
            }
            
        }
        //  用途:設定計數板的車輛數量
        //  路徑 "/api/CountCar/Set?parkingGuid=停車場guid&carCount=20"
        [HttpGet("Set")]
        public ActionResult SetCarCount(string parkingGuid,string carCount)
        {
            var getParkingLotInfo = Verify(parkingGuid);
            if (getParkingLotInfo != null)
            {
                CountCar_ENT countCar_ENT = new CountCar_ENT(getParkingLotInfo.LedPort, 115200, "0", 8, "1");
                var cnt_msg = countCar_ENT.SetCarCount(carCount);
                if (cnt_msg == "open fail")
                {
                    return Json(new Response { Code = "404", ErrMsg = cnt_msg });
                }
                else
                {
                    return Json(new Response { Code = "200", ErrMsg = "", Data = cnt_msg });
                }
            }
            else
            {
                return Json(new Response { Code = "402", ErrMsg = "操作逾時，請重新登入", Data = null });
            }

        }
        public ParkingLotInfo Verify(string key) 
        {
            var x= HttpContext.Session.GetObjectFromJson<ParkingLotInfo>("sessionParkInfo");
            if (x!=null && x.ParkingGuid == key)
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
