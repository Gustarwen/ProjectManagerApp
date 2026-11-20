using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Styling;
using Avalonia.VisualTree;
using ProjectManager.DataTemplates;
using ProjectManager.ViewModels;
using ProjectManager.Views.Windows;

namespace ProjectManager.Views;

public partial class Home2View : UserControl
{
    public void InitializeProjects()
    {
        ProjectPanel.Children.Clear();
        HomeViewModel viewmodel = new HomeViewModel();
   
        foreach (var project in DataBaseController.Projectrepositroy.Query().Execute())
        {
            ProjectCardView projecrRepresenter = new ProjectCardView(project.Id,project.Name,project.ProjectBitmap);

          Viewbox box = new Viewbox();
          box.Child = projecrRepresenter;
            ProjectPanel.Children.Add(box);

            projecrRepresenter.Delete.Click +=(i,v)=>
            {
                DataBaseController.Projectrepositroy.Delete(project);
                foreach (var task in DataBaseController.Taskrepository.Query().Where(p=>p.ProjectId==project.Id).Execute())
                {
                    DataBaseController.Taskrepository.Delete(task);
                }
                InitializeProjects();

            };
        }
    }


    public Home2View()
    {
        InitializeComponent();
        InitializeProjects();

        AddProject.Click += (a, v) =>
        {
            
            MainWindow mainWindow = this.FindAncestorOfType<MainWindow>();
            Window dialog = new ProjectAddView(this);
            dialog.ShowDialog(mainWindow);
            
            
         //tady vytvor novy dialog
        };

    }
}