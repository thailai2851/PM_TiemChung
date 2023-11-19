using System;
using System.Collections.Generic;

namespace PM_TiemChung.Models.Entities;

public partial class DmNhanVien
{
    public long Id { get; set; }

    public string? MaNhanVien { get; set; }

    public string? TenNhanVien { get; set; }

    public DateTime? NgaySinh { get; set; }

    public long? Idgt { get; set; }

    public string? DiaChi { get; set; }

    public string? QueQuan { get; set; }

    public string? DienThoai { get; set; }

    public string? Mabhxh { get; set; }

    public string? Macchn { get; set; }

    public DateTime? Ngaycapcchn { get; set; }

    public string? Noicapcchn { get; set; }

    public bool? Active { get; set; }

    public virtual DmGioiTinh? IdgtNavigation { get; set; }

    public virtual ICollection<LichTiemBn> LichTiemBnIdbskNavigations { get; set; } = new List<LichTiemBn>();

    public virtual ICollection<LichTiemBn> LichTiemBnIdnhanVienThuNavigations { get; set; } = new List<LichTiemBn>();

    public virtual ICollection<LichTiemBn> LichTiemBnIdnhanVienTiemNavigations { get; set; } = new List<LichTiemBn>();

    public virtual ICollection<PhieuNhap> PhieuNhaps { get; set; } = new List<PhieuNhap>();

    public virtual ICollection<PhieuXuat> PhieuXuats { get; set; } = new List<PhieuXuat>();
}
