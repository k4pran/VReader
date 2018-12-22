using System.Linq;
using System.Text;

namespace PdfMetrics {
    public class Styler{

        private static float scaler = 1;
        
        public static void StyleWord(ChunkMetrics chunkMetric, bool openOnly = false){
            string tmp = chunkMetric.ChunkStr;
            chunkMetric.ChunkStr = "";
            StringBuilder sb = new StringBuilder();
            
            sb.AppendFormat("<size={0}pt>", chunkMetric.GetFontSize() * (scaler - 1));

            sb.AppendFormat("<color={0}>", chunkMetric.FillColor.AsHex());

            if(chunkMetric.Bold){
                sb.Append("<b>");
            }

            if (chunkMetric.Italic){
                sb.Append("<i>");
            }

            if (chunkMetric.Strikethrough){
                sb.Append("<s>");
            }

            if (chunkMetric.Underline){
                sb.Append("<u>");
            }

            if (chunkMetric.Superscript){
                sb.Append("<sup>");
            }
            else if (chunkMetric.Subscript){
                sb.Append("<sub>");
            }

            if (openOnly){
                sb.Append(tmp);
//                sb.Append("</font>");
                chunkMetric.ChunkStr = sb.ToString();
                return;
            }

            if (chunkMetric.Subscript){
                sb.Append("</sub>");
            }
            else if (chunkMetric.Superscript){
                sb.Append("</sup>");
            }

            if (chunkMetric.Underline){
                sb.Append("</u>");
            }
            
            sb.Append(chunkMetric);
            
            if (chunkMetric.Strikethrough){
                sb.Append("</s>");
            }
            
            if(chunkMetric.Bold){
                sb.Append("</b>");
            }

            if (chunkMetric.Italic){
                sb.Append("</i>");
            }
            
//            sb.Append("</font>");
            chunkMetric.ChunkStr = tmp;
        }

        /// <summary>
        /// Style line instead of word by word. Some tags such as bold, italic, font etc will be added to multiple words
        /// Less common styles added on each word it applies.
        /// </summary>
        /// <param name="line"></param>
        public static void StyleLine(LineMetrics line){
            StringBuilder sb = new StringBuilder();
            ChunkMetrics prev = line.ChunkMetrices.ElementAt(0);
            StyleWord(prev, true);

            for(int i = 1; i < line.ChunkMetrices.Count; i++){
                StringBuilder closeTags = new StringBuilder();
                
                if (line.ChunkMetrices.ElementAt(i).FontFamily != prev.FontFamily){
                    
                }

                if (line.ChunkMetrices.ElementAt(i).FillColor.AsHex() != prev.FillColor.AsHex()){
                    sb.AppendFormat("<color={0}>", line.ChunkMetrices.ElementAt(i).FillColor.AsHex());
                }
                
                if (line.ChunkMetrices.ElementAt(i).GetFontSize() != prev.GetFontSize()){
                    sb.AppendFormat("<size={0}>", line.ChunkMetrices.ElementAt(i).GetFontSize() * (scaler - 1));
                }

                if (line.ChunkMetrices.ElementAt(i).Bold){
                    if (!prev.Bold){
                        sb.Append("<b>");
                    }
                }
                else{
                    if (prev.Bold){
                        closeTags.Append("</b>");
                    }
                }
                
                if (line.ChunkMetrices.ElementAt(i).Italic){
                    if (!prev.Italic){
                        sb.Append("<i>");
                    }
                }
                else{
                    if (prev.Italic){
                        closeTags.Append("</i>");
                    }
                }

                // The bottom four decorating will be less common and so added word by word
                if (line.ChunkMetrices.ElementAt(i).Strikethrough){
                    sb.Append("<s>");
                    closeTags.Append("</s>");
                }

                if (line.ChunkMetrices.ElementAt(i).Underline){
                    sb.Append("<u>");
                    closeTags.Append("</u>");
                }

                if (line.ChunkMetrices.ElementAt(i).Superscript){
                    sb.Append("<sup>");
                    closeTags.Append("</sup");
                }

                if (line.ChunkMetrices.ElementAt(i).Subscript){
                    sb.Append("<sub>");
                    closeTags.Append("</sub>");
                }
                
                sb.Append(line.ChunkMetrices.ElementAt(i));
                sb.Append(closeTags);
                line.ChunkMetrices.ElementAt(i).ChunkStr = sb.ToString();
            }

            sb = new StringBuilder();
            sb.Append(line.ChunkMetrices.ElementAt(line.ChunkMetrices.Count - 1).ChunkStr);

            if (line.ChunkMetrices.ElementAt(line.ChunkMetrices.Count - 1).Bold){
                sb.Append("</b>");
            }
            if (line.ChunkMetrices.ElementAt(line.ChunkMetrices.Count - 1).Italic){
                sb.Append("</i>");
            }
            if (line.ChunkMetrices.ElementAt(line.ChunkMetrices.Count - 1).Strikethrough){
                sb.Append("</s>");
            }
            if (line.ChunkMetrices.ElementAt(line.ChunkMetrices.Count - 1).Underline){
                sb.Append("</u>");
            }
            if (line.ChunkMetrices.ElementAt(line.ChunkMetrices.Count - 1).Superscript){
                sb.Append("</sup>");
            }
            if (line.ChunkMetrices.ElementAt(line.ChunkMetrices.Count - 1).Subscript){
                sb.Append("</sub>");
            }
            line.ChunkMetrices.ElementAt(line.ChunkMetrices.Count - 1).ChunkStr = sb.ToString();
        }

        public static void SetScaler(float scale){
            scaler = scale;
        }
    }
}