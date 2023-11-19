using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using notes_firebase.Models;
using notes_firebase.Services;

namespace notes_firebase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
            return await _notesService.GetNotes(HttpContext);
        }
        [HttpGet]
        [Route("GetRecicleBin")]
        public async Task<List<Note>> GetRecicleBin()
        {
            return await _notesService.GetRecicleBin();
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<Note> GetNoteById(string id)
        {
            return await _notesService.GetNoteById(id);
        }
        [HttpPost]
        [Route("AddNote")]
        public async Task<Note> AddNote([FromBody] Note note)
        {
            return await _notesService.AddNote(note);
        }
        [HttpPut]
        public async Task<Note> EditNote([FromBody] Note note)
        {
            return await _notesService.EditNote(note);
        }
        [HttpPost]
        [Route("MoveToRecicleBin")]
        public async Task<Note> MoveToRecicleBin(string id)
        {
            return await _notesService.MoveToRecycleBin(id);
        }
        [HttpDelete]
        public async Task<string> DeleteNote(string id)
        {
            return await _notesService.DeleteNote(id);
        }
    }
}
