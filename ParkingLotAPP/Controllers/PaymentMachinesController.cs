using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ParkingLotAPP.HELP;
using ParkingLotAPP.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using static ParkingLotAPP.Models.DataModel;

namespace ParkingLotAPP.Controllers
{
    public class PaymentMachinesController : Controller
    {
        #region 繳費機
        public IActionResult Index()
        {
            return View();
        }
        #endregion
    }
}
