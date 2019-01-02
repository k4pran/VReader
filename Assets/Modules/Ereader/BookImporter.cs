using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using SimpleFileBrowser;
using UnityEditor;
using UnityEngine;

namespace Ereader{
    
    public class BookImporter : MonoBehaviour {
        
        void Start() {
            FileBrowser.SetFilters(false, new FileBrowser.Filter( "Books", ".txt", ".pdf" ));
        }

        private void UpdateLib(string bookTitle){
            string libDir = Config.Instance.BookLibraryPath  + "/VReader";
            string bookLog = libDir + "/book-log.txt";

            if (!File.Exists(bookLog)) {
                File.Create(bookLog).Dispose();
            }
            
            using (StreamWriter sw = File.AppendText(bookLog)){
                sw.WriteLine(bookTitle);
            }	
        }

        private bool DoesLibraryContain(string bookTitle) {
            string libDir = Config.Instance.BookLibraryPath  + "/VReader";
            string bookLog = libDir + "/book-log.txt";
            if (!File.Exists(bookLog)){
                return false;
            }
            
            StreamReader file = new StreamReader(bookLog);  
            
            string line;  
            while((line = file.ReadLine()) != null) {
                if (line == bookTitle){
                    file.Close();  
                    return true;
                }
            }  
  
            file.Close();
            return false;
        }
        
        // Entry point - Brings up file browser where user can select a file meeting filter criteria.
        // If a file is selected then it sets of a chain of events to import the book.
        public void UserSelectFile(){
            StartCoroutine(ShowLoadDialogCoroutine());
        }

        private BookFormat DetermineFormat(string path){
            string[] pathParts = path.Split('.');
            if (pathParts.Length < 2){
                throw new InvalidBookFormatException("Invalid book format. Must include extension");
            }
            
            string ext = pathParts[pathParts.Length - 1];

            switch(ext) {
                case "txt":
                    return BookFormat.TXT;
                
                case "pdf":
                    return BookFormat.PDF;
                
                default:
                    throw new InvalidBookFormatException("Unable to determine book format for path " + path);
            }
        }
        
        IEnumerator ShowLoadDialogCoroutine() {
            // Show a load file dialog and wait for a response from user
            // Load file/folder: file, Initial path: default (Documents), Title: "Load File", submit button text: "Load"
            yield return FileBrowser.WaitForLoadDialog(false, null, "Load File", "Load");

            Debug.Log(FileBrowser.Success + " " + FileBrowser.Result);
            if (FileBrowser.Success) {
                Import(FileBrowser.Result);
            }
        }

        public void Import(string path){
            BookFormat bookFormat = DetermineFormat(path);
            string title = GetTitleFromPath(path);
            if (DoesLibraryContain(title)){
                return; // todo - reimport or skip?
            }
            
            switch(bookFormat) {
                    
                case BookFormat.TXT:
                    ImportDotText(path);
                    break;
                
                case BookFormat.PDF:
                    Config config = Config.Instance;
                    if (config.PdfImportAsImages){
                        ImportPdf(path);
                    }
                    else{
                        ImportPdfInteractive(path);
                    }
                    break;
                
                default:
                    throw new InvalidBookFormatException("Unable to determine book format for path " + path);
            }
            UpdateLib(title);
        }

        private void ImportDotText(string path) {
            string outputDir = GenerateDirs(path);
            string title = GetTitleFromPath(path);
            
            BookInfoMapper.SerializeInfo(outputDir, BookFormat.TXT, path, title);
            CopyOriginal(path, outputDir + "/" + title + ".txt");
        }
        
        private void ImportPdf(string path){
            string outputDir = GenerateDirs(path);
            string title = GetTitleFromPath(path);
             
            PdfConversion.ToJpegs(path, outputDir + "/" + "pages/");
            BookInfoMapper.SerializeInfo(outputDir, BookFormat.PDF, path, title);
            CopyOriginal(path, outputDir + "/" + title + ".pdf");
        }

        // Generates required directories or skips if already exists
        private string GenerateDirs(string path) {
            string imageOutputPath = Config.Instance.BookLibraryPath;
            string bookTitle = GetTitleFromPath(path);
            string libDir = imageOutputPath + "/VReader";
            string outputDir = libDir + "/" + bookTitle;
            
            // Base directory for VReader user data
            if (!Directory.Exists(libDir)) {
                Directory.CreateDirectory(libDir); // todo handle exceptions
            }
            
            // Each book has its' own directory - Renaming book filenames will create duplicates
            if (!Directory.Exists(outputDir)) {
                Directory.CreateDirectory(outputDir);
            }
            
            // For storing images - textures for book pages
            if (!Directory.Exists(outputDir + "/" + "pages")) {
                Directory.CreateDirectory(outputDir + "/" + "pages");
            }
            
            return outputDir;
        }

        private void ImportPdfInteractive(string path) {
            // todo
            
        }

        private string GetTitleFromPath(string path) {
            string[] pathParts = path.Split('.');
            
            if (pathParts.Length < 2){
                throw new InvalidBookFormatException("Invalid book format. Must include extension");
            }

            string filename = pathParts[pathParts.Length - 2];
            string[] filenameParts = filename.Split('/');
            return filenameParts[filenameParts.Length - 1];
        }

        // Backup original document so it can be re-imported if original is deleted - todo option to switch this off by preference
        private void CopyOriginal(string origin, string destination){
            File.Copy(origin, destination);
        }
        
    }
    
    public class InvalidBookFormatException : Exception {
        public InvalidBookFormatException(string message) : base(message){}
    }
}