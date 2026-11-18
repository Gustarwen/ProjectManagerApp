using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using ProjectManager.DataTemplates;
using ProjectManager.ViewModels;

namespace ProjectManager.Views;

public partial class ProjectCardView : UserControl
{
    public ProjectCardView(int id, string name, Bitmap projectBitmap)
    {
        InitializeComponent();
        Name.Text = name;
        ProjectImage.Source = projectBitmap;
        double value =
            (double)DataBaseController.Taskrepository.Query().Where(p => p.ProjectId == id && p.Finished == true)
                .Execute().Count() / DataBaseController.Taskrepository.Query().Where(p => p.ProjectId == id).Execute()
                .Count() * 100;
        ProjectImage.Stretch = Stretch.UniformToFill;
        Console.WriteLine(value);
        ProgressBar.Value = value;
        if (this.DataContext is MainViewModel mvm)
        {
            GoToProject.Command = mvm.GoToProjectBoardCommand;
            GoToProject.CommandParameter = DataBaseController.Projectrepositroy.Query().Where(p => p.Id == id).Execute()
                .FirstOrDefault();
        }
    }
}