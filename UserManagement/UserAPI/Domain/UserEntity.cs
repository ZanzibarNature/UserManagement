using Azure;
using Azure.Data.Tables;

namespace UserAPI.Domain
{
    public class UserEntity : UserBase, ITableEntity
    {
        public int ID { get; set; }

        public DateTime LastActive {  get; set; }
        

        // Azure Table Storage
        public string PartitionKey { get; set; }

        public string RowKey { get; set; }

        public DateTimeOffset? Timestamp { get; set; }

        public ETag ETag { get; set; }
    }
}
