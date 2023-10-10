using System;
using System.Collections.Generic;

namespace PM_TiemChung.Models.Entities;

public partial class DmDanToc
{
    public long Id { get; set; }

    public string? MaDanToc { get; set; }

    public string? TenDanToc { get; set; }

    public string? TenGoiKhac { get; set; }

    public string? DiaBanCuTru { get; set; }

    public int? ThongTu { get; set; }

    public bool? Active { get; set; }

    public virtual ICollection<DmBenhNhan> DmBenhNhans { get; set; } = new List<DmBenhNhan>();
}
