using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ProjectManager.DataTemplates;
using ProjectManager.ViewModels;

namespace ProjectManager.Views;

public partial class Finished : task 
{

    public Finished(ProjectData Data) : base(Data)
    {
        InitializeComponent();
        taskViewModel viewmodel = new taskViewModel();
        Accept.Command = viewmodel.AcceptCommand;
     
        Accept.CommandParameter = Data;

        viewmodel.Finished = Data.Finished;
        DataContext = viewmodel;
        data = Data;
        Name.Text = Data.Name;
        MarkMilestone.Command = viewmodel.MarkAsMilestoneCommand;
        MarkMilestone.CommandParameter = Data;
        MarkAsIssue.Command = viewmodel.MarkAsIssueCommand;
        MarkAsIssue.CommandParameter = Data;
    }
    

}