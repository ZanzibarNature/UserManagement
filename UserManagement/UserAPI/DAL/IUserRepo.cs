using Azure;
using Azure.Data.Tables;

namespace UserAPI.DAL
{
    public interface IUserRepo<T> where T : class, ITableEntity, new()
    {
        Task<T> GetUserByKeyAsync(string partitionKey, string rowKey);
        Task<IEnumerable<T>> GetPageOfUsersAsync();
        Task<Response> UpsertUserAsync(T user);
        Task DeleteUserAsync(string partitionKey, string rowKey);
    }
}
