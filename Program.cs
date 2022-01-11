using System;
using System.IO;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        string pathToEditScript = "/.";
        string freelancerDirectory = "%ProgramFiles%/Microsoft Games/Freelancer";
        string exportDirectory = "./export";
        string exportScript = Path.GetFullPath("./exportmat.cs");

        if (args.Length != 3)
        {
            Console.WriteLine("To use: FLTextureExporter librelancerSDK freelancer export");
            Console.WriteLine("\nllibrelancerSDK - Path to the LibreLancer SDK. E.g. \"C:/Freelancer Tools/librelancer-sdk-win7-x64/\"");
            Console.WriteLine("freelancer - Path to your Freelancer install. E.g. \"C:/Program Files (x86)/Microsoft Games/Freelancer/\"");
            Console.WriteLine("export - Directory to export all the textures to. E.g. \"C:/Freelancer Tools/Textures/\"");
            Console.WriteLine();
            return;
        }

        for (int i = 0; i < args.Length; ++i)
        {
            if (i == 0)
                pathToEditScript = args[i];
            if (i == 1)
                freelancerDirectory = args[i];
            if (i == 2)
                exportDirectory = args[i];
        }
        Console.WriteLine($"\nPath to tool: {pathToEditScript}\nFreelancer directory: {freelancerDirectory}\nExport directory: {exportDirectory}");
        pathToEditScript = Path.Combine(pathToEditScript, "lleditscript.exe");

        if (!File.Exists(pathToEditScript))
        {
            Console.Write($"Could not find lleditscript.exe!\n{pathToEditScript}\n");
            return;
        }

        SelectExport:
        string fileTypeToExport;
        while (true)
        {
            Console.WriteLine("\nSelect file type to export:\n1. txm\n2. mat\n3. 3db");
            Console.Write("\nSelection: ");
            var selectTypeKey = Console.ReadKey().Key;
            Console.WriteLine();

            if (selectTypeKey == ConsoleKey.D1)
            {
                fileTypeToExport = "*.txm";
                break;
            }
            else if (selectTypeKey == ConsoleKey.D2)
            {
                fileTypeToExport = "*.mat";
                break;
            }
            else if (selectTypeKey == ConsoleKey.D3)
            {
                fileTypeToExport = "*.3db";
                break;
            }
            else if (selectTypeKey == ConsoleKey.Escape)
                return;
        }

        var allMaterialPaths = Directory.GetFiles(
            freelancerDirectory, fileTypeToExport,
            SearchOption.AllDirectories);

        if (allMaterialPaths.Length == 0)
            Console.WriteLine("\nNo materials found in the given Freelancer directory!");
        else
            Console.WriteLine("\nBeginning export!\nThis will take a while.");

        HashSet<string> foldersToCheck = new HashSet<string>();
        foreach (var path in allMaterialPaths)
            foldersToCheck.Add(Path.GetDirectoryName(path));

        int currentFolderCount = 1;
        foreach (var folder in foldersToCheck)
        {
            var localPaths = Directory.GetFiles(
                folder, fileTypeToExport,
                SearchOption.TopDirectoryOnly);

            var indexOfData = folder.IndexOf("DATA\\");
            indexOfData += 5;
            var subPath = folder.Substring(indexOfData);

            string exportFolder = Path.Combine(exportDirectory, subPath);
            string arguments = $"\"{exportScript}\" \"{exportFolder}\"";

            foreach (var materialPath in localPaths)
                arguments += $" \"{materialPath}\"";

            // Wait for it to finish one at a time.
            Console.WriteLine($"\n({currentFolderCount}/{foldersToCheck.Count}) Checking {subPath}...");
            var process = System.Diagnostics.Process.Start(pathToEditScript, arguments);
            while (!process.HasExited) { };

            currentFolderCount += 1;
        }

        Console.WriteLine("\nFinished!");

        Console.Write("\nExport another file type? (Yes/No): ");
        var key = Console.ReadKey().Key;
        if (key == ConsoleKey.Y)
        {
            Console.WriteLine();
            goto SelectExport;
        }

        Console.Write("\nOpen export folder? (Yes/No): ");
        key = Console.ReadKey().Key;
        if (key == ConsoleKey.Y)
        {
            Console.WriteLine("\nOpening export folder...");
            System.Diagnostics.Process.Start("explorer.exe", Path.GetFullPath(exportDirectory));
        }
    }
}
