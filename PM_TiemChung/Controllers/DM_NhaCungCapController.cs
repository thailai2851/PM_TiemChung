﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using PM_TiemChung.Models.Entities;
using PM_TiemChung.Models.Mapper;
using PM_TiemChung.Services;

namespace PM_TiemChung.Controllers
{
    [Route("DanhMuc/[controller]")]
    public class DM_NhaCungCapController : Controller
    {
        private INhaCungCapServices _services;
        private ICompositeViewEngine _viewEngine;

        public DM_NhaCungCapController(INhaCungCapServices dmIcdYhctServices, ICompositeViewEngine viewEngine)
        {
            _services = dmIcdYhctServices;
            _viewEngine = viewEngine;
        }
        [HttpGet]
        public async Task<IActionResult> NhaCungCap()
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
        // sửa long
        public async Task<IActionResult> showModal(int idncc)
        {
            var model = await _services.getModelWithId(idncc);

            PartialViewResult partialViewResult = PartialView("FormNhaCungCap", model == null ? new NhaCungCap() : model);
            string viewContent = ConvertViewToString(ControllerContext, partialViewResult, _viewEngine);
            return Ok(new
            {
                view = viewContent,
                title = model == null ? "Thêm nhà cung cấp" : "Chỉnh sửa nhà cung cấp"
            });
        }
        [HttpPost("update")]
        // thêm và cập nhập
        public async Task<IActionResult> update(DmNhaCungCapMap modelMap)
        {
            var result = await _services.UpdateNhaCungCap(modelMap);
            return Ok(result);
        }
        [HttpPost("changeActive")]
        // xóa và khôi phục (chuyển active về false) sửa long
        public async Task<IActionResult> changeActive(int idncc)
        {
            var result = await _services.changeActive(idncc);
            return Ok(result);
        }
        [HttpPost("getListNhaCungCap")]
        public async Task<IActionResult> getListNhaCungCap()
        {
            var result = await _services.getListNhaCungCap();
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
