using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDbGenericRepository.Attributes;

namespace OnlineNotes.Data.Repositories;

public class RepositoryBase<T>
{
	protected readonly IMongoCollection<T> collection;

	public RepositoryBase(IOptions<MongoDbConfig> mongoDbConfig)
	{
		var client = new MongoClient(mongoDbConfig.Value.ConnectionString);
		var database = client.GetDatabase(mongoDbConfig.Value.Name);

		string collectionName = ((CollectionNameAttribute)Attribute
			.GetCustomAttribute(
				typeof(T),
				typeof(CollectionNameAttribute
			)
		)).Name;

		this.collection = database.GetCollection<T>(collectionName);
	}
}
