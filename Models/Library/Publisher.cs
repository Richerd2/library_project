using System;
using System.Collections.Generic;

namespace library77.Models.Library;

public partial class Publisher
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public long CityId { get; set; }
}
