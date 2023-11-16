using Microsoft.EntityFrameworkCore;
using PM_TiemChung.Models.Entities;
using System.Data;

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
    }
}
