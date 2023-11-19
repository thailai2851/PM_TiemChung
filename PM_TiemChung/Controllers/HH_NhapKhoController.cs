using Microsoft.AspNetCore.Mvc;

namespace PM_TiemChung.Controllers
{
    [Route("HangHoa/[controller]")]
    public class HH_NhapKhoController : Controller
    {
        public IActionResult NhapKho()
        {
            return View();
        }
    }
}
