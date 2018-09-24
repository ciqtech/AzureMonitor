using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using LabApplicationBackendLayer.Models;
using LabApplicationBackendLayer.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Extensions.Options;

namespace LabApplicationBackendLayer.DAL
{
    internal class EmployeeCOSMOSDALBkup : ILabBackendDAL
    {
        private readonly string DBEndpoint;
        private readonly string DBKey;
        private readonly string DatabaseId;
        private readonly string CollectionId;
        private DocumentClient globalDocDBclient;
        private readonly string testUserId = "testuser";

        public EmployeeCOSMOSDALBkup(IOptions<AllConfiguration> configs)
        {
            ConfigReader configurationRader = new ConfigReader(configs);
            DBEndpoint = configurationRader.CosmosDBURL;
            DBKey = configurationRader.CosmosDBPrimaryKey;
            DatabaseId = configurationRader.CosmosDBName;
            CollectionId = configurationRader.CosmosCollectionName;

            globalDocDBclient = new DocumentClient(new Uri(DBEndpoint), DBKey, new ConnectionPolicy
            {
                ConnectionMode = ConnectionMode.Direct,
                EnableEndpointDiscovery = true,
                MaxConnectionLimit = 10,
                RequestTimeout = TimeSpan.FromMinutes(TimeZoneInfo.Utc.BaseUtcOffset.Minutes == 0 ? 20 : TimeZoneInfo.Utc.BaseUtcOffset.Minutes)
            },
            ConsistencyLevel.Session);
            CreateDatabaseIfNotExistsAsync().Wait();
            CreateCollectionIfNotExistsAsync().Wait();
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            IQueryable<Employee> queryable = globalDocDBclient.CreateDocumentQuery<Employee>(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId));

            List<Employee> results = queryable.ToList<Employee>();

            return results;
        }

        //Get the details of a particular employee  
        public Employee GetEmployeeData(string id)
        {
            IQueryable<Employee> queryable = globalDocDBclient.CreateDocumentQuery<Employee>(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId)).Where(t => t.EmpId == id);
            return queryable.ToList()[0];
        }

        public async void AddEmployee(Employee employee)
        {
            await globalDocDBclient.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId), employee);
        }

        public async void UpdateEmployee(Employee employee)
        {
            await globalDocDBclient.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, employee.EmpId), employee);
        }

        public async void DeleteEmployee(string id)
        {
            await globalDocDBclient.DeleteDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id.ToString()));
        }

        private async Task CreateDatabaseIfNotExistsAsync()
        {
            try
            {
                ResourceResponse<Database> dbResponse = await globalDocDBclient.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(DatabaseId),
                    new RequestOptions
                    {
                        SessionToken = "CosmosDB"
                    });
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await globalDocDBclient.CreateDatabaseAsync(new Database { Id = DatabaseId });
                }
                else
                {
                    throw;
                }
            }
        }

        private async Task CreateCollectionIfNotExistsAsync()
        {
            try
            {
                await globalDocDBclient.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                    new RequestOptions
                    {
                        SessionToken = "CosmosDB"
                    });
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await globalDocDBclient.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(DatabaseId),
                        new DocumentCollection { Id = CollectionId },
                        new RequestOptions { OfferThroughput = 1000 });
                }
                else
                {
                    throw;
                }
            }
        }
    }
}