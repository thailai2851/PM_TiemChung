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

    public virtual DbSet<HtQuyLuatMa> HtQuyLuatMas { get; set; }

    public virtual DbSet<LichTiemBn> LichTiemBns { get; set; }

    public virtual DbSet<SttCapSoTheoNgay> SttCapSoTheoNgays { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:Connection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
                .HasColumnType("date")
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

            entity.HasOne(d => d.IdvaccineNavigation).WithMany(p => p.DmProfileCts)
                .HasForeignKey(d => d.Idvaccine)
                .HasConstraintName("FK_DM_Profile_CT_DM_Vaccine");
        });

        modelBuilder.Entity<DmQuanCuTru>(entity =>
        {
            entity.ToTable("DM_QuanCuTru");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Idtinh).HasColumnName("IDTinh");
            entity.Property(e => e.MaQuan).HasMaxLength(50);
            entity.Property(e => e.TenQuan).HasMaxLength(500);
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
            entity.Property(e => e.TenVaccine).HasMaxLength(500);
        });

        modelBuilder.Entity<DmXaCuTru>(entity =>
        {
            entity.ToTable("DM_XaCuTru");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Idquan).HasColumnName("IDQuan");
            entity.Property(e => e.MaXa).HasMaxLength(50);
            entity.Property(e => e.TenXa).HasMaxLength(500);
        });

        modelBuilder.Entity<HtQuyLuatMa>(entity =>
        {
            entity.ToTable("HT_QuyLuatMa");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.KyTuDau).HasMaxLength(3);
            entity.Property(e => e.TenBang).HasMaxLength(50);
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

            entity.HasOne(d => d.IdvcNavigation).WithMany(p => p.LichTiemBns)
                .HasForeignKey(d => d.Idvc)
                .HasConstraintName("FK_LichTiemBN_DM_Vaccine");
        });

        modelBuilder.Entity<SttCapSoTheoNgay>(entity =>
        {
            entity.ToTable("STT_CapSoTheoNgay");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IdbenhNhan).HasColumnName("IDBenhNhan");
            entity.Property(e => e.Ngay).HasColumnType("date");

            entity.HasOne(d => d.IdbenhNhanNavigation).WithMany(p => p.SttCapSoTheoNgays)
                .HasForeignKey(d => d.IdbenhNhan)
                .HasConstraintName("FK_STT_CapSoTheoNgay_DM_BenhNhan");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
