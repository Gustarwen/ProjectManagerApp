using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ProjectManager.Views;

public partial class HomeView : UserControl
{
    
    public HomeView()
    {
        InitializeComponent();
        DatePicker1.MinYear = DateTime.Now;
        
    }
}