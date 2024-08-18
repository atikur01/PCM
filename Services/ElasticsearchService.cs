using Nest;

namespace PCM.Services
{
    public class ElasticsearchService
    {
        private string _indexName { get; set; }
        private readonly IElasticClient _client;

        public ElasticsearchService(IElasticClient client, string indexName)
        {
            _client = client;
            _indexName = indexName;
        }

        public ElasticsearchService Index(string indexName)
        {
            _indexName = indexName;
            return this;
        }

        public async Task CreateIndexIfNotExists(string indexName)
        {
            if (!_client.Indices.Exists(indexName).Exists)
            {
                await _client.Indices.CreateAsync(indexName, c => c.Map<dynamic>(m => m.AutoMap()));
            }
            Index(indexName);
        }

        public async Task<bool> AddOrUpdateBulk<T>(IEnumerable<T> documents) where T : class
        {
            var indexResponse = await _client.BulkAsync(b => b
                   .Index(_indexName)
                   .UpdateMany(documents, (ud, d) => ud.Doc(d).DocAsUpsert(true)));
            return indexResponse.IsValid;
        }


        public async Task<bool> AddOrUpdate<T>(T document, Guid id) where T : class
        {
            var indexResponse = await _client.IndexAsync(document, idx => idx
                .Index(_indexName)
                .Id(id) // Ensure the document ID is specified
                .Refresh(Elasticsearch.Net.Refresh.True) // Optional: Refresh the index after operation
            );
            return indexResponse.IsValid;
        }


        //public async Task<bool> AddOrUpdate<T>(T document) where T : class
        //{
        //    var indexResponse = await _client.IndexAsync(document, idx => idx.Index(_indexName));
        //    return indexResponse.IsValid;
        //}

        public async Task<T> Get<T>(string key) where T : class
        {
            var response = await _client.GetAsync<T>(key, g => g.Index(_indexName));
            return response.Source;
        }

        public async Task<List<T>?> GetAll<T>() where T : class
        {
            var searchResponse = await _client.SearchAsync<T>(s => s.Index(_indexName).MatchAll());
            return searchResponse.IsValid ? searchResponse.Documents.ToList() : null;
        }

        public async Task<List<T>?> Query<T>(QueryContainer predicate) where T : class
        {
            var searchResponse = await _client.SearchAsync<T>(s => s.Index(_indexName).Query(q => predicate));
            return searchResponse.IsValid ? searchResponse.Documents.ToList() : null;
        }

        public async Task<bool> Remove<T>(string key) where T : class
        {
            var response = await _client.DeleteAsync<T>(key, d => d.Index(_indexName));
            return response.IsValid;
        }

        public async Task<long> RemoveAll<T>() where T : class
        {
            var response = await _client.DeleteByQueryAsync<T>(d => d.Index(_indexName).MatchAll());
            return response.Deleted;
        }

        public async Task<long> RemoveByTextMatch<T>(string fieldName, string text) where T : class
        {
            var response = await _client.DeleteByQueryAsync<T>(d => d
                .Index(_indexName)
                .Query(q => q
                    .Match(m => m
                        .Field(fieldName)
                        .Query(text)
                    )
                )
            );
            return response.Deleted;
        }


    }
}
