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
                Utilities.Log(new string[] { "No layout.json paths specified.", "Usage: MSFSLayoutGenerator.exe <layout.json> ..." });
            }
            else
            {
                //Some serialization options to match layout.json as closely as possible to the MSFS format.
                JsonSerializerOptions jsonOptions = new JsonSerializerOptions();
                jsonOptions.WriteIndented = true;
                jsonOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(UnicodeRanges.All);

                foreach (string path in layoutPaths)
                {
                    Layout layout = new Layout();

                    //Get absolute path in case we have a relative path.
                    string layoutPath = Path.GetFullPath(path);
                    string json;
                    long totalPackageSize = 0;

                    //Ensure that the specified file is named "layout.json".
                    if (string.Equals(Path.GetFileName(layoutPath), "layout.json", StringComparison.OrdinalIgnoreCase))
                    {
                        foreach (string file in Directory.GetFiles(Path.GetDirectoryName(layoutPath), "*.*", SearchOption.AllDirectories))
                        {
                            // Skip clearly temporary or backup files
                            if (file.EndsWith("~")
                                || file.StartsWith(".")
                                || file.EndsWith(".bak", StringComparison.CurrentCultureIgnoreCase)
                                || file.EndsWith(".tmp", StringComparison.CurrentCultureIgnoreCase))
                                continue;

                            //Certain .NET APIs don't like long file paths, so we check to ensure the length is under the limit.
                            if (file.Length > 259)
                            {
                                Utilities.Log("One or more file paths in the folder containing \"" + layoutPath + "\" are 260 characters or greater in length. Please move this package to a directory with a shorter path.");
                            }

                            string relativePath = Utilities.GetRelativePath(file, Path.GetDirectoryName(layoutPath));

                            Content content = new Content();
                            content.Path = relativePath;
                            content.Size = new FileInfo(file).Length;
                            content.Date = new FileInfo(file).LastWriteTimeUtc.ToFileTimeUtc();
                            
                            //The MSFS virtual file system doesn't need a few select files/folders to be specified in layout.json, so we omit those.
                            if (!relativePath.StartsWith("_CVT_", StringComparison.OrdinalIgnoreCase) && !string.Equals(relativePath, "business.json") && !string.Equals(relativePath, "layout.json") && !string.Equals(relativePath, "manifest.json"))
                            {
                                layout.Content.Add(content);

                                //Log the size of each file so we can update total_package_size in manifest.json later if it exists.
                                totalPackageSize += content.Size;
                            }

                            //The size of these files must be considered for total_package_size as well
                            if(string.Equals(relativePath, "business.json") || string.Equals(relativePath, "manifest.json"))
                            {
                                totalPackageSize += content.Size;
                            }
                        }

                        if (layout.Content.Count() == 0)
                        {
                            Utilities.Log("No files were found in the folder containing \"" + layoutPath + "\". The layout.json will not be updated.");
                        }

                        json = JsonSerializer.Serialize(layout, jsonOptions);

                        try
                        {
                            //Changing new lines to match MSFS format (LF).
                            File.WriteAllText(layoutPath, json.Replace("\r\n", "\n"));
                        }
                        catch (Exception ex)
                        {
                            Utilities.Log("Failed to write layout.json: " + ex.Message);
                        }

                        //Add layout.json size now that it has been written.
                        totalPackageSize += new FileInfo(layoutPath).Length;

                        //Getting the full path to manifest.json so we can update it.
                        string manifestPath = Path.Combine(new string[] { Path.GetDirectoryName(layoutPath), "manifest.json" });

                        if (File.Exists(manifestPath))
                        {
                            Dictionary<string, object> manifest = new Dictionary<string, object>();

                            try
                            {
                                manifest = JsonSerializer.Deserialize<Dictionary<string, object>>(File.ReadAllText(manifestPath));
                            }
                            catch (Exception ex)
                            {
                                Utilities.Log("Failed to parse manifest.json: " + ex.Message);
                            }

                            //If manifest.json exists and contains the total_package_size property, we update it with the size we collected by iterating through each file for layout.json.
                            if (manifest.ContainsKey("total_package_size"))
                            {
                                manifest["total_package_size"] = totalPackageSize.ToString().PadLeft(20, '0');

                                string manifestJson = JsonSerializer.Serialize(manifest, jsonOptions);

                                try
                                {
                                    //Strangly, line endings are different on manifest.json (CRLF)
                                    File.WriteAllText(manifestPath, manifestJson);
                                }
                                catch (Exception ex)
                                {
                                    Utilities.Log("Failed to write manifest.json: " + ex.Message);
                                }
                            }
                        }
                    }
                    else
                    {
                        Utilities.Log("The file \"" + layoutPath + "\" is not named layout.json and will not be updated.");
                    }
                }
            }
        }
    }
}