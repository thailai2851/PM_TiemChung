using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using PM_TiemChung.Models.Entities;
using System.Data;
using System.Globalization;

namespace PM_TiemChung.Services
{
    public static class CommonServices
    {
        public static async Task<string> autoMa(ThaiLaiContext context, string kyKieu)
        {
            var qlMa = await context.QlMas.FirstOrDefaultAsync(x => x.Ngay.Value.Date == DateTime.Now.Date && x.KyHieu == kyKieu);
            if (qlMa == null)
            {
                var dateNow = DateTime.Now.Date;
                QlMa qlMaNew = new QlMa();
                qlMaNew.Stt = 1;
                qlMaNew.KyHieu = kyKieu;
                qlMaNew.Ngay = dateNow;

                await context.QlMas.AddAsync(qlMaNew);
                await context.SaveChangesAsync();

                return kyKieu + dateNow.ToString("yyyyMMdd") + 1.ToString("D4");
            }
            else
            {
                qlMa.Stt += 1;
                context.QlMas.Update(qlMa);
                await context.SaveChangesAsync();

                return kyKieu + qlMa.Ngay.Value.ToString("yyyyMMdd") + qlMa.Stt.Value.ToString("D4");
            }
        }
        public static string FormatEvenNumber<T>(T number) where T : struct, IConvertible
        {
            if (!typeof(T).IsPrimitive)
            {
                throw new ArgumentException("Chỉ được phép nhập số");
            }

            // Chuyển đổi giá trị số sang kiểu dữ liệu double để định dạng
            double doubleValue = Convert.ToDouble(number, CultureInfo.InvariantCulture);

            // Định dạng theo chuỗi "#,###"
            return doubleValue.ToString("#,###", CultureInfo.InvariantCulture);
        }
        public static string FormatOddNumber<T>(T number) where T : struct, IConvertible
        {
            if (!typeof(T).IsPrimitive)
            {
                throw new ArgumentException("Chỉ được phép nhập số");
            }

            // Chuyển đổi giá trị số sang kiểu dữ liệu double để định dạng
            double doubleValue = Convert.ToDouble(number, CultureInfo.InvariantCulture);

            // Định dạng theo chuỗi "#,###"
            return doubleValue.ToString("#,###", CultureInfo.InvariantCulture);
        }
        public static object toEmpty(object data)
        {
            return data == null ? "" : data;
        }
        public static string CalculateAgeString(DateTime birthday)
        {
            int ageInMonths = CalculateAge(birthday);

            if (ageInMonths < 72)
            {
                return $"{ageInMonths} tháng";
            }
            else
            {
                int ageInYears = ageInMonths / 12;
                return $"{ageInYears} tuổi";
            }
        }

        public static int CalculateAge(DateTime birthday)
        {
            DateTime currentDate = DateTime.Now;
            int ageInMonths = (currentDate.Year - birthday.Year) * 12 + currentDate.Month - birthday.Month;

            if (currentDate.Day < birthday.Day)
            {
                ageInMonths--;
            }

            return ageInMonths;
        }
        public static string ConvertViewToString(ControllerContext controllerContext, PartialViewResult pvr, ICompositeViewEngine _viewEngine)
        {
            using (StringWriter writer = new StringWriter())
            {
                ViewEngineResult vResult = _viewEngine.FindView(controllerContext, pvr.ViewName, false);
                ViewContext viewContext = new ViewContext(controllerContext, vResult.View, pvr.ViewData, pvr.TempData, writer, new HtmlHelperOptions());

                vResult.View.RenderAsync(viewContext);

                return writer.GetStringBuilder().ToString();
            }
        }
        public static string ConvertImageToBase64(IWebHostEnvironment h, string imagePath)
        {
            byte[] imageBytes = System.IO.File.ReadAllBytes(h.WebRootPath + imagePath);
            string base64String = Convert.ToBase64String(imageBytes);

            return base64String;
        }
    }
}
