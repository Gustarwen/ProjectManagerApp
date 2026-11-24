using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ProjectManager.DataTemplates;

namespace ProjectManager.Views.Windows;


//for those who will read this text... i created this part in midnight (correctly 11:54 PM) i know...i am not a graphic...so pls...ui wouldnt look better
public partial class ProjectAddView : Window
{
    public ProjectAddView(homeTestView view)
    {
        InitializeComponent();
        Button.Click += (e, v) =>
        {
            if (ProjectName.Text == null || DeadLineDate.SelectedDate == null) return;
      
            Project project = new Project()
            {
                Name = ProjectName.Text,
                Date =  DeadLineDate.SelectedDate,
                Finished = false
            };
            Project withid =DataBaseController.Projectrepositroy.Create(project);
            ProjectData projectData = new ()
            {
                ProjectId = withid.Id,
                Name = ProjectName.Text,
                Date = DeadLineDate.SelectedDate,
                Finished = false
                
            };
            MainProjectData mainprojectData = new MainProjectData(projectData);
            DataBaseController.Taskrepository.Create(projectData);
            Console.WriteLine("projekt byl pridan");
           view.InitializeProjects();
           view.InitializeTasks();

            this.Close();
        };

    }
}