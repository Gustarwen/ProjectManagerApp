namespace versionSystem;

class Program
{
    private static void InitializeProject(List<string> path)
    {
        if (path.Count <2) return;
        if(path==null||path[1].Length==0) return;
        if(!Directory.Exists(path[1]))Console.WriteLine("WARNING: Directory does not exist");
        if (!Directory.Exists(Path.Combine(path[1], ".PMVS")))
        {
            Directory.CreateDirectory(Path.Combine(path[1], ".PMVS"));
            Directory.CreateDirectory(Path.Combine(path[1], ".PMVS","objects"));
            Console.WriteLine($"added {path[1]}");
        }
        else
        {
            Console.WriteLine("WARNING: Directory already exists");
        }
        
    }

    private static void CopyDirectory(string source, string target)
    {
        foreach (string file in Directory.GetFiles(source))
        {
            File.Create(file).Dispose();
            File.Copy(file, Path.Combine(target,Path.GetFileName(file)), true);
            
        }

        foreach (string dir in Directory.GetDirectories(source))
        {
            if(Path.GetFileName(dir)==".PMVS")continue;
            Directory.CreateDirectory(Path.Combine(target,Path.GetFileName(dir)));
            CopyDirectory(dir, Path.Combine(target, Path.GetFileName(dir)));
            Console.WriteLine($"added {Path.GetFileName(dir)}");
        }
    }
    private static void AddFiles(List<string> path)
    {
        if (path.Count < 2) return;
        if (path == null||path[1].Length==0) return;
        CopyDirectory(path[1], Path.Combine(path[1], ".PMVS","objects"));

    }
    private static void CommandDictionary(string cmd)
    {
        List<string> args = cmd.Split(' ').ToList();
        switch (args[0])
        {
            case "--help": break;
            case "--version": break;
            case "--init": InitializeProject(args); break;
            case "--add":AddFiles(args); break;
        }
    }
    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to version system. Write --help for seeing more;");
        while (true)
        {
            CommandDictionary(Console.ReadLine());
        }

    }
}