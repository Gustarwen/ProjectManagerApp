using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ProjectManager.ViewModels;

public partial class SettingViewModel:ViewModelBase
{
    [ObservableProperty] private string buttonContext = "buttonContext";
    [RelayCommand] 
    public void OnPressButton1()
    {
        ButtonContext = "Benjamin Button";
    }
}
