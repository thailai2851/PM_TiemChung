using System;
using System.Collections.Generic;

namespace PM_TiemChung.Models.Entities;

public partial class ChiTietPhieuXuat
{
    public long Id { get; set; }

    public long? Idbn { get; set; }

    public int? Idctpn { get; set; }

    public long? Idvaccine { get; set; }

    public double? SoLuong { get; set; }

    public double? DonGia { get; set; }

    public DateTime? NgayXuat { get; set; }

    public bool? Active { get; set; }

    public virtual ChiTietPhieuNhap? IdctpnNavigation { get; set; }

    public virtual DmVaccine? IdvaccineNavigation { get; set; }
}
