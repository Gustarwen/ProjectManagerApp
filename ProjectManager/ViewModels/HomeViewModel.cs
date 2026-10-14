using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProjectManager.DataTemplates;

namespace ProjectManager.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
    public ObservableCollection<ProjectData> Mydata { get; set; } = new();
    [ObservableProperty] private string names;
    [ObservableProperty] private DateTimeOffset? dates;
    [RelayCommand]
    public void AddProperty()
    {
        ProjectData newData = new ProjectData
        {
            Name = Names,
            Date = this.Dates,
            
        };
        Mydata.Add(newData);
    }
    
    
}