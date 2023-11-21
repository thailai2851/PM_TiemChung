using System;
using System.Collections.Generic;

namespace PM_TiemChung.Models.Entities;

public partial class DmBenhNhan
{
    public long Id { get; set; }

    public string MaBn { get; set; } = null!;

    public string? TenBn { get; set; }

    public DateTime? NgaySinh { get; set; }

    public int? NamSinh { get; set; }

    public long? Idgt { get; set; }

    public string? DiaChi { get; set; }

    public long? Idtinh { get; set; }

    public long? Idquan { get; set; }

    public long? Idpx { get; set; }

    public string? DienThoai { get; set; }

    public string? Email { get; set; }

    public long? Iddt { get; set; }

    public long? Idnn { get; set; }

    public long? Idqg { get; set; }

    public string? SoCccd { get; set; }

    public DateTime? NgayCap { get; set; }

    public string? NoiCap { get; set; }

    public string? GhiChu { get; set; }

    public DateTime? NgayDen { get; set; }

    public DateTime? NgayKham { get; set; }

    public bool? Active { get; set; }

    public virtual DmDanToc? IddtNavigation { get; set; }

    public virtual DmGioiTinh? IdgtNavigation { get; set; }

    public virtual DmNgheNghiep? IdnnNavigation { get; set; }

    public virtual DmXaCuTru? IdpxNavigation { get; set; }

    public virtual DmQuocGium? IdqgNavigation { get; set; }

    public virtual DmQuanCuTru? IdquanNavigation { get; set; }

    public virtual DmTinhCuTru? IdtinhNavigation { get; set; }

    public virtual ICollection<LichTiemBn> LichTiemBns { get; set; } = new List<LichTiemBn>();

    public virtual ICollection<PhieuXuat> PhieuXuats { get; set; } = new List<PhieuXuat>();
}
