using System;
using System.Collections.Generic;
using System.Text;

namespace LabApplicationBackendLayer.Utilities
{
    public class AllConfiguration
    {
        public string BackendDBType { get; set; }
        public string KeyVaultBaseURL { get; set; }
        public string SQLConnectionstringSecretKey { get; set; }
        public string CosmosDBURLKey { get; set; }
        public string CosmosDBPrimaryKey { get; set; }
        public string KVClientIdKey { get; set; }
        public string KVClientSecretKey { get; set; }
        public string CosmosDBNameKey { get; set; }
        public string CosmosCollectionNameKey { get; set; }
    }

    public class ApplicationInsightsConfiguration
    {
        public string InstrumentationKey { get; set; }
    }
}
