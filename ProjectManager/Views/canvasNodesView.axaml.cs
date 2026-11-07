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
    private double offsetX;
    private double offsetY;
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
    private Matrix originalMatrix = new Matrix();
    public List<MainProjectData> Roots = new();

    private int _addPositionToNodes(ProjectData firstData, double vrstva)
    {
        firstData.Y = vrstva;
        if (firstData.Childerns.Count == 0 &&
            firstData.JoindedProject.Count == 0)
        {
            return 500;
        }
        List<int> thedata = new List<int>();
        int sum = 0;
        for (int i = 0; i < firstData.Childerns.Count; i++)

        {
            thedata.Add(_addPositionToNodes(firstData.Childerns[i], vrstva + 250));
            sum += thedata[thedata.Count - 1];

        }

        for (int i = 0; i < firstData.JoindedProject.Count; i++)

        {
            thedata.Add(_addPositionToNodes(firstData.JoindedProject[i], vrstva + 250));
            sum += thedata[thedata.Count - 1];

        }




        int X = -sum / 2;

        var children = firstData.Childerns
            .Cast<ProjectData>()
            .Concat(firstData.JoindedProject)
            .ToList();

        for (int i = 0; i < children.Count; i++)
        {
            children[i].X = X + thedata[i] / 2;
            X += thedata[i];
        }

        return sum;
    }

    private int _addPositionToNodes(MainProjectData firstData, double vrstva)
    {
        firstData.Y = vrstva;

        if (!firstData.rolled)
            return 500;
        if (firstData.rolled)
        {
            firstData.Y = vrstva;
            if (firstData.Childerns.Count == 0 &&
                firstData.JoindedProject.Count == 0)
            {
                return 500;
            }

            List<int> thedata = new List<int>();
            int sum = 0;


            
            for (int i = 0; i < firstData.Childerns.Count; i++)

            {
                thedata.Add(_addPositionToNodes(firstData.Childerns[i], vrstva + 250));
                sum += thedata[thedata.Count - 1];

            }

            for (int i = 0; i < firstData.JoindedProject.Count; i++)

            {
                thedata.Add(_addPositionToNodes(firstData.JoindedProject[i], vrstva + 250));
                sum += thedata[thedata.Count - 1];

            }





            int X = -sum / 2;

            var children = firstData.Childerns
                .Cast<ProjectData>()
                .Concat(firstData.JoindedProject)
                .ToList();

            for (int i = 0; i < children.Count; i++)
            {
                children[i].X = X + thedata[i] / 2;
                X += thedata[i];
            }

            return sum;


        }
        else
        {
            return 500;
        }
    }

    private void _fixPositionToNodes(ProjectData firstData, double X)
    {
        firstData.X = firstData.X + X;
        for (int i = 0; i < firstData.Childerns.Count; i++)
        {
            _fixPositionToNodes(firstData.Childerns[i], firstData.X);
        }

        for (int i = 0; i < firstData.JoindedProject.Count; i++)
        {
            _fixPositionToNodes(firstData.JoindedProject[i], firstData.X);
        }

    }

    private void _fixPositionToNodes(MainProjectData firstData, double X)
    {
        firstData.X = firstData.X + X;
        if (firstData.rolled)
        {
            for (int i = 0; i < firstData.Childerns.Count; i++)
            {
                _fixPositionToNodes(firstData.Childerns[i], firstData.X);
            }

            for (int i = 0; i < firstData.JoindedProject.Count; i++)
            {
                _fixPositionToNodes(firstData.JoindedProject[i], firstData.X);
            }
        }

    }

    private void loadAllNodes(List<ProjectData> allNodes,ProjectData firstData)
    {
        Queue<ProjectData> NodesQueue = new();


        NodesQueue.Enqueue(firstData);
        while (NodesQueue.Count > 0)
        {
            ProjectData currentData = (NodesQueue.Dequeue());


            foreach (var child in DataBaseController.Taskrepository.Query().Where(p => p.ParentId == currentData.Id)
                         .Execute().ToList())
            {

                NodesQueue.Enqueue(child);
                currentData.Childerns.Add(child);


            }

            foreach (var special in DataBaseController.ProjectTaskRepository.Query().Where(p=>p.TaskId==currentData.Id).Execute().ToList() )
            {
              
                MainProjectData joined = new MainProjectData(DataBaseController.Taskrepository.Query()
                    .Where(d => d.Id == special.TaskId2).Execute()
                    .First());
                loadAllNodes(allNodes,joined );
                currentData.JoindedProject.Add(joined);
            }



            allNodes.Add(currentData);

        }

     
    }


public void createMap(ProjectData firstData)
    {
        
        // Vyčištění plátna před novým vykreslením
        MapCanvas.Children.Clear();

        // 1. KROK: Načteme VŠECHNY fi pro daný projekt najednou z DB (obrovské zrychlení)
        List<ProjectData> allNodes=new();
        loadAllNodes(allNodes, firstData);
        Console.WriteLine(allNodes.Count);
        //vytazeni z pameti vsech nodes

        


        //Najdeme náš skutečný kořenový uzel uvnitř této sady (abychom pracovali se správnou instancí)
        MainProjectData root = Roots.Where(p => p.ProjectId == firstData.ProjectId).First();
        root.rolled = true;


        Queue<ProjectData> queue = new Queue<ProjectData>();
        queue.Enqueue(root);

        while (queue.Count > 0)
        {
            ProjectData current = queue.Dequeue();
            int celkovaSirka = _addPositionToNodes(root, 0);


            double startovniX = Math.Max(1000.0, celkovaSirka / 2.0);
            _fixPositionToNodes(root, startovniX);

            task.tasks.Clear();
            _renderNodes(root);
        }
    }

    private void renderSpecialNode(MainProjectData node)
    {



        MainProjectNode myNode = new MainProjectNode(node);
        if (node.rolled)
        {
            foreach (var normal in node.Childerns)
            {
                Line line = new Line()
                {
                    StartPoint = new Point(node.X + 125, node.Y + 25),
                    EndPoint = new Point(normal.X + 125, normal.Y + 25),
                    ZIndex = -1,
                    StrokeThickness = 10

                };
                line.Bind(Line.StrokeProperty, new DynamicResourceExtension("redone"));


                MapCanvas.Children.Add(line);
                _renderNodes(normal);
            }

            foreach (var special in node.JoindedProject)
            {
                Line line = new Line()
                {
                    StartPoint = new Point(node.X + 125, node.Y + 25),
                    EndPoint = new Point(special.X + 125, special.Y + 25),
                    ZIndex = -1,
                    StrokeThickness = 10

                };
                line.Bind(Line.StrokeProperty, new DynamicResourceExtension("redone"));


                MapCanvas.Children.Add(line);
                renderSpecialNode(special);
            }
        }
        Canvas.SetLeft(myNode, node.X);
        Canvas.SetTop(myNode, node.Y);
        MapCanvas.Children.Add(myNode);


    }

    private void _renderNodes(ProjectData node)
    {
        task tasks = new task(node);


        tasks.Add.Click += (a, v) =>
        {
            Window root = this.FindAncestorOfType<Window>();
            viewModel.showDialog((CanvasNodesView)this, root, node);

        };

        tasks.Delete.Click += (a, v) =>
        {
            viewModel.DeleteTask(node);
            onUpdate();
        };
        Canvas.SetLeft(tasks, node.X);
        Canvas.SetTop(tasks, node.Y);
        tasks.Name.Text = node.Name;
        MapCanvas.Children.Add(tasks);
        task.tasks.Add(tasks);




        foreach (var normal in node.Childerns)
        {
            Line line = new Line()
            {
                StartPoint = new Point(node.X + 125, node.Y + 25),
                EndPoint = new Point(normal.X + 125, normal.Y + 25),
                ZIndex = -1,
                StrokeThickness = 10

            };
            line.Bind(Line.StrokeProperty, new DynamicResourceExtension("redone"));


            MapCanvas.Children.Add(line);
            _renderNodes(normal);
        }

        foreach (var special in node.JoindedProject)
        {
            Line line = new Line()
            {
                StartPoint = new Point(node.X + 125, node.Y + 25),
                EndPoint = new Point(special.X + 125, special.Y + 25),
                ZIndex = -1,
                StrokeThickness = 10

            };
            line.Bind(Line.StrokeProperty, new DynamicResourceExtension("redone"));


            MapCanvas.Children.Add(line);
            renderSpecialNode(special);
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
       
        if (viewModel?.Project == null)
        {
            Console.WriteLine("Nelze aktualizovat mapu: Project je null.");
            return;
        }
        
        
        int finisher = 0;
        int sum = 0;
      
        // 3. Znovu vykreslíme
        MainProjectData projectRootNode;
        Roots.Clear();
        foreach (var root in DataBaseController.Taskrepository.Query().Where(p => p.ParentId == null || p.ParentId == 0)
                     .Execute()
                     .ToList())
        {
            

            Roots.Add(new MainProjectData(root));
        }
        
        Console.WriteLine(Roots.Count);

        projectRootNode = Roots.Where(p => p.ProjectId == viewModel.Project.Id).FirstOrDefault();
        if (projectRootNode != null)
        {
            Console.WriteLine("toto je list");
            Console.WriteLine(projectRootNode.Name);
        }
        else
        {
            Console.WriteLine("ne je list");
        }
       

        // 3. Pokud jsme uzel našli, vykreslíme mapu
        if (projectRootNode != null)
        {
            createMap(projectRootNode);
        }
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


    /// public static CanvasNodesView GetInstance
    /// if(item == null)
    ///     item = constructor
    /// return item
    public static CanvasNodesView GetInstance()
    {
        if (item == null)
        {
            return new CanvasNodesView();
        }
        else 
        {
            return item;
        }
        }
    
    
    public CanvasNodesView()
    {
        if(item==null){ item = this;}
       




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