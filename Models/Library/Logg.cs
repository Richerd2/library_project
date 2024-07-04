using System;
using System.Collections.Generic;

namespace library77.Models.Library;

public partial class Logg
{
    public long Id { get; set; }

    public DateTime DateOfIssue { get; set; }

    public DateTime DateOfDelivery { get; set; }

    public long ReaderId { get; set; }

    public long BookId { get; set; }

    public bool Status { get; set; }
}
