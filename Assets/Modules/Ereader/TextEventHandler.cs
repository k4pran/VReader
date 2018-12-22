using System;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace Ereader{
    public class TextEventHandler : MonoBehaviour {
        
        public TextEventEmitter textEventEmitter;

        #region Inspector Debugging Options

        [Header("Debug Handlers")]
        public bool CharHandlersOn;
        public bool SpriteHandlersOn;
        public bool WordHandlersOn;
        public bool LineHandlersOn;
        public bool LinkHandlersOn;

        [Header("Debug Styling")] 
        public TextSelectModes textSelectMode;
                
        #endregion

        void Start(){
            
            textSelectMode = TextSelectModes.WIKIPEDIA;
            
            if (textEventEmitter != null) {
                addAll();
            }
        }

        void OnEnable()
        {
            if (textEventEmitter != null) {
                addAll();
            }
        }
        
        #region Add/Remove Handlers

        private void addAll() {
            textEventEmitter.OnCharacterHover.AddListener(OnCharacterHoverLog);
            textEventEmitter.OnSpriteHover.AddListener(OnSpriteHoverLog);
            textEventEmitter.OnWordHover.AddListener(OnWordHoverLog);
            textEventEmitter.OnLineHover.AddListener(OnLineHoverLog);
            textEventEmitter.OnLinkHover.AddListener(OnLinkHoverLog);
            textEventEmitter.OnWordSelect.AddListener(OnWordSelectionLog);
            
            // for debugging purposes - methods to be replaced with VR interactions.
            textEventEmitter.OnWordSelect.AddListener(HandleMouseCLick); 
        }


        void OnDisable()
        {
            if (textEventEmitter != null) {      
                textEventEmitter.OnCharacterHover.RemoveListener(OnCharacterHoverLog);
                textEventEmitter.OnSpriteHover.RemoveListener(OnSpriteHoverLog);
                textEventEmitter.OnWordHover.RemoveListener(OnWordHoverLog);
                textEventEmitter.OnLineHover.RemoveListener(OnLineHoverLog);
                textEventEmitter.OnLinkHover.RemoveListener(OnLinkHoverLog);
                textEventEmitter.OnWordSelect.RemoveListener(OnWordSelectionLog);
            }
        }
        
        #endregion    

        void HandleMouseCLick(TMP_Text textComponent, TMP_WordInfo wordInfo, int wordIndex) {

            switch(textSelectMode) {
                
                case TextSelectModes.HIGHLIGHT:
                    TextStyler.HighlightText(textComponent, wordInfo, wordIndex);
                    break;
                
                case TextSelectModes.WIKIPEDIA:
                    WikiBridge.LogResults(wordInfo.GetWord());
                    break;
                
                default:
                    Debug.Log("No mode selected");
                    break;
                    
            }
            
        }

        #region LoggerHandlers

        void OnWordSelectionLog(TMP_Text tmp, TMP_WordInfo wordInfo, int wordIndex){
            if (!WordHandlersOn) {
                return;
            }
            
            Debug.Log("Word [" + wordInfo.GetWord() + "] with first character index of " + wordInfo.firstCharacterIndex 
                      + " and length of " +  wordInfo.characterCount + " has been selected.");
        }

        void OnCharacterHoverLog(char c, int index) {
            
            if (!CharHandlersOn){
                return;
            }
            
            Debug.Log("Character [" + c + "] at Index: " + index + " has been hovered.");
        }

        void OnSpriteHoverLog(char c, int index) {
            
            if (!SpriteHandlersOn){
                return;
            }
            
            Debug.Log("Sprite [" + c + "] at Index: " + index + " has been hovered.");
        }

        void OnWordHoverLog(string word, int firstCharacterIndex, int length) {
            
            if (!WordHandlersOn){
                return;
            }
            
            Debug.Log("Word [" + word + "] with first character index of " + firstCharacterIndex + " and length of " + 
                      length + " has been hovered.");
        }

        void OnLineHoverLog(string lineText, int firstCharacterIndex, int length) {
            
            if (!LineHandlersOn){
                return;
            }
            
            Debug.Log("Line [" + lineText + "] with first character index of " + firstCharacterIndex + 
                      " and length of " + length + " has been hovered.");
        }

        void OnLinkHoverLog(string linkID, string linkText, int linkIndex) {
            
            if (!LinkHandlersOn){
                return;
            }
            
            Debug.Log("Link Index: " + linkIndex + " with ID [" + linkID + "] and Text \"" + linkText + "\" " +
                      "has been hovered.");
        }
        
        #endregion
        
    }
}