using System.Security;
using System;
using System.Security.Cryptography;
using System.Text;
namespace versionSystem;


class Program
{
    private static string ProjectRoot => Directory.GetCurrentDirectory();
    
    //--init create a directory for saving versions and files
    private static void InitializeProject()
    {
   


            Directory.CreateDirectory(Path.Combine(ProjectRoot, ".PMVS"));
            Directory.CreateDirectory(Path.Combine(ProjectRoot, ".PMVS","objects"));
       
            Directory.CreateDirectory(Path.Combine(ProjectRoot, ".PMVS","commits"));
            File.Create(Path.Combine(ProjectRoot, ".PMVS","index"));
          Console.WriteLine(Path.GetFullPath(ProjectRoot));
        
        
    }
    //using by --add for recursively copying files
    private static void CopyDirectory(string source, string target)
    {
        foreach (string file in Directory.GetFiles(source))
        {
            try
            {
                string[] fileName = {BitConverter.ToString(SHA256.Create().ComputeHash(File.ReadAllBytes(file))).Replace("-", "").ToLowerInvariant()};
                if(!File.Exists(Path.Combine(target, fileName[0]))) File.Copy(file, Path.Combine(target, fileName[0]), true);
                File.WriteAllLines(Path.Combine(ProjectRoot,".PMVS","index"),fileName);
                
            }
            catch (Exception e)
            {
                Console.WriteLine($"file {file} can't be added bcs:{e.Message}");
            }
        }

        foreach (string dir in Directory.GetDirectories(source))
        {
            
            if(Path.GetFileName(dir)==".PMVS")continue;
            Directory.CreateDirectory(Path.Combine(target,Path.GetFileName(dir)));
            CopyDirectory(dir, Path.Combine(target, Path.GetFileName(dir)));
            Console.WriteLine($"added {Path.GetFileName(dir)}");
        }
    }
    
    //--add for add files to objects if there is nothing like this
    private static void AddFiles(List<string> path)
    {
        if (path.Count < 2) return;
        if (path == null||path[1].Length==0) return;
        CopyDirectory(path[1], Path.Combine(ProjectRoot, ".PMVS","objects"));

    }
    static string ByteArrayToString(byte[] arrInput)
    {
        int i;
        StringBuilder sOutput = new StringBuilder(arrInput.Length);
        for (i=0;i < arrInput.Length; i++)
        {
            sOutput.Append(arrInput[i].ToString("X2"));
        }
        return sOutput.ToString();
    }
    //just for subcommands
    private static void CommandDictionary(string cmd)
    {
        if (!File.Exists(Path.Combine("something", "like", "nothing")))return ;
        List<string> args = cmd.Split(' ').ToList();
        switch (args[0])
        {
            case "--help": break;
            case "--version": break;
            case "--init": InitializeProject(); break;
            case "--add":AddFiles(args); break;
            case "--commit": break;
            default:Console.WriteLine("Unknown command " + args[0]); break;
        }
    }
    //create commit with files
    private static void CreateCommit()
    {
        if(!Directory.Exists(Path.Combine(ProjectRoot, ".PMVS"))){Console.WriteLine("Project nebyl inicializovan");
            return;
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