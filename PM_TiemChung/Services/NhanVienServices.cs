using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PM_TiemChung.Models;
using PM_TiemChung.Models.Entities;
using PM_TiemChung.Models.Mapper;

namespace PM_TiemChung.Services
{
    public interface INhanVienServices
    {
        Task<List<DmNhanVien>> searchWithKeyword(string key, bool active);
        Task<ResponseModel> UpdateNhanVien(DmNhanVienMap modelMap);
        Task<dynamic> getModelsWithNumberPage(int pageNumber);
        Task<DmNhanVien> getModelWithId(long id);
        Task<dynamic> changeActive(long id);
    }
    public class NhanVienServices : INhanVienServices
    {
        private ThaiLaiContext _context;
        private readonly IMapper _mapper;

        public NhanVienServices(IMapper mapper, ThaiLaiContext context)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<DmNhanVien>> searchWithKeyword(string key, bool active)
        {
            // Tìm kiếm theo từ khóa tất cả các thuộc tính
            List<DmNhanVien> models;
            if (key == null)
            {
                models = await getModelsWithNumberPage(1);
            }
            else
            {
                models = await _context.DmNhanViens
               .AsNoTracking()
               .Include(x => x.IdgtNavigation)
               .Where(x => ((x.MaNhanVien != null && x.MaNhanVien.ToLower().Contains(key.ToLower())) ||
                                              (x.TenNhanVien != null && x.TenNhanVien.ToLower().Contains(key.ToLower())) ||
                                              (x.NgaySinh != null && x.NgaySinh.ToString().ToLower().Contains(key.ToLower())) ||
                                              (x.IdgtNavigation.TenGioiTinh != null && x.IdgtNavigation.TenGioiTinh.ToLower().Contains(key.ToLower())) ||
                                              (x.DiaChi != null && x.DiaChi.ToLower().Contains(key.ToLower())) ||
                                              (x.QueQuan != null && x.QueQuan.ToLower().Contains(key.ToLower())) ||
                                              (x.DienThoai != null && x.DienThoai.ToLower().Contains(key.ToLower())) ||
                                              (x.Mabhxh != null && x.Mabhxh.ToLower().Contains(key.ToLower())) ||
                                              (x.Macchn != null && x.Macchn.ToLower().Contains(key.ToLower())) ||
                                              (x.Ngaycapcchn != null && x.Ngaycapcchn.ToString().ToLower().Contains(key.ToLower())) ||
                                              (x.Noicapcchn != null && x.Noicapcchn.ToLower().Contains(key.ToLower()))) &&
                                               x.Active == active)
                    .OrderBy(x => x.TenNhanVien.Trim())
                    .ToListAsync();
            }
            return models;
        }
        public async Task<dynamic> getModelsWithNumberPage(int pageNumber)
        {
            //lấy danh sách tất cả record trong danh mục IcpYhct
            List<DmNhanVien> models = await _context.DmNhanViens
                .Include(x => x.IdgtNavigation).Where(x => x.Active == true)
                .AsNoTracking()
                .OrderBy(x => x.TenNhanVien.Trim())
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
        public async Task<ResponseModel> UpdateNhanVien(DmNhanVienMap modelMap)
        {
            DmNhanVien model = _mapper.Map<DmNhanVien>(modelMap);
            DmNhanVien modelNew = new DmNhanVien();
            using var tran = _context.Database.BeginTransaction();
            try
            {
                if (model.Id == 0)
                {
                    model.Active = true;
                    await _context.DmNhanViens.AddAsync(model);
                }
                else
                {
                    modelNew = await _context.DmNhanViens.FindAsync(model.Id);
                    if (modelNew != null)
                    {
                        modelNew.MaNhanVien = model.MaNhanVien;
                        modelNew.TenNhanVien = model.TenNhanVien;
                        modelNew.NgaySinh = model.NgaySinh;
                        modelNew.Idgt = model.Idgt;
                        modelNew.DiaChi = model.DiaChi;
                        modelNew.QueQuan = model.QueQuan;
                        modelNew.DienThoai = model.DienThoai;
                        modelNew.Mabhxh = model.Mabhxh;
                        modelNew.Macchn = model.Macchn;
                        modelNew.Ngaycapcchn = model.Ngaycapcchn;
                        modelNew.Noicapcchn = model.Noicapcchn;
                        _context.DmNhanViens.Update(modelNew);
                        modelNew = await _context.DmNhanViens.Include(x => x.IdgtNavigation).FirstOrDefaultAsync(x => x.Id == model.Id);
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
        public async Task<DmNhanVien> getModelWithId(long id)
        {
            DmNhanVien model = await _context.DmNhanViens.FindAsync(id);
            return model;
        }
        public async Task<dynamic> changeActive(long id)
        {
            try
            {
                var model = await _context.DmNhanViens.FindAsync(id);
                model.Active = !model.Active;
                _context.DmNhanViens.Update(model);
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
    }
}
