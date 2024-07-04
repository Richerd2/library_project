using System;
using System.Collections.Generic;

namespace library77.Models.Library;

public partial class Reader
{
    public long Id { get; set; }

    public string Lastname { get; set; } = null!;

    public string Middlname { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public string LibraryCard { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public long DateAdd { get; set; }
}
