using System;
using System.Collections.Generic;

namespace PM_TiemChung.Models.Entities;

public partial class QlMa
{
    public long Id { get; set; }

    public string? KyHieu { get; set; }

    public DateTime? Ngay { get; set; }

    public int? Stt { get; set; }
}
