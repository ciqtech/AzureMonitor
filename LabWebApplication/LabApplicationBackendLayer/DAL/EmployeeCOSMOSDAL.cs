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
    internal class EmployeeCOSMOSDAL : ILabBackendDAL
    {
        private readonly string DBEndpoint;
        private readonly string DBKey;
        private readonly string DatabaseId;
        private readonly string CollectionId;
        private DocumentClient globalDocDBclient;
        private readonly string testUserId = "testuser";

        public EmployeeCOSMOSDAL()
        {
            DBEndpoint = ConfigReader.CosmosDBURL;
            DBKey = ConfigReader.CosmosDBPrimaryKey;
            DatabaseId = ConfigReader.CosmosDBName;
            CollectionId = ConfigReader.CosmosCollectionName;

            string token = GetToken(1200).Result;
            globalDocDBclient = new DocumentClient(new Uri(DBEndpoint), token);            
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

        private async Task<string> GetToken(int tokenValiditySeconds)
        {
            DocumentClient docClient = new DocumentClient(new Uri(DBEndpoint), DBKey);
            ResourceResponse<Database> db = await UpsertDb(docClient);
            ResourceResponse<DocumentCollection> collection = await UpsertCollection(docClient, db);

            string dbSelfLink = db.Resource.SelfLink;

            var user = await docClient.UpsertUserAsync(dbSelfLink, new User() { Id = testUserId });

            var permission =
                await docClient.UpsertPermissionAsync(
                    user.Resource.SelfLink,
                    new Permission()
                    {
                        Id = "PersmissionForTestUser",
                        PermissionMode = PermissionMode.All,
                        ResourceLink = collection.Resource.SelfLink
                    },
                    new RequestOptions()
                    {
                        ResourceTokenExpirySeconds = tokenValiditySeconds,
                        ConsistencyLevel = ConsistencyLevel.Session,
                        IndexingDirective = IndexingDirective.Default,

                    });

            permission =
                await docClient.ReadPermissionAsync(
                    permission.Resource.SelfLink,
                    new RequestOptions()
                    {
                        ResourceTokenExpirySeconds = tokenValiditySeconds
                    });

            return permission.Resource.Token;
        }

        private async Task<ResourceResponse<DocumentCollection>> UpsertCollection(DocumentClient docClient, ResourceResponse<Database> db)
        {
            ResourceResponse<DocumentCollection> collection = null;

            try
            {
                collection = await docClient.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId));
            }
            catch (DocumentClientException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    collection = await docClient.CreateDocumentCollectionAsync(db.Resource.SelfLink, new DocumentCollection() { Id = CollectionId });
                }
            }

            return collection;
        }

        private async Task<ResourceResponse<Database>> UpsertDb(DocumentClient docClient)
        {
            ResourceResponse<Database> db = null;
            db = await docClient.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(DatabaseId));
            return db;
        }

        public void PerformDeleteUpdate(string employeeId, int trackNumber)
        {
        }
    }
}