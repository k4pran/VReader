using TMPro;
using UnityEngine;

namespace Ereader {
    
    [ExecuteInEditMode]
    public class TMP_WordFocusHandler : MonoBehaviour {
        
        public bool ShowWords;
        public bool ShowMeshBounds;
        public bool ShowTextBounds;
        public string ObjectStats;

        private TMP_Text m_TextComponent;

        private Transform m_Transform;
        
        void Awake()
        {
            m_TextComponent = gameObject.GetComponent<TMP_Text>();
            
            if (m_Transform == null)
                m_Transform = gameObject.GetComponent<Transform>();
        }

        void OnDrawGizmos()
        {
            // Update Text Statistics
            TMP_TextInfo textInfo = m_TextComponent.textInfo;

            ObjectStats = "Characters: " + textInfo.characterCount + "   Words: " + textInfo.wordCount + "   Spaces: " + textInfo.spaceCount + "   Sprites: " + textInfo.spriteCount + "   Links: " + textInfo.linkCount
                      + "\nLines: " + textInfo.lineCount + "   Pages: " + textInfo.pageCount;

            // Draw Quads around each of the words
            #region Draw Words

            if (ShowWords)
                DrawWordBounds(0); // todo pass word index from listener
            #endregion


            // Draw Quad around the bounds of the text
            #region Draw Bounds
            if (ShowMeshBounds)
                DrawBounds(); // todo pass word index from listener
            #endregion

            // Draw Quad around the rendered region of the text.
            #region Draw Text Bounds
            if (ShowTextBounds)
                DrawTextBounds(); // todo pass word index from listener
            #endregion
        }

        /// <summary>
        /// Method to draw rectangles around each word of the text.
        /// </summary>
        /// <param name="text"></param>
        public void DrawWordBounds(int wordIndex)
        {
            TMP_TextInfo textInfo = m_TextComponent.textInfo;

            TMP_WordInfo wInfo = textInfo.wordInfo[wordIndex];

            bool isBeginRegion = false;

            Vector3 bottomLeft = Vector3.zero;
            Vector3 topLeft = Vector3.zero;
            Vector3 bottomRight = Vector3.zero;
            Vector3 topRight = Vector3.zero;

            float maxAscender = -Mathf.Infinity;
            float minDescender = Mathf.Infinity;

            Color wordColor = Color.blue;

            // Iterate through each character of the word
            for (int j = 0; j < wInfo.characterCount; j++)
            {
                int characterIndex = wInfo.firstCharacterIndex + j;
                TMP_CharacterInfo currentCharInfo = textInfo.characterInfo[characterIndex];
                int currentLine = currentCharInfo.lineNumber;

                bool isCharacterVisible = characterIndex > m_TextComponent.maxVisibleCharacters ||
                                          currentCharInfo.lineNumber > m_TextComponent.maxVisibleLines ||
                                         (m_TextComponent.overflowMode == TextOverflowModes.Page && currentCharInfo.pageNumber + 1 != m_TextComponent.pageToDisplay) ? false : true;

                // Track Max Ascender and Min Descender
                maxAscender = Mathf.Max(maxAscender, currentCharInfo.ascender);
                minDescender = Mathf.Min(minDescender, currentCharInfo.descender);

                if (isBeginRegion == false && isCharacterVisible)
                {
                    isBeginRegion = true;

                    bottomLeft = new Vector3(currentCharInfo.bottomLeft.x, currentCharInfo.descender, 0);
                    topLeft = new Vector3(currentCharInfo.bottomLeft.x, currentCharInfo.ascender, 0);

                    // If Word is one character
                    if (wInfo.characterCount == 1)
                    {
                        isBeginRegion = false;

                        topLeft = m_Transform.TransformPoint(new Vector3(topLeft.x, maxAscender, 0));
                        bottomLeft = m_Transform.TransformPoint(new Vector3(bottomLeft.x, minDescender, 0));
                        bottomRight = m_Transform.TransformPoint(new Vector3(currentCharInfo.topRight.x, minDescender, 0));
                        topRight = m_Transform.TransformPoint(new Vector3(currentCharInfo.topRight.x, maxAscender, 0));

                        // Draw Region
                        DrawRectangle(topLeft, bottomRight, wordColor);
                    }
                }

                // Last Character of Word
                if (isBeginRegion && j == wInfo.characterCount - 1)
                {
                    isBeginRegion = false;

                    topLeft = m_Transform.TransformPoint(new Vector3(topLeft.x, maxAscender, 0));
                    bottomLeft = m_Transform.TransformPoint(new Vector3(bottomLeft.x, minDescender, 0));
                    bottomRight = m_Transform.TransformPoint(new Vector3(currentCharInfo.topRight.x, minDescender, 0));
                    topRight = m_Transform.TransformPoint(new Vector3(currentCharInfo.topRight.x, maxAscender, 0));

                    // Draw Region
                    DrawRectangle(topLeft, bottomRight, wordColor);

                    //Debug.Log("End Word Region at [" + currentCharInfo.character + "]");
                }
                // If Word is split on more than one line.
                else if (isBeginRegion && currentLine != textInfo.characterInfo[characterIndex + 1].lineNumber)
                {
                    isBeginRegion = false;

                    topLeft = m_Transform.TransformPoint(new Vector3(topLeft.x, maxAscender, 0));
                    bottomLeft = m_Transform.TransformPoint(new Vector3(bottomLeft.x, minDescender, 0));
                    bottomRight = m_Transform.TransformPoint(new Vector3(currentCharInfo.topRight.x, minDescender, 0));
                    topRight = m_Transform.TransformPoint(new Vector3(currentCharInfo.topRight.x, maxAscender, 0));

                    // Draw Region
                    DrawRectangle(topLeft, bottomRight, wordColor);
                    //Debug.Log("End Word Region at [" + currentCharInfo.character + "]");
                    maxAscender = -Mathf.Infinity;
                    minDescender = Mathf.Infinity;

                }
            }
        }

        /// <summary>
        /// Draw Rectangle around the bounds of the text object.
        /// </summary>
        void DrawBounds()
        {
            Bounds meshBounds = m_TextComponent.bounds;
            
            // Get Bottom Left and Top Right position of each word
            Vector3 bottomLeft = m_TextComponent.transform.position + (meshBounds.center - meshBounds.extents);
            Vector3 topRight = m_TextComponent.transform.position + (meshBounds.center + meshBounds.extents);

            DrawRectangle(bottomLeft, topRight, new Color(1, 0.5f, 0));
        }


        void DrawTextBounds()
        {
            Bounds textBounds = m_TextComponent.textBounds;

            Vector3 bottomLeft = m_TextComponent.transform.position + (textBounds.center - textBounds.extents);
            Vector3 topRight = m_TextComponent.transform.position + (textBounds.center + textBounds.extents);

            DrawRectangle(bottomLeft, topRight, new Color(0f, 0.5f, 0.5f));
        }

        // Draw Rectangles
        public void DrawRectangle(Vector3 tl, Vector3 br, Color color){
            Gizmos.color = color;
            float wordWidth = br.x - tl.x;
            float wordHeight = br.y - tl.y;
            Vector3 center = new Vector3(br.x - wordWidth / 2, br.y - wordHeight - 2);
            Gizmos.DrawCube(center, new Vector3(wordWidth, wordHeight));
        }
    }
}