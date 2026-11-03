    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using Avalonia;
    using Avalonia.Controls;
    using Avalonia.Input;
    using Avalonia.Markup.Xaml;
    using Avalonia.Media.Imaging;
    using ProjectManager.DataTemplates;

    namespace ProjectManager.Views;

    public partial class HomeView : UserControl
    {
        
        public HomeView()
        {
            InitializeComponent();
            DatePicker1.MinYear = DateTime.Now;
            
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
                    var border = sender as Border;
                    var mysender = border?.DataContext as Project;

                    mysender.MyImage = directory+'/' + path.Name;
                    var dbProject = DataBaseController.Projectrepositroy.Query()
                        .Execute()
                        .FirstOrDefault(x => x.Id == mysender.Id);
                                
                    if (dbProject != null)
                    {
                        dbProject.MyImage = mysender.MyImage; // Zapíšeme cestu přímo do DB objektu
                        DataBaseController.Projectrepositroy.Update(dbProject);

                    }
                }
            }
        }
    }