using System.IO;
using System.Linq;

if(Arguments.Length < 2) {
    Console.Error.WriteLine("usage: matexport.cs outputdir/ file.mat ... ");
    return;
}

var directory = Arguments[0];
Directory.CreateDirectory(directory);

int exportTotal = 0;
for (int i = 1; i < Arguments.Length; ++i)
{
    var file = Arguments[i];

    if(!File.Exists(file)) {
        Console.Error.WriteLine($"Could not open file {file}");
        continue;
    }

    var mat = new EditableUtf(file);

    foreach(var node in mat.Root.IterateAll()) {
        if(node.Name.Equals("texture library", StringComparison.OrdinalIgnoreCase)) {
            Console.WriteLine($"\n{Path.GetFileName(file)}");
            exportTotal = node.Children.Count;
            foreach(var t in node.Children) {
                var mips = t.Children.FirstOrDefault(x => x.Name.Equals("MIPS", StringComparison.OrdinalIgnoreCase));
                if(mips != null) {
                    var fileName = Path.Combine(directory, t.Name + ".dds");
                    File.WriteAllBytes(fileName, mips.Data);
                    Console.WriteLine("  " + Path.GetFileName(fileName));
                    exportTotal += 1;
                } else {
                    var mip0 = t.Children.FirstOrDefault(x => x.Name.Equals("MIP0", StringComparison.OrdinalIgnoreCase));
                    if (mip0 != null){
                        var fileName = Path.Combine(directory, $"{t.Name}.tga");
                        File.WriteAllBytes(fileName, mip0.Data);
                        Console.WriteLine("  " + Path.GetFileName(fileName));
                        exportTotal += 1;
                    }
                }
            }
        }
    }
}

if (exportTotal == 0){
    Console.WriteLine($"\nNo {Path.GetExtension(Arguments[1])}s with textures found in {Path.GetDirectoryName(Arguments[1])}");
    Directory.Delete(directory);
}
else {
    Console.WriteLine($"\nExported {exportTotal} total textures from {Path.GetDirectoryName(Arguments[1])}");
}
