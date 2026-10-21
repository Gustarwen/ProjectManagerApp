using System;
using System.Collections.ObjectModel;
using System.Linq;
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
        foreach (var u in ProjectManager.DataTemplates.DataBaseController._repository.Query().Execute())
        {
            Mydata.Add(u);
        }
        DateTime today = DateTime.Today;
    }

    

    [RelayCommand]
    public void DeleteProperty(ProjectData data)
    {
        Mydata.Remove(data);
        ProjectManager.DataTemplates.DataBaseController.DeleteProperty(data);
    }
    
    [RelayCommand]
    public void Accept(ProjectData data)
    {
        data.Finished = !data.Finished;
        var item = Mydata.FirstOrDefault(x => x.Id == data.Id);
        ProjectManager.DataTemplates.DataBaseController._repository.Update(item);
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
        ProjectManager.DataTemplates.DataBaseController.InsertProperty(newData);
        Names = $"Untitled{Mydata.Count + 1}";
        Dates = null;
    }
    
    
}