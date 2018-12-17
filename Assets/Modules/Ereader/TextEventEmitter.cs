using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Ereader{
    
    public class TextEventEmitter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
        
        [Serializable]
        public class CharacterHoverEvent : UnityEvent<char, int> { }

        [Serializable]
        public class SpriteHoverEvent : UnityEvent<char, int> { }

        [Serializable]
        public class WordHoverEvent : UnityEvent<string, int, int> { }

        [Serializable]
        public class LineHoverEvent : UnityEvent<string, int, int> { }

        [Serializable]
        public class LinkHoverEvent : UnityEvent<string, string, int> { }
        
        [Serializable]
        public class WordSelectEvent : UnityEvent<TMP_Text, TMP_WordInfo, int> { }

        /// <summary>
        /// Event delegate triggered when pointer is over a character.
        /// </summary>
        public CharacterHoverEvent OnCharacterHover
        {
            get { return _mOnCharacterHover; }
            set { _mOnCharacterHover = value; }
        }
        [SerializeField]
        private CharacterHoverEvent _mOnCharacterHover = new CharacterHoverEvent();


        /// <summary>
        /// Event delegate triggered when pointer is over a sprite.
        /// </summary>
        public SpriteHoverEvent OnSpriteHover
        {
            get { return _mOnSpriteHover; }
            set { _mOnSpriteHover = value; }
        }
        [SerializeField]
        private SpriteHoverEvent _mOnSpriteHover = new SpriteHoverEvent();


        /// <summary>
        /// Event delegate triggered when pointer is over a word.
        /// </summary>
        public WordHoverEvent OnWordHover
        {
            get { return _mOnWordHover; }
            set { _mOnWordHover = value; }
        }
        [SerializeField]
        private WordHoverEvent _mOnWordHover = new WordHoverEvent();


        /// <summary>
        /// Event delegate triggered when pointer is over a line.
        /// </summary>
        public LineHoverEvent OnLineHover
        {
            get { return _mOnLineHover; }
            set { _mOnLineHover = value; }
        }
        [SerializeField]
        private LineHoverEvent _mOnLineHover = new LineHoverEvent();


        /// <summary>
        /// Event delegate triggered when pointer is over a link.
        /// </summary>
        public LinkHoverEvent OnLinkHover
        {
            get { return _mOnLinkHover; }
            set { _mOnLinkHover = value; }
        }
        [SerializeField]
        private LinkHoverEvent _mOnLinkHover = new LinkHoverEvent();
        
        /// <summary>
        /// Event delegate triggered when mousedown on a word.
        /// </summary>
        public WordSelectEvent OnWordSelect
        {
            get { return _mOnWordSelect; }
            set { _mOnWordSelect = value; }
        }
        [SerializeField]
        private WordSelectEvent _mOnWordSelect= new WordSelectEvent();

        private TMP_Text m_TextComponent;

        private Camera m_Camera;
        private Canvas m_Canvas;

        private int m_selectedLink = -1;
        private int m_lastCharIndex = -1;
        private int m_lastWordIndex = -1;
        private int m_lastLineIndex = -1;

        void Awake()
        {
            // Get a reference to the text component.
            m_TextComponent = gameObject.GetComponent<TMP_Text>();

            // Get a reference to the camera rendering the text taking into consideration the text component type.
            if (m_TextComponent.GetType() == typeof(TextMeshProUGUI))
            {
                m_Canvas = gameObject.GetComponentInParent<Canvas>();
                if (m_Canvas != null)
                {
                    if (m_Canvas.renderMode == RenderMode.ScreenSpaceOverlay)
                        m_Camera = null;
                    else
                        m_Camera = m_Canvas.worldCamera;
                }
            }
            else
            {
                m_Camera = Camera.main;
            }
        }


        void LateUpdate()
        {
            if (TMP_TextUtilities.IsIntersectingRectTransform(m_TextComponent.rectTransform, Input.mousePosition, m_Camera))
            {
                #region Example of Character or Sprite Hover
                int charIndex = TMP_TextUtilities.FindIntersectingCharacter(m_TextComponent, Input.mousePosition, m_Camera, true);
                if (charIndex != -1 && charIndex != m_lastCharIndex)
                {
                    m_lastCharIndex = charIndex;

                    TMP_TextElementType elementType = m_TextComponent.textInfo.characterInfo[charIndex].elementType;

                    if (elementType == TMP_TextElementType.Character)
                        SendOnCharacterHover(m_TextComponent.textInfo.characterInfo[charIndex].character, charIndex);
                    else if (elementType == TMP_TextElementType.Sprite)
                        SendOnSpriteHover(m_TextComponent.textInfo.characterInfo[charIndex].character, charIndex);
                }
                #endregion


                #region Example of Word Hover
                int wordIndex = TMP_TextUtilities.FindIntersectingWord(m_TextComponent, Input.mousePosition, m_Camera);
                if (wordIndex != -1 && wordIndex != m_lastWordIndex)
                {
                    m_lastWordIndex = wordIndex;

                    TMP_WordInfo wInfo = m_TextComponent.textInfo.wordInfo[wordIndex];

                    SendOnWordHover(wInfo.GetWord(), wInfo.firstCharacterIndex, wInfo.characterCount);
                }
                
                if (wordIndex != -1 && Input.GetMouseButtonDown(0)) {

                    TMP_WordInfo wInfo = m_TextComponent.textInfo.wordInfo[wordIndex];
                    SendOnWordSelect(m_TextComponent, wInfo, wordIndex);
                    m_TextComponent.textInfo.wordInfo[70].firstCharacterIndex = 1000;
                }
                
                #endregion


                #region Example of Line Hover
                int lineIndex = TMP_TextUtilities.FindIntersectingLine(m_TextComponent, Input.mousePosition, m_Camera);
                if (lineIndex != -1 && lineIndex != m_lastLineIndex)
                {
                    m_lastLineIndex = lineIndex;

                    TMP_LineInfo lineInfo = m_TextComponent.textInfo.lineInfo[lineIndex];

                    char[] buffer = new char[lineInfo.characterCount];
                    for (int i = 0; i < lineInfo.characterCount && i < m_TextComponent.textInfo.characterInfo.Length; i++)
                    {
                        buffer[i] = m_TextComponent.textInfo.characterInfo[i + lineInfo.firstCharacterIndex].character;
                    }

                    string lineText = new string(buffer);
                    SendOnLineHover(lineText, lineInfo.firstCharacterIndex, lineInfo.characterCount);
                }
                #endregion


                #region Example of Link Hover
                int linkIndex = TMP_TextUtilities.FindIntersectingLink(m_TextComponent, Input.mousePosition, m_Camera);

                if (linkIndex != -1 && linkIndex != m_selectedLink)
                {
                    m_selectedLink = linkIndex;

                    TMP_LinkInfo linkInfo = m_TextComponent.textInfo.linkInfo[linkIndex];

                    SendOnLinkHover(linkInfo.GetLinkID(), linkInfo.GetLinkText(), linkIndex);
                }
                #endregion
            }
        }


        public void OnPointerEnter(PointerEventData eventData) {
//            Debug.Log("OnPointerEnter()");
        }


        public void OnPointerExit(PointerEventData eventData) {
//            Debug.Log("OnPointerExit()");
        }


        private void SendOnCharacterHover(char character, int characterIndex) {
            if (OnCharacterHover != null)
                OnCharacterHover.Invoke(character, characterIndex);
        }

        private void SendOnSpriteHover(char character, int characterIndex) {
            if (OnSpriteHover != null)
                OnSpriteHover.Invoke(character, characterIndex);
        }

        private void SendOnWordHover(string word, int charIndex, int length) {
            if (OnWordHover != null)
                OnWordHover.Invoke(word, charIndex, length);
        }

        private void SendOnLineHover(string line, int charIndex, int length) {
            if (OnLineHover != null)
                OnLineHover.Invoke(line, charIndex, length);
        }

        private void SendOnLinkHover(string linkID, string linkText, int linkIndex) {
            if (OnLinkHover != null)
                OnLinkHover.Invoke(linkID, linkText, linkIndex);
        }
        
        private void SendOnWordSelect(TMP_Text tmp, TMP_WordInfo wordInfo, int wordIndex) {
            if (OnWordSelect != null)
                OnWordSelect.Invoke(tmp, wordInfo, wordIndex);
        }
    }
}