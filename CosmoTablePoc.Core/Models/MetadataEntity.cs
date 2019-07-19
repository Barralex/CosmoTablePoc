using Microsoft.Azure.Cosmos.Table;
using System;

namespace CosmoTablePoc.Core.Models
{
    public class MetadataEntity : TableEntity
    {
        public MetadataEntity()
        {
        }

        public MetadataEntity(string tenantId, string fileName)
        {
            PartitionKey = tenantId;
            RowKey = $"{fileName}_{Guid.NewGuid().ToString()}";
        }

        public string FileName { get; set; }
        public string Tag { get; set; }
        public string Type { get; set; }
        public string Class { get; set; }
        public string Description { get; set; }
        public string CountryCode { get; set; }
    }
}
