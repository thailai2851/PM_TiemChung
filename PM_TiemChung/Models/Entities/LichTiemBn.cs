using System;
using System.Collections.Generic;

namespace PM_TiemChung.Models.Entities;

public partial class LichTiemBn
{
    public long Id { get; set; }

    public long? Idbn { get; set; }

    public long? Idvc { get; set; }

    public int? SoLanTiem { get; set; }

    public int? TgsomNhat { get; set; }

    public int? TgtreNhat { get; set; }

    public long? IdthoiGian { get; set; }

    public long? MuiTienQuyet { get; set; }

    public long? Idbsk { get; set; }

    public DateTime? NgayKham { get; set; }

    public bool? DeNghiTiem { get; set; }

    public DateTime? NgayDeNghiTiem { get; set; }

    public int? SoLuong { get; set; }

    public double? DonGia { get; set; }

    public bool? DaThu { get; set; }

    public DateTime? NgayThu { get; set; }

    public long? IdnhanVienThu { get; set; }

    public bool? DaTiem { get; set; }

    public DateTime? NgayTiem { get; set; }

    public long? IdnhanVienTiem { get; set; }

    public DateTime? NgayHen { get; set; }

    public virtual DmBenhNhan? IdbnNavigation { get; set; }

    public virtual DmNhanVien? IdbskNavigation { get; set; }

    public virtual DmNhanVien? IdnhanVienThuNavigation { get; set; }

    public virtual DmNhanVien? IdnhanVienTiemNavigation { get; set; }

    public virtual DmThoiGian? IdthoiGianNavigation { get; set; }

    public virtual DmVaccine? IdvcNavigation { get; set; }

    public virtual DmVaccine? MuiTienQuyetNavigation { get; set; }
}
