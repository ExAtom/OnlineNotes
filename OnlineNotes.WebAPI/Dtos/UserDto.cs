using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace OnlineNotes.WebAPI;

public class UserDto
{
	[Required, JsonProperty("username")]
	public string Username { get; init; } = string.Empty;

	[Required, JsonProperty("password")]
	public string Password { get; init; } = string.Empty;

	public override string ToString()
	{
		return JsonConvert.SerializeObject(this);
	}
}
