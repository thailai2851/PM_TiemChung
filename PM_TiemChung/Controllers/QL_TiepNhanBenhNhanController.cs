using Microsoft.AspNetCore.Mvc;

namespace PM_TiemChung.Controllers
{
    [Route("QuanLy/[controller]")]
    public class QL_TiepNhanBenhNhanController : Controller
    {
        public IActionResult TiepNhanBenhNhan()
        {
            return View();
        }
    }
}
