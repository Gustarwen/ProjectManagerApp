    using System;
    using System.Diagnostics;
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

        private void OnDrop(object? sender, DragEventArgs e)
        {
            if (e.DataTransfer.Formats.Contains(DataFormat.File))
            {
                var files = e.DataTransfer.TryGetFiles();
                var path = files?.FirstOrDefault();
                if (path != null)
                {
                  

                    var border = sender as Border;
                    var mysender = border?.DataContext as Project;

                    mysender.MyImage = path.Path.LocalPath;
                    var dbProject = DataBaseController.Projectrepositroy.Query()
                        .Execute()
                        .FirstOrDefault(x => x.Id == mysender.Id);
                                
                    if (dbProject != null)
                    {
                        dbProject.MyImage = mysender.MyImage; // Zapíšeme cestu přímo do DB objektu
                        DataBaseController.Projectrepositroy.Update(dbProject);
                        Console.WriteLine("Obrázek úspěšně zapsán do DB kontextu.");
                        Console.WriteLine($"id daneho projectu je {dbProject.Id}");
                    }
                }
            }
        }
    }