using UnityEngine;

namespace Ereader{
    public class TextEventHandler : MonoBehaviour {
        
        public TextEventListener textEventListener;

        void OnEnable()
        {
            if (textEventListener != null)
            {
                textEventListener.onCharacterSelection.AddListener(OnCharacterSelection);
                textEventListener.onSpriteSelection.AddListener(OnSpriteSelection);
                textEventListener.onWordSelection.AddListener(OnWordSelection);
                textEventListener.onLineSelection.AddListener(OnLineSelection);
                textEventListener.onLinkSelection.AddListener(OnLinkSelection);
            }
        }


        void OnDisable()
        {
            if (textEventListener != null)
            {
                textEventListener.onCharacterSelection.RemoveListener(OnCharacterSelection);
                textEventListener.onSpriteSelection.RemoveListener(OnSpriteSelection);
                textEventListener.onWordSelection.RemoveListener(OnWordSelection);
                textEventListener.onLineSelection.RemoveListener(OnLineSelection);
                textEventListener.onLinkSelection.RemoveListener(OnLinkSelection);
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