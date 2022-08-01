using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreBS.Controllers
{
    public class SearchCarController : Controller
    {
        #region 查詢車牌號
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CheckCar()
        {
            return View();
        }
        #endregion
    }
}