using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PM_TiemChung.Models.Entities;
using PM_TiemChung.Models.Mapper;
using PM_TiemChung.Services;

namespace PM_TiemChung.Controllers
{
    [Route("HangHoa/[controller]")]
    public class HH_NhapKhoController : Controller
    {
        private IVaccineServices _vcServices;
        private INhaCungCapServices _servicesNCC;

        private ThaiLaiContext _context;
        private readonly IMapper _mapper;
        public HH_NhapKhoController(IVaccineServices vaccineServices, ThaiLaiContext context, IMapper mapper, INhaCungCapServices servicesNCC)
        {
            _vcServices = vaccineServices;
            _context = context; 
            _mapper = mapper;
            _servicesNCC = servicesNCC;
        }
        [HttpGet]
        public IActionResult NhapKho()
        {
            return View();
        }
        [HttpPost("getListVaccine")]
        public async Task<dynamic> getListVaccine()
        {
            return Ok(await _vcServices.getListVaccine());
        }
        public string taoSoPhieuNhap()
        {
            DateTime now = DateTime.Now;
            string date = now.ToString("yyyyMMdd");
            var phieuNhap = _context.PhieuNhaps.Where(x => x.SoPn.Contains(date)).ToList();
            return $"PN-{date}-{(phieuNhap.Count() + 1).ToString("D2")}";
        }
        [HttpPost("getListNhaCungCap")]
        public async Task<IActionResult> getListNhaCungCap()
        {
            var result = await _servicesNCC.getListNhaCungCap();
            return Ok(result);
        }
        [HttpPost("getDVT")]
        public async Task<string> getDVT(long idVC)
        {
            DmVaccine vc = await _context.DmVaccines.FindAsync(idVC);
            return vc.DonViTinh;
        }
        [HttpPost("/NhapKho/ThemPhieuNhap")]
        public async Task<dynamic> ThemPhieuNhap([FromBody] TTPhieuNhap data)
        {
            PhieuNhapMap phieuNhapMap = data.PhieuNhap;
            List<ChiTietPhieuNhapMap> chiTietPhieuNhapMaps = data.ChiTietPhieuNhap;


            using var tran = _context.Database.BeginTransaction();
            try
            {
                List<ChiTietPhieuNhap> chiTietPhieuNhaps = _mapper.Map<List<ChiTietPhieuNhap>>(chiTietPhieuNhapMaps);
                PhieuNhap phieuNhap = _mapper.Map<PhieuNhap>(phieuNhapMap);

                phieuNhap.SoPn = taoSoPhieuNhap();
                phieuNhap.Idnv = 1;
                phieuNhap.Active = true;
                await _context.PhieuNhaps.AddAsync(phieuNhap);
                await _context.SaveChangesAsync();
                foreach (ChiTietPhieuNhap chiTiet in chiTietPhieuNhaps)
                {
                    chiTiet.Idpn = phieuNhap.Idpn;
                    chiTiet.Active = true;
                }
                await _context.ChiTietPhieuNhaps.AddRangeAsync(chiTietPhieuNhaps);
                await _context.SaveChangesAsync();
                tran.Commit();
                return new
                {
                    statusCode = 200,
                    message = "Thành công",
                };
            }
            catch (Exception ex)
            {
                tran.Rollback();
                return new
                {
                    statusCode = 500,
                    message = "Thất bại",
                };
            }
        }
        public class TTPhieuNhap
        {
            public PhieuNhapMap PhieuNhap { get; set; }
            public List<ChiTietPhieuNhapMap> ChiTietPhieuNhap { get; set; }
        }
    }
}
