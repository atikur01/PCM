using Nest; // Importing the Nest namespace for Elasticsearch client operations

namespace PCM.Services
{
    // This service class provides methods for interacting with Elasticsearch, 
    // including indexing, searching, updating, and deleting documents.
    public class ElasticsearchService
    {
        // Private field to store the current index name used for operations
        private string _indexName { get; set; }

        // Public readonly field for the IElasticClient, which is the main interface for interacting with Elasticsearch
        public readonly IElasticClient _client;

        // Constructor to initialize the Elasticsearch service with the client and an index name
        public ElasticsearchService(IElasticClient client, string indexName)
        {
            _client = client;
            _indexName = indexName;
        }

        // Method to set the index name and return the service instance, allowing for method chaining
        public ElasticsearchService Index(string indexName)
        {
            _indexName = indexName;
            return this;
        }

        // Method to set the index name without returning the service instance
        public void SetIndex(string indexName)
        {
            _indexName = indexName;
        }

        // Method to create an index in Elasticsearch if it doesn't already exist
        public async Task CreateIndexIfNotExists(string indexName)
        {
            // Check if the index exists
            if (!_client.Indices.Exists(indexName).Exists)
            {
                // Create the index with dynamic mapping if it doesn't exist
                await _client.Indices.CreateAsync(indexName, c => c.Map<dynamic>(m => m.AutoMap()));
            }
            // Set the index name to the newly created or existing index
            Index(indexName);
        }

        // Method to add or update a bulk of documents in the current index
        public async Task<bool> AddOrUpdateBulk<T>(IEnumerable<T> documents) where T : class
        {
            // Perform a bulk operation to add or update documents
            var indexResponse = await _client.BulkAsync(b => b
                   .Index(_indexName)
                   .UpdateMany(documents, (ud, d) => ud.Doc(d).DocAsUpsert(true))); // Upsert ensures that documents are inserted if they don't exist
            return indexResponse.IsValid; // Return true if the operation was successful
        }

        // Method to add or update a single document in the current index
        public async Task<bool> AddOrUpdate<T>(T document, Guid id) where T : class
        {
            // Index a single document by its ID
            var indexResponse = await _client.IndexAsync(document, idx => idx
                .Index(_indexName)
                .Id(id) // Specify the document ID
                .Refresh(Elasticsearch.Net.Refresh.True) // Optionally refresh the index after the operation
            );
            return indexResponse.IsValid; // Return true if the operation was successful
        }

        // Method to retrieve a single document by its key from the current index
        public async Task<T> Get<T>(string key) where T : class
        {
            // Get the document from Elasticsearch using the key
            var response = await _client.GetAsync<T>(key, g => g.Index(_indexName));
            return response.Source; // Return the document source (content)
        }

        // Method to retrieve all documents from the current index
        public async Task<List<T>?> GetAll<T>() where T : class
        {
            // Perform a search with a MatchAll query to retrieve all documents
            var searchResponse = await _client.SearchAsync<T>(s => s.Index(_indexName).MatchAll());
            return searchResponse.IsValid ? searchResponse.Documents.ToList() : null; // Return the list of documents if the search was valid
        }

        // Method to query documents based on a custom predicate
        public async Task<List<T>?> Query<T>(QueryContainer predicate) where T : class
        {
            // Perform a search based on the provided query predicate
            var searchResponse = await _client.SearchAsync<T>(s => s.Index(_indexName).Query(q => predicate));
            return searchResponse.IsValid ? searchResponse.Documents.ToList() : null; // Return the list of matching documents if the search was valid
        }

        // Method to remove a single document by its key from the current index
        public async Task<bool> Remove<T>(string key) where T : class
        {
            // Delete the document using its key
            var response = await _client.DeleteAsync<T>(key, d => d.Index(_indexName));
            return response.IsValid; // Return true if the deletion was successful
        }

        // Method to remove all documents from the current index
        public async Task<long> RemoveAll<T>() where T : class
        {
            // Perform a delete-by-query operation to remove all documents
            var response = await _client.DeleteByQueryAsync<T>(d => d.Index(_indexName).MatchAll());
            return response.Deleted; // Return the number of deleted documents
        }

        // Method to remove documents that match a specific text in a specific field
        public async Task<long> RemoveByTextMatch<T>(string fieldName, string text) where T : class
        {
            // Perform a delete-by-query operation based on a text match in a specific field
            var response = await _client.DeleteByQueryAsync<T>(d => d
                .Index(_indexName)
                .Query(q => q
                    .Match(m => m
                        .Field(fieldName)
                        .Query(text)
                    )
                )
            );
            return response.Deleted; // Return the number of deleted documents
        }
    }
}
