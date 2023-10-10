using System;
using System.Collections.Generic;

namespace PM_TiemChung.Models.Entities;

public partial class DmNgheNghiep
{
    public long Id { get; set; }

    public string? MaNgheNghiep { get; set; }

    public string? TenNgheNghiep { get; set; }

    public int? ThongTu { get; set; }

    public bool? Active { get; set; }

    public virtual ICollection<DmBenhNhan> DmBenhNhans { get; set; } = new List<DmBenhNhan>();
}
