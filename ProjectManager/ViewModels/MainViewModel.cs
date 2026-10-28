using System;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProjectManager.DataTemplates;
using ProjectManager.Views;

namespace ProjectManager.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty] public partial string Greeting { get; set; } = "Welcome to Avalonia!";
    
    private  HomeViewModel _home = new ();
    [ObservableProperty]private  QaAViewModel _qaa = new();
   
    [ObservableProperty] private ViewModelBase _currentPage;

    public MainViewModel()
    {
        CurrentPage = _home;
        
    }

    [RelayCommand]
    private void GoToHome()
    {
        CurrentPage = _home;
    }



    [RelayCommand]
    private void GoToQaA()
    {
        CurrentPage = _qaa;
        Console.WriteLine("Nacetl se QaA.");
    }
    

    [RelayCommand]
    private void OpenSettingDialog(Window? parentWindow)
    {
        var dialog = new SettingsView();
        dialog.DataContext = new SettingViewModel();

        if (parentWindow != null)
        {
            dialog.ShowDialog<bool>(parentWindow);
        }
    }

    [RelayCommand]
    private void GoToProjectBoard(Project project)
    {
        CurrentPage = new ProjectBoardViewModel(project);
    }
}
