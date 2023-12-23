namespace PM_TiemChung.Models.Mapper
{
    public class DmAccountMap
    {
        public long Id { get; set; }

        public string? UserName { get; set; }

        public string? Password { get; set; }

        public long? IdnhanVien { get; set; }

        public bool? QuanLy { get; set; }

        public bool? Bsyte { get; set; }

        public bool? Active { get; set; }

    }
}
