using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OnlineNotes.Data.Models;

namespace OnlineNotes.Data.Repositories;

public class NoteRepository : RepositoryBase<NoteModel>
{
	public NoteRepository(IOptions<MongoDbConfig> mongoDbConfig) : base(mongoDbConfig) { }

	public async Task<NoteModel> GetByIdAsync(string id) =>
		await collection.Find(note => note.Id == id).FirstOrDefaultAsync();

	public async Task<List<NoteModel>> GetAllByOwnerAsync(string owner) =>
		await collection.Find(note => note.OwnerId == owner).ToListAsync();

	public async Task<NoteModel> CreateAsync(NoteModel note)
	{
		await collection.InsertOneAsync(note);
		return note;
	}

	public async Task UpdateAsync(NoteModel note) =>
		await collection.ReplaceOneAsync(n => n.Id == note.Id, note);

	public async Task DeleteAsync(string id) =>
		await collection.DeleteOneAsync(note => note.Id == id);
}
