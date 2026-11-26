using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;
using ProjectManager.DataTemplates;
using ProjectManager.ViewModels;
using ProjectManager.Views.Windows;

namespace ProjectManager.Views;

public partial class ProjectAddDialogView : Window
{
    public ProjectAddDialogView()
    {
    

    InitializeComponent();
    List<ProjectData> projectDatas = new ();
        foreach (var project in DataBaseController.Taskrepository.Query().Execute().ToList())
        {
            ComboBoxItem item = new ComboBoxItem();
            item.Content = project.Name;
            item.Tag = project.Id;
            projectDatas.Add(project);
            ProjectCombo.Items.Add(item);
        }
        Accept.Click += (a, p) =>
        {
           
            var informations = DataContext as CanvasNodesViewModel;
            if (MyDatePicker.SelectedDate == null||NameBox.Text == null) return;
            ProjectData data = new ProjectData();
            data.Type = 'N';
            data.deadLine =(DateTimeOffset)MyDatePicker.SelectedDate;
            if (ProjectCombo.SelectedItem is not ComboBoxItem item) return;
            data.ProjectId = DataBaseController.Taskrepository.Query().Where(p=> p.Id==(int)item.Tag).Execute().FirstOrDefault().ProjectId;
            data.start =DateTime.Now;
            data.Name = NameBox.Text;
            data.Date = MyDatePicker.SelectedDate;
            data.ParentId = (int)item.Tag;
          
            DataBaseController.Taskrepository.Create(data);
            this.Close();
        };

        Close.Click += (a, p) =>
        {
            this.Close();
        };
        
        
    }
}