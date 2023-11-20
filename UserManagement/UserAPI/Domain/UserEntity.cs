﻿using Azure;
using Azure.Data.Tables;

namespace UserAPI.Domain
{
    public class UserEntity : UserBase, ITableEntity
    {
        public string? PartitionKey { get; set; }

        public string? RowKey { get; set; }

        public DateTimeOffset? Timestamp { get; set; }

        public ETag ETag { get; set; }

        // Custom Properties
        public UserType UserType { get; set; }
    }
}
