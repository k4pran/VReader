using System.Diagnostics;
using System.IO;
using Debug = UnityEngine.Debug;

namespace System{
    public class PdfConversion{

        private static string pdfToCairo = "pdftocairo";

        public static void ToJpegs(string inputPath, string outputPath){
            
            
            if (File.Exists(inputPath)){
                var p = new Process();
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.FileName = "/Users/ryan/Documents/Workspace/Unity/VReader/Assets/Modules/Converter/pdftocairo";
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.Arguments = "-jpeg '" + inputPath + "' '" + outputPath + "'";


                p.Start();

                Debug.Log(p.StandardError.ReadToEnd()); 
                Debug.Log(p.StandardOutput.ReadToEnd());

                p.WaitForExit(); 
            }
            else{
                Console.WriteLine("File " + inputPath + " does not exist");
            }
            
        }
    }
}