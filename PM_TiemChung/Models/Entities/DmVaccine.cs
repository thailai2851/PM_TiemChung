using System;
using System.Collections.Generic;

namespace PM_TiemChung.Models.Entities;

public partial class DmVaccine
{
    public long Id { get; set; }

    public string? MaVaccine { get; set; }

    public string? TenVaccine { get; set; }

    public string? DonViTinh { get; set; }

    public double? GiaBan { get; set; }

    public bool? Active { get; set; }

    public virtual ICollection<DmProfileCt> DmProfileCts { get; set; } = new List<DmProfileCt>();

    public virtual ICollection<LichTiemBn> LichTiemBns { get; set; } = new List<LichTiemBn>();
}
