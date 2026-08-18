using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
using Firebase.Auth.Providers;
using System.Text.Json;

namespace linux_desktop.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    private readonly FirebaseAuthConfig _config = new FirebaseAuthConfig
    {
        ApiKey = "AIzaSyCdCCeFFwSYFYwD6STaWEGsqdnLkYwtrE0",
        AuthDomain = "transcriptive-ai.firebaseapp.com",
        Providers = new FirebaseAuthProvider[]
        {
            new EmailProvider(),
        }
    };

    [ObservableProperty] private string? _email;
    [ObservableProperty] private string? _password;
    [ObservableProperty] private string? _errorMessage;

    private readonly FirebaseAuthClient _client;

    public Action<Models.User>? OnLoginSuccess { get; set; }

    public LoginViewModel()
    {
        _client = new FirebaseAuthClient(_config);
    }

    [RelayCommand]
    private async Task Login()
    {

        if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            return;
        
        ErrorMessage = string.Empty;

        try
        {
            UserCredential userCredential = await _client.SignInWithEmailAndPasswordAsync(Email, Password);
            string uid = userCredential.User.Uid;
            string idToken = await userCredential.User.GetIdTokenAsync();
            
            Models.User? loggedInUser = await FetchUserDataAsync(uid, idToken);

            if (loggedInUser == null)
            {
                ErrorMessage = "User has no Firestore documents";
                return;
            }
            
            OnLoginSuccess?.Invoke(loggedInUser);

        }
        catch (Exception ex)
        {
            ErrorMessage = $"Login Failed: {ex.Message}";
        }
        
    }

    private async Task<Models.User?> FetchUserDataAsync(string uid, string idToken)
    {
        using HttpClient httpClient = new HttpClient();

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", idToken);
        
        string url = $"https://firestore.googleapis.com/v1/projects/transcriptive-ai/databases/(default)/documents/users/{uid}";
        HttpResponseMessage response = await httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
            return null;

        string jsonResponse = await response.Content.ReadAsStringAsync();

        using JsonDocument jsonDocument = JsonDocument.Parse(jsonResponse);
        if (!jsonDocument.RootElement.TryGetProperty("fields", out JsonElement fields))
            return null;

        return new Models.User
        {
            Uid = uid,
            IdToken = idToken,
            Email = GetFirestoreString(fields,"email"),
            Name = GetFirestoreString(fields, "FirstnameLastname"),
            WorkPosition = GetFirestoreBoolean(fields, "admin"),
            TranscriptionCount = GetFirestoreInt(fields, "transcriptionCount"),
            LastTranscription =  GetFirestoreString(fields, "lastTranscription"),
        };
    }

    private static string? GetFirestoreString(JsonElement fields, string propertyName)
    {
        if (fields.TryGetProperty(propertyName, out var prop) && prop.TryGetProperty("stringValue", out var stringVal))
            return stringVal.GetString();

        return null;
    }

    private static bool? GetFirestoreBoolean(JsonElement fields, string propertyName)
    {
        if (fields.TryGetProperty(propertyName, out JsonElement prop) && prop.TryGetProperty("booleanValue", out JsonElement booleanVal))
            return booleanVal.GetBoolean();

        return null;
    }

    private static int? GetFirestoreInt(JsonElement fields, string propertyName)
    {
        if (fields.TryGetProperty(propertyName, out JsonElement prop) && prop.TryGetProperty("integerValue", out JsonElement intVal))
        {
            if (intVal.ValueKind == JsonValueKind.String && int.TryParse(intVal.GetString(), out int result))
                return result;
            if (intVal.ValueKind == JsonValueKind.Number)
                return intVal.GetInt32();
        }
        return null;
    }
    
}