
using System.Collections.Generic;

using Avalonia.Controls;

using ProjectManager.DataTemplates;
using ProjectManager.ViewModels;

namespace ProjectManager.Views;

public partial class task : UserControl
{
    public static readonly List<task> tasks = new List<task>();
    public ProjectData data;

    public task(ProjectData Data)
    {
       
        InitializeComponent();
        taskViewModel viewmodel = new taskViewModel();
        Accept.Command = viewmodel.AcceptCommand;
        Accept.CommandParameter = Data;
        viewmodel.Finished = Data.Finished;
        DataContext = viewmodel;
        data = Data;
        task.tasks.Add(this);
       
    }
    ~task()
    {
        task.tasks.Remove(this);
    }
}