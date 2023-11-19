using System;
using System.Collections.Generic;

namespace PM_TiemChung.Models.Entities;

public partial class ThongTinDoanhNghiep
{
    public int Id { get; set; }

    public string? TenDoanhNghiep { get; set; }

    public string? DiaChi { get; set; }

    public string? DienThoai { get; set; }

    public string? Email { get; set; }

    public string? Mst { get; set; }

    public string? SoTk { get; set; }

    public string? NganHang { get; set; }

    public string? ChuTk { get; set; }
}
