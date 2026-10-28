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
    [ObservableProperty] private string _names="Untitled";
    [ObservableProperty] private DateTimeOffset? _dates;
    [ObservableProperty] private DateTime _today = DateTime.Today;
    

    public HomeViewModel()
    {
        foreach (var u in DataBaseController.Projectrepositroy.Query().Execute())
        {
            u.Tasks = new List<ProjectData>();
            foreach (var v in DataBaseController.Taskrepository.Query().Execute())
            {
                
                u.Tasks.Add(v);
            }
            Mydata.Add(u);
            
        }
     
    }

    

    [RelayCommand]
    public void DeleteProperty(Project data)
    {
        Mydata.Remove(data);
       DataBaseController.DeleteProject(data);
    }
    
    [RelayCommand]
    public void Accept(Project data)
    {
        data.Finished =!data.Finished;
        var item = Mydata.FirstOrDefault(x => x.Id == data.Id);
        if (item != null)
        {
            DataBaseController.Projectrepositroy.Update(item);
        }
    }
    
    

    
    [RelayCommand]
    public void AddProperty()
    {
        Project newData = new Project
        {
            Name = Names,
            Date = this.Dates,
         
            
        };
        if (Dates == null)
        {
            return;
            
        }
        Mydata.Add(newData);
        newData=DataBaseController.InsertProject(newData);
        ProjectData NewTask = new ProjectData();
        NewTask.ProjectId = newData.Id;
        NewTask.Name = newData.Name;
        NewTask.Date = newData.Date;
        DataBaseController.InsertTask(NewTask);
        Names = $"Untitled{Mydata.Count + 1}";
        Dates = null;
    }
    
    
}