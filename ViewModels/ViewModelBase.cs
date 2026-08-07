using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace linux_desktop.ViewModels;

public abstract class ViewModelBase : ObservableObject
{
    public virtual void Dispose() { GC.SuppressFinalize(this); }
}
