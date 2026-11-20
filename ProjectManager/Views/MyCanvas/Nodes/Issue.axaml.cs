using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ProjectManager.DataTemplates;
using ProjectManager.ViewModels;

namespace ProjectManager.Views;

public partial class Issue : task
{
    public Issue(ProjectData Data) : base(Data)
    {
        InitializeComponent();
        taskViewModel viewmodel = new taskViewModel();
        Accept.Command = viewmodel.AcceptCommand;
        Accept.CommandParameter = viewmodel;
        Accept.CommandParameter = Data;

        viewmodel.Finished = Data.Finished;
        DataContext = viewmodel;
        data = Data;
        MarkMilestone.Command = viewmodel.MarkAsMilestoneCommand;
        MarkMilestone.CommandParameter = Data;
        MarkAsInProgress.Command = viewmodel.MarkAsInProgressCommand;
        MarkAsInProgress.CommandParameter = Data;
    }
}