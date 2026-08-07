using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using linux_desktop.Models;

namespace linux_desktop.ViewModels;

public partial class NavigationViewModel : ViewModelBase
{
    //Basic Initial State
    [ObservableProperty] private bool _paneState;
    [ObservableProperty] public partial ObservableObject CurrentView { get; set; } = new LoginViewModel();
    
    [ObservableProperty] private User _currentUser;
    
    //I only did this for settings to keep toggle states on view switching - I should really just implement a settings file though
    private readonly SettingsViewModel _settingsView = new();

    public NavigationViewModel()
    {
        CurrentView = new DashboardViewModel(_currentUser);
    }
    
    //Button Commands
    [RelayCommand] private void PaneToggle() => PaneState = !PaneState;
    
    [RelayCommand] private void ToDash() => CurrentView = new DashboardViewModel(_currentUser);
    [RelayCommand] private void ToTranscribe() => CurrentView = new TranscribeViewModel();
    [RelayCommand] private void ToTraining() => CurrentView = new TrainingViewModel();
    [RelayCommand] private void ToUpload() => CurrentView = new UploadViewModel();
    [RelayCommand] private void ToHistory() => CurrentView = new HistoryViewModel();
    [RelayCommand] private void ToSettings() => CurrentView = _settingsView;

}