using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Tmds.DBus.Protocol;

namespace linux_desktop.ViewModels;

public partial class TranscribeViewModel : ViewModelBase
{

    [ObservableProperty] private string? _sampleName;
    [ObservableProperty] private string? _fieldMedicine;
    [ObservableProperty] private string? _transcriptionValue;
    [ObservableProperty] private string? _descriptionValue;
    [ObservableProperty] private string? _keyWords;
    [ObservableProperty] private string? _outputTranscription;

    
    public ObservableCollection<string>? MedicalField { get; } = new() { "Bariatrics", "Cardiology", "Dentistry","General Medicine", "Immunology", "Neurology", "Urology" };
    
    //Connection To Server -- Needs Manual IP And Port Assignment -- Permanent Port Has Been Set To 5867
    //Additional Command To Send To Server
    private readonly Connection _connection = new Connection("10.12.121.220", 5867);
    
    [RelayCommand]
    public void Send()
    {
        
        string payload = $$"""
                           {
                               "command": "CLASSIFY",
                               "timestamp": "{{DateTime.UtcNow:yyyy-MM-ddTHH:mm:ss}}",
                               "fields": {
                                   "Description": "{{DescriptionValue}}",
                                   "Transcription": "{{TranscriptionValue}}",
                                   "Keywords": "{{KeyWords}}"
                               }
                           }
                           """;
        
        string response = _connection.ExchangeData(payload);
        
        OutputTranscription = response;
        
    }
    
}