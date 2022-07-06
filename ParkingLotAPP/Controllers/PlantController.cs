using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingLotAPP.Controllers
{
    public class PlantController : Controller
    {
        #region 修改車場相關資料
        public IActionResult Index()
        {
                return View();
        }
        #endregion
    }
}