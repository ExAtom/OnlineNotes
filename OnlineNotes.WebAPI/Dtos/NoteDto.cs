using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace OnlineNotes.WebAPI;

public class NoteDto
{
	[Required, JsonProperty("title")]
	public string Title { get; init; } = string.Empty;

	[JsonProperty("content")]
	public string Content { get; init; } = string.Empty;

	[JsonProperty("isPublic")]
	public bool IsPublic { get; init; }

	public override string ToString()
	{
		return JsonConvert.SerializeObject(this);
	}
}
