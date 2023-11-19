using System;
using System.Collections.Generic;

namespace PM_TiemChung.Models.Entities;

public partial class ChiTietPhieuNhap
{
    public int Idctpn { get; set; }

    public int? Idpn { get; set; }

    public long? Idvaccine { get; set; }

    public double? SoLuong { get; set; }

    public double? DonGia { get; set; }

    public double? Cktm { get; set; }

    public double? Thue { get; set; }

    public DateTime? Nsx { get; set; }

    public DateTime? Hsd { get; set; }

    public string? GhiChu { get; set; }

    public bool? Active { get; set; }

    public virtual ICollection<ChiTietPhieuXuat> ChiTietPhieuXuats { get; set; } = new List<ChiTietPhieuXuat>();

    public virtual PhieuNhap? IdpnNavigation { get; set; }

    public virtual DmVaccine? IdvaccineNavigation { get; set; }

    public virtual ICollection<TonKho> TonKhos { get; set; } = new List<TonKho>();
}
