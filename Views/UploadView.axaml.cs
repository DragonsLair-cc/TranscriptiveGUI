using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;

namespace linux_desktop.Views;

public partial class UploadView : UserControl
{
    public UploadView()
    {
        InitializeComponent();
    }
    
    private async void OpenFileButton_Clicked(object sender, RoutedEventArgs args)
    {
        
        var topLevel = TopLevel.GetTopLevel(this);
        
        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open Text File",
            AllowMultiple = false
        });

        if (files.Count >= 1)
        {
            await using var stream = await files[0].OpenReadAsync();
            using var streamReader = new StreamReader(stream);

            var fileContent = await streamReader.ReadToEndAsync();
        }
    }
    
}