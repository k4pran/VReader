using UnityEngine;

namespace Ereader{
    
    public class EReaderBridge : MonoBehaviour{

        public BookPro bookPro;
        public DynamicPageController dynamicPageController;
        private BasicBook book;
        
        void Start(){
                        
            book = new BasicBook("Assets/Resources/Books/dracula.txt");
            book.LoadBook();
            bookPro.OnFlip.AddListener(pageChanged);

            for(int i = 1; i < 3; i += 2){
                
                Paper paper = dynamicPageController.AddPaper();
                
                book.Pages[i - 1].SetParent(paper.Front);
                book.Pages[i].SetParent(paper.Back);
                
                dynamicPageController.fitTMP(book.Pages[i - 1].Tmp, 
                    book.PaddingTop, book.PaddingBottom, book.PaddingLeft, book.PaddingRight);
                dynamicPageController.fitTMP(book.Pages[i].Tmp, 
                    book.PaddingTop, book.PaddingBottom, book.PaddingLeft, book.PaddingRight);
            }  
        }

        public void pageChanged() {
            book.GoTo((bookPro.CurrentPaper - 1) * 2);
            Debug.Log((bookPro.CurrentPaper - 1) * 2);
        }

        private void Update(){
            
        }
    }
}