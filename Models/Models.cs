namespace library77.Models
{
    public class RegisterUserModel
    {
        public long Id { get; set; } = 1;
        /// <summary>
        /// Id активности
        /// </summary>
        public string Username { get; set; } = "";
        /// <summary>
        /// Тип цикличности
        /// </summary>
        public string Password { get; set; } = "";
        /// <summary>
        /// Имя
        /// </summary>
        public string Firstname { get; set; } = "";
        /// <summary>
        /// Фамилия
        /// </summary>
        public string Lastname { get; set; } = "";
        /// <summary>
        /// Отчество
        /// </summary>
        public string Middlename { get; set; } = "";
        /// <summary>
        /// Номер читательского билета
        /// </summary>
        public string ReaderCard { get; set; } = "";
    }
    public class GetTokenModel
    {
        /// <summary>
        /// логин пользователя
        /// </summary>
        public string Username { get; set; } = "";
        /// <summary>
        /// пароль
        /// </summary>
        public string Password { get; set; } = "";

       

        
    }
    //описываю свои модели
    public class UserModels
    {
        /// <summary>
        /// ADMIN это роль админа
        /// </summary>
        public string ADMIN { get; set; } = "";
        /// <summary>
        /// это роль читателя
        /// </summary>
        public string READER { get; set; } = "";
        /// <summary>
        /// это роль технического специалиста
        /// </summary>
        public string TechSpec { get; set; } = "";
    }


    /// <summary>
    /// Модель ролей пользователя
    /// </summary>
    public class UserRolesModel
    { 
        /// <summary>
        /// Админимтратор
        /// </summary>
        public bool Admin { get; set; } = false;
        /// <summary>
        /// Технический специалист
        /// </summary>
        public bool TechSpec { get; set; } = false;
        /// <summary>
        /// Читатель
        /// </summary>
        public bool Reader { get; set; } = false;
    }


    public class UserData
    {
        /// <summary>
        /// айдишник читателя
        /// </summary>
        public long id { get; set; } = 1;
        /// <summary>
        /// карта читателя
        /// </summary>
        public string LibraryCard { get; set; } = "";

        /// <summary>
        /// дата создания
        /// </summary>

        public long DateAdd { get; set; } = -1;

        /// <summary>
        /// имя
        /// </summary>

        public string Lastname { get; set; } = "";
        /// <summary>
        /// фамилия
        /// </summary>
        public string Middlname { get; set; } = "";

        /// <summary>
        /// отчество
        /// </summary>
        public string Fullname { get; set; } = "";
    }

    public class Logg
    {
        /// <summary>
        /// айдишник logg 
        /// </summary>
        public long id { get; set; } = 0;
        /// <summary>
        /// айдишник читателя
        /// </summary>
        public int ReaderId { get; set; } = 0;

        /// <summary>
        /// айдшиник книги добавил 01.01.2024 для метода, где нужно вытащить айди читателя и айди книги
        /// </summary>
        public int BookId { get; set; } = 0;

        /// <summary>
        /// статус есть или нет книга у чела
        /// </summary>
        public bool Status { get; set; } = false;


    }

    /// <summary>
    /// Модель журнала взятия/возврата книг
    /// </summary>
    public class BookHistoryModel
    {
        /// <summary>
        /// айдишник записи в журнале
        /// </summary>
        public long Id { get; set; } = 0;
        /// <summary>
        /// айдишник книги
        /// </summary>
        public long BookId { get; set; } = 0;
        /// <summary>
        /// дата публикации
        /// </summary>
        public int YearOfPublication { get; set; } = 0;
        /// <summary>
        /// Название книги
        /// </summary>
        public string Name { get; set; } = "";
        /// <summary>
        /// айдишник публикации 
        /// </summary>
        public PublisherModel Publisher { get; set; } = new PublisherModel();
        /// <summary>
        /// авторы книг
        /// </summary>
        public List<AuthorModel> Authors { get; set; } = new List<AuthorModel>();        
        /// <summary>
        /// Дата взятия книги
        /// </summary>
        public long DateOfDelivery {  get; set; } = 0; 
        /// <summary>
        /// Дата возврата
        /// </summary>
        public long DateOfIssue {  get; set; } = 0;
        /// <summary>
        /// True - Вернул, false - еще не вернул, но обещал вернуть
        /// </summary>
        public bool Status { get; set; } = false;
        

    }

    /// <summary>
    /// моделька публикации
    /// </summary>
    public class PublisherModel
    {
        /// <summary>
        /// айди публикации
        /// </summary>
        public long Id { get; set; } = 0;

        /// <summary>
        /// название публикации
        /// </summary>
        public string Name { get; set; } = "";


        /// <summary>
        /// айдишник города
        /// </summary>
        public CityModel City { get; set; } = new CityModel();

        

    }
    public class CityModel
    {

        /// <summary>
        /// айдишник города
        /// </summary>
        public long Id { get; set; } = 0;


        /// <summary>
        /// название города 
        /// </summary>
        public string Name { get; set; } = "";
    }

    public class AuthorModel
    {
        /// <summary>
        /// айдишник автора
        /// </summary>
        public long Id { get; set; } = 0;

        /// <summary>
        /// Фамилия
        /// </summary>
        public string Lastname { get; set; } = "";

        /// <summary>
        /// Имя
        /// </summary>
        public string Middlname { get; set; } = "";

        /// <summary>
        /// Отчество
        /// </summary>
        public string Fullname { get; set; } = "";

        /// <summary>
        /// сообщение о выполнении программы, которое находится UI
        /// </summary>
        public string Message { get; set; } = "";
    }

    public class SearchPatternModel
    {

        /// <summary>
        /// имя книги
        /// </summary>
        public string Name { get; set; } = "";
        /// <summary>
        /// Фио автора
        /// </summary>
        public string Fio { get; set; } = "";
        /*/// <summary>
        /// Фамилия автора
        /// </summary>
        public string Author { get; set; } = "";*/

        /// <summary>
        /// издательство
        /// </summary>
        public string Publisher { get; set; } = "";

        /// <summary>
        /// жанр
        /// </summary>
        public string Gerne { get; set; } = "";
    }
    public class bookmodel
    {
        /// <summary>
        /// название книги
        /// </summary>
        public string Name { get; set; } = "";
        /// <summary>
        /// фио автора
        /// </summary>
        public string Fio { get; set; } = "";
        /// <summary>
        /// издательство 
        /// </summary>
        public string Publisher { get; set; } = "";
        /// <summary>
        /// жанр
        /// </summary>
        public string Gerne { get; set; } = "";

    }

}
