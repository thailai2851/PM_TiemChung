using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PM_TiemChung.Models.Entities;
using System.Globalization;

namespace PM_TiemChung.Controllers
{
    [Route("HangHoa/[controller]")]
    public class HH_BaoCaoDoThiController : Controller
    {
        private ThaiLaiContext _context;
        public HH_BaoCaoDoThiController(ThaiLaiContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost("BaoCaoChiTieu")]
        public async Task<dynamic> BaoCaoChiTieu(string fromDay, string toDay)
        {
            DateTime FromDay = DateTime.ParseExact(fromDay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            DateTime ToDay = DateTime.ParseExact(toDay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            var chiTietHoaDons = await _context.LichTiemBns
                .Where(x => x.NgayThu >= FromDay.Date && x.NgayThu <= ToDay.Date)
                .ToListAsync();
            var thucDons = await _context.DmVaccines
                .Where(x => x.Active == true)
                .Select(x => new
                {
                    label = x.TenVaccine,
                    soLuong = HH_BaoCaoDoThiController.tinhSLThucDon(x.Id, chiTietHoaDons),
                })
                .ToListAsync();
            return new
            {
                doThiThucDon = thucDons.Where(x => x.soLuong != 0).OrderByDescending(x => x.soLuong).Take(5)
,
            };
        }
        public static int tinhSLThucDon(long idtd, List<LichTiemBn> chiTietHoaDons)
        {
            int soluong = 0;
            foreach (LichTiemBn c in chiTietHoaDons)
            {
                if (c.Idvc == idtd)
                {
                    soluong += (int)c.SoLuong;
                }
            }
            return soluong;
        }

    }
}
