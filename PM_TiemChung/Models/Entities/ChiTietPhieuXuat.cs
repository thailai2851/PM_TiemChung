using System;
using System.Collections.Generic;

namespace PM_TiemChung.Models.Entities;

public partial class ChiTietPhieuXuat
{
    public int Idctpx { get; set; }

    public int? Idpx { get; set; }

    public int? Idctpn { get; set; }

    public long? Idvaccine { get; set; }

    public double? SoLuong { get; set; }

    public double? DonGia { get; set; }

    public double? Cktm { get; set; }

    public double? Thue { get; set; }

    public bool? Active { get; set; }

    public virtual ChiTietPhieuNhap? IdctpnNavigation { get; set; }

    public virtual PhieuXuat? IdpxNavigation { get; set; }

    public virtual DmVaccine? IdvaccineNavigation { get; set; }
}
