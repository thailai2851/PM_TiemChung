using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PM_TiemChung.Models.Entities;

namespace PM_TiemChung.Controllers
{
    [Route("QuanLy/[controller]")]
    public class QL_KhamBenhController : Controller
    {
        private ThaiLaiContext _context;
        private readonly IMapper _mapper;
        public QL_KhamBenhController(IMapper mapper, ThaiLaiContext context)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> KhamBenh()
        {
            ViewBag.BenhNhans = await _context.DmBenhNhans
                .Include(x => x.IdgtNavigation)
                .Include(x => x.IdtinhNavigation)
                .Include(x => x.IdquanNavigation)
                .Include(x => x.IdpxNavigation)
                .Include(x => x.IddtNavigation)
                .Include(x => x.IdnnNavigation)
                .Include(x => x.IdqgNavigation)
                .Where(x => x.Active == true && x.NgayKham.Value.Date == DateTime.Now.Date).ToListAsync();
            return View();
        }
        [HttpPost("reloadTableDSTN")]
        public async Task<IActionResult> reloadTableDSTN()
        {
            return Ok(await _context.DmBenhNhans
                .Include(x => x.IdgtNavigation)
                .Include(x => x.IdtinhNavigation)
                .Include(x => x.IdquanNavigation)
                .Include(x => x.IdpxNavigation)
                .Include(x => x.IddtNavigation)
                .Include(x => x.IdnnNavigation)
                .Include(x => x.IdqgNavigation)
                .Where(x => x.Active == true && x.NgayKham.Value.Date == DateTime.Now.Date
                ).ToListAsync());
        }
    }
}
