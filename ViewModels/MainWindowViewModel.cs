using CommunityToolkit.Mvvm.ComponentModel;

namespace LinuxGUI.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase _currentPage;

    public MainWindowViewModel()
    {
        _currentPage = new HomeViewModel();
    }

    public void GoToSettings()
    {
        //New thing
    }
}
