using System;
using System.Collections.Generic;

namespace PM_TiemChung.Models.Entities;

public partial class PhieuXuat
{
    public int Idpx { get; set; }

    public long? Idbn { get; set; }

    public long? Idnv { get; set; }

    public string? SoPx { get; set; }

    public string? SoHd { get; set; }

    public DateTime? NgayHd { get; set; }

    public DateTime? NgayTao { get; set; }

    public string? GhiChu { get; set; }

    public bool? Active { get; set; }

    public virtual ICollection<ChiTietPhieuXuat> ChiTietPhieuXuats { get; set; } = new List<ChiTietPhieuXuat>();

    public virtual DmBenhNhan? IdbnNavigation { get; set; }

    public virtual DmNhanVien? IdnvNavigation { get; set; }
}
