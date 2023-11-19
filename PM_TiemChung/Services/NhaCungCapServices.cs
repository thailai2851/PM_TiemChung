using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PM_TiemChung.Models;
using PM_TiemChung.Models.Entities;
using PM_TiemChung.Models.Mapper;

namespace PM_TiemChung.Services
{
    public interface INhaCungCapServices
    {
        Task<List<NhaCungCap>> searchWithKeyword(string key, bool active);
        Task<ResponseModel> UpdateNhaCungCap(DmNhaCungCapMap modelMap);
        Task<dynamic> getModelsWithNumberPage(int pageNumber);
        Task<NhaCungCap> getModelWithId(long id);
        Task<dynamic> changeActive(long id);
        Task<dynamic> getListNhaCungCap();
    }
    public class NhaCungCapServices : INhaCungCapServices
    {
        private ThaiLaiContext _context;
        private readonly IMapper _mapper;

        public NhaCungCapServices(IMapper mapper, ThaiLaiContext context)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<NhaCungCap>> searchWithKeyword(string key, bool active)
        {
            // Tìm kiếm theo từ khóa tất cả các thuộc tính
            List<NhaCungCap> models;
            if (key == null)
            {
                models = await getModelsWithNumberPage(1);
            }
            else
            {
                models = await _context.NhaCungCaps.Where(x => ((x.MaNcc != null && x.MaNcc.ToLower().Contains(key.ToLower())) ||
                                                (x.DiaChi != null && x.DiaChi.ToLower().Contains(key.ToLower())) ||
                                                (x.DienThoai != null && x.DienThoai.ToLower().Contains(key.ToLower())) ||
                                                (x.TenNcc != null && x.TenNcc.ToLower().Contains(key.ToLower()))) &&
                                                x.Active == active)
                    .OrderBy(x => x.TenNcc.Trim())
                    .ToListAsync();
            }
            return models;
        }
        public async Task<dynamic> getModelsWithNumberPage(int pageNumber)
        {
            //lấy danh sách tất cả record trong danh mục IcpYhct
            List<NhaCungCap> models = await _context.NhaCungCaps.Where(x => x.Active == true)
               .OrderBy(x => x.TenNcc.Trim())
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
        public async Task<ResponseModel> UpdateNhaCungCap(DmNhaCungCapMap modelMap)
        {
            NhaCungCap model = _mapper.Map<NhaCungCap>(modelMap);
            NhaCungCap modelNew = new NhaCungCap();
            using var tran = _context.Database.BeginTransaction();
            try
            {
                if (model.Idncc == 0)
                {
                    model.Active = true;
                    await _context.NhaCungCaps.AddAsync(model);
                }
                else
                {
                    modelNew = await _context.NhaCungCaps.FindAsync(model.Idncc);
                    if (modelNew != null)
                    {
                        modelNew.MaNcc = model.MaNcc;
                        modelNew.TenNcc = model.TenNcc;
                        modelNew.DiaChi = model.DiaChi;
                        modelNew.DienThoai = model.DienThoai;
                        modelNew.Mail = model.Mail;
                        modelNew.GhiChu = model.GhiChu;
                        _context.NhaCungCaps.Update(modelNew);
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
        public async Task<NhaCungCap> getModelWithId(long id)
        {
            NhaCungCap model = await _context.NhaCungCaps.FindAsync(id);
            return model;
        }
        public async Task<dynamic> changeActive(long id)
        {
            try
            {
                var model = await _context.NhaCungCaps.FindAsync(id);
                model.Active = !model.Active;
                _context.NhaCungCaps.Update(model);
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
        public async Task<dynamic> getListNhaCungCap()
        {
            return await _context.NhaCungCaps
                .Select(x => new
                {
                    id = x.Idncc,
                    ma = x.MaNcc,
                    ten = x.TenNcc,
                })
                .ToListAsync();
        }
    }
}
