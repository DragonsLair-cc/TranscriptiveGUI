using CommunityToolkit.Mvvm.ComponentModel;
using linux_desktop.Models;

namespace linux_desktop.ViewModels;

public partial class DashboardViewModel : ViewModelBase
{
    [ObservableProperty] private User _currentUser;
    [ObservableProperty] private string _userNameMessage;
    [ObservableProperty] private string _userJob;
    
    public DashboardViewModel(User currentUser)
    {
        _currentUser = currentUser;
        _userNameMessage = "Currently logged in as:\n" + CurrentUser.Name;
        
        if (_currentUser.WorkPosition == false)
        {
            UserJob = "Doctor";
        }
        else
        {
            UserJob = "Administrator";
        }
        
    }
    
    
}