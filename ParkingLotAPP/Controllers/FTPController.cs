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
    public class FTPController : Controller
    {
        /*  <目的>    依照所選的頁面回傳5個停車場車輛的資訊   </目的>
         *  <參數>    
         *            參數1 停車場guid:  parkingGuid 
         *            參數2 頁數:  page
         *  </參數>
         *  <路徑>    "/api/FTP"
         *  <回傳>    ymhdm(進場時間) platenum(車牌) tickno(票卡序號) jpg(圖片)   </回傳>
         *  <使用方式>  <img src="data:image/jpg;base64,jpg">    </使用方式> */
        [HttpGet]
        public ActionResult FTP_GetFile(string parkingGuid,string page)
        {
            Response response;
            var getParkingLotInfo = Verify(parkingGuid);
            if (getParkingLotInfo != null)
            {
                int start= (int.Parse(page)-1)*5;
                int end = (int.Parse(page)*5)-1;
                int current = start;
                ParkingLot_SQL parkingLot_SQL = new ParkingLot_SQL(getParkingLotInfo.SQLIP,getParkingLotInfo.SQLPort,getParkingLotInfo.SQLDBName,getParkingLotInfo.SQLAccount,getParkingLotInfo.SQLPassword);
                var tempinfo = parkingLot_SQL.GetAlljpg();
                var carinfos = new List<CarInfo>();

                
                ParkingLot_FTP parkingLot_FTP = new ParkingLot_FTP(getParkingLotInfo.FTPIP, getParkingLotInfo.FTPAccount, getParkingLotInfo.FTPPassword);
                while (current< tempinfo.Count  && current<=end)
                {
                    if (tempinfo.ElementAt(current) != null)
                    {
                        tempinfo.ElementAt(current).JPG = parkingLot_FTP.fileDownloadAsync(tempinfo.ElementAt(current).JPG).Result;
                    }
                    else
                    {
                        tempinfo.ElementAt(current).JPG = "null";
                    }
                    carinfos.Add(tempinfo.ElementAt(current));
                    current++;
                }
                response = new Response { Code = "200", ErrMsg = "", Data = carinfos };
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
         *  <路徑>    "/api/FTP/changePlateNum"
         *  <回傳>    訊息   </回傳>*/
        [HttpPost("changePlateNum")]
        public ActionResult ChangePlateNum(string parkingGuid,string n_PlateNum,string c_PlateNum)
        {
            Response response;
            var getParkingLotInfo = Verify(parkingGuid);
            if (getParkingLotInfo != null)
            {
                ParkingLot_SQL parkingLot_SQL = new ParkingLot_SQL(getParkingLotInfo.SQLIP, getParkingLotInfo.SQLPort, getParkingLotInfo.SQLDBName, getParkingLotInfo.SQLAccount, getParkingLotInfo.SQLPassword);
                var x=parkingLot_SQL.ChangePlateNum(n_PlateNum, c_PlateNum);
                if (x != "From " + n_PlateNum + " To " + c_PlateNum + " update sucess")
                {
                    response = new Response { Code = "404", ErrMsg = x };
                }
                else
                {
                    var manager = HttpContext.Session.GetObjectFromJson<Manager>("sessionManger");
                    parkingLot_SQL.InsertLog(manager.UserName, "修改車號" + n_PlateNum + " TO " + c_PlateNum);
                    response = new Response { Code = "200", ErrMsg = "", Data = x };
                }            }
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
         *            參數3 進場時間: ymhdm  ,(格式 ex:2022年5月10號 10時11分12秒 => 20220510101112) 
         *  </參數>
         *  <路徑>    "/api/FTP/insertPlateNum"
         *  <回傳>    訊息   </回傳>*/
        [HttpPost("insertPlateNum")]
        public ActionResult InsertPlateNum(string parkingGuid, string plateNum, string ymhdm)
        {
            Response response;
            var getParkingLotInfo = Verify(parkingGuid);
            if (getParkingLotInfo != null)
            {
                ParkingLot_SQL parkingLot_SQL = new ParkingLot_SQL(getParkingLotInfo.SQLIP, getParkingLotInfo.SQLPort, getParkingLotInfo.SQLDBName, getParkingLotInfo.SQLAccount, getParkingLotInfo.SQLPassword);
                var x = parkingLot_SQL.InsertPlateNum( getParkingLotInfo. ParkingNo, plateNum, ymhdm);
                if(x== "Repeat, Insert fail")
                {
                    response = new Response { Code = "404", ErrMsg = x };
                }
                else
                {
                    var manager = HttpContext.Session.GetObjectFromJson<Manager>("sessionManger");
                    parkingLot_SQL.InsertLog(manager.UserName, "新增車號" + plateNum);
                    response = new Response { Code = "200", ErrMsg = "", Data = x };
                }
            }
            else
            {
                response = new Response { Code = "402", ErrMsg = "操作逾時，請重新登入" };
            }
            return Json(response);
        }
        /* 
         * <目的>    驗證SESSION 並取得停車場的授權資料   </目的>
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
