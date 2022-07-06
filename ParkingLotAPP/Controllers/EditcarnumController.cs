using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingLotWeb_Edit.Controllers
{
    public class EditcarnumController : Controller
    {
        #region 修改車號
        public IActionResult Index()
        {
                return View();
        }
        #endregion
    }
}