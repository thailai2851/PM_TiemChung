using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using PM_TiemChung.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace PM_TiemChung.Controllers
{
    [AllowAnonymous]
    [Route("[controller]")]
    public class DangNhapController : Controller
    {
        private ThaiLaiContext _context;
        public DangNhapController(ThaiLaiContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> DangNhap()
        {
            ViewBag.ThongTinDoanhNghiep = await _context.ThongTinDoanhNghieps.FirstOrDefaultAsync();
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }
            else
            {
                return View();
            }
        }
        [HttpGet("/NotFound")]
        public IActionResult NotFound()
        {
            return View("404");
        }
        [HttpPost]
        public async Task<IActionResult> DangNhap(string UserName, string PassWord)
        {
            if (UserName == null || PassWord == null)
            {
                return Ok(new
                {
                    statusCode = 500,
                    message = "Đăng nhập thất bại!"
                });
            }
            var taiKhoanAdmin = _context.Accounts.Include(x => x.IdnhanVienNavigation)
                .FirstOrDefault(x => x.UserName.ToLower() == UserName.ToLower() && x.Password == PassWord);
            var claims = new List<Claim>();
            if (taiKhoanAdmin != null)
            {
                claims.Add(new Claim(ClaimTypes.Name, taiKhoanAdmin.IdnhanVien.ToString(), taiKhoanAdmin.IdnhanVienNavigation.TenNhanVien));
                claims.Add(new Claim(ClaimTypes.Role, "NhanVien"));
                claims.Add(new Claim("VaiTro", taiKhoanAdmin.QuanLy.ToString()));
                await signIn(claims);
                return Ok(new
                {
                    statusCode = 200,
                    message = "Đăng nhập thành công!",
                    url = "/"
                });
            }
            else
            {
                return Ok(new
                {
                    statusCode = 500,
                    message = "Đăng nhập thất bại!"
                });
            }
        }
       
        [AllowAnonymous]
        [HttpGet("/logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/DangNhap");
        }

        // lưu Cookie
        public async Task signIn(List<Claim> claims)
        {
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
            new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1),
                IsPersistent = false
            });
        }
    }
}
