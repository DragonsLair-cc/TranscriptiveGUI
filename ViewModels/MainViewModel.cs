using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using linux_desktop.Models;

namespace linux_desktop.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    //Basic Initial State
    [ObservableProperty] private bool _paneState;
    [ObservableProperty] private ViewModelBase _currentView;
    
    [ObservableProperty] private User? _currentUser;
    
    //I only did this for settings to keep toggle states on view switching - I should really just implement a settings file though
    private readonly SettingsViewModel _settingsView = new();
    private readonly HistoryViewModel _historyView = new();

    public MainViewModel()
    {
        var LoginVM = new LoginViewModel();
        var registerVM = new RegisterViewModel();

        LoginVM.OnLoginSuccess = (user) =>
        {
            CurrentUser = user;
            CurrentView = new DashboardViewModel(_currentUser);
        };
        
        registerVM.OnRegisterSuccess = () =>
        {
            CurrentView = LoginVM;
        };
        
        registerVM.OnNavigateToLogin = () => CurrentView = LoginVM;
        
        _currentView = LoginVM;
    }
    
    //Button Commands
    [RelayCommand] private void PaneToggle() => PaneState = !PaneState;
    
    [RelayCommand] private void ToDash() => CurrentView = new DashboardViewModel(_currentUser);
    [RelayCommand] private void ToTranscribe() => CurrentView = new TranscribeViewModel();
    [RelayCommand] private void ToTraining() => CurrentView = new TrainingViewModel();
    [RelayCommand] private void ToUpload() => CurrentView = new UploadViewModel();
    [RelayCommand] private void ToHistory() => CurrentView = _historyView;
    [RelayCommand] private void ToSettings() => CurrentView = _settingsView;
    
}
