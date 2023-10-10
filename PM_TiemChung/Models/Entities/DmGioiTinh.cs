using System;
using System.Collections.Generic;

namespace PM_TiemChung.Models.Entities;

public partial class DmGioiTinh
{
    public long Id { get; set; }

    public string? MaGioiTinh { get; set; }

    public string? TenGioiTinh { get; set; }

    public int? ThongTu { get; set; }

    public bool? Active { get; set; }

    public virtual ICollection<DmBenhNhan> DmBenhNhans { get; set; } = new List<DmBenhNhan>();

    public virtual ICollection<DmNhanVien> DmNhanViens { get; set; } = new List<DmNhanVien>();
}
