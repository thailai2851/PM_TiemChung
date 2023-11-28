using AutoMapper;
using PM_TiemChung.Models.Entities;
using System.Globalization;

namespace PM_TiemChung.Models.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<DmTinhCuTru, DmTinhCuTruMap>()
           .ForMember(dest => dest.ThongTu, opt => opt.MapFrom(src => src.ThongTu.ToString()));
            CreateMap<DmTinhCuTruMap, DmTinhCuTru>()
            .ForMember(dest => dest.ThongTu, opt => opt.MapFrom(src => int.Parse(src.ThongTu ?? "0")));

            CreateMap<DmGioiTinh, DmGioiTinhMap>()
            .ForMember(dest => dest.ThongTu, opt => opt.MapFrom(src => src.ThongTu.ToString()));
            CreateMap<DmGioiTinhMap, DmGioiTinh>()
            .ForMember(dest => dest.ThongTu, opt => opt.MapFrom(src => int.Parse(src.ThongTu ?? "0")));

            CreateMap<DmThoiGian, DmThoiGianMap>();
            CreateMap<DmThoiGianMap, DmThoiGian>();

            CreateMap<DmDanToc, DmDanTocMap>()
            .ForMember(dest => dest.ThongTu, opt => opt.MapFrom(src => src.ThongTu.ToString()));
            CreateMap<DmDanTocMap, DmDanToc>()
            .ForMember(dest => dest.ThongTu, opt => opt.MapFrom(src => int.Parse(src.ThongTu ?? "0")));

            CreateMap<DmNgheNghiep, DmNgheNghiepMap>()
            .ForMember(dest => dest.ThongTu, opt => opt.MapFrom(src => src.ThongTu.ToString()));
            CreateMap<DmNgheNghiepMap, DmNgheNghiep>()
            .ForMember(dest => dest.ThongTu, opt => opt.MapFrom(src => int.Parse(src.ThongTu ?? "0")));

            CreateMap<DmQuocGium, DmQuocGiumMap>()
            .ForMember(dest => dest.ThongTu, opt => opt.MapFrom(src => src.ThongTu.ToString()));
            CreateMap<DmQuocGiumMap, DmQuocGium>()
            .ForMember(dest => dest.ThongTu, opt => opt.MapFrom(src => int.Parse(src.ThongTu ?? "0")));

            CreateMap<DmVaccine, DmVaccineMap>();
            CreateMap<DmVaccineMap, DmVaccine>();

            CreateMap<DmQuanCuTru, DmQuanCuTruMap>()
            .ForMember(dest => dest.ThongTu, opt => opt.MapFrom(src => src.ThongTu.ToString()));
            CreateMap<DmQuanCuTruMap, DmQuanCuTru>()
            .ForMember(dest => dest.ThongTu, opt => opt.MapFrom(src => int.Parse(src.ThongTu ?? "0")));

            CreateMap<DmXaCuTru, DmXaCuTruMap>()
            .ForMember(dest => dest.ThongTu, opt => opt.MapFrom(src => src.ThongTu.ToString()));
            CreateMap<DmXaCuTruMap, DmXaCuTru>()
            .ForMember(dest => dest.ThongTu, opt => opt.MapFrom(src => int.Parse(src.ThongTu ?? "0")));

            CreateMap<DmNhanVien, DmNhanVienMap>();
            CreateMap<DmNhanVienMap, DmNhanVien>();

            CreateMap<DmBenhNhanMap, DmBenhNhan>()
                .ForMember(dest => dest.NgayCap, opt => opt.MapFrom(src => src.NgayCap != "" ? DateTime.ParseExact(src.NgayCap, "dd-MM-yyyy", CultureInfo.InvariantCulture) : (DateTime?)null))
                .ForMember(dest => dest.NgayKham, opt => opt.MapFrom(src => src.NgayKham != "" ? DateTime.ParseExact(src.NgayKham, "dd-MM-yyyy", CultureInfo.InvariantCulture) : (DateTime?)null))
                .ForMember(dest => dest.NgaySinh, opt => opt.MapFrom(src => src.NgaySinh != "" ? DateTime.ParseExact(src.NgaySinh, "dd-MM-yyyy", CultureInfo.InvariantCulture) : (DateTime?)null))
                .ForMember(dest => dest.NgayDen, opt => opt.MapFrom(src => src.NgayDen != "" ? DateTime.ParseExact(src.NgayDen, "dd-MM-yyyy", CultureInfo.InvariantCulture) : (DateTime?)null));
            CreateMap<LichTiemBnMap, LichTiemBn>()
                .ForMember(dest => dest.NgayDeNghiTiem, opt => opt.MapFrom(src => src.NgayDeNghiTiem != "" ? DateTime.ParseExact(src.NgayDeNghiTiem, "dd-MM-yyyy", CultureInfo.InvariantCulture) : (DateTime?)null))
                .ForMember(dest => dest.NgayKham, opt => opt.MapFrom(src => src.NgayKham != "" ? DateTime.ParseExact(src.NgayKham, "dd-MM-yyyy", CultureInfo.InvariantCulture) : (DateTime?)null))
                .ForMember(dest => dest.NgayThu, opt => opt.MapFrom(src => src.NgayThu != "" ? DateTime.ParseExact(src.NgayThu, "dd-MM-yyyy", CultureInfo.InvariantCulture) : (DateTime?)null))
                .ForMember(dest => dest.NgayHen, opt => opt.MapFrom(src => src.NgayHen != "" ? DateTime.ParseExact(src.NgayHen, "dd-MM-yyyy", CultureInfo.InvariantCulture) : (DateTime?)null))
                .ForMember(dest => dest.NgayTiem, opt => opt.MapFrom(src => src.NgayTiem != "" ? DateTime.ParseExact(src.NgayTiem, "dd-MM-yyyy", CultureInfo.InvariantCulture) : (DateTime?)null));

            CreateMap<NhaCungCap, DmNhaCungCapMap>();
            CreateMap<DmNhaCungCapMap, NhaCungCap>();

            CreateMap<Account, DmAccountMap>();
            CreateMap<DmAccountMap, Account>();

            CreateMap<PhieuNhap, PhieuNhapMap>()
            .ForMember(dest => dest.Idncc, opt => opt.MapFrom(src => src.Idncc.ToString()))
            .ForMember(dest => dest.Idnv, opt => opt.MapFrom(src => src.Idnv.ToString()))
            .ForMember(dest => dest.NgayNhap, opt => opt.MapFrom(src => src.NgayNhap.ToString()))
            .ForMember(dest => dest.NgayHd, opt => opt.MapFrom(src => src.NgayHd.ToString()));
            CreateMap<PhieuNhapMap, PhieuNhap>()
            .ForMember(dest => dest.NgayNhap, opt => opt.MapFrom(src => src.NgayNhap != "" ? DateTime.ParseExact(src.NgayNhap, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture) : (DateTime?)null))
            .ForMember(dest => dest.NgayHd, opt => opt.MapFrom(src => src.NgayHd != "" ? DateTime.ParseExact(src.NgayHd, "dd-MM-yyyy", CultureInfo.InvariantCulture) : (DateTime?)null))
            .ForMember(dest => dest.Idncc, opt => opt.MapFrom(src => src.Idncc != null ? long.Parse(src.Idncc) : (long?)null))
            .ForMember(dest => dest.Idnv, opt => opt.MapFrom(src => src.Idnv != null ? long.Parse(src.Idnv) : (long?)null));
            CreateMap<ChiTietPhieuNhap, ChiTietPhieuNhapMap>()
            .ForMember(dest => dest.Idpn, opt => opt.MapFrom(src => src.Idpn.ToString()))
            .ForMember(dest => dest.Idvaccine, opt => opt.MapFrom(src => src.Idvaccine.ToString()))
            .ForMember(dest => dest.SoLuong, opt => opt.MapFrom(src => src.SoLuong.ToString()))
            .ForMember(dest => dest.DonGia, opt => opt.MapFrom(src => src.DonGia.ToString()))
            .ForMember(dest => dest.Nsx, opt => opt.MapFrom(src => src.Nsx.ToString()))
            .ForMember(dest => dest.Hsd, opt => opt.MapFrom(src => src.Hsd.ToString()));
            CreateMap<ChiTietPhieuNhapMap, ChiTietPhieuNhap>()
            .ForMember(dest => dest.Idpn, opt => opt.MapFrom(src => src.Idpn != null ? long.Parse(src.Idpn) : (long?)null))
            .ForMember(dest => dest.Idvaccine, opt => opt.MapFrom(src => src.Idvaccine != null ? long.Parse(src.Idvaccine) : (long?)null))
             .ForMember(dest => dest.SoLuong, opt => opt.MapFrom(src => src.SoLuong != "" ? double.Parse(src.SoLuong.Replace(",", "")) : (double?)null))
             .ForMember(dest => dest.DonGia, opt => opt.MapFrom(src => src.DonGia != "" ? double.Parse(src.DonGia.Replace(",", "")) : (double?)null))
             .ForMember(dest => dest.Nsx, opt => opt.MapFrom(src => src.Nsx != "" ? DateTime.ParseExact(src.Nsx, "dd-MM-yyyy", CultureInfo.InvariantCulture) : (DateTime?)null))
            .ForMember(dest => dest.Hsd, opt => opt.MapFrom(src => src.Hsd != "" ? DateTime.ParseExact(src.Hsd, "dd-MM-yyyy", CultureInfo.InvariantCulture) : (DateTime?)null));

        }
    }
}
