using System;
using System.Collections.Generic;
using System.IO;

namespace PathLearn
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = "/path/to/file.png";

            Console.WriteLine($"Path: '{ path }'");
            Console.WriteLine($"Extension: '{ Path.GetExtension(path) }'");
            Console.WriteLine($"Filename: '{ Path.GetFileName(path) }'");
            Console.WriteLine($"DirectoryName: '{ Path.GetDirectoryName(path) }'");

            var mimetypeMapping = new Dictionary<string, string>()
            {
                {".bmp", "image/bmp"},
                {".gif", "image/gif"},
                {".jpe", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".jpg", "image/jpeg"},
                {".png", "image/png"},
                {".tif", "image/tiff"},
                {".tiff", "image/tiff"},
            };
            string mimeType;
            if (!mimetypeMapping.TryGetValue(Path.GetExtension(path).ToLower(), out mimeType)) {

                mimeType = "application/octet-stream";
            }

            Console.WriteLine($"Mimetype: '{ mimeType }'");

            Console.WriteLine();

            // Filename with and without extension.
            var filePath = $@"C:\Users\Foo\Something.zip";
            Console.WriteLine($"FileName: {Path.GetFileName(filePath)}");
            Console.WriteLine($"FileNameWithoutExtension: {Path.GetFileNameWithoutExtension(filePath)}");
            
            Console.WriteLine("End.");
            Console.ReadKey();
        }
    }
}
