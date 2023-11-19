using System;
using System.Collections.Generic;

namespace PM_TiemChung.Models.Entities;

public partial class TonKho
{
    public int Idtk { get; set; }

    public int? Idctpn { get; set; }

    public double? Slcon { get; set; }

    public DateTime? NgayNhap { get; set; }

    public virtual ChiTietPhieuNhap? IdctpnNavigation { get; set; }
}
