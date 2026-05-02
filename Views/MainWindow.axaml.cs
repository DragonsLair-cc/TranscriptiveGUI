using System;
using Avalonia.Controls;
using Avalonia.Input;

namespace linux_desktop.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void WindowDrag(object? sender, PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            this.BeginMoveDrag(e);
        }
    }

    private void WindowClose(object? sender, PointerPressedEventArgs e)
    {
        if (!e.GetCurrentPoint(this).Properties.IsLeftButtonPressed) return;
        
        Console.WriteLine("Clicked");
        Environment.Exit(0);
        
    }

    private void WindowMinimize(object? sender, PointerPressedEventArgs e)
    {
        if (!e.GetCurrentPoint(this).Properties.IsLeftButtonPressed) return;
        
        Console.WriteLine("Clicked");
        this.WindowState = WindowState.Minimized;
        
    }
    
}