using System;
using System.Collections.Generic;

namespace PM_TiemChung.Models.Entities;

public partial class HtQuyLuatMa
{
    public long Id { get; set; }

    public string? TenBang { get; set; }

    public string? KyTuDau { get; set; }

    public int? DoDaiMa { get; set; }
}
