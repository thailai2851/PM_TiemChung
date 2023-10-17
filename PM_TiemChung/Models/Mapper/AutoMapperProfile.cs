using AutoMapper;
using PM_TiemChung.Models.Entities;

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
        }
    }
}
