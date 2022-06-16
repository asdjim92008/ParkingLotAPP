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
    
    public class UDPController : Controller
    {
        
        /*  <目的>    取得計數板的車輛數量   </目的>
         *  <參數>    
         *            參數1 停車場guid:  parkingGuid 
         *  </參數>
         *  <路徑>    "/api/CountCar/Get"
         *  <回傳>    訊息:車輛數   </回傳>*/
        [HttpGet("Get")]
        public ActionResult GetCarCount(string parkingGuid)
        {
            var getParkingLotInfo = Verify(parkingGuid);

            if (getParkingLotInfo!=null)
            {
                Thread_UDP thread_UDP = new Thread_UDP(getParkingLotInfo.LedPort);
                var cnt_msg = thread_UDP.GetCarCount();
                if (cnt_msg == "connect fail")
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
        


        /*  <目的>    設定計數板的車輛數量   </目的>
         *  <參數>    
         *            參數1 停車場guid:  parkingGuid 
         *            參數2 設定車輛數:  carCount
         *  </參數>
         *  <路徑>    "/api/CountCar/Set"
         *  <回傳>    訊息:車輛數   </回傳>*/
        [HttpGet("Set")]
        public ActionResult SetCarCount(string parkingGuid,string carCount)
        {
            var getParkingLotInfo = Verify(parkingGuid);
            if (getParkingLotInfo != null)
            {
                Thread_UDP thread_UDP = new Thread_UDP(getParkingLotInfo.LedPort);
                var cnt_msg = thread_UDP.SetCarCount(carCount);
                if (cnt_msg == "connect fail")
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



        /*  <目的>    開啟柵欄   </目的>
         *  <參數>    
         *            參數1 停車場guid:  parkingGuid 
         *            參數2 柵欄位置:  place 晶片場=>(entrace 或 exit)    車辨場=>(001、002....)
         *  </參數>
         *  <路徑>    "/api/CountCar/open"
         *  <回傳>    訊息:是否成功   </回傳>*/
        [HttpGet("open")]
        public ActionResult OpenFence(string parkingGuid,string place)
        {
            var getParkingLotInfo = Verify(parkingGuid);
            if (getParkingLotInfo != null)
            {
                //晶片閘門
                if (getParkingLotInfo.ParkingType == "CT")
                {
                    var fence = (place == "entrace") ? getParkingLotInfo.EntracePort : getParkingLotInfo.ExitPort;
                    Thread_UDP thread_UDP = new Thread_UDP(fence);
                    var cnt_msg = thread_UDP.OpenFence();
                    var x = (cnt_msg == "connect fail") ? Json(new Response { Code = "404", ErrMsg = cnt_msg }) : Json(new Response { Code = "200", ErrMsg = "", Data = cnt_msg });
                    return x;
                }
                //車辨閘門
                else if (getParkingLotInfo.ParkingType == "CD")
                {
                    ParkingLot_SQL open= new ParkingLot_SQL(getParkingLotInfo.SQLIP, getParkingLotInfo.SQLPort, getParkingLotInfo.SQLDBName, getParkingLotInfo.SQLAccount, getParkingLotInfo.SQLPassword);
                    var msg = open.OpenFence(place);
                    var x = (msg == "connect fail")? Json(new Response { Code = "404", ErrMsg = msg }): Json(new Response { Code = "200", ErrMsg = "", Data = msg });
                    return x;
                }
                //車擋板閘門
                else
                {
                    //車擋板閘門尚未完成
                    ParkingLot_SQL open = new ParkingLot_SQL(getParkingLotInfo.SQLIP, getParkingLotInfo.SQLPort, getParkingLotInfo.SQLDBName, getParkingLotInfo.SQLAccount, getParkingLotInfo.SQLPassword);
                    var msg = open.OpenFence(place);
                    var x = (msg == "connect fail") ? Json(new Response { Code = "404", ErrMsg = msg }) : Json(new Response { Code = "200", ErrMsg = "", Data = msg });
                    return x;
                }
                
            }
            else
            {
                return Json(new Response { Code = "402", ErrMsg = "操作逾時，請重新登入", Data = null });
            }
        }



        /*  <目的>    驗證SESSION 並取得停車場的授權資料   </目的>
         */
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
