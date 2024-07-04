using System;
using System.Collections.Generic;

namespace library77.Models.Library;

/// <summary>
/// Таблица авторов
/// </summary>
public partial class Author
{
    /// <summary>
    /// код записи
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// фамилия
    /// </summary>
    public string Lastname { get; set; } = null!;

    /// <summary>
    /// фамилия
    /// </summary>
    public string Middlname { get; set; } = null!;

    /// <summary>
    /// отчество
    /// </summary>
    public string Fullname { get; set; } = null!;
}
