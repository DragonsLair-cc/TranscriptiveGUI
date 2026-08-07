using System;
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
    
    [RelayCommand]
    private void Transcribe()
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

    }
    
    public record TranscriptionMessage(string Transcription);
    
}