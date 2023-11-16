using Azure;
using Azure.Data.Tables;

namespace UserAPI.DAL
{
    public class UserRepo<T> where T : class, ITableEntity, new()
    {
        private readonly TableClient _tableClient;

        public UserRepo()
        {
            TableServiceClient serviceClient = new TableServiceClient("UseDevelopmentStorage=true");
            serviceClient.CreateTableIfNotExistsAsync("user-table");
            _tableClient = serviceClient.GetTableClient("user-table");
        }

        public async Task<T> GetUserByKeyAsync(string partitionKey, string rowKey)
        {
            var response = await _tableClient.GetEntityAsync<T>(partitionKey, rowKey);
            return response.Value;
        }

        public async Task<Tuple<string, IEnumerable<T>>?> GetPageOfUsersAsync(string continuationToken)
        {
            try
            {
                var query = _tableClient.QueryAsync<T>(filter: "", maxPerPage: 10);
                var results = new List<T>();

                await foreach (var page in query.AsPages(continuationToken))
                {
                    return Tuple.Create<string, IEnumerable<T>>(page.ContinuationToken, page.Values);
                }

                throw new RequestFailedException("No new pages available");
            }
            catch (RequestFailedException ex)
            {
                // Handle exceptions
                Console.WriteLine($"Error getting all entities: {ex.Message}");
                return null;
            }
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
