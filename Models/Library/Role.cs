using System;
using System.Collections.Generic;

namespace library77.Models.Library;

/// <summary>
/// Все роли системы
/// </summary>
public partial class Role
{
    /// <summary>
    /// код роли
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// название роли
    /// </summary>
    public string Title { get; set; } = null!;
}
