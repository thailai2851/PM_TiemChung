using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PM_TiemChung.Models;
using PM_TiemChung.Models.Entities;
using PM_TiemChung.Models.Mapper;
using PM_TiemChung.Services;
using System.Collections.Generic;

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
        [HttpPost("luuLichTiem")]
        public async Task<IActionResult> luuLichTiem([FromBody] List<LichTiemBnMap> lichTiemMaps)
        {
            List<LichTiemBn> lichTiems = _mapper.Map<List<LichTiemBn>>(lichTiemMaps);
            using var tran = _context.Database.BeginTransaction();
            try
            {
                var lichTiemsMoi = lichTiems.Where(x => x.Id == 0).ToList();
                await _context.LichTiemBns.AddRangeAsync(lichTiemsMoi);
                _context.LichTiemBns.UpdateRange(lichTiems.Where(x => x.Id != 0));
                await _context.SaveChangesAsync();

                tran.Commit();
                return Ok(new ResponseModel()
                {
                    statusCode = 200,
                    message = "Thành công!",
                    data = lichTiemsMoi.Select(x => x.Id)
                }) ;
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
        [HttpPost("loadTTLichTiem")]
        public async Task<IActionResult> loadTTLichTiem(long idBn)
        {
            var ttLichTiem = await _context.DmBenhNhans
                .Include(x => x.LichTiemBns).FirstOrDefaultAsync(x => x.Id == idBn);
            var profile = await _context.DmProfiles
                    .Include(x=>x.DmProfileCts)
                    .ThenInclude(x=>x.IdthoiGianNavigation)
                    .Include(x => x.DmProfileCts)
                    .ThenInclude(x => x.MuiTienQuyetNavigation)
                    .Include(x => x.DmProfileCts)
                    .ThenInclude(x => x.IdvaccineNavigation)
                    .FirstOrDefaultAsync(x => x.Idgt == ttLichTiem.Idgt);
            
            var listIdVc = profile.DmProfileCts.Select(x => x.Idvaccine).ToList();
            foreach(var ct in profile.DmProfileCts)
            {
                if (ttLichTiem.LichTiemBns.FirstOrDefault(x=>x.Idvc == ct.Idvaccine) == null)
                {
                    ttLichTiem.LichTiemBns.Add(new LichTiemBn()
                    {
                        Idvc = ct.Idvaccine,
                        SoLanTiem = ct.SoLanTiem,
                        TgsomNhat = ct.TgsomNhat,
                        TgtreNhat = ct.TgtreNhat,
                        IdthoiGianNavigation = ct.IdthoiGianNavigation,
                        IdvcNavigation = ct.IdvaccineNavigation,
                        MuiTienQuyetNavigation = ct.MuiTienQuyetNavigation
                    });
                }
            }
            string tuoi;
            int soNgayTuoi;
            if (ttLichTiem.NgaySinh == null)
            {
                tuoi = CommonServices.CalculateAgeString(new DateTime(ttLichTiem.NamSinh.Value, 1, 1));
                soNgayTuoi = CommonServices.CalculateAge(new DateTime(ttLichTiem.NamSinh.Value, 1, 1)) * 30;
            }
            else
            {
                tuoi = CommonServices.CalculateAgeString(ttLichTiem.NgaySinh.Value);
                soNgayTuoi = CommonServices.CalculateAge(ttLichTiem.NgaySinh.Value) * 30;
            }
            return Ok(new
            {
                tenProFile = profile.TenProfile,
                idBenhNhan = ttLichTiem.Id,
                tenBenhNhan = ttLichTiem.TenBn,
                tuoi,
                lichTiems = ttLichTiem.LichTiemBns,
                soNgayTuoi
            });
        }
    }
}
