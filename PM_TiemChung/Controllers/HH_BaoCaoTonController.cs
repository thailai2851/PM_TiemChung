using Microsoft.AspNetCore.Mvc;

namespace PM_TiemChung.Controllers
{
    [Route("HangHoa/[controller]")]
    public class HH_BaoCaoTonController : Controller
    {
        public IActionResult BaoCaoTon()
        {
            return View();
        }
    }
}
