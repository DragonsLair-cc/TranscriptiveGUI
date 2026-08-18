using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using linux_desktop.Models;
using linux_desktop.Views;

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
            CurrentView = new NavigationViewModel(CurrentUser);
        };
        
        registerVM.OnRegisterSuccess = () =>
        {
            CurrentView = LoginVM;
        };
        
        registerVM.OnNavigateToLogin = () => CurrentView = LoginVM;
        
        _currentView = LoginVM;
    }
    
}
