using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls.Shapes;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace ProjectManager.Views;

public partial class blockGraphView : UserControl
{
    public List<int> graphData=new(){10,10,5,5,2,5,6,6,9,9,5};

    public void GenerateGraph()
    {
       
        double width = this.Bounds.Width/(graphData.Count);
        int max = graphData.Max();
        for (int i = 0; i < graphData.Count; i++)
        {
            
        
            Rectangle rect = new Rectangle()
            {
                Width = width-10,
                Height = (double)graphData[i]/max*this.Bounds.Height,
                Fill = Brushes.Red
            };
            Canvas.SetLeft(rect, width*i);
            Canvas.SetBottom(rect, 10);
            BlockGraph.Children.Add(rect);
            ToolTip.SetTip(rect, graphData[i].ToString());
            
            
            
        }
    }
    public blockGraphView()
    {
        InitializeComponent();
        this.SizeChanged += (a, v) =>
        {
            if (this.Bounds.Height > 0 && this.Bounds.Width > 0)
            {
                GenerateGraph();
            }
        };
    }
}