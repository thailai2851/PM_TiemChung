using System;
using System.Collections.Generic;

namespace PM_TiemChung.Models.Entities;

public partial class DmLichTiem
{
    public long Id { get; set; }

    public long? Idgt { get; set; }

    public long? Idvc { get; set; }

    public int? TgsomNhat { get; set; }

    public int? TgtreNhat { get; set; }

    public long? IdthoiGian { get; set; }

    public int? SoLanTiem { get; set; }

    public long? MuiTienQuyet { get; set; }
}
