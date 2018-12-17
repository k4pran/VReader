using System;
using TMPro;
using UnityEngine;

namespace Ereader{
    [Serializable] 
    public class Page {
            
        private TextMeshProUGUI tmp;
        private TextEventEmitter textEventEmitter;
        private TextEventHandler textEventHandler;
        private string objName;
        private int pageNum;

        private Vector2 preferredVals;

        public Page(TextMeshProUGUI tmp, string objName, int pageNum){
            this.tmp = tmp;
            this.tmp.enableAutoSizing = true;
            this.tmp.fontSizeMin = 10;
            this.tmp.color = Color.black;
            this.objName = objName;
            this.pageNum = pageNum;
            
            textEventEmitter = tmp.gameObject.AddComponent<TextEventEmitter>();
            GameObject textEventProcessor = new GameObject("TextEventProcessor" + pageNum);
            textEventHandler = textEventProcessor.AddComponent<TextEventHandler>();

            Disable();
            SetHandler();
        }

        private void SetHandler(){
            textEventHandler.textEventEmitter = textEventEmitter;
        }

        public void Update() {
            tmp.ForceMeshUpdate();
        }

        public void Disable(){
            tmp.enabled = false;
            textEventEmitter.enabled = false;
        }

        public void Enable() {
            tmp.enabled = true;
            textEventEmitter.enabled = true;
        }

        public string GetText(){
            return tmp.text;
        }
        
        public static Page GetBlank(){
            GameObject gameObj = new GameObject("BlankTMP");
            gameObj.AddComponent<TextMeshProUGUI>();
            TextMeshProUGUI tmp = gameObj.GetComponent<TextMeshProUGUI>();
            tmp.text = "";
            return new Page(tmp, "BlankTMP", -1);
        }

        public Vector2 PreferredVals{
            get{ return preferredVals; }
            set{ preferredVals = value; }
        }

        public TextMeshProUGUI Tmp{
            get{ return tmp; }
        }

        public void SetParent(GameObject parent){
            tmp.transform.parent = parent.transform;
            textEventHandler.transform.parent = parent.transform;
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