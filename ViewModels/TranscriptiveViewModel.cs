using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace LinuxGUI.ViewModels;

public partial class TranscriptiveViewModel : ViewModelBase
{
    // This is the code for the {Binding SampleName} - it just generates the public property
    [ObservableProperty]
    private string? _sampleName;

    public ObservableCollection<string> Options { get; } = new() { "Cardiology", "Neurology", "General Medicine" };

    [ObservableProperty]
    private string? _mediField;

    public void saveField()
    {
        System.Diagnostics.Debug.WriteLine($"Saved: {MediField}");
    }
}