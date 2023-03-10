using Microsoft.AspNetCore.Mvc;
using OnlineNotes.Data.Models;
using OnlineNotes.Data.Repositories;

namespace OnlineNotes.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
	private readonly UserRepository userRepository;

	public UserController(UserRepository userRepository) =>
		this.userRepository = userRepository;

	[HttpGet]
	public async Task<ActionResult<List<UserModel>>> GetAll() =>
		await this.userRepository.GetAllAsync();

	[HttpGet("{id}")]
	public async Task<ActionResult<UserModel>> GetById(string id)
	{
		var user = await this.userRepository.GetByIdAsync(id);

		if (user is null)
			return NotFound();

		return user;
	}

	[HttpPost]
	public async Task<ActionResult<UserModel>> Create(UserDto reqUser)
	{
		var user = await this.userRepository.CreateAsync(
			new()
			{
				Password = reqUser.Password,
				Username = reqUser.Username
			}
		);

		return Created($"http://localhost:5146/api/User/{user.Id}", user);
	}
}
