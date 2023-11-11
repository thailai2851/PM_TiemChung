namespace PM_TiemChung.Models.Mapper
{
    public class DmBenhNhanMap
    {
        public long Id { get; set; }

        public string MaBn { get; set; } = null!;

        public string? TenBn { get; set; }

        public string? NgaySinh { get; set; }

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

        public string? NgayCap { get; set; }

        public string? NoiCap { get; set; }

        public string? GhiChu { get; set; }

        public string? NgayDen { get; set; }

        public string? NgayKham { get; set; }

        public bool? Active { get; set; }
    }
}
