using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

namespace linux_desktop.ViewModels;

public partial class HistoryViewModel : ViewModelBase, IRecipient<TranscribeViewModel.TranscriptionMessage>
{
    public ObservableCollection<string> TranscriptionHistory { get; } = new();

    public HistoryViewModel()
    {
        WeakReferenceMessenger.Default.Register(this);
    }

    public void Receive(TranscribeViewModel.TranscriptionMessage message)
    {
        if (!string.IsNullOrWhiteSpace(message.Transcription))
        {
            TranscriptionHistory.Add(message.Transcription);
        }
    }
    
}