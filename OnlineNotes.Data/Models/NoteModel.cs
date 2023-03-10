using System.ComponentModel.DataAnnotations;
using MongoDbGenericRepository.Attributes;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace OnlineNotes.Data.Models;

[CollectionName("notes")]
public class NoteModel
{
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string Id { get; init; }

	[Required]
	[BsonElement("title")]
	public string Title { get; init; }

	[BsonElement("content")]
	public string Content { get; init; }

	[BsonElement("owner")]
	public string OwnerId { get; init; }

	[BsonElement("isPublic")]
	public bool IsPublic { get; init; }
}
