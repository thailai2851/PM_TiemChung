﻿using System;
using System.Collections.Generic;

namespace PM_TiemChung.Models.Entities;

public partial class ChiTietPhieuNhap
{
    public int Idctpn { get; set; }

    public int? Idpn { get; set; }

    public long? Idvaccine { get; set; }

    public double? SoLuong { get; set; }

    public double? DonGia { get; set; }

    public DateTime? Nsx { get; set; }

    public DateTime? Hsd { get; set; }

    public string? GhiChu { get; set; }

    public int? SoLuongXuat { get; set; }

    public bool? Active { get; set; }

    public virtual PhieuNhap? IdpnNavigation { get; set; }

    public virtual DmVaccine? IdvaccineNavigation { get; set; }

    public virtual ICollection<LichTiemBn> LichTiemBns { get; set; } = new List<LichTiemBn>();
}
