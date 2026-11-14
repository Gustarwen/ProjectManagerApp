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
    public List<int> graphData = new (){1,5};
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
                Stroke = Brushes.Blue,
                ZIndex = 2
            };
            Canvas.SetLeft(ellipse, points[i].X);
            Canvas.SetTop(ellipse, points[i].Y);
            ToolTip.SetTip(ellipse,$"position (X,Y): ({points[i].X},{graphData[i]})");
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
            for (int i = 0; i < p.Count - 3; i++)
            {
                // Jdeme od 0 do 50 (u posledního segmentu včetně 50, aby křivka dojela až do konce)
                int steps = 50;
                for (int step = 0; step <= steps; step++)
                {
                    double t = step / (double)steps;
                    double t2 = t * t;
                    double t3 = t2 * t;

                    double x = 0.5 * ((2 * p[i + 1].X) + (-p[i].X + p[i + 2].X) * t +
                                      (2 * p[i].X - 5 * p[i + 1].X + 4 * p[i + 2].X - p[i + 3].X) * t2 +
                                      (-p[i].X + 3 * p[i + 1].X - 3 * p[i + 2].X + p[i + 3].X) * t3);

                    double y = 0.5 * ((2 * p[i + 1].Y) + (-p[i].Y + p[i + 2].Y) * t +
                                      (2 * p[i].Y - 5 * p[i + 1].Y + 4 * p[i + 2].Y - p[i + 3].Y) * t2 +
                                      (-p[i].Y + 3 * p[i + 1].Y - 3 * p[i + 2].Y + p[i + 3].Y) * t3);

                    polyline.Points.Add(new Point(x, y));
                }
            }
            Graph.Children.Add(polyline);
        

        
    }
    public void generateGraph()
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
                 point = new Point((double)i / (double)graphData.Count * Graph.Width+10,
                     Graph.Height-((double)graphData[i] - min) / (max - min) * Graph.Height+5);
            }
            points.Add(point);
            GeneratePoints(points);
           
            
        }
        generateLines(points);
    }
    public graphView()
    {
        InitializeComponent();
        this.SizeChanged += (s, e) =>
        {
            if (this.Bounds.Height > 0 && this.Bounds.Width > 0)
            {
                Graph.Height = this.Bounds.Height;
                Graph.Width = this.Bounds.Width;
                generateGraph();
        
                
            }
           
        };

    }
}