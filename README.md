# Transcriptive GUI - Linux
Here my aim is to bring the Transcriptive AI's GUI to Linux. The current build is only supported on Windows, which this repo aims to fix.
# Structure
```
LinuxGUI
|   Models - Empty for now
|   |   Empty
|   View -- These are design files for how each tab looks
|   |   HomeView.axaml
|   |   HomeView.axaml.cs
|   |   MainWindow.axaml
|   |   MainWindow.axaml.cs
|   |   SettingsView.axaml
|   |   SettingsView.axaml.cs
|   |   TranscriptiveView.axaml
|   |   TranscriptiveView.axaml.cs
|   |   UploadView.axaml
|   |   UploadView.axaml.cs
|   ViewModels -- Contains extra logic for a tab if needed
|   |   HomeViewModel.cs
|   |   MainWindowsViewModel.cs
|   |   SettingsViewModel.cs
|   |   TranscriptiveViewModel.cs
|   |   UploadViewModel.cs
|   |   ViewModelBase.cs
|
|   Program.cs
|   App.axaml
|   App.axaml.cs
```
