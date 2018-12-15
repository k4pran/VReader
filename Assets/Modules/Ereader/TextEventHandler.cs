using UnityEngine;

namespace Ereader{
    public class TextEventHandler : MonoBehaviour {
        
        public TextEventEmitter textEventEmitter;

        void Start(){
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

        private void addAll() {
            textEventEmitter.onCharacterSelection.AddListener(OnCharacterSelection);
            textEventEmitter.onSpriteSelection.AddListener(OnSpriteSelection);
            textEventEmitter.onWordSelection.AddListener(OnWordSelection);
            textEventEmitter.onLineSelection.AddListener(OnLineSelection);
            textEventEmitter.onLinkSelection.AddListener(OnLinkSelection);
        }


        void OnDisable()
        {
            if (textEventEmitter != null) {      
                textEventEmitter.onCharacterSelection.RemoveListener(OnCharacterSelection);
                textEventEmitter.onSpriteSelection.RemoveListener(OnSpriteSelection);
                textEventEmitter.onWordSelection.RemoveListener(OnWordSelection);
                textEventEmitter.onLineSelection.RemoveListener(OnLineSelection);
                textEventEmitter.onLinkSelection.RemoveListener(OnLinkSelection);
            }
        }


        void OnCharacterSelection(char c, int index)
        {
            Debug.Log("Character [" + c + "] at Index: " + index + " has been selected.");
        }

        void OnSpriteSelection(char c, int index)
        {
            Debug.Log("Sprite [" + c + "] at Index: " + index + " has been selected.");
        }

        void OnWordSelection(string word, int firstCharacterIndex, int length)
        {
            Debug.Log("Word [" + word + "] with first character index of " + firstCharacterIndex + " and length of " + length + " has been selected.");
        }

        void OnLineSelection(string lineText, int firstCharacterIndex, int length)
        {
            Debug.Log("Line [" + lineText + "] with first character index of " + firstCharacterIndex + " and length of " + length + " has been selected.");
        }

        void OnLinkSelection(string linkID, string linkText, int linkIndex)
        {
            Debug.Log("Link Index: " + linkIndex + " with ID [" + linkID + "] and Text \"" + linkText + "\" has been selected.");
        }
        
    }
}