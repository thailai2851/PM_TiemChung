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
    [Route("QuanLy/[controller]")]

    public class QL_BaoCaoLichHenController : Controller
    {
        private ThaiLaiContext _context;
        private readonly IMapper _mapper;
        private readonly IConverter _converter;
        private ICompositeViewEngine _viewEngine;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public QL_BaoCaoLichHenController(IMapper mapper, ThaiLaiContext context
            , IConverter converter, ICompositeViewEngine viewEngine, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _converter = converter;
            _viewEngine = viewEngine;
            _hostingEnvironment = hostingEnvironment;
        }
        [HttpGet]
        public IActionResult BaoCaoLichHen()
        {
            return View();
        }
        [HttpPost("searchBaoCaoLichHen")]
        public async Task<IActionResult> searchBaoCaoLichHen(string TuNgay, string DenNgay)
        {
            DateTime tuNgay = DateTime.ParseExact(TuNgay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            DateTime denNgay = DateTime.ParseExact(DenNgay, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            return Ok(await _context.LichTiemBns
                .AsNoTracking()
                .Include(x => x.IdbnNavigation)
                .ThenInclude(x => x.IdnnNavigation)
                .Include(x => x.IdbnNavigation)
                .ThenInclude(x => x.IdgtNavigation)
                .Include(x => x.IdvcNavigation)
                .Where(x => x.NgayThu.Value.Date <= denNgay
                                                        && x.NgayThu.Value.Date >= tuNgay
                                                        ).OrderByDescending(x => x.NgayThu).ToListAsync());
        }
        [HttpPost("inBaoCaoLichHen")]
        public async Task<IActionResult> inBaoCaoLichHen(string TuNgay, string DenNgay)
        {
            DateTime tuNgay = DateTime.ParseExact(TuNgay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            DateTime denNgay = DateTime.ParseExact(DenNgay, "dd-MM-yyyy", CultureInfo.InvariantCulture);


            ViewBag.Datas = await _context.LichTiemBns
                .AsNoTracking()
                .Include(x => x.IdbnNavigation)
                .ThenInclude(x => x.IdnnNavigation)
                .Include(x => x.IdbnNavigation)
                .ThenInclude(x => x.IdgtNavigation)
                .Include(x => x.IdvcNavigation)
                .Where(x => x.NgayThu.Value.Date <= denNgay
                                                        && x.NgayThu.Value.Date >= tuNgay
                                                        ).OrderByDescending(x => x.NgayThu).ToListAsync();
            ViewBag.TuNgay = tuNgay.ToString("dd-MM-yyyy");
            ViewBag.DenNgay = denNgay.ToString("dd-MM-yyyy");
            ViewBag.ttDoanhNghiep = await _context.ThongTinDoanhNghieps.FirstOrDefaultAsync();
            ViewBag.logo = CommonServices.ConvertImageToBase64(_hostingEnvironment, "/images/logo.png");
            PartialViewResult partialViewResult = PartialView("baoCaoLichHenPdf");
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

            return File(pdfBytes, "application/pdf", "file.pdf");
        }
    }
}
