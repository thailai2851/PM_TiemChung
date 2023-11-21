using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PM_TiemChung.Models;
using PM_TiemChung.Models.Entities;

namespace PM_TiemChung.Controllers
{
    [Authorize]
    [Route("DanhMuc/[controller]")]
    public class DM_ProfileController : Controller
    {
        private ThaiLaiContext _context;
        private readonly IMapper _mapper;
        public DM_ProfileController(IMapper mapper, ThaiLaiContext context)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            ViewBag.Profiles = await _context.DmProfiles.Where(x => x.Active == true).ToListAsync();
            ViewBag.GioiTinhs = await _context.DmGioiTinhs.Where(x => x.Active == true).ToListAsync();
            return View();
        }
        [HttpPost("showProfile")]
        public async Task<IActionResult> showProfile(long id)
        {
            return Ok(await _context.DmProfiles.Include(x => x.DmProfileCts).FirstOrDefaultAsync(x => x.Id == id)); 
        }
        [HttpPost("modifyProfile")]
        public async Task<IActionResult> modifyProfile([FromBody]DmProfile profile)
        {
            using var tran = _context.Database.BeginTransaction();
            try
            {
                var addProFile = new List<DmProfileCt>();
                profile.Active = true;
                foreach (var ct in profile.DmProfileCts.ToList())
                {
                    if (ct.Id == 0)
                    {
                        profile.DmProfileCts.Remove(ct);
                        ct.Idprofile = profile.Id;
                        addProFile.Add(ct);
                    }
                }

                _context.DmProfiles.Update(profile);
                /*_context.DmProfileCts.UpdateRange(profile.DmProfileCts);*/
                await _context.DmProfileCts.AddRangeAsync(addProFile);
                _context.SaveChanges();
                tran.Commit();

                return Ok(new ResponseModel()
                {
                    statusCode = 200,
                    message = "Thành công!",
                    data = profile
                });
            }
            catch (Exception ex)
            {
                tran.Rollback();
                return Ok(new ResponseModel()
                {
                    statusCode = 500,
                    message = "Thất bại!"
                });
            }
        }
        [HttpPost("removeProfileCt")]
        public async Task<IActionResult> removeProfileCt(long id)
        {
            using var tran = _context.Database.BeginTransaction();
            try
            {
                var ct = await _context.DmProfileCts.FindAsync(id);
                _context.DmProfileCts.Remove(ct);
                _context.SaveChanges();
                tran.Commit();

                return Ok(new ResponseModel()
                {
                    statusCode = 200,
                    message = "Thành công!",
                });
            }
            catch (Exception ex)
            {
                tran.Rollback();
                return Ok(new ResponseModel()
                {
                    statusCode = 500,
                    message = "Thất bại!"
                });
            }
        }
    }
}
