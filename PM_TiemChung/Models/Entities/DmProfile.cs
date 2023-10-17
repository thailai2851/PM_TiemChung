using System;
using System.Collections.Generic;

namespace PM_TiemChung.Models.Entities;

public partial class DmProfile
{
    public long Id { get; set; }

    public long? Idgt { get; set; }

    public string? TenProfile { get; set; }

    public bool? Active { get; set; }

    public virtual ICollection<DmProfileCt> DmProfileCts { get; set; } = new List<DmProfileCt>();

    public virtual DmGioiTinh? IdgtNavigation { get; set; }
}
