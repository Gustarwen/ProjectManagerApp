using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls.Shapes;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using ProjectManager.ViewModels;

namespace ProjectManager.Views;

public partial class blockGraphView : UserControl
{
    public List<int> graphData=new(){10,10,5,5,2,5,6,6,9,9,5};

    public void GenerateGraph()
    {
        BlockGraph.Children.Clear();
        double width = this.Bounds.Width/(graphData.Count);
        if(width<12){width=12;}
        int max = graphData.Max();
        for (int i = 0; i < graphData.Count; i++)
        {
            
        
            Rectangle rect = new Rectangle()
            {
                Width = width-10,
                MinWidth = 12,
                Height = (double)graphData[i]/max*this.Bounds.Height,
                Fill = Brush.Parse("#0077b6")
            };
            Canvas.SetLeft(rect, width*i);
            Canvas.SetBottom(rect, 0);
            BlockGraph.Children.Add(rect);
            ToolTip.SetTip(rect, graphData[i].ToString());
            
            
            
        }
    }
    public blockGraphView()
    {
        InitializeComponent();
        this.DataContextChanged += (a,v) =>
        {
            if (DataContext is blockGraphView vm)
            {
                graphData=vm.graphData.ToList();
                vm.PropertyChanged += (e, v) =>
                {
                    GenerateGraph();
                };
                GenerateGraph();
            }
            this.SizeChanged += (a, v) =>
            {
                if (Bounds.Height != 0 && Bounds.Width != 0)
                {
                    GenerateGraph();
                }
              
           
            };
        };
       
    }
}