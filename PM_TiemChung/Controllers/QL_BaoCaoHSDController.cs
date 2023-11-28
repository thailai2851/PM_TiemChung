using Microsoft.AspNetCore.Mvc;

namespace PM_TiemChung.Controllers
{
    [Route("QuanLy/[controller]")]
    public class QL_BaoCaoHSDController : Controller
    {
        public IActionResult BaoCaoHSD()
        {
            return View();
        }
    }
}
