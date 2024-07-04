using library77.Models.Library;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace library77.Clasess
{
    public class CommonMethods 
    {
        /// <summary>
        /// метод находит по логину пользователя, должен найди его айдишник и вернуть через return.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public long GetUserIdByUsername(string username)
        {
            using (LibraryContext context = new LibraryContext())
            {
                Reader result = context.Readers
                                .Where(x => x.Username == username)
                                .Select(x => x)
                                .FirstOrDefault();
                /*.FirstOrDefault(x => x.Username == username);*/

                if (result != null)
                {
                    // Если пользователь найден, возвращаем его айдишник
                    return result.Id;
                }
                else
                {
                    return -1;
                }
            }
        } 

    }
}

