namespace notes_firebase.Services;

using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using notes_firebase.Models;
using System.Security.Cryptography;
using System.Text.Json;

public class AuthService
{
    static string firebaseDatabaseUrl = "https://notes-19150-default-rtdb.firebaseio.com/";
    static string firebaseDatabaseDocument = "User";
    static readonly HttpClient client = new HttpClient();
    public AuthService() { }

    public async Task<string> FindUser(string username, string password)
    {
        try
        {
            var firebase = new FirebaseClient(firebaseDatabaseUrl);
            var userDictionary = await firebase.Child(firebaseDatabaseDocument)
                               .OrderBy("Email")
                               .EqualTo(username)
                               .OnceSingleAsync<Dictionary<string, User>>();
            if (userDictionary.Count() == 0) return null;
            string key = userDictionary.Keys.First();
            User user = userDictionary[key];
            if(user.Email.Equals(username) && user.Password.Equals(password)) return user.Id.ToString();
        }
        catch (Exception ex)
        {

        }
        return null;
    }

    public string GenerateRandomKey()
    {
        // Adjust the key size based on your security requirements
        int keySizeInBytes = 32; // 256 bits
        byte[] keyBytes = new byte[keySizeInBytes];

        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(keyBytes);
        }

        // Convert the byte array to a base64-encoded string
        string base64Key = Convert.ToBase64String(keyBytes);

        return base64Key;
    }

}
