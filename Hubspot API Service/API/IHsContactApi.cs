using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Hubspot_API_Service.API
{
    public interface IHsContactApi
    {
        Task<(HttpStatusCode statusCode, string msg, long vid)> CreateOrUpdateUserContact(object contact, string email);

        Task<(HttpStatusCode statusCode, string msg)> CreateOrUpdateUserContacts<T>(List<T> contacts);

        Task<(HttpStatusCode statusCode, string msg)> Delete(long id);
    }
}
