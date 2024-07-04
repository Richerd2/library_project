using System;
using System.Collections.Generic;

namespace library77.Models.Library;

public partial class ReaderRole
{
    public long Id { get; set; }

    public long ReaderId { get; set; }

    public long RolesId { get; set; }
}
