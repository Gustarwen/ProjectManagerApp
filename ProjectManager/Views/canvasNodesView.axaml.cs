using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Media;
using Avalonia.VisualTree;
using ProjectManager.DataTemplates;
using ProjectManager.ViewModels;

namespace ProjectManager.Views;

public partial class CanvasNodesView : UserControl
{
    public static CanvasNodesView item;
    private bool _pan = false;
    private Point start;
// Přidej tyto 3 proměnné pro držení přesného stavu transformace:
    private double currentZoom = 1.0;
    private double offsetX = 0.0;
    private double offsetY = 0.0;
    private double startOffsetX;
    private double startOffsetY;

    private double x;
    private double y;
    private double linex;
    private double liney;
    private Ellipse template;
    private List<task> mapaobjects;
    private List<ProjectData> currentData = new List<ProjectData>();
    private CanvasNodesViewModel viewModel;
    private Matrix originalMatrix = new  Matrix();

    private int _addPositionToNodes(ProjectData firstData,double vrstva)
    {
        firstData.Y = vrstva;
        if (firstData.Childerns.Count == 0)
        {
            return 500;}
        List<int> thedata = new List<int>();
        int sum = 0;
        for (int i = 0; i < firstData.Childerns.Count; i++)
        
        {
            thedata.Add(_addPositionToNodes(firstData.Childerns[i],vrstva + 250));
            sum += thedata[thedata.Count - 1];
        }

        for (int o = 0; o < thedata.Count; o++)
        {
            
        }

        int X = -sum / 2;
 
        for (int i = 0; i < thedata.Count; i++)
        {
                firstData.Childerns[i].X = X+ thedata[i]/2;
                X += thedata[i];
             
          
        }

        return sum;
    }

    private void _fixPositionToNodes(ProjectData firstData,double X)
    {
        firstData.X = firstData.X+ X;
        for (int i = 0; i < firstData.Childerns.Count; i++)
        {
            _fixPositionToNodes(firstData.Childerns[i], firstData.X);
        }
        
    }
    public void createMap(ProjectData firstData)
    {
        // Vyčištění plátna před novým vykreslením
        MapCanvas.Children.Clear();

        // 1. KROK: Načteme VŠECHNY uzly pro daný projekt najednou z DB (obrovské zrychlení)
        List<ProjectData> allNodes = DataBaseController.Taskrepository.Query().Execute()
            .Where(p => p.ProjectId == firstData.ProjectId).ToList();

        // Najdeme náš skutečný kořenový uzel uvnitř této sady (abychom pracovali se správnou instancí)
        ProjectData root = allNodes.FirstOrDefault(p => p.Id == firstData.Id) ?? firstData;

        // 2. KROK: Sestavení stromu v paměti pomocí fronty (BFS)
        Queue<ProjectData> queue = new Queue<ProjectData>();
        queue.Enqueue(root);
    
        while (queue.Count > 0)
        {
            ProjectData current = queue.Dequeue();
        
            // Hledáme v lokálním seznamu v paměti, tím pádem provazujeme stále stejné instance!
            current.Childerns = allNodes.Where(p => p.ParentId == current.Id).ToList();
        
            foreach (var childern in current.Childerns)
            {
                queue.Enqueue(childern);
            }
        }
    
        // 3. KROK: Výpočet pozic (s opravenou matematikou)
        int celkovaSirka = _addPositionToNodes(root, 0);
    
        // Posuneme kořen doprostřed plátna (např. 1000px), aby levá větev neutekla do záporu
        double startovniX = Math.Max(1000.0, celkovaSirka / 2.0);
        _fixPositionToNodes(root, startovniX);
    
        // 4. KROK: Samotné vykreslení
        task.tasks.Clear();
        _renderNodes(root);
    }

    private void _renderNodes(ProjectData node)
    {
        task tasks = new task(node);
        

        tasks.Add.Click += (a, v) =>
        {
            Window root = this.FindAncestorOfType<Window>();
            viewModel.showDialog((CanvasNodesView)this,root, node);

        };
        
        tasks.Delete.Click += (a, v) => {viewModel.DeleteTask(node); onUpdate(); };
        Canvas.SetLeft(tasks,node.X);
        Canvas.SetTop(tasks,node.Y);
        tasks.Name.Text = node.Name;
        MapCanvas.Children.Add(tasks);
        task.tasks.Add(tasks);
      
       
        


        foreach (var childern in node.Childerns)
        {
            Line line = new Line()
            {
                StartPoint = new Point(node.X+125,node.Y+25),
                EndPoint = new Point(childern.X+125,childern.Y+25),
                ZIndex = -1,
                StrokeThickness = 10
             
            };
            line.Bind(Line.StrokeProperty, new DynamicResourceExtension("redone"));

            
            MapCanvas.Children.Add(line);
            _renderNodes(childern);
        }
    }

    private void OnPointerPressed(object sender, PointerPressedEventArgs e)
    {
        var pointerProperties = e.GetCurrentPoint(Container).Properties;

        if (pointerProperties.IsLeftButtonPressed)
        {
            _pan = true;
            start = e.GetPosition(Container);
        

            startOffsetX = offsetX;
            startOffsetY = offsetY;

            e.Pointer.Capture(Container);
        }
    }


    private void OnPointerMoved(object sender, PointerEventArgs e)
    {
        if (_pan)
        {
            Point current = e.GetPosition(Container);
            
            offsetX = startOffsetX + (current.X - start.X);
            offsetY = startOffsetY + (current.Y - start.Y);

            if (MapCanvas.RenderTransform is MatrixTransform matrixTransform)
            {
                matrixTransform.Matrix = new Matrix(currentZoom, 0, 0, currentZoom, offsetX, offsetY);
            }
        }
        
    }


    private void OnPointerReleased(object sender, PointerReleasedEventArgs e)   
    {
        
        _pan = false;
        e.Pointer.Capture(null);
    }

  
   
  

    private void OnWheelChanged(object sender, PointerWheelEventArgs e)
    {
        if (MapCanvas.RenderTransform is not MatrixTransform transform)
            return;
        
        Point p = e.GetPosition(Container);

        Matrix m = transform.Matrix;

        double factor = e.Delta.Y > 0 ? 1.1 : 1.0 / 1.1;

        double currentScale = Math.Sqrt(m.M11 * m.M11 + m.M12 * m.M12);
        double newScale = currentScale * factor;

        if (newScale < 0.1 || newScale > 5.0)
            return;
        
        double ratio = factor;

        Matrix zoom = new Matrix(
            ratio, 0,
            0, ratio,
            p.X * (1 - ratio),
            p.Y * (1 - ratio));

        m = m * zoom;
        currentZoom = currentZoom*factor;

        transform.Matrix = m;
        offsetX = m.M31;
        offsetY = m.M32;

    }

    public  void onUpdate()
    {
       
       
        var freshFirst = DataBaseController.Taskrepository.Query()
            .Where(p => (p.ParentId == 0||p.ParentId == null) && p.ProjectId==viewModel.Project.Id).Execute().First();
        int finisher = 0;
        int sum = 0;
      
        // 3. Znovu vykreslíme
        createMap(freshFirst);
        foreach (task myvar in task.tasks)
        {
            sum++;
            if (myvar.data.Finished)
            {
                finisher++;
            }
        }

        ProgressBar.Value = (double)finisher / sum * 100;


    }

    private void ProgressBarAnimation(ProgressBar bar, double value)
    {
        
        while (ProgressBar.Value != value)
        {
            bar.Value -= (bar.Value - value)/2;
            Thread.Sleep(20);
        }
        
    }




    public CanvasNodesView()
    {

        item = this;
        
        
        
        
      
        InitializeComponent();
        viewModel = DataContext as CanvasNodesViewModel;

        DataContextChanged += (sender, args) =>
        {
            viewModel = this.DataContext as CanvasNodesViewModel;

            if (viewModel != null)
            {
                if (viewModel.Project != null)
                {
                  
                    onUpdate();
                }
            }
        };
      




    }
}