using System;
using System.Collections.Generic;

namespace library77.Models.Library;

/// <summary>
/// роли пользователей
/// </summary>
public partial class UserRole
{
    /// <summary>
    /// код записи
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// код пользователя
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// код роли
    /// </summary>
    public long RoleId { get; set; }
}
