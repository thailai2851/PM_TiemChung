using Microsoft.EntityFrameworkCore;
using PM_TiemChung.Models.Entities;
using PM_TiemChung.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddDbContext<ThaiLaiContext>(c =>
        c.UseSqlServer(builder.Configuration.GetConnectionString("Connection")));
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ITinhCuTruServices, TinhCuTruServices>();
builder.Services.AddScoped<IGioiTinhServices,  GioiTinhServices>();
builder.Services.AddScoped<IThoiGianServices, ThoiGianServices>();
builder.Services.AddScoped<IDanTocServices, DanTocServices>();
builder.Services.AddScoped<INgheNghiepServices, NgheNghiepServices>();
builder.Services.AddScoped<IQuocGiaServices, QuocGiaServices>();
builder.Services.AddScoped<IVaccineServices, VaccineServices>();
builder.Services.AddScoped<IQuanCuTruServices, QuanCuTruServices>();
builder.Services.AddScoped<IXaCuTruServices, XaCuTruServices>();
builder.Services.AddScoped<INhanVienServices, NhanVienServices>();






var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
