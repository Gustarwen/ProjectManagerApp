using System;
using System.Reflection.Metadata.Ecma335;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ProjectManager.DataTemplates;

namespace ProjectManager.ViewModels;

public partial class canvasNodesViewModel:ViewModelBase
{
    public Project project;
    public  canvasNodesViewModel(Project selectedProject)
    {
        project = selectedProject;
    }
    
 
 
    public  void OnClicked(ProjectData selectedTask)
    {
        ProjectData newData = new ProjectData();
        newData.Name = selectedTask.Name;
        newData.Finished = selectedTask.Finished;
        newData.ProjectId = selectedTask.ProjectId;
        newData.parentId = selectedTask.Id;
        ProjectManager.DataTemplates.DataBaseController.InsertTask(newData);

    }
    
}