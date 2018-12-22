using System;
using System.Collections;
using System.IO;
using SimpleFileBrowser;
using UnityEditor;
using UnityEngine;

namespace Ereader{
    
    public class BookImporter : MonoBehaviour {
        
        void Start() {
            FileBrowser.SetFilters(false, new FileBrowser.Filter( ".txt", ".pdf"));
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
        }

        private void ImportDotText(string path) {
            string[] lines = File.ReadAllLines(path);
            string.Join("\n", lines);
        }
        
        private void ImportPdf(string path){
            string imageOutputPath = Config.Instance.BookLibraryPath;
            string bookTitle = GetTitleFromPath(path);
            
            string outputDir = imageOutputPath + "/" + bookTitle;
            if (!Directory.Exists(outputDir)){
                Directory.CreateDirectory(outputDir); // todo handle exceptions
                Directory.CreateDirectory(outputDir + "/" + "pages");
            }
                        
            PdfConversion.ToJpegs(path, Config.Instance.BookLibraryPath);
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
    }

    public class InvalidBookFormatException : Exception {
        public InvalidBookFormatException(string message) : base(message){}
    }
}