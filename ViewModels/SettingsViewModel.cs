using Avalonia;
using Avalonia.Styling;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace linux_desktop.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    [ObservableProperty] private bool _themeToggle = false;
    
    [RelayCommand] partial void OnThemeToggleChanged(bool value)
    {
        if (Application.Current is { } app)
        {
            app.RequestedThemeVariant = value ? ThemeVariant.Light : ThemeVariant.Dark;
        }
    }
    
}