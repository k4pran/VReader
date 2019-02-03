using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Ereader {
    
    public class EReaderBridge : MonoBehaviour {

        public BookPro bookPro;
        public DynamicPageController dynamicPageController;
        private HashSet<string> books;
        private Book book;
        
        void Start(){

            if (books == null){
                LoadLibrary();
            }                        
        }

        public void PageChanged() {
            book.GoTo((bookPro.CurrentPaper - 1) * 2);
            Debug.Log((bookPro.CurrentPaper - 1) * 2);
        }

        public void InitBook(string title) {
            
            if (books == null){
                LoadLibrary();
            }    

            if (!books.Contains(title)){
                // throw error
            }

            BookFormat bookType = BookInfoMapper.DeserializeInfo(title).format;

            switch(bookType) {
                    
                case BookFormat.TXT:
                    InitTxtBasedBook(title);
                    break;
                
                default:
                    Debug.Log("Invalid book format");
                    return;
            }
        }

        public void InitTxtBasedBook(string title) {
            book = new BasicBook(title);
            
            book.LoadBook();
            bookPro.OnFlip.AddListener(PageChanged);

            for(int i = 1; i < book.PageCount(); i += 2){
                
                Paper paper = dynamicPageController.AddPaper();
                
                book.GetPages()[i - 1].SetParent(paper.Front);
                book.GetPages()[i].SetParent(paper.Back);
                
                dynamicPageController.fitTMP(book.GetPages()[i - 1].Tmp, 
                    book.PaddingTop, book.PaddingBottom, book.PaddingLeft, book.PaddingRight);
                dynamicPageController.fitTMP(book.GetPages()[i].Tmp, 
                    book.PaddingTop, book.PaddingBottom, book.PaddingLeft, book.PaddingRight);
            }  
        }

        public void LoadLibrary() {
            string libDir = Config.Instance.BookLibraryPath + "/VReader";
            string bookLog = libDir + "/book-log.txt";

            // Empty library - lib directory created on first import
            if (!Directory.Exists(libDir) || !File.Exists(bookLog)) {
                return;
            }
            
            StreamReader file = new StreamReader(bookLog);
            string line;
            books = new HashSet<string>();
            while((line = file.ReadLine()) != null) {
                if (line.Length > 0) books.Add(line);
            }  
            
            Debug.Log("Loaded " + books.Count + " books into library");
        }
    }
}