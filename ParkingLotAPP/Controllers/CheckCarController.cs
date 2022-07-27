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
    public class CheckCarController : Controller
    {
        #region 選擇車牌號
        public IActionResult Index()
        {
            return View();
        }
        #endregion
    }
}