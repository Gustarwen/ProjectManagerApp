
using Durable.Sqlite;


namespace ProjectManager.DataTemplates;

public class DataBaseController
{
    //definice ovladačů pro SQLite databázi
    public static SqliteRepository<ProjectData> Taskrepository = new SqliteRepository<ProjectData>("Data Source=./task.db");
    public static SqliteRepository<Project> Projectrepositroy = new SqliteRepository<Project>("Data Source=./task.db");
    public static SqliteRepository<Project_Task> ProjectTaskRepository = new ("Data Source=./task.db");

    //konstruktor pro class spravující databázi
    static DataBaseController()
    {
        Taskrepository.InitializeTable(typeof(ProjectData));
        Projectrepositroy.InitializeTable(typeof(Project));
        ProjectTaskRepository.InitializeTable(typeof(Project_Task));
    }



 
    //funkce ukládající Task
    public static void InsertTask(ProjectData data)
    {
       
            Taskrepository.Create(data);
            Project_Task task = new();

            foreach (var VARIABLE in data.Projects)
            {
                task.ProjectId = VARIABLE;
                task.TaskId = data.Id;
                ProjectTaskRepository.Create(task);
            }
           

    }
    public static void DeleteTask(ProjectData data)
    {
        Taskrepository.Delete(data);
        
        Project_Task task = new();
        ;
        task.TaskId = data.Id;
        ProjectTaskRepository.Delete(task);
    }
    public static Project InsertProject(Project data)
    {
        return Projectrepositroy.Create(data);
    }
    public static void DeleteProject(Project data)
    {
        Projectrepositroy.Delete(data);
    }

    public static void CreateRelationShip(ProjectData data)
    {
        foreach (var VARIABLE in data.Projects)
        {
            Project_Task newTask = new();
            newTask.TaskId = data.Id;
            newTask.ProjectId = VARIABLE;
            ProjectTaskRepository.Create(newTask);
        }
        
        
    }

    public static void CreateRelationShip(Project project)
    {
        foreach (var data in project.Tasks)
        {
            Project_Task newTask = new();
            newTask.TaskId = project.Id;
            newTask.ProjectId = data.Id;
            ProjectTaskRepository.Create(newTask);
        }
    }
}