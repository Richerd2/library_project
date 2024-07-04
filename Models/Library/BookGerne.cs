using System;
using System.Collections.Generic;

namespace library77.Models.Library;

public partial class BookGerne
{
    public long Id { get; set; }

    public long GernesId { get; set; }

    public long BookId { get; set; }
}
