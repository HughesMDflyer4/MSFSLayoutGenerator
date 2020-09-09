using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using System.Text.Unicode;

namespace MSFSLayoutGenerator
{
    class Program
    {
        static void Main(string[] layoutPaths)
        {
            if(layoutPaths.Count() == 0)
            {
                Utilities.Log(new  string[] { "No layout.json paths specified.", "Usage: MSFSLayoutGenerator.exe <layout.json> ..." });
            }
            else
            {
                foreach (string path in layoutPaths)
                {
                    Layout layout = new Layout();
                    string layoutPath = Path.GetFullPath(path);
                    string json;

                    if (string.Equals(Path.GetFileName(layoutPath), "layout.json", StringComparison.OrdinalIgnoreCase))
                    {
                        foreach (string file in Directory.GetFiles(Path.GetDirectoryName(layoutPath), "*.*", SearchOption.AllDirectories))
                        {
                            string relativePath = Utilities.GetRelativePath(file, Path.GetDirectoryName(layoutPath));

                            Content content = new Content();
                            content.Path = relativePath;
                            content.Size = new FileInfo(file).Length;
                            content.Date = new FileInfo(file).LastWriteTimeUtc.ToFileTimeUtc();

                            if (!relativePath.ToUpper().StartsWith("_CVT_",StringComparison.OrdinalIgnoreCase) && !string.Equals(relativePath, "business.json") && !string.Equals(relativePath, "layout.json") && !string.Equals(relativePath, "manifest.json"))
                            {
                                layout.Content.Add(content);
                            }
                        }

                        if(layout.Content.Count() == 0)
                        {
                            Utilities.Log("No files were found in the folder containing \"" + layoutPath + "\". The layout.json will not be updated.");
                        }

                        JsonSerializerOptions jsonOptions = new JsonSerializerOptions();
                        jsonOptions.WriteIndented = true;
                        jsonOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(UnicodeRanges.All);

                        json = JsonSerializer.Serialize(layout, jsonOptions);

                        try
                        {
                            File.WriteAllText(layoutPath, json.Replace("\r\n", "\n"));
                        }
                        catch (Exception ex)
                        {
                            Utilities.Log("Error: " + ex.Message);
                        }
                    }
                }
            }
        }
    }
}