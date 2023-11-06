using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using notes_firebase.Models;
using notes_firebase.Services;

namespace notes_firebase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public  class NotesController : ControllerBase
    {
        private readonly NotesService _notesService;

        public NotesController(NotesService notesService) 
        {
            this._notesService = notesService;
        }
        [HttpGet]
        public async Task<List<Note>> GetNotes()
        {
            return await _notesService.GetNotes();
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<Note> GetNoteById(string id)
        {
            return await _notesService.GetNoteById(id);
        }
        [HttpPost]
        public async Task<Note> AddNote(Note note)
        {
            return await _notesService.AddNote(note);
        }
    }
}
