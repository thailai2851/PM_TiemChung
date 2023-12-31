﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PM_TiemChung.Models;
using PM_TiemChung.Models.Entities;
using PM_TiemChung.Models.Mapper;

namespace PM_TiemChung.Services
{
    public interface IThoiGianServices
    {
        Task<List<DmThoiGian>> searchWithKeyword(string key, bool active);
        Task<ResponseModel> UpdateThoiGian(DmThoiGianMap modelMap);
        Task<dynamic> getModelsWithNumberPage(int pageNumber);
        Task<DmThoiGian> getModelWithId(long id);
        Task<dynamic> changeActive(long id);
        Task<dynamic> getListThoiGian();
    }
    public class ThoiGianServices : IThoiGianServices
    {
        private ThaiLaiContext _context;
        private readonly IMapper _mapper;

        public ThoiGianServices(IMapper mapper, ThaiLaiContext context)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<DmThoiGian>> searchWithKeyword(string key, bool active)
        {
            // Tìm kiếm theo từ khóa tất cả các thuộc tính
            List<DmThoiGian> models;
            if (key == null)
            {
                models = await getModelsWithNumberPage(1);
            }
            else
            {
                models = await _context.DmThoiGians.Where(x => ((x.TenTg != null && x.TenTg.ToLower().Contains(key.ToLower())) ||
                                               (x.SoNgay != null && x.SoNgay.ToString().ToLower().Contains(key.ToLower()))) &&
                                                x.Active == active)
                    .OrderBy(x => x.SoNgay.ToString().Trim())
                    .ToListAsync();
            }
            return models;
        }
        public async Task<dynamic> getModelsWithNumberPage(int pageNumber)
        {
            //lấy danh sách tất cả record trong danh mục IcpYhct
            List<DmThoiGian> models = await _context.DmThoiGians.Where(x => x.Active == true)
               .OrderBy(x => x.SoNgay)
               .ToListAsync();
            // trường hợp trang cuối, lấy các dòng record cuối của bảng
            if (pageNumber == -1)
            {
                int nextPage = 0;

                // hiển thị số trang trước = phần nguyên của (tổng tất cả record/ 10)
                int prePage = (models.Count()) / 10;

                int check = models.Skip(prePage * 10).Count();
                if (check == 0)
                {
                    var result = models.Skip((prePage - 1) * 10).ToList();

                    return new
                    {
                        prePage = prePage - 1,
                        nextPage = nextPage,
                        result = result
                    };
                }
                else
                {
                    var result = models.Skip(prePage * 10).ToList();

                    return new
                    {
                        prePage = prePage,
                        nextPage = nextPage,
                        result = result
                    };
                }
            }
            //trường hợp bình thường
            else
            {
                int prePage = pageNumber - 1;
                int nextPage = pageNumber + 1;
                // kết quả hiển thị = bỏ qua các dòng record hiển thị ở các trang trước (trang hiện tại trừ 1) và hiển thị 10 record tiếp theo 
                var result = models.Skip((pageNumber - 1) * 10).Take(10).ToList();
                // kết quả 2 (result2) = bỏ qua các dòng record hiện tại và lấy 10 dòng record tiếp theo, result 2 dùng để xử lí nextPage
                var result2 = models.Skip((pageNumber) * 10).Take(10).ToList();
                // Nếu result2 = 0 và result = 0 => hết dữ liệu, người dùng k thể xem trang tiếp theo
                if (result2.Count == 0 || result.Count == 0)
                {
                    nextPage = 0;
                }

                return new
                {
                    prePage = prePage,
                    nextPage = nextPage,
                    result = result
                };

            }
        }
        public async Task<ResponseModel> UpdateThoiGian(DmThoiGianMap modelMap)
        {
            DmThoiGian model = _mapper.Map<DmThoiGian>(modelMap);
            DmThoiGian modelNew = new DmThoiGian();
            using var tran = _context.Database.BeginTransaction();
            try
            {
                if (model.Id == 0)
                {
                    model.Active = true;           
                    await _context.DmThoiGians.AddAsync(model);
                }
                else
                {
                    modelNew = await _context.DmThoiGians.FindAsync(model.Id);
                    if (modelNew != null)
                    {
                        modelNew.TenTg = model.TenTg;
                        modelNew.SoNgay = model.SoNgay;
                        _context.DmThoiGians.Update(modelNew);
                    }
                }

                await _context.SaveChangesAsync();

                tran.Commit();
                return new ResponseModel()
                {
                    statusCode = 200,
                    message = "Thành công",
                    data = modelNew
                };

            }
            catch (Exception ex)
            {
                tran.Rollback();
                return new ResponseModel()
                {
                    statusCode = 500,
                    message = "Thất bại!"
                };
            }
        }
        public async Task<DmThoiGian> getModelWithId(long id)
        {
            DmThoiGian model = await _context.DmThoiGians.FindAsync(id);
            return model;
        }
        public async Task<dynamic> changeActive(long id)
        {
            try
            {
                var model = await _context.DmThoiGians.FindAsync(id);
                model.Active = !model.Active;
                _context.DmThoiGians.Update(model);
                await _context.SaveChangesAsync();

                return new ResponseModel()
                {
                    statusCode = 200,
                    message = "Thành công!"
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel()
                {
                    statusCode = 500,
                    message = "Thất bại! "
                };
            }
        }
        public async Task<dynamic> getListThoiGian()
        {
            return await _context.DmThoiGians.Where(x => x.Active == true).ToListAsync();
        }
    }
}
