using Azure;
using Azure.Data.Tables;
using UserAPI.Domain;

namespace UserAPI.DAL
{
    public class UserRepo<T> : IUserRepo<T> where T : class, ITableEntity, new()
    {
        private readonly TableClient _tableClient;

        public UserRepo()
        {
            TableServiceClient serviceClient = new TableServiceClient("UseDevelopmentStorage=true");
            serviceClient.CreateTableIfNotExists("users");
            _tableClient = serviceClient.GetTableClient("users");
        }

        public async Task<T> GetUserByKeyAsync(string partitionKey, string rowKey)
        {
            var response = await _tableClient.GetEntityAsync<T>(partitionKey, rowKey);
            return response.Value;
        }

        public async Task<IEnumerable<T>> GetPageOfUsersAsync()
        {
            IList<T> results = new List<T>();
            var users = _tableClient.QueryAsync<T>(maxPerPage: 10);
            await foreach (var user in users)
            {
                results.Add(user);
            }
            return results;
        }

        public async Task<T> UpsertUserAsync(T user)
        {
            await _tableClient.UpsertEntityAsync(user);
            return user;
        }

        public async Task DeleteUserAsync(string partitionKey, string rowKey)
        {
            try
            {
                await _tableClient.DeleteEntityAsync(partitionKey, rowKey);
            }
            catch (RequestFailedException ex)
            {
                // Handle exceptions
                Console.WriteLine($"Error deleting entity: {ex.Message}");
            }
        }
    }
}
