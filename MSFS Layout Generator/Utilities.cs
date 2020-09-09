using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSFSLayoutGenerator
{
    public static class Utilities
    {
        public static string GetRelativePath(string fullPath, string basePath)
        {
            if (!basePath.EndsWith("\\"))
                basePath += "\\";

            Uri baseUri = new Uri(basePath);
            Uri fullUri = new Uri(fullPath);
            Uri relativeUri = baseUri.MakeRelativeUri(fullUri);

            return Uri.UnescapeDataString(relativeUri.ToString());
        }

        public static void Log(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine("\r\n");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            Environment.Exit(0);
        }

        public static void Log(string[] messages)
        {
            foreach(string message in messages)
            {
                Console.WriteLine(message);
            }
            Console.WriteLine("\r\n");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            Environment.Exit(0);
        }
    }
}