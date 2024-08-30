using atikapps;
using AutoMapper;
using PCM.AutomapperMappingProfile;
using PCM.ElasticSearchModels;
using PCM.Models;
using PCM.Repositories;

namespace PCM.Services
{
    public class CollectionService
    {
        private readonly ICollectionRepository _repository;
        private ElasticsearchService _elasticsearchService;   
        
        private bool IsElasticSearchServerConfigured = false;    

        public CollectionService(ICollectionRepository repository,ElasticsearchService elasticsearchService)
        {
            _repository = repository;
            _elasticsearchService = elasticsearchService;   
        }

        public async Task<Collection>? GetByIdAsync(Guid id)
        {
            var collection = await _repository.GetByIdAsync(id);
            if (collection == null)
            {
                return null;
            }
            return collection;
        }

        public async Task<IEnumerable<Collection>>? GetAllAsync()
        {
            var collections = await _repository.GetAllAsync();
            return collections;
        }

        public async Task AddAsync(Collection collection)
        {
            await _repository.AddAsync(collection);
            if (IsElasticSearchServerConfigured)
            {
                await AddToElasticsearchAsync(collection);
            }
        }

        public async Task AddToElasticsearchAsync(Collection collection)
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            var mapper = config.CreateMapper();
            EsCollection target = mapper.Map<EsCollection>(collection);
            await _elasticsearchService.CreateIndexIfNotExists("collection-index");
            var result = await _elasticsearchService.AddOrUpdate(target, target.CollectionId);
        }

        public async Task UpdateAsync(Collection collection)
        {
            await _repository.UpdateAsync(collection);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Collection>> GetByUserId(Guid userId)
        {
            var collections = await _repository.GetByUserId(userId);
            return collections;
        }

        public async Task<IEnumerable<Category>> GetAllCategoryAsync()
        {
            return await _repository.GetAllCategoryAsync();  
        }   
    }
}
