using Microsoft.AspNetCore.Mvc;
using notes_firebase.Models;
using System.Text;
using System.Text.Json;

namespace notes_firebase.Services
{
    public class NotesService
    {
        static string firebaseDatabaseUrl = "https://notes-19150-default-rtdb.firebaseio.com/";
        static string firebaseDatabaseDocument = "Notes";
        static readonly HttpClient client = new HttpClient();

        public async Task<Note> GetNotes()
        {

        }
        public  async Task<Note> Add([FromBody] Note note)
        {
            note.NoteId = Guid.NewGuid();
            string noteJsonString = JsonSerializer.Serialize(note);
            var payload = new StringContent(noteJsonString, Encoding.UTF8, "application/json");
            string url = $"{firebaseDatabaseUrl}" +
                        $"{firebaseDatabaseDocument}/" +
                        $"{note.NoteId}.json";
            var httpResponse = await client.PutAsync(url, payload);
            if (httpResponse.IsSuccessStatusCode)
            {
                var content = await httpResponse.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Note>(content);
            }
            return null;
        }
    }
}
