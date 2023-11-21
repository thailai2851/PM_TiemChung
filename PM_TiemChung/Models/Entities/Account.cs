using System;
using System.Collections.Generic;

namespace PM_TiemChung.Models.Entities;

public partial class Account
{
    public long Id { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public long? IdnhanVien { get; set; }
}
