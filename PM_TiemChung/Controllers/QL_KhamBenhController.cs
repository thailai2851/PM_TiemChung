using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;
using PM_TiemChung.Models;
using PM_TiemChung.Models.Entities;
using PM_TiemChung.Models.Mapper;
using PM_TiemChung.Services;
using System.Collections.Generic;
using System.Globalization;
using WkHtmlToPdfDotNet;
using WkHtmlToPdfDotNet.Contracts;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PM_TiemChung.Controllers
{
    [Authorize]
    [Route("QuanLy/[controller]")]
    public class QL_KhamBenhController : Controller
    {
        private ThaiLaiContext _context;
        private readonly IMapper _mapper;
        private readonly IConverter _converter;
        private ICompositeViewEngine _viewEngine;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public QL_KhamBenhController(IMapper mapper, ThaiLaiContext context
            , IConverter converter, ICompositeViewEngine viewEngine, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _converter = converter;
            _viewEngine = viewEngine;
            _hostingEnvironment = hostingEnvironment;
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
            DateTime d = DateTime.Now.Date;
            ViewBag.DanhSachDeNghi = await _context.LichTiemBns
                .Include(x=>x.IdbnNavigation)
                .Include(x=>x.IdvcNavigation)
                .Where(x => x.NgayDeNghiTiem.Value.Date == d && x.DeNghiTiem.Value).ToListAsync();
            ViewBag.DanhSachTiemChung = await _context.LichTiemBns
                .Include(x => x.IdbnNavigation)
                .Include(x => x.IdvcNavigation)
                .Where(x => x.NgayThu.Value.Date == d && x.DaThu == true)
                .OrderBy(x => x.DaTiem)
                .ToListAsync();
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
        [HttpPost("reloadDSThuNgan")]
        public async Task<IActionResult> reloadDSThuNgan(string ngay)
        {
            var d = DateTime.ParseExact(ngay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            return Ok(await _context.LichTiemBns
                .Include(x => x.IdbnNavigation)
                .Include(x => x.IdvcNavigation)
                .Where(x => x.NgayDeNghiTiem.Value.Date == d && x.DeNghiTiem.Value)
                .GroupBy(x => x.IdbnNavigation)
                .Select(x => new
                {
                    idbnNavigation = x.Key,
                    datas = x.ToList(),
                    sl = x.Where(y => y.DaThu != true).Count()
                })
                .OrderByDescending(x => x.sl)
                .ToListAsync());
        }
        [HttpPost("reloadDSTiemChung")]
        public async Task<IActionResult> reloadDSTiemChung(string ngay)
        {
            var d = DateTime.ParseExact(ngay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            return Ok(await _context.LichTiemBns
                .Include(x => x.IdbnNavigation)
                .Include(x => x.IdvcNavigation)
                .Where(x => x.NgayThu.Value.Date == d && x.DaThu == true)
                .OrderBy(x => x.DaTiem)
                .ToListAsync());
        }
        [HttpPost("luuLichTiem")]
        public async Task<IActionResult> luuLichTiem([FromBody] List<LichTiemBnMap> lichTiemMaps)
        {
            List<LichTiemBn> lichTiems = _mapper.Map<List<LichTiemBn>>(lichTiemMaps);
            using var tran = _context.Database.BeginTransaction();
            try
            {
                long _userId = int.Parse(User.Identity.Name);
                foreach (var lt in lichTiems)
                {
                    lt.Idbsk = _userId;
                    if (lt.Id != 0)
                    {
                        var lichTiem = await _context.LichTiemBns.FindAsync(lt.Id);
                        lichTiem.Idbsk = _userId;
                        lichTiem.Idvc = lt.Idvc;
                        lichTiem.SoLanTiem = lt.SoLanTiem;
                        lichTiem.TgsomNhat = lt.TgsomNhat;
                        lichTiem.TgtreNhat = lt.TgtreNhat;
                        lichTiem.IdthoiGian = lt.IdthoiGian;
                        lichTiem.MuiTienQuyet = lt.MuiTienQuyet;
                        lichTiem.DaTiem = lt.DaTiem;
                        lichTiem.NgayTiem = lt.NgayTiem;
                        lichTiem.DaTiem = lt.DaTiem;
                        lichTiem.NgayDeNghiTiem = lt.NgayDeNghiTiem;
                        lichTiem.DeNghiTiem = lt.DeNghiTiem;
                        lichTiem.NgayHen = lt.NgayHen;
                    }
                }
                var lichTiemsMoi = lichTiems.Where(x => x.Id == 0).ToList();
                await _context.LichTiemBns.AddRangeAsync(lichTiemsMoi);
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
        [HttpPost("printThuNgan")]
        public async Task<IActionResult> printThuNhan([FromBody] ModelInThuNgan model)
        {
            var lichTiemBenhNhan = await _context.LichTiemBns
                .Include(x=>x.IdvcNavigation)
                .Where(x => model.ListIn.Any(y=>y==x.Id) && x.DaThu == true).ToListAsync();
            var bn = await _context.DmBenhNhans
                .Include(x => x.IdgtNavigation)
                .FirstOrDefaultAsync(x => x.Id == model.IdBn);
            bn.LichTiemBns = lichTiemBenhNhan;
            ViewBag.PhieuThuNgan = bn;
            ViewBag.ttDoanhNghiep = await _context.ThongTinDoanhNghieps.FirstOrDefaultAsync();
            ViewBag.logo = CommonServices.ConvertImageToBase64(_hostingEnvironment, "/images/logo.png");
            PartialViewResult partialViewResult = PartialView("ThuNganPDF");
            string viewContent = CommonServices.ConvertViewToString(ControllerContext, partialViewResult, _viewEngine);

            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                        ColorMode = ColorMode.Color,
                        Orientation = Orientation.Portrait,
                        PaperSize = PaperKind.A5,
                        Margins = new MarginSettings()
                        {
                            Left = 0.5,
                            Right = 0.5,
                            Unit = Unit.Centimeters
                        },
                    },
                Objects = {
                        new ObjectSettings() {
                            PagesCount = true,
                            HtmlContent = viewContent,
                            WebSettings = {
                                DefaultEncoding = "utf-8",
                            },
                            UseLocalLinks = true,
                            FooterSettings = { FontSize = 9, Right = "Trang [page]", Line = true, Spacing = 2.812 }
                        }
                    }
            };
            var pdfBytes = _converter.Convert(doc);

            return File(pdfBytes, "application/pdf", "ThuNgan.pdf");
        }
        [HttpPost("luuThuNgan")]
        public async Task<IActionResult> luuThuNgan(long id, string ngayThu)
        {
            var NgayThu = DateTime.ParseExact(ngayThu, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            using var tran = _context.Database.BeginTransaction();
            try
            {
                long _userId = int.Parse(User.Identity.Name);
                var lichTiem = await _context.LichTiemBns
                    .Include(x=>x.IdvcNavigation)
                    .FirstOrDefaultAsync(x => x.Id == id);
                lichTiem.DaThu = true;
                lichTiem.NgayThu = NgayThu;
                lichTiem.SoLuong = 1;
                lichTiem.DonGia = lichTiem.IdvcNavigation.GiaBan;
                lichTiem.IdnhanVienThu = _userId;
                _context.LichTiemBns.Update(lichTiem);
                await _context.SaveChangesAsync();

                var ctPhieuNhap = await _context.ChiTietPhieuNhaps
                    .FirstOrDefaultAsync(x => (x.Idvaccine == lichTiem.Idvc) && (x.SoLuong > (x.SoLuongXuat ?? 0)));

                if (ctPhieuNhap == null)
                {
                    tran.Rollback();
                    return Ok(new ResponseModel()
                    {
                        statusCode = 500,
                        message = "Đã hết vacine!"
                    });
                }
                if (ctPhieuNhap.SoLuongXuat == null)
                {
                    ctPhieuNhap.SoLuongXuat = 1;
                }
                else
                {
                    ctPhieuNhap.SoLuongXuat += 1;
                }
                await _context.SaveChangesAsync();

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
        [HttpPost("tiemChung")]
        public async Task<IActionResult> tiemChung(long id, string ngayTiem)
        {
            var NgayTiem = DateTime.ParseExact(ngayTiem, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            using var tran = _context.Database.BeginTransaction();
            try
            {
                long _userId = int.Parse(User.Identity.Name);
                var lichTiem = await _context.LichTiemBns
                    .Include(x => x.IdvcNavigation)
                    .FirstOrDefaultAsync(x => x.Id == id);
                lichTiem.DaTiem = true;
                lichTiem.NgayTiem = NgayTiem;
                lichTiem.IdnhanVienTiem = _userId;

                _context.LichTiemBns.Update(lichTiem);
                await _context.SaveChangesAsync();

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
        [HttpPost("loadTTLichTiem")]
        public async Task<IActionResult> loadTTLichTiem(long idBn)
        {
            var ttLichTiem = await _context.DmBenhNhans.AsNoTracking()
                .Include(x => x.LichTiemBns)
                .ThenInclude(x => x.IdthoiGianNavigation)
                .Include(x => x.LichTiemBns)
                .ThenInclude(x => x.IdvcNavigation)
                .Include(x => x.LichTiemBns)
                .ThenInclude(x => x.MuiTienQuyetNavigation)
                .FirstOrDefaultAsync(x => x.Id == idBn);
            var profile = await _context.DmProfiles.AsNoTracking()
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
                if (ttLichTiem.LichTiemBns.FirstOrDefault(x=>x.Idvc == ct.Idvaccine && x.SoLanTiem == ct.SoLanTiem) == null)
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
            var lichTiems = ttLichTiem.LichTiemBns.ToList();
            return Ok(new
            {
                tenProFile = profile.TenProfile,
                idBenhNhan = ttLichTiem.Id,
                tenBenhNhan = ttLichTiem.TenBn,
                tuoi,
                lichTiems,
                soNgayTuoi
            });
        }
        [HttpPost("reloadTTLichTiem")]
        public async Task<IActionResult> reloadTTLichTiem(long idBn)
        {
            using var tran = _context.Database.BeginTransaction();
            try
            {
                var ttLichTiem = await _context.DmBenhNhans
                    .Include(x=>x.LichTiemBns)
                .FirstOrDefaultAsync(x => x.Id == idBn);
                var profileCts = _context.DmProfileCts
                        .Include(x => x.IdprofileNavigation)
                        .Where(x => x.IdprofileNavigation.Idgt == ttLichTiem.Idgt);
                foreach (var ct in profileCts)
                {
                    var lichTiemBn = ttLichTiem.LichTiemBns.FirstOrDefault(x => x.Idvc == ct.Idvaccine && x.SoLanTiem == ct.SoLanTiem);
                    if (lichTiemBn == null)
                    {
                        LichTiemBn lichNew = new LichTiemBn()
                        {
                            Idbn = idBn,
                            Idvc = ct.Idvaccine,
                            SoLanTiem = ct.SoLanTiem,
                            TgsomNhat = ct.TgsomNhat,
                            TgtreNhat = ct.TgtreNhat,
                            IdthoiGian = ct.IdthoiGian,
                            MuiTienQuyet = ct.MuiTienQuyet
                        };
                        await _context.LichTiemBns.AddAsync(lichNew);
                    }
                    else
                    {
                        lichTiemBn.SoLanTiem = ct.SoLanTiem;
                        lichTiemBn.TgsomNhat = ct.TgsomNhat;
                        lichTiemBn.TgtreNhat = ct.TgtreNhat;
                        lichTiemBn.IdthoiGian = ct.IdthoiGian;
                        lichTiemBn.MuiTienQuyet = ct.MuiTienQuyet;
                        _context.LichTiemBns.Update(lichTiemBn);
                    }
                }
                await _context.SaveChangesAsync();

                tran.Commit();
                return Ok(new ResponseModel()
                {
                    statusCode = 200,
                    message = "Thành công!"
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
    public class ModelInThuNgan
    {
        public long IdBn { get; set; }
        public List<long> ListIn { get; set; }
    }
}
