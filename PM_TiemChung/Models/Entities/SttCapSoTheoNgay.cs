using System;
using System.Collections.Generic;

namespace PM_TiemChung.Models.Entities;

public partial class SttCapSoTheoNgay
{
    public long Id { get; set; }

    public DateTime? Ngay { get; set; }

    public long? IdbenhNhan { get; set; }

    public int? SoThuTu { get; set; }

    public virtual DmBenhNhan? IdbenhNhanNavigation { get; set; }
}
