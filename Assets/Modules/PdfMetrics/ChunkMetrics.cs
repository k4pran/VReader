using System;
using System.Text;

namespace PdfMetrics {
    public class ChunkMetrics : IComparable<ChunkMetrics>{

        private string chunkStr;
        private StringBuilder chunkSb;
        
        private bool bold;
        private bool italic;
        private bool strikethrough;
        private bool underline;
        private bool subscript;
        private bool superscript;
            
        private Color fillColor;
        private Color strokeColor;
        private string fontFamily;
        
        private Vector3D topLeft;
        private Vector3D topRight;
        private Vector3D bottomLeft;
        private Vector3D bottomRight;
        private float ascent;
        private float descent;
        private float baseline;
        private Rect rect;
        private float singleWhiteSpaceWidth;

        private int pageNb;
        private int lineNb;
                
        public ChunkMetrics(){
            chunkSb = new StringBuilder();
            chunkStr = "";
            bold = false;
            italic = false;

            ascent = -1;
            descent = -1;
            topRight = null;
            topLeft = null;
            bottomLeft = null;
            bottomRight = null;
            baseline = -1; // todo check for unset variables during get

            rect = null;
        }

        public Rect GetRect(){
            return new Rect(bottomLeft, bottomRight, topLeft, topRight);
        }

        public float GetFontSize(){
            return ascent - descent;
        }

        public string ChunkStr{
            get{ return chunkStr; }
            set{ chunkStr = value; }
        }

        public bool Bold{
            get{ return bold; }
            set{ bold = value; }
        }

        public bool Italic{
            get{ return italic; }
            set{ italic = value; }
        }

        public bool Strikethrough{
            get{ return strikethrough; }
            set{ strikethrough = value; }
        }

        public bool Underline{
            get{ return underline; }
            set{ underline = value; }
        }

        public bool Subscript{
            get{ return subscript; }
            set{ subscript = value; }
        }

        public bool Superscript{
            get{ return superscript; }
            set{ superscript = value; }
        }

        public Color FillColor{
            get{ return fillColor; }
            set{ fillColor = value; }
        }

        public Color StrokeColor{
            get{ return strokeColor; }
            set{ strokeColor = value; }
        }

        public string FontFamily{
            get{ return fontFamily; }
            set{ fontFamily = value; }
        }

        public float SingleWhiteSpaceWidth{
            get{ return singleWhiteSpaceWidth; }
            set{ singleWhiteSpaceWidth = value; }
        }

        public Vector3D TopLeft{
            get{ return topLeft; }
            set{ topLeft = value; }
        }

        public Vector3D TopRight{
            get{ return topRight; }
            set{ topRight = value; }
        }

        public Vector3D BottomLeft{
            get{ return bottomLeft; }
            set{ bottomLeft = value; }
        }

        public Vector3D BottomRight{
            get{ return bottomRight; }
            set{ bottomRight = value; }
        }

        public float Ascent{
            get{ return ascent; }
            set{ ascent = value; }
        }

        public float Descent{
            get{ return descent; }
            set{ descent = value; }
        }

        public float Baseline{
            get{ return baseline; }
            set{ baseline = value; }
        }

        public override string ToString(){
            return ChunkStr;
        }

        public int CompareTo(ChunkMetrics other){
            if (other == null || other.TopLeft == null || other.BottomRight == null ||
                this.topLeft == null || this.bottomRight == null){
                throw new Exception("Invalid comparison"); // todo
            }

            if (this.baseline > other.Baseline){
                return -1;
            }
            else if (this.baseline < other.Baseline){
                return 1;
            }
            else{
                if (this.BottomLeft.X < other.BottomLeft.X){
                    return -1;
                }
                else if (this.bottomLeft.X > other.BottomLeft.X){
                    return 1;
                }
            }
            return 1;
        }   
    }
}