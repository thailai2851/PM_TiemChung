using Microsoft.AspNetCore.Mvc;

namespace PM_TiemChung.Controllers
{
    [Route("QuanLy/[controller]")]

    public class QL_BaoCaoLichHenController : Controller
    {
        public IActionResult BaoCaoLichHen()
        {
            return View();
        }
    }
}
