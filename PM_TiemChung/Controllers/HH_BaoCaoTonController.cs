using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;
using PM_TiemChung.Models.Entities;
using PM_TiemChung.Services;
using System.Globalization;
using WkHtmlToPdfDotNet;
using WkHtmlToPdfDotNet.Contracts;

namespace PM_TiemChung.Controllers
{
    [Route("HangHoa/[controller]")]
    public class HH_BaoCaoTonController : Controller
    {
        private ThaiLaiContext _context;
        private readonly IMapper _mapper;
        private readonly IConverter _converter;
        private ICompositeViewEngine _viewEngine;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public HH_BaoCaoTonController(IMapper mapper, ThaiLaiContext context
            , IConverter converter, ICompositeViewEngine viewEngine, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _converter = converter;
            _viewEngine = viewEngine;
            _hostingEnvironment = hostingEnvironment;
        }
        [HttpGet]
        public IActionResult BaoCaoTon()
        {
            return View();
        }
        [HttpPost("searchBaoCaoTon")]
        public async Task<IActionResult> searchBaoCaoTon(string TuNgay, string DenNgay, long IdNhaCungCap, long IdHangHoa)
        {
            DateTime tuNgay = DateTime.ParseExact(TuNgay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            DateTime denNgay = DateTime.ParseExact(DenNgay, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            return Ok(await _context.ChiTietPhieuNhaps
                .AsNoTracking()
                .Include(x => x.IdvaccineNavigation)
                .Include(x => x.IdpnNavigation)
                .ThenInclude(x => x.IdnccNavigation)
                .Where(x => x.IdpnNavigation.NgayNhap.Value.Date <= denNgay
                                                        && x.IdpnNavigation.NgayNhap.Value.Date >= tuNgay
                                                        && (IdNhaCungCap == 0 ? true : x.IdpnNavigation.Idncc == IdNhaCungCap)
                                                        && (IdHangHoa == 0 ? true : x.Idvaccine == IdHangHoa)
                                                        ).OrderByDescending(x => x.IdpnNavigation.NgayNhap)
                                                        .Select(x => new
                                                        {
                                                            NgayGioNhap = x.IdpnNavigation.NgayNhap.Value.ToString("dd-MM-yyyy HH:mm"),
                                                            SoPhieuNhap = x.IdpnNavigation.SoPn,
                                                            MaNcc = x.IdpnNavigation.IdnccNavigation.MaNcc,
                                                            TenNcc = x.IdpnNavigation.IdnccNavigation.TenNcc,
                                                            SoHd = x.IdpnNavigation.SoHd,
                                                            NgayHd = x.IdpnNavigation.NgayHd,
                                                            MaHh = x.IdvaccineNavigation.MaVaccine,
                                                            TenHh = x.IdvaccineNavigation.TenVaccine,
                                                            SoLuongNhap = x.SoLuong,
                                                            DonViTinh = x.IdvaccineNavigation.DonViTinh,
                                                            GiaNhap = x.DonGia,
                                                            SoLuongTon = (x.SoLuong - (x.SoLuongXuat ?? 0)),
                                                            HanDung = x.Hsd
                                                        })
                                                        .ToListAsync());
        }
        [HttpPost("searchBaoCaoLoiNhuan")]
        public async Task<IActionResult> searchBaoCaoLoiNhuan(string TuNgay, string DenNgay)
        {
            DateTime tuNgay = DateTime.ParseExact(TuNgay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            DateTime denNgay = DateTime.ParseExact(DenNgay, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            return Ok(await _context.LichTiemBns
                .AsNoTracking()
                .Include(x => x.IdbnNavigation)
                .Include(x => x.IdvcNavigation)
                .Include(x => x.IdpnctNavigation)
                .Where(x => x.NgayThu.Value.Date <= denNgay
                                                        && x.NgayThu.Value.Date >= tuNgay
                                                        ).OrderByDescending(x => x.NgayThu).ToListAsync());
        }
        [HttpPost("inBaoCaoTon")]
        public async Task<IActionResult> inBaoCaoTon(string TuNgay, string DenNgay, long IdNhaCungCap, long IdHangHoa)
        {
            DateTime tuNgay = DateTime.ParseExact(TuNgay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            DateTime denNgay = DateTime.ParseExact(DenNgay, "dd-MM-yyyy", CultureInfo.InvariantCulture);


            ViewBag.Datas = await _context.ChiTietPhieuNhaps
                .AsNoTracking()
                .Include(x => x.IdvaccineNavigation)
                .Include(x => x.IdpnNavigation)
                .ThenInclude(x => x.IdnccNavigation)
                .Where(x => x.IdpnNavigation.NgayNhap.Value.Date <= denNgay
                                                        && x.IdpnNavigation.NgayNhap.Value.Date >= tuNgay
                                                        && (IdNhaCungCap == 0 ? true : x.IdpnNavigation.Idncc == IdNhaCungCap)
                                                        && (IdHangHoa == 0 ? true : x.Idvaccine == IdHangHoa)
                                                        ).OrderByDescending(x => x.IdpnNavigation.NgayNhap).ToListAsync();
            ViewBag.TuNgay = tuNgay.ToString("dd-MM-yyyy");
            ViewBag.DenNgay = denNgay.ToString("dd-MM-yyyy");
            ViewBag.ttDoanhNghiep = await _context.ThongTinDoanhNghieps.FirstOrDefaultAsync();
            ViewBag.logo = CommonServices.ConvertImageToBase64(_hostingEnvironment, "/images/logo.png");
            PartialViewResult partialViewResult = PartialView("inBaoCaoTonPdf");
            string viewContent = CommonServices.ConvertViewToString(ControllerContext, partialViewResult, _viewEngine);

            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                        ColorMode = ColorMode.Color,
                        Orientation = Orientation.Landscape,
                        PaperSize = PaperKind.A4,
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

            return File(pdfBytes, "application/pdf", "BaoCaoTon.pdf");
        }
        [HttpPost("inBaoCaoLoiNhuan")]
        public async Task<IActionResult> inBaoCaoLoiNhuan(string TuNgay, string DenNgay)
        {
            DateTime tuNgay = DateTime.ParseExact(TuNgay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            DateTime denNgay = DateTime.ParseExact(DenNgay, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            ViewBag.Datas = await _context.LichTiemBns
                .AsNoTracking()
                .Include(x => x.IdbnNavigation)
                .Include(x => x.IdvcNavigation)
                .Include(x => x.IdpnctNavigation)
                .Where(x => x.NgayThu.Value.Date <= denNgay
                                                        && x.NgayThu.Value.Date >= tuNgay
                                                        ).OrderByDescending(x => x.NgayThu).ToListAsync();
            ViewBag.TuNgay = tuNgay.ToString("dd-MM-yyyy");
            ViewBag.DenNgay = denNgay.ToString("dd-MM-yyyy");
            ViewBag.ttDoanhNghiep = await _context.ThongTinDoanhNghieps.FirstOrDefaultAsync();
            ViewBag.logo = CommonServices.ConvertImageToBase64(_hostingEnvironment, "/images/logo.png");
            PartialViewResult partialViewResult = PartialView("inBaoCaoLoiNhuanPdf");
            string viewContent = CommonServices.ConvertViewToString(ControllerContext, partialViewResult, _viewEngine);

            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                        ColorMode = ColorMode.Color,
                        Orientation = Orientation.Portrait,
                        PaperSize = PaperKind.A4,
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

            return File(pdfBytes, "application/pdf", "BaoCaoLoiNhuan.pdf");
        }
    }
}
