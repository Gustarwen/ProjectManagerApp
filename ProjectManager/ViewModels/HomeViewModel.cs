using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProjectManager.DataTemplates;

namespace ProjectManager.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
    public ObservableCollection<Project> Mydata { get; set; } = new();
    [ObservableProperty] private string names="Untitled";
    [ObservableProperty] private DateTimeOffset? dates;
    [ObservableProperty] private DateTime today = DateTime.Today;
    

    public HomeViewModel()
    {
        foreach (var u in ProjectManager.DataTemplates.DataBaseController._projectrepositroy.Query().Execute())
        {
            u.tasks = new List<ProjectData>();
            foreach (var v in ProjectManager.DataTemplates.DataBaseController._taskrepository.Query().Execute())
            {
                
                u.tasks.Add(v);
            }
            Mydata.Add(u);
            
        }
        DateTime today = DateTime.Today;
    }

    

    [RelayCommand]
    public void DeleteProperty(Project data)
    {
        Mydata.Remove(data);
        ProjectManager.DataTemplates.DataBaseController.DeleteProject(data);
    }
    
    [RelayCommand]
    public void Accept(Project data)
    {
        data.Finished =1;
        var item = Mydata.FirstOrDefault(x => x.Id == data.Id);
        ProjectManager.DataTemplates.DataBaseController._projectrepositroy.Update(item);
    }
    
    

    
    [RelayCommand]
    public void AddProperty()
    {
        Project newData = new Project
        {
            Name = Names,
            Date = this.Dates,
         
            
        };
        if (Dates == null || newData.Name == null)
        {
            return;
            
        }
        Mydata.Add(newData);
        ProjectManager.DataTemplates.DataBaseController.InsertProject(newData);
        Names = $"Untitled{Mydata.Count + 1}";
        Dates = null;
    }
    
    
}