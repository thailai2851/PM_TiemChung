﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using PM_TiemChung.Models.Entities;
using PM_TiemChung.Models.Mapper;
using PM_TiemChung.Services;

namespace PM_TiemChung.Controllers
{
    [Authorize]
    [Route("DanhMuc/[controller]")]
    public class DM_QuanCuTruController : Controller
    {
        private IQuanCuTruServices _services;
        private ICompositeViewEngine _viewEngine;

        public DM_QuanCuTruController(IQuanCuTruServices dmIcdYhctServices, ICompositeViewEngine viewEngine)
        {
            _services = dmIcdYhctServices;
            _viewEngine = viewEngine;
        }
        [HttpGet]
        public async Task<IActionResult> QuanCuTru()
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

            PartialViewResult partialViewResult = PartialView("FormQuanCuTru", model == null ? new DmQuanCuTru() : model);
            string viewContent = ConvertViewToString(ControllerContext, partialViewResult, _viewEngine);
            return Ok(new
            {
                view = viewContent,
                title = model == null ? "Thêm quận cư trú" : "Chỉnh sửa quận cư trú"
            });
        }
        [HttpPost("update")]
        // thêm và cập nhập
        public async Task<IActionResult> update(DmQuanCuTruMap modelMap)
        {
            var result = await _services.UpdateQuanCuTru(modelMap);
            return Ok(result);
        }
        [HttpPost("changeActive")]
        // xóa và khôi phục (chuyển active về false)
        public async Task<IActionResult> changeActive(long id)
        {
            var result = await _services.changeActive(id);
            return Ok(result);
        }
        [HttpPost("getListQuanCuTru")]
        public async Task<IActionResult> getListQuanCuTru()
        {
            var result = await _services.getListQuanCuTru();
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
