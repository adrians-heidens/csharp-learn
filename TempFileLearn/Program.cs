using System;
using System.IO;

namespace TempFileLearn
{
    /// <summary>
    /// Creates and destroys directory in temporary location.
    /// </summary>
    class TemporaryDirectory : IDisposable
    {
        public string Name { get; }

        public TemporaryDirectory(string dirName)
        {
            var userTempDir = Path.GetTempPath();
            Name = Path.Combine(userTempDir, dirName);

            Console.WriteLine($"Creating dir '{Name}'");
            Directory.CreateDirectory(Name);
        }

        public void Dispose()
        {
            Console.WriteLine($"Deleting dir '{Name}'");
            Directory.Delete(Name, recursive: true);
        }

        public override string ToString()
        {
            return $"TemporaryDirectory('{Name}')";
        }
    }

    /// <summary>
    /// Example of creating a temporary directory and files in it.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            using (var tempDir = new TemporaryDirectory("MyTest"))
            using (var f1 = File.Create(Path.Combine(tempDir.Name, "file1.txt")))
            using (var f2 = File.Create(Path.Combine(tempDir.Name, "file2.txt")))
            {
                Console.WriteLine(tempDir);
                Console.WriteLine(f1.Name);
                Console.WriteLine(f2.Name);
            }

            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }
    }
}
