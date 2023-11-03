using Microsoft.AspNetCore.Mvc;

namespace PM_TiemChung.Controllers
{
    [Route("DanhMuc/[controller]")]
    public class DM_ProfileController : Controller
    {
        public IActionResult Profile()
        {
            return View();
        }
    }
}
