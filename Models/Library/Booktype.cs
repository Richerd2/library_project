using System;
using System.Collections.Generic;

namespace library77.Models.Library;

public partial class Booktype
{
    public long Id { get; set; }

    public bool PaperVersion { get; set; }

    public bool ElectronicVersion { get; set; }
}
