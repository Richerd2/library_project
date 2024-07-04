using library77.Enums;
using library77.Models;
using library77.Models.Library;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using NuGet.LibraryModel;
using NuGet.Protocol.Core.Types;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Reflection.Metadata.BlobBuilder;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace library77.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        //тут надо два метода
        //1 метод который выгружает из 3 свойств админ,реадер, техспец, булевые тру фолс
        [HttpGet]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = Configs.ReaderRole + ", " + Configs.TechSpecRole + ", " + Configs.AdminRole)]
        public async Task<ActionResult> GetRoles()
        {

            #region Определяем id пользователя
            long userId = new Clasess.CommonMethods().GetUserIdByUsername(User.Identity.Name);
            #endregion
            using (LibraryContext context = new LibraryContext())
            {
                var roless = context.ReaderRoles
                            .Where(x => x.ReaderId == userId)
                            .Select(x => x)
                            .ToList();

                var result = new Models.UserRolesModel();


                for (int i = 0; i < roless.Count; i++)
                {
                    if (roless[i].RolesId == (int)Enums.roles.READER)
                    {
                        result.Reader = true;
                    }
                    if (roless[i].RolesId == (int)Enums.roles.ADMIN)
                    {
                        result.Admin = true;
                    }
                    if (roless[i].RolesId == (int)Enums.roles.TEXNSPEC)
                    {
                        result.TechSpec = true;
                    }
                }


                return Ok(result);
            }
        }


        
        //отдельно в models // все данные из токена
        [HttpGet]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = Configs.ReaderRole + ", " + Configs.TechSpecRole + ", " + Configs.AdminRole)]
        public async Task<ActionResult> GetUserData()
        {
            #region Определяем id пользователя
            long userId = new Clasess.CommonMethods().GetUserIdByUsername(User.Identity.Name);
            #endregion

            using (LibraryContext context = new LibraryContext())
            {
                Reader result = context.Readers
                                .Where(x => x.Id == userId)
                                .Select(x => x)
                                .FirstOrDefault();

                if (result != null)
                {
                    var userdata = new Models.UserData()
                    {
                        Lastname = result.Lastname,
                        Fullname = result.Fullname,
                        Middlname = result.Middlname,
                        id = result.Id,
                        DateAdd = result.DateAdd,
                        LibraryCard = result.LibraryCard

                    };
                    return Ok(userdata);
                }
                return NoContent();
            }
        }



        [HttpGet]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = Configs.ReaderRole + ", " + Configs.TechSpecRole + ", " + Configs.AdminRole)]
        public async Task<ActionResult> GetUserBookHistory()
        {
            #region Определяем id пользователя
            long userId = new Clasess.CommonMethods().GetUserIdByUsername(User.Identity.Name);
            #endregion

            var result = new List<BookHistoryModel>();

            using (LibraryContext context = new LibraryContext())
            {
                var userBooksHistory = context.Loggs
                                       .Where(x => x.ReaderId == userId)
                                       .Select(x => x)
                                       .OrderByDescending(x => x.DateOfDelivery)
                                       .ToList();

                var userBooksIds = userBooksHistory
                                   .Select(x => x.BookId)
                                   .Distinct()
                                   .ToList();

                var userBooks = context.Books
                                .Where(x => userBooksIds.Contains(x.Id))
                                .Select(x => x)
                                .ToList();

                var autorsBooks = context.AuthorsBooks
                                  .Where(x => userBooksIds.Contains(x.BookId))
                                  .Select(x => x)
                                  .ToList();

                var autorsIds = autorsBooks
                                .Select(x => x.AuthorsId)
                                .Distinct()
                                .ToList();

                var authors = context.Authors
                              .Where(x => autorsIds.Contains(x.Id))
                              .Select(x => x)
                              .ToList();
                //поменял с publisherid на publisher
                var publishersIds = userBooks
                                    .Select(x => x.PublisherId)
                                    .Distinct()
                                    .ToList();

                var publishers = context.Publishers
                                 .Where(x => publishersIds.Contains(x.Id))
                                 .Select(x => x)
                                 .ToList();

                var publishersCitiesIds = publishers
                                          .Select(x => x.CityId)
                                          .Distinct()
                                          .ToList();

                var cities = context.Cities
                             .Where(x => publishersCitiesIds.Contains(x.Id))
                             .Select(x => x)
                             .ToList();
                /*
                                Stopwatch sw1 = new Stopwatch();
                                sw1.Start();

                                result = userBooksHistory
                                         .Select(x => new BookHistoryModel()
                                         {
                                             Id = x.Id,
                                             BookId = x.BookId,
                                             DateOfDelivery = new DateTimeOffset(DateTime.SpecifyKind(x.DateOfDelivery, DateTimeKind.Utc)).ToUnixTimeMilliseconds(),
                                             DateOfIssue = new DateTimeOffset(DateTime.SpecifyKind(x.DateOfIssue, DateTimeKind.Utc)).ToUnixTimeMilliseconds(),
                                             Status = x.Status,
                                             Name = userBooks
                                                    .Where(y => y.Id == x.BookId)
                                                    .Select(x => x.Name)
                                                    .DefaultIfEmpty("Название не найдено!")
                                                    .FirstOrDefault(),
                                             YearOfPublication = userBooks
                                                                 .Where(y => y.Id == x.BookId)
                                                                 .Select(x => x.YearOfPublication)
                                                                 .DefaultIfEmpty(-1)
                                                                 .FirstOrDefault(),
                                             Authors = authors
                                                       .Where(y => autorsBooks.Where(z => z.BookId == z.BookId).Select(z => z.AuthorsId).ToList().Contains(y.Id))
                                                       .Select(y => new AuthorModel()
                                                       {
                                                           Id = y.Id,
                                                           Fullname = y.Fullname,
                                                           Lastname = y.Lastname,
                                                           Message = "",
                                                           Middlname = y.Middlname
                                                       })
                                                       .ToList(),
                                             Publisher = publishers
                                                         .Where(y => y.Id == userBooks.Where(z => z.Id == x.BookId).Select(z => z.PublisherId).DefaultIfEmpty(-1).FirstOrDefault())
                                                         .Select(y => new PublisherModel()
                                                         {
                                                             Id = y.Id,
                                                             Name = y.Name,
                                                             City = cities
                                                                   .Where(z => z.Id == y.CityId)
                                                                   .Select(z => new CityModel()
                                                                   {
                                                                       Id = z.Id,
                                                                       Name = z.Name
                                                                   })
                                                                   .FirstOrDefault()
                                                         })
                                                         .FirstOrDefault()
                                         })
                                         .Where(x => x.Authors.Count > 0 &&
                                                     x.Publisher != null &&
                                                     x.Publisher.City != null)
                                         .ToList();

                                sw1.Stop();
                                result.Clear();
                                Stopwatch sw2 = new Stopwatch();
                                sw2.Start();*/

                for (int i = 0; i < userBooksHistory.Count; i++)
                {
                    var book = userBooks
                               .Where(x => x.Id == userBooksHistory[i].BookId)
                               .Select(x => x)
                               .FirstOrDefault();

                    if (book != null)
                    {
                        var publisher = publishers
                            ///поменял с publisherid на publisher
                                        .Where(x => x.Id == book.PublisherId)
                                        .Select(x => x)
                                        .FirstOrDefault();

                        if (publisher != null)
                        {
                            var city = cities
                                       .Where(x => x.Id == publisher.CityId)
                                       .Select(x => x)
                                       .FirstOrDefault();

                            if (city != null)
                            {
                                var bookAuthorsIds = autorsBooks
                                                     .Where(x => x.BookId == book.Id)
                                                     .Select(x => x.AuthorsId)
                                                     .ToList();

                                var bookAuthors = authors
                                                  .Where(x => bookAuthorsIds.Contains(x.Id))
                                                  .Select(x => x)
                                                  .ToList();

                                var oneResult = new BookHistoryModel()
                                {
                                    Id = userBooksHistory[i].Id,
                                    BookId = book.Id,
                                    Name = book.Name,
                                    YearOfPublication = book.YearOfPublication,
                                    DateOfDelivery = new DateTimeOffset(DateTime.SpecifyKind(userBooksHistory[i].DateOfDelivery, DateTimeKind.Utc)).ToUnixTimeMilliseconds(),
                                    DateOfIssue = new DateTimeOffset(DateTime.SpecifyKind(userBooksHistory[i].DateOfIssue, DateTimeKind.Utc)).ToUnixTimeMilliseconds(),
                                    Status = userBooksHistory[i].Status,
                                    Publisher = new PublisherModel()
                                    {
                                        Id = publisher.Id,
                                        Name = publisher.Name,
                                        City = new CityModel()
                                        {
                                            Id = city.Id,
                                            Name = city.Name
                                        }
                                    },
                                    Authors = bookAuthors
                                              .Select(x => new AuthorModel()
                                              {
                                                  Id = x.Id,
                                                  Fullname = x.Fullname,
                                                  Lastname = x.Lastname,
                                                  Message = "",
                                                  Middlname = x.Middlname
                                              })
                                              .ToList()
                                };

                                result.Add(oneResult);
                            }
                        }
                    }
                }

                /*      sw2.Stop();

                      string test = sw1.ElapsedMilliseconds.ToString() + Environment.NewLine + sw2.ElapsedMilliseconds.ToString();*/

                #region Старый код
                //var userbooks = context.Loggs
                //                .Where(x => x.ReaderId == userId)
                //                .Select(x => x)
                //                .ToList();


                //var bookids = userbooks
                //              .Select(x => x.BookId)
                //              .Distinct()
                //              .ToList();


                //var books = context.Books
                //               .Where(x => bookids.Contains(x.Id))
                //               .Select(x => x)
                //               .ToList();

                //var publisherid = books
                //              .Select(x => x.PublisherId)
                //              .Distinct()
                //              .FirstOrDefault();


                //var cityid = context.Publishers
                //            .Where(x => x.Id == publisherid)
                //            .Select(x => x.CityId)
                //            .FirstOrDefault();

                //var cities = context.Cities
                //            .Where(x => x.Id == cityid)
                //            .Select(x => x)
                //            .FirstOrDefault();

                //var publisher = context.Publishers
                //                .Where(x => x.Id == publisherid)
                //                .Select(x => x)
                //                .FirstOrDefault();

                //var authorid = context.AuthorsBooks
                //                .Where(x => bookids.Contains(x.Id))
                //                .Select(x => x.AuthorsId)
                //                .Distinct()
                //                .ToList();


                //var authors = context.Authors
                //              .Where(x => authorid.Contains(x.Id))
                //              .Select(x => x)
                //              .ToList();

                /////вытаскиваю по зд
                //var loggBooks = context.Loggs
                //                .Where(x => x.ReaderId == userId)
                //                .Select(x => x)
                //                .ToList();

                //var statusLoggBooks = loggBooks
                //                      .Where(x => bookids.Contains(x.BookId))
                //                      .ToList();    

                //if (userbooks.Count > 0 )
                //{
                //    for (int i = 0; i < userbooks.Count(); i++)
                //    {

                //        for(int j = 0; j < authors.Count(); j++)
                //        {

                //            var book = books
                //                .Where(x => x.Id == userbooks[i].BookId)
                //                .Select(x => x)
                //                .FirstOrDefault();

                //            var author = authors
                //                .Where(x => x.Id == authors[j].Id)
                //                .Select (x => x)
                //                .FirstOrDefault();


                //            BookModel bookModel = new BookModel()
                //            {
                //                Id = userbooks[i].Id,
                //                Name = book.Name,
                //                Publisher = new Models.PublisherModel()
                //                {
                //                    Id = book.PublisherId,
                //                    City = new CityModel() { Id = cityid, Name = cities.Name },
                //                    Name = publisher.Name
                //                },

                //                YearOfPublication = book.YearOfPublication,
                //                Authors = new List<AuthorModel>()

                //            };
                //            bookModel.Authors = authors
                //                                .Where(x => x.Id == authors[j].Id)
                //                                .Select(x => new AuthorModel()

                //                                {
                //                                    Id = x.Id,
                //                                    Fullname = x.Fullname,
                //                                    Lastname = x.Lastname,
                //                                    Middlname = x.Middlname,
                //                                    Message = UI_Texts.SUCCESS_MESSAGE

                //                                })
                //                                .ToList();


                //            result.Add(bookModel);
                //        }

                //        #region Комменты
                //        /*var book = books
                //                .Where(x => x.Id == userbooks[i].BookId)
                //                .Select(x => x)
                //                .FirstOrDefault();

                //        BookModel bookModel = new BookModel()
                //        {
                //            Id = userbooks[i].Id,
                //            Name = book.Name,
                //            Publisher = new Models.PublisherModel()
                //            {
                //                Id = book.PublisherId,
                //                City = new CityModel() { Id = cityid, Name = cities.Name },
                //                Name = publisher.Name
                //            },

                //            YearOfPublication = book.YearOfPublication,
                //            *//*Authors = new AuthorModel() { Id = A*//*
                //        };

                //        result.Add(bookModel);*/
                //        #endregion

                //    }

                //}
                #endregion

                if (result.Count > 0)
                {
                    return Ok(result);
                }
                else
                {
                    return NoContent();
                }
            }
        }


        ///метод реализации(достуепн всем кроме читателя в этот метод нужно передать айдишник читателя и айдишник книги через from-хедеры) который выдает книгу пользователю, должен записывать в журналу выдачу книг со статусом 0.
        ///они должны быть обязательными хеадеры метод по итогу должен добавить в табличку журнала запись в которой будет отражен айдишник пользователя и айдишник книги и статус 0 по дефолту
        [HttpPost]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = Configs.TechSpecRole + ", " + Configs.AdminRole)]
        public async Task<ActionResult> PostBook([FromHeader(Name = "ReaderId")][Required] long ReaderId,
                                                 [FromHeader(Name = "BookId")][Required] long BookId)
        {
            /* #region Определяем id пользователя
             long userId = new Clasess.CommonMethods().GetUserIdByUsername(User.Identity.Name);
             #endregion*/


            /*var result = new List<BookHistoryModel>();*/
            /*
                        if(roles == Reader)
                        {
                            return forbid;//403-ошибка, которая обозначает, что сервер понял запрос, но не будет его выполнять
                        }*/

            using (LibraryContext context = new LibraryContext())
            {
                // Проверяем, есть ли указанный читатель и книга в базе данных
                var readerr = context.Readers
                              .Where(x => x.Id == ReaderId)
                              .Select(x => x)
                              .FirstOrDefault();

                var bookk = context.Books
                            .Where(x => x.Id == BookId)
                            .Select(x => x)
                            .FirstOrDefault();



                if (readerr == null || bookk == null)
                {
                    return NoContent(); // Если не найдены, возвращаем 204 код
                }


                // Создаем новую запись в журнале выдачи книг
                var newRecordLogg = new Models.Library.Logg
                {
                    ReaderId = ReaderId,
                    BookId = BookId,
                    Status = false,
                    DateOfDelivery = DateTime.Now,
                    DateOfIssue = new DateTime(1970, 1, 2, 0, 0, 0)
                };
                context.Loggs.Add(newRecordLogg);

                await context.SaveChangesAsync();

                /*result.Add(oneResult);*/

                return Ok();
            }
        }
        //метод возвращение книги(несколько книг) черед bodyx передать lsitid записи в журнал, все записи по id статус с false на true, все записи журналы

        [HttpPost]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = Configs.TechSpecRole + ", " + Configs.AdminRole)]
        public async Task<ActionResult> PostReturnBook([FromBody] List<long> model)
        {
            using (LibraryContext context = new LibraryContext())
            {
                var Logss = context.Loggs
                            .Where(x => model.Contains(x.Id))
                            .Select(x => x)
                            .ToList();
                for (int i = 0; i < Logss.Count; i++)
                {
                    /*Logss[i];*/
                }

                /*  var newTrue = new Models.Library.Logg
                  {
                      Status = true,
                      DateOfDelivery = DateTime.Now,
                      DateOfIssue = new DateTime(1970, 1, 2, 0, 0, 0)

                  };

                  context.Loggs.Update(newTrue);
                  await context.SaveChangesAsync();*/


                return Ok();
            }
        }



        [HttpPost]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = Configs.TechSpecRole + ", " +
                                                                                                                                         Configs.AdminRole + ", " +
                                                                                                                                         Configs.ReaderRole)]
        public async Task<ActionResult> TestVoid()
        {
            using (LibraryContext lbContext = new LibraryContext())
            {
                #region Задание 1(Работа на contains)
                // Выгрузить всех авторов в authors с id из листа authorIdsList
                //принято//выполнено
                var authorIdsList = new List<long>
                {
                    1,
                    3
                };

                var authors = lbContext.Authors
                              .Where(x => authorIdsList.Contains(x.Id))
                              .Select(x => x)
                              .ToList();



                #endregion

                #region Задание 2 (Работа с датами)
                // Выгрузить все логи выдачи/возврата книг после 01.01.2023 в logs
                //выполнено/принято
                DateTime date1 = new DateTime(2023, 1, 1, 0, 0, 0);

                var loggg = lbContext.Loggs
                            .Where(x => x.Status && (x.DateOfDelivery > date1 && x.DateOfIssue > date1))
                            .OrderByDescending(x => x.DateOfDelivery)
                            .ThenByDescending(x => x.DateOfIssue)
                            .ToList();



                var logs = new List<Models.Library.Logg>();


                #endregion

                #region Задание 3 (Работа на contains)
                // Выгрузить все id книг, которые взял читатель readerId в лист readerBooksIds после 01.01.2023. Выгрузить информацию по всем этим книгам в readerBooks

                long readerId = 1;

                var readerBooksIds = new List<long>()
                {
                    1
                };

                var bookss = lbContext.Books
                             .Where(x => readerBooksIds.Contains(x.Id))
                             .Select(x => x)
                             .FirstOrDefault();


                DateTime date3 = new DateTime(2023, 1, 1, 0, 0, 0);
                var bookss2 = lbContext.Loggs
                              .Where(x => x.Status && (x.DateOfDelivery > date3))
                              .Select(x => x)
                              .FirstOrDefault();

                var readerBooks = new List<Book>();


                /*return Ok(bookss2);*/

                #endregion

                #region Задание 4 (Работа на contains)
                // Выгрузить все книги в books жанров gernes, которые содержат в своем названии подстроку substring

                string substring = "A"; // rus

                var gernec = lbContext.Gernes
                             .Where(x => x.Title.Contains(substring))
                             .Select(x => x.Id)
                             .ToList();

                var gernes = new List<Gerne>();


                var bookGernes = lbContext.BookGernes
                                .Where(x => gernec.Contains(x.GernesId))
                                .Select(x => x.BookId)
                                .ToList();

                var bookIds = new List<long>();

                var booksss = lbContext.Books
                              .Where(x => bookGernes.Contains(x.Id))
                              .Select(x => x)
                              .ToList();

                var books = new List<Book>();


                #endregion

                #region Задание 5 (Работа со строками)
                // Выгрузить всех авторов в authors2 фамилия у которых начинается на start и заканчивается на end. Поиск регистронезависимый

                string start = "э";
                string end = "х";//исправил, так как таких авторов у меня не имеется. только Антон

                var authorsss = lbContext.Authors
                                .Where(x => x.Lastname.StartsWith(start) && x.Lastname.EndsWith(end))
                                .Select(x => x)
                                .ToList();




                var authors2 = new List<Author>();
                #endregion

                #region Задание 6 (Работа с датами в миллисекундах)
                // Выгрузить все записи логов в logs2, дата которых меньше даты now (now - миллисекунды)


                var now = DateTimeOffset.Now.ToUnixTimeMilliseconds();

                /*var logg = lbContext.Loggs
                            .Where(x => new DateTimeOffset(x.DateOfDelivery).ToUnixTimeMilliseconds() < now && new DateTimeOffset(x.DateOfIssue).ToUnixTimeMilliseconds() < now)
                            .Select(x => x)
                            .ToList();*/

                /*var logg = lbContext.Loggs
                          .Where(x => (long)(x.DateOfDelivery - new DateTime(1970, 1, 1)).TotalMilliseconds < now && (long)(x.DateOfIssue - new DateTime(1970, 1, 1)).TotalMilliseconds < now)
                          .ToList();
                */
                /*var logg = lbContext.Loggs
                            .Where(x => new DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond < now && new DateTimeOffset(x.DateOfIssue).ToUnixTimeMilliseconds() < now)
                            .Select(x => x)
                            .ToList();
*/
                /*var now = DateTimeOffset.Now.ToUnixTimeMilliseconds();*/

                /*var logg = lbContext.Loggs
                            .Where(x => new DateTimeOffset(x.DateOfDelivery).ToUnixTimeMilliseconds() < now && new DateTimeOffset(x.DateOfIssue).ToUnixTimeMilliseconds() < now)
                            .ToList();*/



                var logg = lbContext.Loggs
                            .AsEnumerable()
                            .Where(x => new DateTimeOffset(x.DateOfDelivery).ToUnixTimeMilliseconds() < now && new DateTimeOffset(x.DateOfIssue).ToUnixTimeMilliseconds() < now)
                            .ToList();



                var logs2 = new List<Models.Library.Logg>();





                #endregion
            }


            return Ok();
        }



        //доступ у техн спец и админа
        //метод который будет выгружать информацию по книгам с ученом фильтров(название, жанр,издательство, фамилия автора книги)
        [HttpPost]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme, Roles = Configs.TechSpecRole + ", " + Configs.AdminRole)]
        public async Task<ActionResult> GetBook2([FromBody]/*[Required]*/ SearchPatternModel model)
        {
            var result = new List<Book>();

            using (LibraryContext context = new LibraryContext())
            {
                /*//1 книга
                var bookk = context.Books
                           .Where(x => x.Id == x.Id)
                           .Select(x => x)
                           .ToList();
                //2 жанр
                var genress = context.Gernes
                              .Where(x => x.Title == x.Title)
                              .Select(x => x)
                              .ToList();
                //3 жанр
                var publisher = context.Books
                               .Where(x => x.Name == x.Name)
                               .Select(x => x)
                               .ToList();
                //4 год
                var year_publication = context.Books
                                       .Where(x => x.YearOfPublication == x.YearOfPublication)
                                       .Select(x => x)
                                       .ToList();

                //5 fio
                var authorsFIO = context.Authors
                                .Where(x => x.Id == x.Id)
                                .Select(x => x)
                                .ToList();

                if (bookk == null || genress == null || publisher == null || year_publication == null || authorsFIO == null)
                {
                    return NoContent(); // Если не найдены, возвращаем 204 код
                }


                // Создаем новую запись 
                var newlist = new Models.Library.Logg
                {
                    
                    
                };
                context.Loggs.Add(newlist);

                await context.SaveChangesAsync();

                *//*result.Add(oneResult);*//*

                *//*return Ok();*/
                /*LibraryContext contexte = new LibraryContext();

                var booksQuery = contexte.Books.AsQueryable();
                if (!string.IsNullOrEmpty())
                {
                    booksQuery = booksQuery.Where(b => b.Name == name);
                }

                var filteredBooks = booksQuery.ToList();

                return Ok(filteredBooks);*//*
                // достаем книги
                var booksQuery = context.Books
                                .AsQueryable();

                if (!string.IsNullOrEmpty(Name))
                {
                    booksQuery = booksQuery
                                .Where(x => x.Name == Name);
                                
                }
                var books = booksQuery.ToList();
                
                
                //достаем жанры
                var titleQuery = context.Gernes
                                .AsQueryable();

                if (!string.IsNullOrEmpty(Title))
                {
                    titleQuery = titleQuery
                                 .Where(x => x.Title == Title);
                }
                var genress = titleQuery.ToList();

                //достаем публикацию
                var publisherQuery = context.Publishers
                                    .AsQueryable();

                if (!string.IsNullOrEmpty(Publisher))
                {
                    publisherQuery = publisherQuery
                                    .Where(x => x.Name == Name);
                }
                var publisher = publisherQuery.ToList();

                //достаем фамилию автора
                var authorQuery = context.Authors
                                 .AsQueryable();

                if(!string.IsNullOrEmpty(FullName))
                {
                    authorQuery = authorQuery
                                  .Where(x => x.Fullname == FullName);
                }
                var author = authorQuery.ToList();
*/
                ////////////////

                /*var BooksHistory = context.Loggs
                                   .Where(x => x.Id == x.Id)
                                   .Select(x => x)
                                   .ToList();

                var BooksIds = BooksHistory
                               .Select(x => x.BookId)
                               .Distinct()
                               .ToList();

                var Books = context.Books
                            .Where(x => x.Name.Contains(x.Name))
                            .Select(x => x)
                            .ToList();

                var autorsBooks = context.AuthorsBooks
                                  .Where(x => BooksIds.Contains(x.BookId))
                                  .Select(x => x)
                                  .ToList();

                var autorsIds = autorsBooks
                                .Select(x => x.AuthorsId)
                                .Distinct()
                                .ToList();

                var authors = context.Authors
                              .Where(x => autorsIds.Contains(x.Id))
                              .Select(x => x)
                              .ToList();

                var publishersIds = Books
                                    .Select(x => x.PublisherId)
                                    .Distinct()
                                    .ToList();

                var publishers = context.Publishers
                                 .Where(x => publishersIds.Contains(x.Id))
                                 .Select(x => x)
                                 .ToList();*/


                /*var name = context.Books
                          .Where(x => x.Name.Contains(model.Name))
                          .Select(x => x)
                          .ToList();

                var genre = context.Gernes
                            .Where(x => x.Title.Contains(model.Gerne))
                            .Select(x => x)
                            .ToList();

                var publisher = context.Publishers
                                .Where(x => x.Name.Contains(model.Publisher))
                                .Select(x => x)
                                .ToList();

                var author = context.Authors
                            .Where(x => x.Fullname.Contains(model.Author))
                            .Select(x => x)
                            .ToList();



                if (name == null && genre == null && publisher == null && author == null )
                {
                    return NoContent(); // Если не найдены, возвращаем 204 код
                }

                

                return Ok(result);*/

                /* var books = context.Books
                              .Where(x => x.Name.Contains(model.Name))
                              .Where(x => x.Gerne.Contains(model.Gerne))
                              .Where(x => x.Name.Contains(model.Publisher))
                              .Where(x => x.Author.Contains(model.Author))
                              .Select(b => new Book()
                              {
                                  Name = b.Name,
                                  Author = b.Author,
                                  Gerne = b.Gerne,
                                  PublisherId = b.PublisherId

                              })
                              .ToList();

                              result.AddRange(books);

                 return Ok();*/
                ///id по сущностям и пересечения


                var bookIds = context.Books
                                .Where(x => x.Name.ToLower().Contains(model.Name.ToLower()))
                                .Select(x => x.Id)
                                .Distinct()
                                .ToList();

                var books = context.Books
                            .Where(x => bookIds.Contains(x.Id))
                            .ToList();

                var authorIds = context.Authors
                                .Where(x => (x.Lastname + " " + x.Middlname + " " + x.Fullname).ToLower().Contains(model.Fio.ToLower()))
                                .Select(x => x.Id)
                                .Distinct()
                                .ToList();

                var authorsBooks = context.AuthorsBooks
                                    .Where(x => bookIds.Contains(x.BookId) && authorIds.Contains(x.AuthorsId))
                                    .ToList();

                var publisherIds = context.Publishers
                                    .Where(x => x.Name.ToLower().Contains(model.Publisher.ToLower()))
                                    .Select(x => x.Id)
                                    .Distinct()
                                    .ToList();

                var genreIds = context.Gernes
                                .Where(x => x.Title.ToLower().Contains(model.Gerne.ToLower()))
                                .Select(x => x.Id)
                                .Distinct()
                                .ToList();

                result = books
                        .Where(x => publisherIds.Contains(x.PublisherId) || genreIds.Contains(x.Id) || authorIds.Contains(x.Id))
                        .ToList();

            }
            if (result.Count == 0)
            {
                return NoContent(); // если не найдены, возвращаем 204 код
            }
            return Ok(result);

        }
    }
}
