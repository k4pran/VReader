using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ereader{
    public class PageControl : MonoBehaviour{
    
        private string[] lines;

        private BasicBook book;
        private int linePos;
        private int loadFromPos;
        private int loadToPos;

        public Button backButton, nextButton;
        public InputField goTo;
        private bool wasFocused;

        // Use this for initialization
        void Start (){

    //	    SVGImporter svgImporter = new SVGImporter();
    //	    svgImporter.SvgPixelsPerUnit = 1f;
    //	    
    //	    var svg = Resources.Load<Sprite>("vec_test");
    //	    GameObject gameObject = new GameObject();
    //	    gameObject.name = "svg_test";
    //	    gameObject.AddComponent<SpriteRenderer>();
    //	    SpriteRenderer sprRenderer = gameObject.GetComponent<SpriteRenderer>();
    //	    sprRenderer.sprite = svg; todo - figure how to custom import and set pixelsPU to 1 then scale.
                    
    //      var epub = new EPubBook("/Users/Shared/Unity/book_testing/Assets/EPubReader/UnityEPubReader/Assets/Books/austen-pride-and-prejudice-illustrations.epub");
    //      epub.LoadBook();
    //	    
    //	    PdfBook book = new PdfBook("/Users/ryan/RiderProjects/Sharpen Pdf Parser/test_new.json");
    //	    book.LoadBook();
    //	    book.Display();
                // next 65 -300 160 30
                // back -78 -300 160 30
                // can 5 202 300 300
    //            
            book = new BasicBook("/Users/Shared/Unity/book_testing/Assets/dracula.txt");
            book.LoadBook();
            book.Display();
            goTo.text = book.CurrentPageNum().ToString();
                
            backButton.GetComponent<Button>().onClick.AddListener(Back);
            nextButton.GetComponent<Button>().onClick.AddListener(Next);
            goTo.characterValidation = InputField.CharacterValidation.Integer;
            goTo.onValueChanged.AddListener(delegate {OnGotoValueChanged(goTo.text, book.PageCount());});
        }
		
        // Update is called once per frame
        void Update (){
            if (wasFocused && Input.GetKeyDown(KeyCode.Return)){
                book.GoTo(int.Parse(goTo.text));
                Debug.Log(goTo.text);
            }
            wasFocused = goTo.isFocused;
        }

        public void Back(){
            book.Back();
            goTo.text = book.CurrentPageNum().ToString();
        }

        public void Next(){
            book.Next();
            goTo.text = book.CurrentPageNum().ToString();
        }

        public void OnGotoValueChanged(string text, int lastPageNum){

            text = text.Length == 0 ? "1" : text;
            int pageNum = int.Parse(text);
            pageNum = pageNum < 1 ? 1 : pageNum;
            pageNum = pageNum > lastPageNum ? lastPageNum : pageNum;
            goTo.text = pageNum.ToString();
        }        
    }    
}
