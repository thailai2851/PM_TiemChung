using System;
using System.Collections.Generic;

namespace PM_TiemChung.Models.Entities;

public partial class DmXaCuTru
{
    public long Id { get; set; }

    public string? MaXa { get; set; }

    public string? TenXa { get; set; }

    public long? Idquan { get; set; }

    public int? ThongTu { get; set; }

    public bool? Active { get; set; }

    public virtual ICollection<DmBenhNhan> DmBenhNhans { get; set; } = new List<DmBenhNhan>();
}
