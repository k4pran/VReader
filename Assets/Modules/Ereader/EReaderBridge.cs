using UnityEngine;

namespace Ereader{
    
    public class EReaderBridge : MonoBehaviour{

        public BookPro bookPro;
        public DynamicPageController dynamicPageController;
        private Book book;
        
        void Start(){
                        
            BasicBook book = new BasicBook("Assets/Resources/Books/dracula.txt");
            book.LoadBook();
            bookPro.OnFlip.AddListener(pageChanged);

            for(int i = 1; i < book.PageCount(); i += 2){
                
                dynamicPageController.AddPaper(book.Pages[i - 1].Tmp, book.Pages[i].Tmp);
                
                dynamicPageController.fitTMP(book.Pages[i - 1].Tmp, 
                    book.PaddingTop, book.PaddingBottom, book.PaddingLeft, book.PaddingRight);
                dynamicPageController.fitTMP(book.Pages[i].Tmp, 
                    book.PaddingTop, book.PaddingBottom, book.PaddingLeft, book.PaddingRight);
            }  
        }

        // todo enable only pages currently displayed
        public void pageChanged(){
            Debug.Log((bookPro.CurrentPaper - 1) * 2);
        }

        private void Update(){
            
        }
    }
}