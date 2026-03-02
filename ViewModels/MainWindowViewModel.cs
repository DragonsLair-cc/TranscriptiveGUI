using CommunityToolkit.Mvvm.ComponentModel;

namespace LinuxGUI.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] private string _greeting = "Welcome to Transcriptive";

    public void ChangeMessage()
    {
        Greeting = "Button Clicked";
    }
}
