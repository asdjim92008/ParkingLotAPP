using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkingLotAPP.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingLotAPP.Controllers
{
    [Route("api/[controller]")]
    
    public class CountCarController : Controller
    {
        CountCar_ENT countCar_ENT = new CountCar_ENT("COM3", 19200, "0", 8, "1");
        [HttpGet("Get")]
        public ActionResult GetCarCount()
        {
            var x = countCar_ENT.SetCarCount("1");
            return Ok(x);
        }
    }
}
