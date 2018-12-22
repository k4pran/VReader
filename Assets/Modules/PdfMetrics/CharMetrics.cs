using System;

namespace PdfMetrics {
    public class CharMetrics : IComparable<CharMetrics>{
        private char c;

        private float fontSize;

        private Vector3D topLeft;
        private Vector3D bottomLeft;
        private Vector3D topRight;
        private Vector3D bottomRight;

        public CharMetrics(char c){
            this.c = c;
        }

        public char C{
            get{ return c; }
            set{ c = value; }
        }

        public float FontSize{
            get{ return fontSize; }
            set{ fontSize = value; }
        }

        public Vector3D TopLeft{
            get{ return topLeft; }
            set{ topLeft = value; }
        }

        public Vector3D BottomLeft{
            get{ return bottomLeft; }
            set{ bottomLeft = value; }
        }

        public Vector3D TopRight{
            get{ return topRight; }
            set{ topRight = value; }
        }

        public Vector3D BottomRight{
            get{ return bottomRight; }
            set{ bottomRight = value; }
        }

        public int CompareTo(CharMetrics other){
            if (other == null || other.TopLeft == null || other.BottomRight == null ||
                topLeft == null || bottomRight == null){
                throw new Exception("Invalid comparison"); // todo
            }

            if (bottomLeft.Y > other.BottomLeft.Y){
                return -1;
            }
            if (bottomLeft.Y < other.BottomLeft.Y){
                return 1;
            }
            if (bottomLeft.X < other.BottomLeft.X){
                return -1;
            }
            if (bottomLeft.X > other.BottomLeft.X){
                return 1;
            }
            return 1;
        }
    }
}