using Microsoft.VisualBasic;
using System.Diagnostics;
using System.IO;

namespace Tester
{
    class Program
    {

        static string execPath = @"..\CSharp\bin\Debug\net8.0\Csharp.exe";

        static void Main(string[] args)
        {

            StreamWriter writer = new StreamWriter("output.csv", true);
            
            Process process = new Process();

            int threads = 12;
            int testData = 5000000;
            bool opt = false;
            while (testData <= 10000000)
            {
                process.StartInfo.FileName = execPath;
                process.StartInfo.Arguments = $"{testData} {threads} true {opt}";
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                process.WaitForExit();
                int end = int.Parse(process.StandardOutput.ReadLine());
                writer.WriteLine($"{testData};{threads};{end};{opt}");
                Console.WriteLine(end);
                testData += 100000;
                writer.Flush();
            }
            testData = 5000000;
            opt = true;
            while (testData <= 10000000)
            {
                process.StartInfo.FileName = execPath;
                process.StartInfo.Arguments = $"{testData} {threads} true {opt}";
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                process.WaitForExit();
                int end = int.Parse(process.StandardOutput.ReadLine());
                writer.WriteLine($"{testData};{threads};{end};{opt}");
                Console.WriteLine(end);
                testData += 100000;
                writer.Flush();
            }
            writer.Close();
            
        }
    }
}