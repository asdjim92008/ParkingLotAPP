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
    public class ManualaddController : Controller
    {
        #region 手動新增
        public IActionResult Index()
        {
            return View();
        }
        #endregion
    }
}
