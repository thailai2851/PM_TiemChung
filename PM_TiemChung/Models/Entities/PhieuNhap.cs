using System;
using System.Collections.Generic;

namespace PM_TiemChung.Models.Entities;

public partial class PhieuNhap
{
    public int Idpn { get; set; }

    public int? Idncc { get; set; }

    public long? Idnv { get; set; }

    public string? SoPn { get; set; }

    public string? SoHd { get; set; }

    public DateTime? NgayNhap { get; set; }

    public DateTime? NgayHd { get; set; }

    public string? GhiChu { get; set; }

    public bool? Active { get; set; }

    public virtual ICollection<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; } = new List<ChiTietPhieuNhap>();

    public virtual NhaCungCap? IdnccNavigation { get; set; }

    public virtual DmNhanVien? IdnvNavigation { get; set; }
}
