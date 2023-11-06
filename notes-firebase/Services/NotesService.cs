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

        public async Task<List<Note>> GetNotes()
        {
            string url = $"{firebaseDatabaseUrl}" +
                       $"{firebaseDatabaseDocument}.json";
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if(content != null || content != "null") {
                    return JsonSerializer.Deserialize<Dictionary<string,Note>>(content)
                        .Select(x => x.Value)
                        .Where(x => x.IsDeleted.Equals(false)).ToList();
                }
            }
            return null;
        }
        public  async Task<Note> Add([FromBody] Note note)
        {
            note.Id = Guid.NewGuid();
            note.IsDeleted = false;
            string noteJsonString = JsonSerializer.Serialize(note);
            var payload = new StringContent(noteJsonString, Encoding.UTF8, "application/json");
            string url = $"{firebaseDatabaseUrl}" +
                        $"{firebaseDatabaseDocument}/" +
                        $"{note.Id}.json";
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
