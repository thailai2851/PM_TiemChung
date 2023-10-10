using System;
using System.Collections.Generic;

namespace PM_TiemChung.Models.Entities;

public partial class DmThoiGian
{
    public long Id { get; set; }

    public string? TenTg { get; set; }

    public int? SoNgay { get; set; }

    public bool? Active { get; set; }

    public virtual ICollection<DmProfileCt> DmProfileCts { get; set; } = new List<DmProfileCt>();

    public virtual ICollection<LichTiemBn> LichTiemBns { get; set; } = new List<LichTiemBn>();
}
