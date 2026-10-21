using Avalonia.Controls;
using DataBaseDemo.DataBase;

namespace DataBaseDemo.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        DataBaseDemo.DataBase.db.LoadDataBase();
        
        InitializeComponent();
    }
}