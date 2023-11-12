using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PM_TiemChung.Models;
using PM_TiemChung.Models.Entities;
using PM_TiemChung.Models.Mapper;
using PM_TiemChung.Services;
using System.Globalization;

namespace PM_TiemChung.Controllers
{
    [Route("QuanLy/[controller]")]
    public class QL_TiepNhanBenhNhanController : Controller
    {
        private ThaiLaiContext _context;
        private readonly IMapper _mapper;
        public QL_TiepNhanBenhNhanController(IMapper mapper, ThaiLaiContext context)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> TiepNhanBenhNhan()
        {
            ViewBag.GioiTinhs = await _context.DmGioiTinhs.Where(x => x.Active == true).ToListAsync();
            ViewBag.BenhNhans = await _context.DmBenhNhans
                .Include(x=>x.IdgtNavigation)
                .Include(x=>x.IdtinhNavigation)
                .Include(x=>x.IdquanNavigation)
                .Include(x=>x.IdpxNavigation)
                .Include(x=>x.IddtNavigation)
                .Include(x=>x.IdnnNavigation)
                .Include(x=>x.IdqgNavigation)
                .Where(x => x.Active == true && x.NgayKham.Value.Date == DateTime.Now.Date).ToListAsync();
            return View();
        }
        [HttpPost("LuuBenhNhan")]
        public async Task<IActionResult> LuuBenhNhan(DmBenhNhanMap benhNhanMap)
        {
            using var tran = _context.Database.BeginTransaction();
            DmBenhNhan benhNhan = _mapper.Map<DmBenhNhan>(benhNhanMap);
            try
            {
                benhNhan.NgayKham = DateTime.Now;
                benhNhan.Active = true;
                if (benhNhan.Id == 0)
                {
                    benhNhan.MaBn = await CommonServices.autoMa(_context, "BN");
                    await _context.DmBenhNhans.AddAsync(benhNhan);
                }
                else
                {
                    _context.DmBenhNhans.Update(benhNhan);
                }

                _context.SaveChanges();
                tran.Commit();

                return Ok(new ResponseModel()
                {
                    statusCode = 200,
                    message = "Thành công!",
                    data = benhNhan
                });
            }
            catch (Exception ex)
            {
                tran.Rollback();
                return Ok(new ResponseModel()
                {
                    statusCode = 500,
                    message = "Thất bại!"
                });
            }
        }
        [HttpPost("timKiemBenhNhan")]
        public async Task<IActionResult> timKiemBenhNhan(string ma, string ten, string sdt, string ngay)
        {
            DateTime Ngay = DateTime.ParseExact(ngay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            return Ok(await _context.DmBenhNhans
                .Include(x => x.IdgtNavigation)
                .Include(x => x.IdtinhNavigation)
                .Include(x => x.IdquanNavigation)
                .Include(x => x.IdpxNavigation)
                .Include(x => x.IddtNavigation)
                .Include(x => x.IdnnNavigation)
                .Include(x => x.IdqgNavigation)
                .Where(x => (x.Active == true)
                && (x.NgayKham.Value.Date == Ngay.Date)
                && (ma == null ? true : x.MaBn.ToLower().Contains(ma.ToLower()))
                && (ten == null ? true : x.TenBn.ToLower().Contains(ten.ToLower()))
                && (sdt == null ? true : x.DienThoai.ToLower().Contains(sdt.ToLower()))
                ).ToListAsync());
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
        [HttpPost("getBenhNhan")]
        public async Task<IActionResult> getBenhNhan(long id)
        {
            return Ok(await _context.DmBenhNhans.FindAsync(id));
        }
        [HttpPost("kichHoat")]
        public async Task<IActionResult> kichHoat(long id)
        {
            using var tran = _context.Database.BeginTransaction();
            try
            {
                DmBenhNhan benhNhan = await _context.DmBenhNhans.FindAsync(id);
                benhNhan.NgayKham = DateTime.Now;
                _context.DmBenhNhans.Update(benhNhan);
                _context.SaveChanges();
                tran.Commit();

                return Ok(new ResponseModel()
                {
                    statusCode = 200,
                    message = "Thành công!",
                });
            }
            catch (Exception ex)
            {
                tran.Rollback();
                return Ok(new ResponseModel()
                {
                    statusCode = 500,
                    message = "Thất bại!"
                });
            }
        }
        [HttpPost("huykichhoat")]
        public async Task<IActionResult> huykichhoat(long id)
        {
            using var tran = _context.Database.BeginTransaction();
            try
            {
                DmBenhNhan benhNhan = await _context.DmBenhNhans.FindAsync(id);
                benhNhan.NgayKham = null;
                _context.DmBenhNhans.Update(benhNhan);
                _context.SaveChanges();
                tran.Commit();

                return Ok(new ResponseModel()
                {
                    statusCode = 200,
                    message = "Thành công!",
                });
            }
            catch (Exception ex)
            {
                tran.Rollback();
                return Ok(new ResponseModel()
                {
                    statusCode = 500,
                    message = "Thất bại!"
                });
            }
        }
        [HttpPost("remove")]
        public async Task<IActionResult> remove(long id)
        {
            using var tran = _context.Database.BeginTransaction();
            try
            {
                DmBenhNhan benhNhan = await _context.DmBenhNhans.FindAsync(id);
                benhNhan.Active = false;
                _context.DmBenhNhans.Update(benhNhan);
                _context.SaveChanges();
                tran.Commit();

                return Ok(new ResponseModel()
                {
                    statusCode = 200,
                    message = "Thành công!",
                });
            }
            catch (Exception ex)
            {
                tran.Rollback();
                return Ok(new ResponseModel()
                {
                    statusCode = 500,
                    message = "Thất bại!"
                });
            }
        }
    }
}
