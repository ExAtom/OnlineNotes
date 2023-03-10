using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OnlineNotes.Data.Repositories;

namespace OnlineNotes.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
	private readonly JwtConfig jwtConfig;
	private readonly UserRepository userRepository;

	public AuthController(
		IOptions<JwtConfig> jwtConfig,
		UserRepository userRepository)
	{
		this.jwtConfig = jwtConfig.Value;
		this.userRepository = userRepository;
	}

	[HttpPost("login")]
	public async Task<ActionResult> Login([FromBody] UserDto reqUser)
	{

		var users = await userRepository.GetAllAsync();
		var user = users.Find(u =>
			u.Username == reqUser.Username &&
			u.Password == reqUser.Password
		);

		if (user is null)
			return null;

		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(new Claim[]
			{
				new Claim(ClaimTypes.Name, user.Id)
			}),
			Expires = DateTime.UtcNow.AddMinutes(jwtConfig.Expiration),
			SigningCredentials = new(
				new SymmetricSecurityKey(
					Encoding.ASCII.GetBytes(jwtConfig.Secret)
				),
				SecurityAlgorithms.HmacSha256Signature
			)
		};

		var tokenHandler = new JwtSecurityTokenHandler();
		var securityToken = tokenHandler.CreateToken(tokenDescriptor);

		string token = tokenHandler.WriteToken(securityToken);

		if (string.IsNullOrWhiteSpace(token))
			return Unauthorized();

		return Ok(new { token, user });
	}
}
