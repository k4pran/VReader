using System.Collections.Generic;
using System.Text;
using TMPro;

namespace Ereader{
    
    public class TextStyler{
        
        private static Dictionary<int, int> mapToWordLength = new Dictionary<int, int>();

        public static void HighlightText(TMP_Text tmp, TMP_WordInfo wordInfo, int wordIndex) {

            // Add marked word with length
            if (!mapToWordLength.ContainsKey(wordInfo.firstCharacterIndex)){
                mapToWordLength.Add(wordInfo.firstCharacterIndex, wordInfo.characterCount);
            }
            // If already added and selected again, undo highlight.
            else{
                mapToWordLength.Remove(wordInfo.firstCharacterIndex);
            }

            StringBuilder sb = new StringBuilder();

            int countToCloseTag = 0;
            bool countingToClose = false;
            for(int i = 0; i < tmp.textInfo.characterCount; i++) {

                // If marked word begin a count of word length to close tag
                if (!countingToClose && mapToWordLength.TryGetValue(i, out countToCloseTag)){
                    sb.Append("<mark=#ffff00aa>");
                    countingToClose = true;
                }
                            
                sb.Append(tmp.textInfo.characterInfo[i].character);
                
                // Close tag after marked word ends
                if (countingToClose){
                    countToCloseTag--;
                    if (countToCloseTag == 0){
                        sb.Append("</mark>");
                        countingToClose = false;
                    }
                }
            }

            tmp.text = sb.ToString();

        }
    }
}