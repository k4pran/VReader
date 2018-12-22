using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PdfMetrics {
    public class LineMetrics : IComparable<LineMetrics>{
        private float ascent;
        private float baseline;
        private float descent;
        private float lineSpacingAbove;
        private float lineSpacingBelow;
        private float leftMostPos;
        private float rightMostPos;
        private List<ChunkMetrics> chunkMetrices;

        public LineMetrics(float ascent, float baseline, float descent){
            chunkMetrices = new List<ChunkMetrics>();
            this.ascent = ascent;
            this.baseline = baseline;
            this.descent = descent;
        }

        public string PaddedLine(float pageWidth, float spaceHeight, float spaceWidth, float fontSize){

            int newlinesAbove = TopPaddingCount(spaceHeight);
            
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<size={0}pt>", fontSize);
            for(; newlinesAbove > 0; newlinesAbove--){
                sb.Append('\n');
            }

            sb.Append(AddLineMargins(pageWidth, spaceWidth));
            return sb.ToString();
        }
        
        /// <summary>
        /// Uses width of pdf and positions of first / last chunks to append space
        /// characters which is easier than absolute positioning in Text mesh pro
        /// </summary>
        public string AddLineMargins(float pageWidth, float spaceWidth){
            float leftMost = chunkMetrices.ElementAt(0).BottomLeft.X;
            float rightMost = chunkMetrices.ElementAt(chunkMetrices.Count - 1).BottomRight.X;

            int leftSpaceCount = ((int) Math.Floor(leftMost / spaceWidth));
//            int rightSpaceCount = (int) Math.Ceiling(
//                (pageWidth - rightMost) / spaceWidth);

            StringBuilder sb = new StringBuilder();
            
            for(; leftSpaceCount > 0; leftSpaceCount--){
                sb.Append(' ');
            }

            sb.Append(String.Join(" ", chunkMetrices.Select(x => x.ToString()).ToArray()));

//            for(; rightSpaceCount > 0; rightSpaceCount--){
//                sb.Append(' ');
//            }
            return sb.ToString();
        }

        public void AddChunks(List<ChunkMetrics> chunks){
            this.chunkMetrices.AddRange(chunks);
        }

        public void AddChunk(ChunkMetrics chunk){
            chunkMetrices.Add(chunk);
        }

        public int TopPaddingCount(float spaceHeight){
            return (int) Math.Ceiling(LineSpacingAbove / spaceHeight);
        }

        public List<ChunkMetrics> ChunkMetrices{
            get{ return chunkMetrices; }
        }

        public float Ascent{
            get{ return ascent; }
            set{ ascent = value; }
        }

        public float Baseline{
            get{ return baseline; }
            set{ baseline = value; }
        }

        public float Descent{
            get{ return descent; }
            set{ descent = value; }
        }

        public float LineSpacingAbove{
            get{ return lineSpacingAbove; }
            set{ lineSpacingAbove = value; }
        }

        public float LineSpacingBelow{
            get{ return lineSpacingBelow; }
            set{ lineSpacingBelow = value; }
        }

        public float LeftMostPos{
            get{ return leftMostPos; }
            set{ leftMostPos = value; }
        }

        public float RightMostPos{
            get{ return rightMostPos; }
            set{ rightMostPos = value; }
        }

        public float ContentHeight(){
            return Math.Abs(ascent - descent);
        }
        
        public float ContentWidth(){
            return Math.Abs(RightMostPos - LeftMostPos);
        }

        public override bool Equals(object obj){
            if (obj == null){
                return false;
            }
            
            var isCorrectType = obj is LineMetrics;
            if (!isCorrectType){
                return false;
            }

            LineMetrics toCompare = (LineMetrics) obj;
            return this.baseline == toCompare.baseline;
        }

        public override string ToString(){
            return String.Join(" ", ChunkMetrices.Select(x => x.ToString()).ToArray());
        }

        public override int GetHashCode(){
            int hash = 13;
            hash = (hash * 7) + baseline.GetHashCode();
            return hash;
        }
        
        public int CompareTo(LineMetrics other){
            if (other == null) {
                throw new Exception("Invalid comparison"); // todo
            }

            if (baseline > other.baseline){
                return -1;
            }
            if (baseline < other.baseline){
                return 1;
            }
            return 1;
        }
    }
}