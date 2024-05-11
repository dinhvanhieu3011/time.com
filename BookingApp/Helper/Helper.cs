using Hangfire;
using System.Collections.Generic;
using System.Text.Json;
using System;
using Hangfire.Storage;
using System.Linq;
using System.Diagnostics;
using System.IO;
using BookingApp.Service;

namespace BookingApp
{
    public class Helper
    {
        public static string CreateContent(List<string> profiles)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            return System.Text.Json.JsonSerializer.Serialize(profiles);
        }

        public static DateTime epoch2string(int epoch)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(epoch);
        }
  
        public static string CallPython(string filePath)
        {
        
            try
            {
                CreateBatFile(filePath);
                return RunBatFile();

            }
            catch (Exception ex)
            {
                Log(ex.ToString());
                return "";
            }
        }
        public static string RunBatFile()
        {


            string batchFileDirectory = System.IO.Directory.GetCurrentDirectory(); // Change this to the desired directory
            string batchFilePath = Path.Combine(batchFileDirectory, "runscript.bat");

            ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd.exe");
            processStartInfo.Verb = "runas";
            processStartInfo.CreateNoWindow = true;
            processStartInfo.UseShellExecute = false;
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.RedirectStandardError = true;
            processStartInfo.Arguments = "/c " + batchFilePath;
            Process process = Process.Start(processStartInfo);
            process.WaitForExit();


            // Read the output and error
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            // Wait for the process to exit
            process.WaitForExit();
            return "";
            //if (error == "")
            //{
            //    string outputFile = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "user_videos1.json");
            //    string outputText = File.ReadAllText(outputFile);
            //    return outputText;
            //}
            //else
            //{

            //    return "";
            //}
        }
        public static void CreateBatFile(string filePath)
        {


            string fileName = "translate.exe"; // Example file name

            // Specify the directory where you want to create the batch file
            string batchFileDirectory = System.IO.Directory.GetCurrentDirectory(); // Change this to the desired directory
            string newFilePath = Path.Combine(batchFileDirectory, "temp.txt");
            File.Copy(filePath, newFilePath, overwrite: true);

            // Create the batch file path
            string batchFilePath = Path.Combine(batchFileDirectory, "runscript.bat");

            // Create the batch file content
            string batchFileContent = $"";
            batchFileContent += $"{fileName} {newFilePath}";
            batchFileContent += $" "; // Optional: pause at the end to keep the command window open

            // Write the content to the batch file
            File.WriteAllText(batchFilePath, batchFileContent);

        }
        public static void Log(string text)
        {
            string filePath = "output.txt";

            // Write the string to the file (appending to existing content)
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine(DateTime.Now.ToString() + " : "+ text);
            }
        }
        public static string TimeAgo(DateTime dt)
        {
            TimeSpan span = DateTime.Now - dt;
            if (span.Days > 365)
            {
                int years = (span.Days / 365);
                if (span.Days % 365 != 0)
                    years += 1;
                return String.Format("about {0} {1} ago",
                years, years == 1 ? "year" : "years");
            }
            if (span.Days > 30)
            {
                int months = (span.Days / 30);
                if (span.Days % 31 != 0)
                    months += 1;
                return String.Format("about {0} {1} ago",
                months, months == 1 ? "month" : "months");
            }
            if (span.Days > 0)
                return String.Format("about {0} {1} ago",
                span.Days, span.Days == 1 ? "day" : "days");
            if (span.Hours > 0)
                return String.Format("about {0} {1} ago",
                span.Hours, span.Hours == 1 ? "hour" : "hours");
            if (span.Minutes > 0)
                return String.Format("about {0} {1} ago",
                span.Minutes, span.Minutes == 1 ? "minute" : "minutes");
            if (span.Seconds > 5)
                return String.Format("about {0} seconds ago", span.Seconds);
            if (span.Seconds <= 5)
                return "just now";
            return string.Empty;
        }
    }
}
