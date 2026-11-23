using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Utils;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.VisualTree;
using ProjectManager.DataTemplates;
using ProjectManager.ViewModels;

namespace ProjectManager.Views;

public partial class ProjectCardView : UserControl
{
    private int Id = 0;

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        if (Id == null) return;
        MainWindow mainWindow = this.FindAncestorOfType<MainWindow>();
        if (mainWindow == null) return;
        if (mainWindow.DataContext is MainViewModel vm)
        {
            GoToProject.Command = vm.GoToProjectBoardCommand;
            GoToProject.CommandParameter = DataBaseController.Projectrepositroy.Query().Where(p => p.Id == Id).Execute()
                .FirstOrDefault();
        }
        base.OnAttachedToVisualTree(e);
    }

    public ProjectCardView(int id, string name, Bitmap projectBitmap)
    {
        this.Id = id;
        InitializeComponent();
        Name.Text = name;
        
        
       
        ProjectBitMap.Source = projectBitmap;
        double value =
            (double)DataBaseController.Taskrepository.Query().Where(p => p.ProjectId == id && p.Finished == true)
                .Execute().Count() / DataBaseController.Taskrepository.Query().Where(p => p.ProjectId == id).Execute()
                .Count() * 100;
    
  
   
        
        Console.WriteLine(value);
        ProgressBar.Value = value;
      
       
    }
    public ProjectCardView()
    {
      
        InitializeComponent();

       
    }
    private async void OnDrop(object? sender, DragEventArgs e)
    {
        if (e.DataTransfer.Formats.Contains(DataFormat.File))
        {
            var files = e.DataTransfer.TryGetFiles();
            string directory = @"./images";
            Directory.CreateDirectory(directory);
            var path = files?.FirstOrDefault();
            if (path != null)
            {
                  
                File.Copy(path.Path.AbsolutePath,directory+'/'+path.Name,true);


              
                var dbProject = DataBaseController.Projectrepositroy.Query()
                    .Execute()
                    .FirstOrDefault(x => x.Id == Id);
                                
                if (dbProject != null)
                {
                    dbProject.MyImage = directory+'/' + path.Name; // Zapíšeme cestu přímo do DB objektu
                    ProjectBitMap.Source = new Bitmap(dbProject.MyImage);
                    
                    
                    DataBaseController.Projectrepositroy.Update(dbProject);

                }
            }
        }
    }
}