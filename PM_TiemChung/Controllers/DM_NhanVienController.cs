using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using PM_TiemChung.Models.Entities;
using PM_TiemChung.Models.Mapper;
using PM_TiemChung.Services;

namespace PM_TiemChung.Controllers
{
    [Authorize]
    [Route("DanhMuc/[controller]")]
    public class DM_NhanVienController : Controller
    {
        private INhanVienServices _services;
        private ICompositeViewEngine _viewEngine;
        private ThaiLaiContext _context;

        public DM_NhanVienController(INhanVienServices dmIcdYhctServices, ICompositeViewEngine viewEngine, ThaiLaiContext context)
        {
            _services = dmIcdYhctServices;
            _viewEngine = viewEngine;
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> NhanVien()
        {
            ViewBag.Models = await _services.getModelsWithNumberPage(1);
            return View();
        }
        [HttpPost("changePage")]
        // Chuyển trang 
        public async Task<IActionResult> changePage(int pageNumber)
        {
            var result = await _services.getModelsWithNumberPage(pageNumber);
            return Ok(result);
        }
        [HttpPost("api/getModelsWithNumberPage")]
        // Phân trang
        public async Task<IActionResult> getModelsWithNumberPage()
        {
            var models = await _services.getModelsWithNumberPage(1);
            return Ok(models);
        }
        [HttpPost("api/searchWithKeyword")]
        // Tìm kiếm trong danh mục với từ khóa
        public async Task<IActionResult> searchWithKeyword(string key, bool active)
        {
            var result = await _services.searchWithKeyword(key, active);
            return Ok(result);
        }
        [HttpPost("showModal")]
        public async Task<IActionResult> showModal(long id)
        {
            var model = await _services.getModelWithId(id);
            ViewBag.gioiTinhs = _context.DmGioiTinhs.Where(x => x.Active == true);
            PartialViewResult partialViewResult = PartialView("FormNhanVien", model == null ? new DmNhanVien() : model);
            string viewContent = ConvertViewToString(ControllerContext, partialViewResult, _viewEngine);
            return Ok(new
            {
                view = viewContent,
                title = model == null ? "Thêm nhân viên" : "Chỉnh sửa nhân viên"
            });
        }
        [HttpPost("update")]
        // thêm và cập nhập
        public async Task<IActionResult> update(DmNhanVienMap modelMap)
        {
            var result = await _services.UpdateNhanVien(modelMap);
            return Ok(result);
        }
        [HttpPost("changeActive")]
        // xóa và khôi phục (chuyển active về false)
        public async Task<IActionResult> changeActive(long id)
        {
            var result = await _services.changeActive(id);
            return Ok(result);
        }
        public string ConvertViewToString(ControllerContext controllerContext, PartialViewResult pvr, ICompositeViewEngine _viewEngine)
        {
            using (StringWriter writer = new StringWriter())
            {
                ViewEngineResult vResult = _viewEngine.FindView(controllerContext, pvr.ViewName, false);
                ViewContext viewContext = new ViewContext(controllerContext, vResult.View, pvr.ViewData, pvr.TempData, writer, new HtmlHelperOptions());

                vResult.View.RenderAsync(viewContext);

                return writer.GetStringBuilder().ToString();
            }
        }
    }
}
