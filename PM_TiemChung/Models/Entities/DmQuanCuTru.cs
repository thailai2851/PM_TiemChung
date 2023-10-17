using System;
using System.Collections.Generic;

namespace PM_TiemChung.Models.Entities;

public partial class DmQuanCuTru
{
    public long Id { get; set; }

    public string? MaQuan { get; set; }

    public string? TenQuan { get; set; }

    public long? Idtinh { get; set; }

    public int? ThongTu { get; set; }

    public bool? Active { get; set; }

    public virtual ICollection<DmBenhNhan> DmBenhNhans { get; set; } = new List<DmBenhNhan>();

    public virtual ICollection<DmXaCuTru> DmXaCuTrus { get; set; } = new List<DmXaCuTru>();

    public virtual DmTinhCuTru? IdtinhNavigation { get; set; }
}
