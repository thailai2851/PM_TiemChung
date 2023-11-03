using Microsoft.AspNetCore.Mvc;

namespace PM_TiemChung.Controllers
{
    [Route("QuanLy/[controller]")]
    public class QL_KhamBenhController : Controller
    {
        public IActionResult KhamBenh()
        {
            return View();
        }
    }
}
