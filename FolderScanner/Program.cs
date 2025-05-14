using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;

public class FileInfoData
{
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public long FileSizeInBytes { get; set; }
}

class Program
{
    static void Main()
    {
        Console.Write("Enter folder path: ");
        string folderPath = Console.ReadLine();

        Console.Write("Enter file extension to filter (e.g. .txt), or leave blank for all: ");
        string extensionFilter = Console.ReadLine()?.Trim();
        //string extensionFilter = Console.ReadLine()?.Trim().ToLower();

        if (!Directory.Exists(folderPath))
        {
            Console.WriteLine("Directory not found.");
            return;
        }

        List<FileInfoData> fileList = new List<FileInfoData>();

        try
        {
            string[] files = Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                if (string.IsNullOrEmpty(extensionFilter) || Path.GetExtension(file).Equals(extensionFilter, StringComparison.OrdinalIgnoreCase))
                {
                    var info = new FileInfo(file);
                    fileList.Add(new FileInfoData
                    {
                        FileName = info.Name,
                        FilePath = info.FullName,
                        FileSizeInBytes = info.Length
                    });
                }
            }

            // Output to console
            Console.WriteLine($"\nFound {fileList.Count} file(s):\n");
            foreach (var f in fileList)
            {
                Console.WriteLine($"{f.FileName} ({f.FileSizeInBytes} bytes) - {f.FilePath}");
            }

            // Export to JSON
            string json = JsonSerializer.Serialize(fileList, new JsonSerializerOptions { WriteIndented = true });
            string outputPath = Path.Combine(folderPath, "scan_result.json");
            File.WriteAllText(outputPath, json);
            Console.WriteLine($"\n Results exported to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        //test github
    }
}

