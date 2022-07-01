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
         *  <路徑>    "/api/UDP/GetCarCount"
         *  <回傳>    List.led_no(計數板編號)    List.led_CarCount(取得的車輛數)     List.led_Remark(名稱)   List.state(成功或失敗)   </回傳>*/
        [HttpGet("GetCarCount")]
        public ActionResult GetCarCount(string parkingGuid)
        {
            var getParkingLotInfo = Verify(parkingGuid);

            if (getParkingLotInfo!=null)
            {
                List<Led> target= getParkingLotInfo.Led_info;
                List<Led_msg> led_Msgs = new List<Led_msg>();
                if (target.Count() == 0)
                {
                    return Json(new Response { Code = "404", ErrMsg = "無 "+getParkingLotInfo.ParkingName+" 計數板資料" });
                }
                else
                {
                    foreach (var item in target)
                    {
                        Thread_UDP thread_UDP = new Thread_UDP("192.168.1.30", item.Led_port);
                        Led_msg led_Msg = new Led_msg();
                        var cnt_msg = thread_UDP.GetCarCount();
                        if (cnt_msg == "connect fail")
                        {
                            led_Msg.State = "失敗";
                        }
                        else
                        {
                            led_Msg.Led_CarCount = cnt_msg;
                            led_Msg.State = "成功";
                        }
                        led_Msg.Led_no = item.Led_no;
                        led_Msg.Led_Remark = item.Remark;
                        led_Msgs.Add(led_Msg);
                    }
                    return Json(new Response { Code = "200", ErrMsg = "", Data = led_Msgs });
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
         *            參數2 位置:  place
         *            參數3 編號:  num  (001,002....)(板子編號為0 => 該地點所有板子 ，板子編號!=0 => 該版子)
         *  </參數>
         *  <路徑>    "/api/UDP/SetCarCount"
         *  <回傳>    List.led_no(計數板編號)    List.led_CarCount(取得的車輛數)     List.led_Remark(名稱)   List.state(成功或失敗)   </回傳>
         *            List.led.no格式:    ex:1001   =>[0]為區域    [1]~[3]為編號
         */
        [HttpGet("SetCarCount")]
        public ActionResult SetCarCount(string parkingGuid,string carCount,string place,string num)
        {
            var getParkingLotInfo = Verify(parkingGuid);
            if (getParkingLotInfo != null)
            {
                ParkingLot_SQL parkingLot_SQL = new ParkingLot_SQL(getParkingLotInfo.SQLIP, getParkingLotInfo.SQLPort, getParkingLotInfo.SQLDBName, getParkingLotInfo.SQLAccount, getParkingLotInfo.SQLPassword);
                var manager = HttpContext.Session.GetObjectFromJson<Manager>("sessionManger");
                
                //板子編號為0 => 該地點所有板子 ，板子編號!=0 => 該版子
                List<Led> target =(num==null)? getParkingLotInfo.Led_info.Where(x => x.Led_no.Substring(0, 1) == place).ToList() 
                    : getParkingLotInfo.Led_info.Where(x => x.Led_no.Substring(0,1) == place &&x.Led_no.Substring(1,3)==num).ToList();
                List<Led_msg> led_Msgs = new List<Led_msg>();
                if (target.Count()==0)
                {
                    parkingLot_SQL.InsertLog(manager.Account, "設定 " + getParkingLotInfo.ParkingName + " 場地 " + place+" 編號 "+num + " 車輛數失敗，無此編號");
                    return Json(new Response { Code = "404", ErrMsg = "設定 " + getParkingLotInfo.ParkingName + " 場地 " + place + " 編號 " + num + " 車輛數失敗，無此編號" });
                }
                else
                {
                    foreach (var item in target)
                    {
                        Thread_UDP thread_UDP = new Thread_UDP("192.168.1.30", item.Led_port);
                        Led_msg led_Msg = new Led_msg();
                        var cnt_msg = thread_UDP.SetCarCount(carCount);
                        if (cnt_msg == "connect fail")
                        {
                            parkingLot_SQL.InsertLog(manager.Account, "設定 " + item.Remark + " 車位數" + carCount + "連接失敗");
                            led_Msg.State = "失敗";
                        }
                        else
                        {
                            parkingLot_SQL.InsertLog(manager.Account, "設定 " + item.Remark + " 車位數" + carCount + "成功");
                            led_Msg.State = "成功";
                        }
                        led_Msg.Led_no = item.Led_no;
                        led_Msg.Led_CarCount = carCount;
                        led_Msg.Led_Remark = item.Remark;
                        led_Msgs.Add(led_Msg);
                    }
                    return Json(new Response { Code = "200", ErrMsg = "", Data = led_Msgs });
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
         *            參數2 柵欄位置:  place 晶片場=>[1(入口) 或 2(出口)]    車辨場&車板場=>(不須輸入)
         *            參數3 柵欄編號:  num 皆為(001, 002, 003.....)
         *  </參數>
         *  <路徑>    "/api/UDP/OpenGate"
         *  <回傳>    訊息:是否成功   </回傳>*/
        [HttpGet("OpenGate")]
        public ActionResult OpenFence(string parkingGuid,string place,string num)
        {
            var getParkingLotInfo = Verify(parkingGuid);
            if (getParkingLotInfo != null)
            {
                ParkingLot_SQL parkingLot_SQL = new ParkingLot_SQL(getParkingLotInfo.SQLIP, getParkingLotInfo.SQLPort, getParkingLotInfo.SQLDBName, getParkingLotInfo.SQLAccount, getParkingLotInfo.SQLPassword);
                var manager = HttpContext.Session.GetObjectFromJson<Manager>("sessionManger");
                Response response;
                //晶片閘門
                if (getParkingLotInfo.ParkingType == "CT")
                {
                    Fence target = getParkingLotInfo.Fence_info.Where(x => x.Fence_no.Substring(0, 1) == place && x.Fence_no.Substring(1, 3) == num).ToList().FirstOrDefault();
                    if (target == null)
                    {
                        parkingLot_SQL.InsertLog(manager.Account, "開啟晶片場 " + getParkingLotInfo.ParkingName + " " + place + num + " 柵欄失敗");
                        return Json(new Response { Code = "404", ErrMsg = "無柵欄編號 " + place + num + " 資料" });
                    }
                    else
                    {
                        Thread_UDP thread_UDP = new Thread_UDP("192.168.1.30", target.Fence_port);
                        var cnt_msg = thread_UDP.OpenFence(num);
                        
                        if(cnt_msg=="connect fail")
                        {
                            response = new Response { Code = "404", ErrMsg = "連接晶片場 " + getParkingLotInfo.ParkingName + " 柵欄編號 " + place + num + " 失敗" };
                            parkingLot_SQL.InsertLog(manager.Account, "連接晶片場 " + getParkingLotInfo.ParkingName + " 柵欄編號 " + place + num + " 失敗");
                        }
                        else
                        {
                            response = new Response { Code = "200", ErrMsg = "", Data = "開啟晶片場 " + getParkingLotInfo.ParkingName + " 柵欄 " + place + num + " 成功" };
                            parkingLot_SQL.InsertLog(manager.Account, "開啟晶片場 " + getParkingLotInfo.ParkingName + " 柵欄 " + place + num + " 成功");
                        }                    
                    }
                    
                }
                //車擋板閘門 & 車辨閘門
                else
                {
                    string str_type = (getParkingLotInfo.ParkingType == "CD") ? "車辨場" : "車擋板場";
                    var msg = parkingLot_SQL.OpenFence(num);
                    if(msg=="open fail")
                    {
                        response = new Response { Code = "404", ErrMsg = "無"+str_type+" " + getParkingLotInfo.ParkingName + " 柵欄編號 " + num + " 資料" };
                        parkingLot_SQL.InsertLog(manager.Account, "無"+str_type+" " + getParkingLotInfo.ParkingName + " 柵欄編號 " + num + " 資料");
                    }
                    else
                    {
                        response = new Response { Code = "200", ErrMsg = "", Data = "開啟"+str_type+" " + getParkingLotInfo.ParkingName + " 柵欄 " + num + " 成功" };
                        parkingLot_SQL.InsertLog(manager.Account, "開啟"+str_type+" " + getParkingLotInfo.ParkingName + " 柵欄 " + num + " 成功");
                    }
                }
                
                return Json(response);
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
