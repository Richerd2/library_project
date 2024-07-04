using System;
using System.Collections.Generic;

namespace library77.Models.Library;

public partial class BookBooktype
{
    public long Id { get; set; }

    public long BookId { get; set; }

    public long BookTypeId { get; set; }
}
