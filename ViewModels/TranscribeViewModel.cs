using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using linux_desktop.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace linux_desktop.ViewModels;

public partial class TranscribeViewModel : ViewModelBase
{
    [ObservableProperty] public partial string? PatientDescription {get; set;}
    [ObservableProperty] public partial string? PatientReport {get; set;}
    [ObservableProperty] public partial string? KeyWords {get; set;}
    [ObservableProperty] public partial string? TranscriptiveOutput {get; set;}
    [ObservableProperty] private User _currentUser;

    public TranscribeViewModel(User currentUser)
    {
        _currentUser = currentUser;
    }
    
    [RelayCommand]
    private async Task Transcribe()
    {
        
        //I really need to get back into the command handler and undo most of the rushed logic from the night before the live demo
        //We shouldn't need to use MedicalSpecialty, SampleName, or p_first/lastname for CLASSIFY
        var payload = $$"""
                        {
                            "command": "CLASSIFY",
                            "timestamp": "{{DateTime.UtcNow:yyyy-MM-ddTHH:mm:ss}}",
                            "fields": {
                                "Description": "{{PatientDescription}}",
                                "Transcription": "{{PatientReport}}",
                                "Keywords": "{{KeyWords}}",
                                "MedicalSpecialty": "",
                                "SampleName": "",
                                "p_firstname": "",
                                "p_lastname": "",
                                "confidence": ""
                            }
                        }
                        """;

        var connection = new Connection("127.0.0.1", 5867);
        TranscriptiveOutput = connection.ExchangeData(payload);
        var transcriptionHistory = PatientDescription + "\n" + PatientReport + "\n" + TranscriptiveOutput;
        WeakReferenceMessenger.Default.Send(new TranscriptionMessage(transcriptionHistory));
        CurrentUser.LastTranscription = PatientReport;
        CurrentUser.TranscriptionCount = CurrentUser.TranscriptionCount + 1;
        await UpdateFirestoreAsync();
    }

    [RelayCommand]
    private async Task UpdateFirestoreAsync()
    {
        using HttpClient httpClient = new();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CurrentUser.IdToken);
        
        var fields = new
        {
            fields = new Dictionary<string, object>
            {
                { "transcriptionCount", new { integerValue = CurrentUser.TranscriptionCount } },
                { "lastTranscription", new { stringValue = CurrentUser.LastTranscription } }
            }
        };

        string jsonBody = JsonSerializer.Serialize(fields);
        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
        
        string url = $"https://firestore.googleapis.com/v1/projects/transcriptive-ai/databases/(default)/documents/users/{CurrentUser.Uid}" + 
                     $"?updateMask.fieldPaths=transcriptionCount" + 
                     $"&updateMask.fieldPaths=lastTranscription";
        
        try
        {
            HttpResponseMessage response = await httpClient.PatchAsync(url, content);
            
            if (!response.IsSuccessStatusCode)
            {
                string errorResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Firestore Stats Update Failed ({response.StatusCode}): {errorResponse}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception during Firestore update: {ex.Message}");
        }
        
    }
    
    public record TranscriptionMessage(string Transcription);
    
}