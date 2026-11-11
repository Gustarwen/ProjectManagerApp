using System;
using System.Linq;
using Avalonia.Controls;
using ProjectManager.DataTemplates;
using ProjectManager.Views;

namespace ProjectManager.ViewModels;

public partial class CanvasNodesViewModel:ViewModelBase
{
    public Project Project;
    public  CanvasNodesViewModel(Project selectedProject)
    {
        Project = selectedProject;
    }

    public void showAddDialog(CanvasNodesView nodesview,Window root,ProjectData selectedProject)
    {
        Window dialog = new NodConfiguration(selectedProject,nodesview);
        dialog.DataContext = this;
        dialog.ShowDialog(root);
        
    }
    public void showConnectionDialog(Window root,ProjectData selectedProject)
    {
        connectNodesViewModel viewModel = new();
        viewModel.SelectedTask = selectedProject;
        connectNodes dialog = new connectNodes();
        
        foreach (var task in DataBaseController.Taskrepository.Query().Execute())
        {
            if (task == null) return;
            if (task.Id != selectedProject.Id)
            {

                DataTemplates.Project project  = DataBaseController.Projectrepositroy.Query()
                    .Where(p => p.Id == task.ProjectId).Execute()
                    .FirstOrDefault();
                if (project != null)
                    viewModel.Tasks.Add(task);
            }


        }
     
    
        dialog.DataContext = viewModel;
        Window dialog2 = dialog;
        dialog2.ShowDialog(root);
        
        
    }
    

    public void DeleteTask(ProjectData data)
    {
        DataBaseController.DeleteTask(data);
        
    }
    public event Action? OnUpdate;

    public void OnUpdateFunction()
    {
        OnUpdate?.Invoke();
    }
    public  void OnClicked(ProjectData selectedTask)
    {
       

// Spusťte insert uvnitř try-catch, jak máte teď

      
        ProjectData newData = new ProjectData();
        newData.Name = selectedTask.Name;
        newData.Finished = selectedTask.Finished;
        newData.ProjectId = Project.Id;
        
    
        newData.ParentId = selectedTask.Id;
        ProjectData parent = DataBaseController.Taskrepository.Query().Where(p=>p.Id==newData.ParentId).Execute().First();
        newData.Finished = parent.Finished;
        DataBaseController.InsertTask(newData);
        

    }
    
}