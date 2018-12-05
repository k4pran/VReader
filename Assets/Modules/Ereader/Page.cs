using System;
using TMPro;
using UnityEngine;

namespace Ereader{
    [Serializable] 
    public class Page{
            
        private TextMeshPro tmp;
        private TMP_TextEventHandler textHandler;
        private string objName;
        private int pageNum;

        private Vector2 preferredVals;

        public Page(TextMeshPro tmp, TMP_TextEventHandler textHandler, string objName, int pageNum){
            this.tmp = tmp;
            this.textHandler = textHandler;
            this.objName = objName;
            this.pageNum = pageNum;
            
            SetHandlers();
            Disable();
        }

        private void SetHandlers(){
            textHandler.onWordSelection.AddListener(WordHovered);
            textHandler.onMouseHoldClick.AddListener(WordFocused);
        }

        private void WordHovered(string word, int x, int y){
            Debug.Log("word " + word + " selected");
        }

        private void WordFocused(string word, int x, int y){
            Debug.Log("Word = " + word);
        }

        public void Update() {
            tmp.ForceMeshUpdate();
        }

        public void Disable(){
            tmp.enabled = false;
            textHandler.enabled = false;
        }

        public void Enable() {
            tmp.enabled = true;
            textHandler.enabled = true;
        }

        public string GetText(){
            return tmp.text;
        }
        
        public static Page GetBlank(){
            GameObject gameObj = new GameObject("BlankTMP");
            gameObj.AddComponent<TextMeshPro>();
            TMP_TextEventHandler textHandler = gameObj.AddComponent<TMP_TextEventHandler>();
            TextMeshPro tmp = gameObj.GetComponent<TextMeshPro>();
            tmp.text = "";
            return new Page(tmp, textHandler, "BlankTMP", -1);
        }

        public Vector2 PreferredVals{
            get{ return preferredVals; }
            set{ preferredVals = value; }
        }

        public TextMeshPro Tmp{
            get{ return tmp; }
        }

        public class TextIndex {
            int lineIndex;
            int charIndex;

            public TextIndex(int lineIndex, int charIndex){
                this.lineIndex = lineIndex;
                this.charIndex = charIndex;
            }

            public int LineIndex{
                get{ return lineIndex; }
                set{ lineIndex = value; }
            }

            public int CharIndex{
                get{ return charIndex; }
                set{ charIndex = value; }
            }
        }
    }
}