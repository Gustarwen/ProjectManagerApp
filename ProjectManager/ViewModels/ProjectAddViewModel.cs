using System;
using CommunityToolkit.Mvvm.Input;
using ProjectManager.DataTemplates;

namespace ProjectManager.ViewModels;

public partial class ProjectAddViewModel:ViewModelBase
{
    public void AddProject(DateTime pickedDate, string name)
    {
        Project project = new();
        project.Name = name;
        project.Date = pickedDate;
        DataBaseController.Projectrepositroy.Create(project);
    }
}