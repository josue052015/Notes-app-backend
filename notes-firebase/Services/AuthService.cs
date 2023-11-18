namespace notes_firebase.Services;
using Firebase.Database;
using Firebase.Database.Query;
using notes_firebase.Models;

public class AuthService
{
    static string firebaseDatabaseUrl = "https://notes-19150-default-rtdb.firebaseio.com/";
    static string firebaseDatabaseDocument = "User";
    static readonly HttpClient client = new HttpClient();
    public AuthService() { }

    public async Task<Boolean> UserExist(string username, string password)
    {
        try
        {
            var firebase = new FirebaseClient(firebaseDatabaseUrl);
            //TODO: resolver el error del "\" en la url
            var result = await  firebase.Child(firebaseDatabaseDocument)
                               .OrderBy("Email")
                               .EqualTo(username)
                               .OnceSingleAsync<User>();
            string documentId = result.Id.ToString();
        }
        catch(Exception ex)
        {

        }
        return false;
    }

}
