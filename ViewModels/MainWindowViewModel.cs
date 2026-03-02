using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace LinuxGUI.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase _currentPage;

    public MainWindowViewModel()
    {
        _currentPage = new HomeViewModel();
    }

    public void ToHome()
    {
        CurrentPage = new HomeViewModel();
    }
    [RelayCommand]
    public void ToSettings()
    {
        CurrentPage = new SettingsViewModel();
    }
}
