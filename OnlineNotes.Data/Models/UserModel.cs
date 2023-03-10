using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;

namespace OnlineNotes.Data.Models;

[CollectionName("users")]
public class UserModel
{
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string Id { get; init; }

	[Required]
	[BsonElement("username")]
	public string Username { get; init; }

	[Required]
	[BsonElement("password")]
	public string Password { get; init; }
}
