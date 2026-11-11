using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using ProjectManager.DataTemplates;

namespace ProjectManager.ViewModels;

public partial class connectNodesViewModel :ViewModelBase
{
    public ObservableCollection<ProjectData> Tasks{get;set;}=new();
    public ProjectData SelectedTask { get; set; }
 

    public connectNodesViewModel()
    {

    }

    [RelayCommand]
  private  void AddConnection(ProjectData toAdd)
    {
        
        Task_Task newTask = new();
        newTask.TaskId = toAdd.Id;
        newTask.TaskId2 = SelectedTask.Id;
        DataBaseController.ProjectTaskRepository.Create(newTask);
        
    }
    
    
}