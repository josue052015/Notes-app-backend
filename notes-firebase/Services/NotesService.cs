using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using notes_firebase.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace notes_firebase.Services
{
    public class NotesService
    {
        static string firebaseDatabaseUrl = "https://notes-19150-default-rtdb.firebaseio.com/";
        static string firebaseDatabaseDocument = "Notes";
        static readonly HttpClient client = new HttpClient();

        public async Task<List<Note>> GetNotes(HttpContext httpContext)
        {
            string jwtToken = httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(jwtToken) as JwtSecurityToken;

            if (jsonToken != null && jwtToken != null)
            {
               
                var userIdClaim = jsonToken.Claims.FirstOrDefault(claim => claim.Type == "nameid");
                var firebase = new FirebaseClient(firebaseDatabaseUrl);
                var noteDictionary = await firebase.Child(firebaseDatabaseDocument)
                                   .OrderBy("UserId")
                                   .EqualTo(userIdClaim.Value.ToString())
                                   .OnceSingleAsync<Dictionary<string, Note>>();
                return noteDictionary.Values.ToList();
            }
           
            return null;
        }
        public async Task<Note> GetNoteById(string id)
        {
            string url = $"{firebaseDatabaseUrl}" +
                       $"{firebaseDatabaseDocument}/" + 
                       $"{id}.json";

            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (content != null || content != "null")
                {
                    return JsonSerializer.Deserialize<Note>(content);  
                }
            }
            return null;
        }
        public async Task<List<Note>> GetRecicleBin()
        {
            string url = $"{firebaseDatabaseUrl}" +
                       $"{firebaseDatabaseDocument}.json";
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (content != null || content != "null")
                {
                    return JsonSerializer.Deserialize<Dictionary<string, Note>>(content)
                        .Select(x => x.Value)
                        .Where(x => x.IsDeleted.Equals(true)).ToList();
                }
            }
            return null;
        }
        public  async Task<Note> AddNote(Note note)
        {
            note.Id = Guid.NewGuid();
            note.IsDeleted = false;
            note.DateModified = DateTime.UtcNow;
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
        public async Task<Note> EditNote(Note note)
        {
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
        public async Task<Note> MoveToRecycleBin(string id)
        {
            var note = await GetNoteById(id);
            note.IsDeleted = true;
            return await EditNote(note);   
        } 

        public async Task<string> DeleteNote(string id)
        {
            string url = $"{firebaseDatabaseUrl}" +
                     $"{firebaseDatabaseDocument}/" +
                     $"{id}.json";
            var httpResponse = await client.DeleteAsync(url);

            return await httpResponse.Content.ReadAsStringAsync();
        }
    }
}
