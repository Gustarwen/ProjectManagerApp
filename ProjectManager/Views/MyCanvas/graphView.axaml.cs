using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace ProjectManager.Views;

public partial class graphView : UserControl
{
    public List<int> graphData = new (){1,5,2,5,10,5,2,6,9,8};
    private List<Line> lines;

    private void GeneratePoints(List<Point> points)
    {
        for (int i = 0; i < points.Count; i++)
        {
            Ellipse ellipse = new Ellipse()
            {
                Width = 10,
                Height = 10,
                StrokeThickness = 5,
                Stroke = Brushes.Blue
            };
            Canvas.SetLeft(ellipse, points[i].X);
            Canvas.SetTop(ellipse, points[i].Y);
            Graph.Children.Add(ellipse);
        }
        
    }

    private void generateLines(List<Point> points)
    {
        
        /*
         *      Math
         *  t = cislo mezi 0 a 1...proste procento...kde na krivce jsme
         *  p0 = pocatecni bod
         *  p1 = prostredni bod
         *  p2 = koncovy bod
         *  p3 = urcuje tak nejak spise smer, kam ma krivka mirit
         *
         *  vypocet - Cubic Hermite Spline odkaz: https://en.wikipedia.org/wiki/Cubic_Hermite_spline#Interpolation_on_a_single_interval
         *      p = (2*t*t*t-3*t*t+1)*p0+(t*t*t-2*t*t+t)*p1+(-2*t*t*t+3*t*t)*p2+(t*t*t-t*t)*p3
         *
         * 
         */
     
            if (points.Count < 2) return;

            // Přidáme na začátek a konec pomocný bod, aby křivka vedla od 1. až do 10. bodu
            var p = new List<Point> { points[0] };
            p.AddRange(points);
            p.Add(points[^1]);
            Polyline polyline = new Polyline()
            {
                Stroke = Brushes.Black,
                StrokeThickness = 3
            };
            for (int i = 0; i < p.Count - 4; i++)
            {
                for (int t = 1; t <= 50; t++)
                {
                    double t1 = (t - 1) / 50.0;
                    double t2 = t / 50.0;
                    
                    double y1 = 0.5 * ((2 * p[i + 1].Y) + (-p[i].Y + p[i + 2].Y) * t1 +
                                       (2 * p[i].Y - 5 * p[i + 1].Y + 4 * p[i + 2].Y - p[i + 3].Y) * t1 * t1 +
                                       (-p[i].Y + 3 * p[i + 1].Y - 3 * p[i + 2].Y + p[i + 3].Y) * t1 * t1 * t1);

                    double y2 = 0.5 * ((2 * p[i + 1].Y) + (-p[i].Y + p[i + 2].Y) * t2 +
                                       (2 * p[i].Y - 5 * p[i + 1].Y + 4 * p[i + 2].Y - p[i + 3].Y) * t2 * t2 +
                                       (-p[i].Y + 3 * p[i + 1].Y - 3 * p[i + 2].Y + p[i + 3].Y) * t2 * t2 * t2);
                    
                    double x1 = p[i + 1].X + (p[i + 2].X - p[i + 1].X) * t1;
                    double x2 = p[i + 1].X + (p[i + 2].X - p[i + 1].X) * t2;


                    polyline.Points.Add(new Point(x1, y1));
                    polyline.Points.Add(new Point(x2, y2));
                }
            }
            Graph.Children.Add(polyline);
        

        
    }
    private void generateGraph()
    {
        List<Point> points = new();
        int min = graphData.Min();
        int max = graphData.Max();
       
        for (int i = 0; i < graphData.Count; i++)
        {
            Point point;
            if(max==min){  point = new Point((double)i/(double)graphData.Count*Graph.Width, Graph.Height/2);}
            else
            {
                 point = new Point((double)i / (double)graphData.Count * Graph.Width,
                    (double)((double)graphData[i] - min) / (max - min) * Graph.Height);
            }
            points.Add(point);
            GeneratePoints(points);
            generateLines(points);
            
        }
    }
    public graphView()
    {
        InitializeComponent();
        generateGraph();
    }
}