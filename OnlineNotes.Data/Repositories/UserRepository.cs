using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OnlineNotes.Data.Models;

namespace OnlineNotes.Data.Repositories;

public class UserRepository : RepositoryBase<UserModel>
{
	public UserRepository(IOptions<MongoDbConfig> mongoDbConfig) : base(mongoDbConfig) { }

	public async Task<List<UserModel>> GetAllAsync() =>
		await collection.Find(u => true).ToListAsync();

	public async Task<UserModel> GetByIdAsync(string id) =>
		await collection.Find(u => u.Id == id).FirstOrDefaultAsync();

	public async Task<UserModel> CreateAsync(UserModel user)
	{
		await collection.InsertOneAsync(user);
		return user;
	}
}
