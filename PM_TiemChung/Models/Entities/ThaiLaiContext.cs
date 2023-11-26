using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PM_TiemChung.Models.Entities;

public partial class ThaiLaiContext : DbContext
{
    public ThaiLaiContext()
    {
    }

    public ThaiLaiContext(DbContextOptions<ThaiLaiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; }

    public virtual DbSet<ChiTietPhieuXuat> ChiTietPhieuXuats { get; set; }

    public virtual DbSet<DmBenhNhan> DmBenhNhans { get; set; }

    public virtual DbSet<DmDanToc> DmDanTocs { get; set; }

    public virtual DbSet<DmGioiTinh> DmGioiTinhs { get; set; }

    public virtual DbSet<DmNgheNghiep> DmNgheNghieps { get; set; }

    public virtual DbSet<DmNhanVien> DmNhanViens { get; set; }

    public virtual DbSet<DmProfile> DmProfiles { get; set; }

    public virtual DbSet<DmProfileCt> DmProfileCts { get; set; }

    public virtual DbSet<DmQuanCuTru> DmQuanCuTrus { get; set; }

    public virtual DbSet<DmQuocGium> DmQuocGia { get; set; }

    public virtual DbSet<DmThoiGian> DmThoiGians { get; set; }

    public virtual DbSet<DmTinhCuTru> DmTinhCuTrus { get; set; }

    public virtual DbSet<DmVaccine> DmVaccines { get; set; }

    public virtual DbSet<DmXaCuTru> DmXaCuTrus { get; set; }

    public virtual DbSet<LichTiemBn> LichTiemBns { get; set; }

    public virtual DbSet<NhaCungCap> NhaCungCaps { get; set; }

    public virtual DbSet<PhieuNhap> PhieuNhaps { get; set; }

    public virtual DbSet<PhieuXuat> PhieuXuats { get; set; }

    public virtual DbSet<QlMa> QlMas { get; set; }

    public virtual DbSet<ThongTinDoanhNghiep> ThongTinDoanhNghieps { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:Connection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.ToTable("Account");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IdnhanVien).HasColumnName("IDNhanVien");
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.UserName).HasMaxLength(50);
            entity.HasOne(d => d.IdnhanVienNavigation).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.IdnhanVien)
                .HasConstraintName("FK__Account__IDNhanV__0C85DE4D");
        });

        modelBuilder.Entity<ChiTietPhieuNhap>(entity =>
        {
            entity.HasKey(e => e.Idctpn);

            entity.ToTable("ChiTietPhieuNhap");

            entity.Property(e => e.Idctpn).HasColumnName("IDCTPN");
            entity.Property(e => e.Cktm).HasColumnName("CKTM");
            entity.Property(e => e.GhiChu).HasMaxLength(500);
            entity.Property(e => e.Hsd)
                .HasColumnType("datetime")
                .HasColumnName("HSD");
            entity.Property(e => e.Idpn).HasColumnName("IDPN");
            entity.Property(e => e.Idvaccine).HasColumnName("IDVaccine");
            entity.Property(e => e.Nsx)
                .HasColumnType("datetime")
                .HasColumnName("NSX");

            entity.HasOne(d => d.IdpnNavigation).WithMany(p => p.ChiTietPhieuNhaps)
                .HasForeignKey(d => d.Idpn)
                .HasConstraintName("FK_ChiTietPhieuNhap_PhieuNhap");

            entity.HasOne(d => d.IdvaccineNavigation).WithMany(p => p.ChiTietPhieuNhaps)
                .HasForeignKey(d => d.Idvaccine)
                .HasConstraintName("FK_ChiTietPhieuNhap_DM_Vaccine");
        });

        modelBuilder.Entity<ChiTietPhieuXuat>(entity =>
        {
            entity.HasKey(e => e.Idctpx);

            entity.ToTable("ChiTietPhieuXuat");

            entity.Property(e => e.Idctpx).HasColumnName("IDCTPX");
            entity.Property(e => e.Cktm).HasColumnName("CKTM");
            entity.Property(e => e.Idctpn).HasColumnName("IDCTPN");
            entity.Property(e => e.Idpx).HasColumnName("IDPX");
            entity.Property(e => e.Idvaccine).HasColumnName("IDVaccine");

            entity.HasOne(d => d.IdctpnNavigation).WithMany(p => p.ChiTietPhieuXuats)
                .HasForeignKey(d => d.Idctpn)
                .HasConstraintName("FK_ChiTietPhieuXuat_ChiTietPhieuNhap");

            entity.HasOne(d => d.IdpxNavigation).WithMany(p => p.ChiTietPhieuXuats)
                .HasForeignKey(d => d.Idpx)
                .HasConstraintName("FK_ChiTietPhieuXuat_PhieuXuat");

            entity.HasOne(d => d.IdvaccineNavigation).WithMany(p => p.ChiTietPhieuXuats)
                .HasForeignKey(d => d.Idvaccine)
                .HasConstraintName("FK_ChiTietPhieuXuat_DM_Vaccine");
        });

        modelBuilder.Entity<DmBenhNhan>(entity =>
        {
            entity.ToTable("DM_BenhNhan");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DiaChi).HasMaxLength(500);
            entity.Property(e => e.DienThoai).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.GhiChu).HasMaxLength(500);
            entity.Property(e => e.Iddt).HasColumnName("IDDT");
            entity.Property(e => e.Idgt).HasColumnName("IDGT");
            entity.Property(e => e.Idnn).HasColumnName("IDNN");
            entity.Property(e => e.Idpx).HasColumnName("IDPX");
            entity.Property(e => e.Idqg).HasColumnName("IDQG");
            entity.Property(e => e.Idquan).HasColumnName("IDQuan");
            entity.Property(e => e.Idtinh).HasColumnName("IDTinh");
            entity.Property(e => e.MaBn)
                .HasMaxLength(50)
                .HasColumnName("MaBN");
            entity.Property(e => e.NgayCap).HasColumnType("date");
            entity.Property(e => e.NgayDen).HasColumnType("date");
            entity.Property(e => e.NgayKham).HasColumnType("datetime");
            entity.Property(e => e.NgaySinh).HasColumnType("datetime");
            entity.Property(e => e.NoiCap).HasMaxLength(500);
            entity.Property(e => e.SoCccd)
                .HasMaxLength(50)
                .HasColumnName("SoCCCD");
            entity.Property(e => e.TenBn)
                .HasMaxLength(500)
                .HasColumnName("TenBN");

            entity.HasOne(d => d.IddtNavigation).WithMany(p => p.DmBenhNhans)
                .HasForeignKey(d => d.Iddt)
                .HasConstraintName("FK_DM_BenhNhan_DM_DanToc");

            entity.HasOne(d => d.IdgtNavigation).WithMany(p => p.DmBenhNhans)
                .HasForeignKey(d => d.Idgt)
                .HasConstraintName("FK_DM_BenhNhan_DM_GioiTinh");

            entity.HasOne(d => d.IdnnNavigation).WithMany(p => p.DmBenhNhans)
                .HasForeignKey(d => d.Idnn)
                .HasConstraintName("FK_DM_BenhNhan_DM_NgheNghiep");

            entity.HasOne(d => d.IdpxNavigation).WithMany(p => p.DmBenhNhans)
                .HasForeignKey(d => d.Idpx)
                .HasConstraintName("FK_DM_BenhNhan_DM_XaCuTru");

            entity.HasOne(d => d.IdqgNavigation).WithMany(p => p.DmBenhNhans)
                .HasForeignKey(d => d.Idqg)
                .HasConstraintName("FK_DM_BenhNhan_DM_QuocGia");

            entity.HasOne(d => d.IdquanNavigation).WithMany(p => p.DmBenhNhans)
                .HasForeignKey(d => d.Idquan)
                .HasConstraintName("FK_DM_BenhNhan_DM_QuanCuTru");

            entity.HasOne(d => d.IdtinhNavigation).WithMany(p => p.DmBenhNhans)
                .HasForeignKey(d => d.Idtinh)
                .HasConstraintName("FK_DM_BenhNhan_DM_TinhCuTru");
        });

        modelBuilder.Entity<DmDanToc>(entity =>
        {
            entity.ToTable("DM_DanToc");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MaDanToc).HasMaxLength(50);
            entity.Property(e => e.TenDanToc).HasMaxLength(500);
            entity.Property(e => e.TenGoiKhac).HasMaxLength(500);
        });

        modelBuilder.Entity<DmGioiTinh>(entity =>
        {
            entity.ToTable("DM_GioiTinh");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MaGioiTinh).HasMaxLength(50);
            entity.Property(e => e.TenGioiTinh).HasMaxLength(100);
        });

        modelBuilder.Entity<DmNgheNghiep>(entity =>
        {
            entity.ToTable("DM_NgheNghiep");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MaNgheNghiep).HasMaxLength(50);
        });

        modelBuilder.Entity<DmNhanVien>(entity =>
        {
            entity.ToTable("DM_NhanVien");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DiaChi).HasMaxLength(500);
            entity.Property(e => e.DienThoai).HasMaxLength(50);
            entity.Property(e => e.Idgt).HasColumnName("IDGT");
            entity.Property(e => e.MaNhanVien).HasMaxLength(50);
            entity.Property(e => e.Mabhxh)
                .HasMaxLength(50)
                .HasColumnName("MABHXH");
            entity.Property(e => e.Macchn)
                .HasMaxLength(50)
                .HasColumnName("MACCHN");
            entity.Property(e => e.NgaySinh).HasColumnType("date");
            entity.Property(e => e.Ngaycapcchn)
                .HasColumnType("date")
                .HasColumnName("NGAYCAPCCHN");
            entity.Property(e => e.Noicapcchn)
                .HasMaxLength(500)
                .HasColumnName("NOICAPCCHN");
            entity.Property(e => e.QueQuan).HasMaxLength(500);
            entity.Property(e => e.TenNhanVien).HasMaxLength(200);

            entity.HasOne(d => d.IdgtNavigation).WithMany(p => p.DmNhanViens)
                .HasForeignKey(d => d.Idgt)
                .HasConstraintName("FK_DM_NhanVien_DM_GioiTinh");
        });

        modelBuilder.Entity<DmProfile>(entity =>
        {
            entity.ToTable("DM_Profile");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Idgt).HasColumnName("IDGT");
            entity.Property(e => e.TenProfile).HasMaxLength(500);

            entity.HasOne(d => d.IdgtNavigation).WithMany(p => p.DmProfiles)
                .HasForeignKey(d => d.Idgt)
                .HasConstraintName("FK_DM_Profile_DM_GioiTinh");
        });

        modelBuilder.Entity<DmProfileCt>(entity =>
        {
            entity.ToTable("DM_Profile_CT");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Idprofile).HasColumnName("IDProfile");
            entity.Property(e => e.IdthoiGian).HasColumnName("IDThoiGian");
            entity.Property(e => e.Idvaccine).HasColumnName("IDVaccine");
            entity.Property(e => e.TgsomNhat).HasColumnName("TGSomNhat");
            entity.Property(e => e.TgtreNhat).HasColumnName("TGTreNhat");

            entity.HasOne(d => d.IdprofileNavigation).WithMany(p => p.DmProfileCts)
                .HasForeignKey(d => d.Idprofile)
                .HasConstraintName("FK_DM_Profile_CT_DM_Profile");

            entity.HasOne(d => d.IdthoiGianNavigation).WithMany(p => p.DmProfileCts)
                .HasForeignKey(d => d.IdthoiGian)
                .HasConstraintName("FK_DM_Profile_CT_DM_ThoiGian");

            entity.HasOne(d => d.IdvaccineNavigation).WithMany(p => p.DmProfileCtIdvaccineNavigations)
                .HasForeignKey(d => d.Idvaccine)
                .HasConstraintName("FK_DM_Profile_CT_DM_Vaccine");

            entity.HasOne(d => d.MuiTienQuyetNavigation).WithMany(p => p.DmProfileCtMuiTienQuyetNavigations)
                .HasForeignKey(d => d.MuiTienQuyet)
                .HasConstraintName("FK__DM_Profil__MuiTi__6383C8BA");
        });

        modelBuilder.Entity<DmQuanCuTru>(entity =>
        {
            entity.ToTable("DM_QuanCuTru");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Idtinh).HasColumnName("IDTinh");
            entity.Property(e => e.MaQuan).HasMaxLength(50);
            entity.Property(e => e.TenQuan).HasMaxLength(500);

            entity.HasOne(d => d.IdtinhNavigation).WithMany(p => p.DmQuanCuTrus)
                .HasForeignKey(d => d.Idtinh)
                .HasConstraintName("FK_DM_QuanCuTru_DM_TinhCuTru");
        });

        modelBuilder.Entity<DmQuocGium>(entity =>
        {
            entity.ToTable("DM_QuocGia");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MaQuocGia).HasMaxLength(50);
            entity.Property(e => e.TenQuocGia).HasMaxLength(500);
        });

        modelBuilder.Entity<DmThoiGian>(entity =>
        {
            entity.ToTable("DM_ThoiGian");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.TenTg)
                .HasMaxLength(50)
                .HasColumnName("TenTG");
        });

        modelBuilder.Entity<DmTinhCuTru>(entity =>
        {
            entity.ToTable("DM_TinhCuTru");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MaTinh).HasMaxLength(50);
            entity.Property(e => e.TenTinh).HasMaxLength(500);
        });

        modelBuilder.Entity<DmVaccine>(entity =>
        {
            entity.ToTable("DM_Vaccine");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DonViTinh).HasMaxLength(50);
            entity.Property(e => e.MaVaccine).HasMaxLength(50);
            entity.Property(e => e.SoCode).HasMaxLength(50);
            entity.Property(e => e.TenVaccine).HasMaxLength(500);
        });

        modelBuilder.Entity<DmXaCuTru>(entity =>
        {
            entity.ToTable("DM_XaCuTru");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Idquan).HasColumnName("IDQuan");
            entity.Property(e => e.MaXa).HasMaxLength(50);
            entity.Property(e => e.TenXa).HasMaxLength(500);

            entity.HasOne(d => d.IdquanNavigation).WithMany(p => p.DmXaCuTrus)
                .HasForeignKey(d => d.Idquan)
                .HasConstraintName("FK_DM_XaCuTru_DM_QuanCuTru");
        });

        modelBuilder.Entity<LichTiemBn>(entity =>
        {
            entity.ToTable("LichTiemBN");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Idbn).HasColumnName("IDBN");
            entity.Property(e => e.Idbsk).HasColumnName("IDBSK");
            entity.Property(e => e.IdnhanVienThu).HasColumnName("IDNhanVienThu");
            entity.Property(e => e.IdnhanVienTiem).HasColumnName("IDNhanVienTiem");
            entity.Property(e => e.IdthoiGian).HasColumnName("IDThoiGian");
            entity.Property(e => e.Idvc).HasColumnName("IDVC");
            entity.Property(e => e.NgayDeNghiTiem).HasColumnType("datetime");
            entity.Property(e => e.NgayHen).HasColumnType("date");
            entity.Property(e => e.NgayKham).HasColumnType("datetime");
            entity.Property(e => e.NgayThu).HasColumnType("datetime");
            entity.Property(e => e.NgayTiem).HasColumnType("datetime");
            entity.Property(e => e.TgsomNhat).HasColumnName("TGSomNhat");
            entity.Property(e => e.TgtreNhat).HasColumnName("TGTreNhat");

            entity.HasOne(d => d.IdbnNavigation).WithMany(p => p.LichTiemBns)
                .HasForeignKey(d => d.Idbn)
                .HasConstraintName("FK_LichTiemBN_DM_BenhNhan");

            entity.HasOne(d => d.IdbskNavigation).WithMany(p => p.LichTiemBnIdbskNavigations)
                .HasForeignKey(d => d.Idbsk)
                .HasConstraintName("FK_LichTiemBN_DM_NhanVien");

            entity.HasOne(d => d.IdnhanVienThuNavigation).WithMany(p => p.LichTiemBnIdnhanVienThuNavigations)
                .HasForeignKey(d => d.IdnhanVienThu)
                .HasConstraintName("FK_LichTiemBN_DM_NhanVien1");

            entity.HasOne(d => d.IdnhanVienTiemNavigation).WithMany(p => p.LichTiemBnIdnhanVienTiemNavigations)
                .HasForeignKey(d => d.IdnhanVienTiem)
                .HasConstraintName("FK_LichTiemBN_DM_NhanVien2");

            entity.HasOne(d => d.IdthoiGianNavigation).WithMany(p => p.LichTiemBns)
                .HasForeignKey(d => d.IdthoiGian)
                .HasConstraintName("FK_LichTiemBN_DM_ThoiGian");

            entity.HasOne(d => d.IdvcNavigation).WithMany(p => p.LichTiemBnIdvcNavigations)
                .HasForeignKey(d => d.Idvc)
                .HasConstraintName("FK_LichTiemBN_DM_Vaccine");

            entity.HasOne(d => d.MuiTienQuyetNavigation).WithMany(p => p.LichTiemBnMuiTienQuyetNavigations)
                .HasForeignKey(d => d.MuiTienQuyet)
                .HasConstraintName("FK__LichTiemB__MuiTi__66603565");
        });

        modelBuilder.Entity<NhaCungCap>(entity =>
        {
            entity.HasKey(e => e.Idncc);

            entity.ToTable("NhaCungCap");

            entity.Property(e => e.Idncc).HasColumnName("IDNCC");
            entity.Property(e => e.DiaChi).HasMaxLength(500);
            entity.Property(e => e.DienThoai).HasMaxLength(50);
            entity.Property(e => e.GhiChu).HasMaxLength(500);
            entity.Property(e => e.MaNcc)
                .HasMaxLength(100)
                .HasColumnName("MaNCC");
            entity.Property(e => e.Mail).HasMaxLength(100);
            entity.Property(e => e.TenNcc)
                .HasMaxLength(100)
                .HasColumnName("TenNCC");
        });

        modelBuilder.Entity<PhieuNhap>(entity =>
        {
            entity.HasKey(e => e.Idpn);

            entity.ToTable("PhieuNhap");

            entity.Property(e => e.Idpn).HasColumnName("IDPN");
            entity.Property(e => e.GhiChu).HasMaxLength(500);
            entity.Property(e => e.Idncc).HasColumnName("IDNCC");
            entity.Property(e => e.Idnv).HasColumnName("IDNV");
            entity.Property(e => e.NgayHd)
                .HasColumnType("datetime")
                .HasColumnName("NgayHD");
            entity.Property(e => e.NgayNhap).HasColumnType("datetime");
            entity.Property(e => e.SoHd)
                .HasMaxLength(100)
                .HasColumnName("SoHD");
            entity.Property(e => e.SoPn)
                .HasMaxLength(100)
                .HasColumnName("SoPN");

            entity.HasOne(d => d.IdnccNavigation).WithMany(p => p.PhieuNhaps)
                .HasForeignKey(d => d.Idncc)
                .HasConstraintName("FK_PhieuNhap_NhaCungCap");

            entity.HasOne(d => d.IdnvNavigation).WithMany(p => p.PhieuNhaps)
                .HasForeignKey(d => d.Idnv)
                .HasConstraintName("FK_PhieuNhap_DM_NhanVien");
        });

        modelBuilder.Entity<PhieuXuat>(entity =>
        {
            entity.HasKey(e => e.Idpx);

            entity.ToTable("PhieuXuat");

            entity.Property(e => e.Idpx).HasColumnName("IDPX");
            entity.Property(e => e.GhiChu).HasMaxLength(500);
            entity.Property(e => e.Idbn).HasColumnName("IDBN");
            entity.Property(e => e.Idnv).HasColumnName("IDNV");
            entity.Property(e => e.NgayHd)
                .HasColumnType("datetime")
                .HasColumnName("NgayHD");
            entity.Property(e => e.NgayTao).HasColumnType("datetime");
            entity.Property(e => e.SoHd)
                .HasMaxLength(100)
                .HasColumnName("SoHD");
            entity.Property(e => e.SoPx)
                .HasMaxLength(100)
                .HasColumnName("SoPX");

            entity.HasOne(d => d.IdbnNavigation).WithMany(p => p.PhieuXuats)
                .HasForeignKey(d => d.Idbn)
                .HasConstraintName("FK_PhieuXuat_DM_BenhNhan");

            entity.HasOne(d => d.IdnvNavigation).WithMany(p => p.PhieuXuats)
                .HasForeignKey(d => d.Idnv)
                .HasConstraintName("FK_PhieuXuat_DM_NhanVien");
        });

        modelBuilder.Entity<QlMa>(entity =>
        {
            entity.ToTable("QL_Ma");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.KyHieu).HasMaxLength(50);
            entity.Property(e => e.Ngay).HasColumnType("date");
            entity.Property(e => e.Stt).HasColumnName("STT");
        });

        modelBuilder.Entity<ThongTinDoanhNghiep>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("ThongTinDoanhNghiep");
            entity.Property(e => e.ChuTk).HasMaxLength(200);
            entity.Property(e => e.DiaChi).HasMaxLength(500);
            entity.Property(e => e.DienThoai).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Mst)
                .HasMaxLength(50)
                .HasColumnName("MST");
            entity.Property(e => e.NganHang).HasMaxLength(50);
            entity.Property(e => e.SoTk).HasMaxLength(50);
            entity.Property(e => e.TenDoanhNghiep).HasMaxLength(500);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
