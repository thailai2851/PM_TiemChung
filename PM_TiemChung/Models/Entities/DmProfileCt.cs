using System;
using System.Collections.Generic;

namespace PM_TiemChung.Models.Entities;

public partial class DmProfileCt
{
    public long Id { get; set; }

    public long? Idprofile { get; set; }

    public long? Idvaccine { get; set; }

    public int? SoLanTiem { get; set; }

    public int? TgsomNhat { get; set; }

    public int? TgtreNhat { get; set; }

    public long? IdthoiGian { get; set; }

    public long? MuiTienQuyet { get; set; }

    public virtual DmProfile? IdprofileNavigation { get; set; }

    public virtual DmThoiGian? IdthoiGianNavigation { get; set; }

    public virtual DmVaccine? IdvaccineNavigation { get; set; }
}
