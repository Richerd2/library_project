using System;
using System.Collections.Generic;

namespace library77.Models.Library;

public partial class Book
{
    public long Id { get; set; }

    public int YearOfPublication { get; set; }

    public string Name { get; set; } = null!;
    /// <summary>
    /// поменял с publisherId на publisher
    /// </summary>

    public long PublisherId { get; set; }
    /// <summary>
    /// добавил автора для метода поиска книг/фильтрации
    /// </summary>
    
    ///лист авторов, для того, чтобы выгружать несколько авторов
    public List<AuthorModel> Authors { get; set; }


    
}
