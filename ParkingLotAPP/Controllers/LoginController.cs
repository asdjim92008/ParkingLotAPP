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
    public class LoginController : Controller
    {
        #region 登入
        public IActionResult Index()
        {
                return View();
        }
        #endregion
    }
}