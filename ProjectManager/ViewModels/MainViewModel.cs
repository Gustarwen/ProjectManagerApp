using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ProjectManager.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty] public partial string Greeting { get; set; } = "Welcome to Avalonia!";
    
    private readonly HomeViewModel _home = new();
    private readonly QaAViewModel _QaA = new();
    [ObservableProperty] private ViewModelBase currentPage;

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
        CurrentPage = _QaA;
        Console.WriteLine("Nacetl se QaA.");
    }
}
