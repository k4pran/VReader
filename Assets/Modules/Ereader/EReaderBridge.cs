using UnityEngine;

namespace Ereader{
    
    public class EReaderBridge : MonoBehaviour{

        public BookPro bookPro;
        public DynamicPageController dynamicPageController;
        
        void Start(){
            
            BasicBook book = new BasicBook("Assets/Resources/Books/dracula.txt");
            book.LoadBook();
            dynamicPageController.AddPaper(book.Pages[0].Tmp, book.Pages[1].Tmp);
            dynamicPageController.fitTMP(book.Pages[0].Tmp, 
                book.PaddingTop, book.PaddingBottom, book.PaddingLeft, book.PaddingRight);
            dynamicPageController.fitTMP(book.Pages[1].Tmp, 
                book.PaddingTop, book.PaddingBottom, book.PaddingLeft, book.PaddingRight);
        }

        private void Update(){
            
        }
    }
}