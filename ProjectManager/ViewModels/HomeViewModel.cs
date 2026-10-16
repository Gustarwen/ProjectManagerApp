using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProjectManager.DataTemplates;

namespace ProjectManager.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
    public ObservableCollection<ProjectData> Mydata { get; set; } = new();
    [ObservableProperty] private string names="Untitled";
    [ObservableProperty] private DateTimeOffset? dates;
    [ObservableProperty] private DateTime today = DateTime.Today;
    

    public HomeViewModel()
    {
        DateTime today = DateTime.Today;
    }

    [RelayCommand]
    public void DeleteProperty()
    {
    }

    [RelayCommand]
    public void Accept(int index)
    {
        Mydata[index].Finished = true;
    }
    [RelayCommand]
    public void AddProperty()
    {
        ProjectData newData = new ProjectData
        {
            Name = Names,
            Date = this.Dates,
            Index = Mydata.Count
            
        };
        if (Dates == null || newData.Name == null)
        {
            return;
            
        }
        Mydata.Add(newData);
        Names = $"Untitled{Mydata.Count + 1}";
        Dates = null;
    }
    
    
}