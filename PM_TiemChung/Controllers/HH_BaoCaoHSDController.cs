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
    public class HH_BaoCaoHSDController : Controller
    {
        private ThaiLaiContext _context;
        private readonly IMapper _mapper;
        private readonly IConverter _converter;
        private ICompositeViewEngine _viewEngine;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public HH_BaoCaoHSDController(IMapper mapper, ThaiLaiContext context
            , IConverter converter, ICompositeViewEngine viewEngine, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _converter = converter;
            _viewEngine = viewEngine;
            _hostingEnvironment = hostingEnvironment;
        }
        [HttpGet]
        public IActionResult BaoCaoHSD()
        {
            return View();
        }
        [HttpPost("searchBaoCaoHSD")]
        public async Task<IActionResult> searchBaoCaoTon(string TuNgay, string DenNgay, long IdHangHoa)
        {
            DateTime tuNgay = DateTime.ParseExact(TuNgay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            DateTime denNgay = DateTime.ParseExact(DenNgay, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            return Ok(await _context.ChiTietPhieuNhaps
                .AsNoTracking()
                .Where(x => x.Hsd != null 
                && (x.Hsd.Value.Date <= denNgay)
                && (x.Hsd.Value.Date >= tuNgay)
                && (IdHangHoa == 0 ? true : x.Idvaccine == IdHangHoa))
                                                        .Select(x => new
                                                        {
                                                            nsx = x.Nsx,
                                                            hsd = x.Hsd,
                                                            tenvc = x.IdvaccineNavigation.TenVaccine,
                                                            mavc = x.IdvaccineNavigation.MaVaccine,
                                                            ncc = x.IdpnNavigation.IdnccNavigation.TenNcc,
                                                            dvt = x.IdvaccineNavigation.DonViTinh,
                                                            sl = x.SoLuong,
                                                            slx = x.SoLuongXuat
                                                        })
                                                        .ToListAsync());
        }
        [HttpPost("inBaoCaoHSD")]
        public async Task<IActionResult> inBaoCaoTon(string TuNgay, string DenNgay, long IdHangHoa)
        {
            DateTime tuNgay = DateTime.ParseExact(TuNgay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            DateTime denNgay = DateTime.ParseExact(DenNgay, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            ViewBag.Datas = await _context.ChiTietPhieuNhaps.Include(x => x.IdvaccineNavigation)
                .Include(x => x.IdpnNavigation)
                .ThenInclude(x => x.IdnccNavigation)
                .Where(x => x.Hsd != null
                && (x.Hsd.Value.Date <= denNgay)
                && (x.Hsd.Value.Date >= tuNgay)
                && (IdHangHoa == 0 ? true : x.Idvaccine == IdHangHoa))
                .ToListAsync();
            ViewBag.TuNgay = tuNgay.ToString("dd-MM-yyyy");
            ViewBag.DenNgay = denNgay.ToString("dd-MM-yyyy");
            ViewBag.ttDoanhNghiep = await _context.ThongTinDoanhNghieps.FirstOrDefaultAsync();
            ViewBag.logo = CommonServices.ConvertImageToBase64(_hostingEnvironment, "/images/logo.png");
            
            PartialViewResult partialViewResult = PartialView("inBaoCaoHSDPdf");
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

            return File(pdfBytes, "application/pdf", "BaoCaoHSD.pdf");
        }
    }
}
