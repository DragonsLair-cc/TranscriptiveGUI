using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
using Firebase.Auth.Providers;

namespace linux_desktop.ViewModels;

public partial class RegisterViewModel : ViewModelBase
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
    [ObservableProperty] private string? _confirmPassword;
    [ObservableProperty] private string? _fullName;
    [ObservableProperty] private bool _isAdmin;
    [ObservableProperty] private string? _errorMessage;

    private readonly FirebaseAuthClient _client;
    
    public Action OnRegisterSuccess { get; set; }
    
    public Action? OnNavigateToLogin { get; set; }

    public RegisterViewModel()
    {
        _client = new FirebaseAuthClient(_config);
    }

    [RelayCommand]
    private async Task Register()
    {
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(FullName))
        {
            ErrorMessage = "Please fill in all required fields";
            return;
        }
        if (Password != ConfirmPassword)
        {
            ErrorMessage = "Please ensure that passwords match";
            return;
        }
        
        ErrorMessage = string.Empty;

        try
        {
            //This is part 1 - where we create the firebase user
            UserCredential newUserCredential = await _client.CreateUserWithEmailAndPasswordAsync(Email, Password);
            string uid = newUserCredential.User.Uid;
            string idToken = await newUserCredential.User.GetIdTokenAsync();
            
            //This is part 2 - now we create the firestore database entries for user informatio tied to the Uid of the firebase user
            bool newProfile = await CreateFirestoreUserProfileAsync(uid, idToken, Email, FullName, IsAdmin);

            if (!newProfile)
            {
                ErrorMessage = "Firebase authentication succeeded, but Firestore documents were not generated";
                return;
            }
            
            OnRegisterSuccess?.Invoke();
            
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Registration Failed: {ex.Message}";
        }
    }

    [RelayCommand]
    private void NavigateToLogin()
    {
        OnNavigateToLogin?.Invoke();
    }

    private async Task<bool> CreateFirestoreUserProfileAsync(string uid, string idToken, string email, string name, bool admin)
    {
        using HttpClient httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", idToken);
        
        var fields = new
        {
            fields = new Dictionary<string, object>
            {
                { "email", new { stringValue = email } },
                { "FirstnameLastname", new { stringValue = name } },
                { "admin", new {booleanValue = admin} }
            }
        };

        string jsonBody = JsonSerializer.Serialize(fields);
        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
        
        string url = $"https://firestore.googleapis.com/v1/projects/transcriptive-ai/databases/(default)/documents/users/{uid}" + $"?updateMask.fieldPaths=email" + $"&updateMask.fieldPaths=FirstnameLastname" + $"&updateMask.fieldPaths=admin";

        HttpResponseMessage response = await httpClient.PatchAsync(url, content);

        if (!response.IsSuccessStatusCode)
        {
            string errorResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Firestore Patch Failed ({response.StatusCode}): {errorResponse}");
        }
        
        return response.IsSuccessStatusCode;
    }
}