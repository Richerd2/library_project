using System;
using System.Collections.Generic;

namespace library77.Models.Library;

public partial class AuthorsBook
{
    public long Id { get; set; }

    public long AuthorsId { get; set; }

    public long BookId { get; set; }
}
