using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineNotes.Data.Models;
using OnlineNotes.Data.Repositories;

namespace OnlineNotes.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class NoteController : ControllerBase
{
	private readonly NoteRepository noteRepository;
	private readonly UserRepository userRepository;

	public NoteController(NoteRepository noteRepository, UserRepository userRepository)
	{
		this.noteRepository = noteRepository;
		this.userRepository = userRepository;
	}

	[AllowAnonymous]
	[HttpGet]
	public async Task<ActionResult<List<NoteModel>>> GetAllByOwner(string owner)
	{
		string userId = User.Identity.Name;

		var user = userId is null ?
			new() :
			await userRepository.GetByIdAsync(userId);

		var notes = await this.noteRepository.GetAllByOwnerAsync(owner);
		return notes.Where(n => n.OwnerId == user.Id || n.IsPublic).ToList();
	}

	[AllowAnonymous]
	[HttpGet("{id}")]
	public async Task<ActionResult<NoteModel>> GetById(string id)
	{
		string userId = User.Identity.Name;

		var user = userId is null ?
			new() :
			await userRepository.GetByIdAsync(userId);
		var note = await this.noteRepository.GetByIdAsync(id);

		if (!note.IsPublic && user.Id != note.OwnerId)
			return Unauthorized();

		if (note is null)
			return NotFound();

		return note;
	}

	[HttpPost]
	public async Task<ActionResult<NoteModel>> Create([FromBody] NoteDto reqNote)
	{
		var user = await userRepository.GetByIdAsync(User.Identity.Name);

		var note = await this.noteRepository.CreateAsync(
			new()
			{
				Title = reqNote.Title,
				Content = reqNote.Content,
				OwnerId = user.Id,
				IsPublic = reqNote.IsPublic,
			}
		);

		return Created($"http://localhost:5146/api/Note/{note.Id}", note);
	}

	[HttpPut]
	public async Task<IActionResult> Update([FromBody] NoteModel note)
	{
		var user = await userRepository.GetByIdAsync(User.Identity.Name);

		if (user.Id != note.OwnerId)
			return Unauthorized();

		await this.noteRepository.UpdateAsync(note);

		return Ok();
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(string id)
	{
		var user = await userRepository.GetByIdAsync(User.Identity.Name);
		var note = await noteRepository.GetByIdAsync(id);

		if (user.Id != note.OwnerId)
			return Unauthorized();

		await this.noteRepository.DeleteAsync(id);

		return Ok();
	}
}
