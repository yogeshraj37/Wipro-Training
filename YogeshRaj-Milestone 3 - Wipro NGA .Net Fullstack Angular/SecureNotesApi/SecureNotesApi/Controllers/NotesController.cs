using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureNotesApi.Data;
using SecureNotesApi.Models;
using System.Security.Claims;

namespace SecureNotesApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/notes")]
    public class NotesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public NotesController(
            ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddNote(
            Note note)
        {
            note.UserId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            _context.Notes.Add(note);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message =
                    "Note added successfully.",
                noteId = note.Id
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetNotes()
        {
            var userId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            var notes =
                await _context.Notes
                .Where(x => x.UserId == userId)
                .ToListAsync();

            return Ok(notes);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNote(
            int id,
            Note updatedNote)
        {
            var userId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            var note =
                await _context.Notes
                .FirstOrDefaultAsync(
                    x => x.Id == id &&
                    x.UserId == userId);

            if (note == null)
                return NotFound();

            note.Title = updatedNote.Title;
            note.Content = updatedNote.Content;

            await _context.SaveChangesAsync();

            return Ok("Updated");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote(
            int id)
        {
            var userId =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

            var note =
                await _context.Notes
                .FirstOrDefaultAsync(
                    x => x.Id == id &&
                    x.UserId == userId);

            if (note == null)
                return NotFound();

            _context.Notes.Remove(note);

            await _context.SaveChangesAsync();

            return Ok("Deleted");
        }
    }
}