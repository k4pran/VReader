using System;
using System.Collections.Generic;
using System.Text;

namespace PdfMetrics {
    public class PageMetrics{

        private int pageNumber;
        private List<LineMetrics> lineMetrices;
        private List<ChunkMetrics> chunkMetrices;
        private List<WordMetrics> wordMetrices;
        private List<CharMetrics> charMetrices;
        
        private string text;
        private float width;
        private float height;
        private float rotation;
        private float avgLineSpacing;

        public PageMetrics(int pageNumber){
            this.pageNumber = pageNumber;
            lineMetrices = new List<LineMetrics>();
            chunkMetrices = new List<ChunkMetrics>();
            text = "";
        }

        public void AddChunk(ChunkMetrics chunk){
            chunkMetrices.Add(chunk);
        }

        public void PrintLines(){
            foreach(LineMetrics line in lineMetrices){
                StringBuilder sb = new StringBuilder();
                foreach(ChunkMetrics chunkMetric in line.ChunkMetrices){
                    sb.Append(chunkMetric.ChunkStr);
                    sb.Append(" ");
                }
                Console.WriteLine(sb.ToString());
            }
        }

        public List<LineMetrics> LineMetrices{
            get{ return lineMetrices; }
            set{ lineMetrices = value; }
        }

        public List<ChunkMetrics> ChunkMetrices{
            get{ return chunkMetrices; }
            set{ chunkMetrices = value; }
        }

        public List<WordMetrics> WordMetrices{
            get{ return wordMetrices; }
            set{ wordMetrices = value; }
        }

        public List<CharMetrics> CharMetrices{
            get{ return charMetrices; }
            set{ charMetrices = value; }
        }

        public int PageNumber{
            get{ return pageNumber; }
        }

        public string Text{
            get{ return text; }
            set{ text = value; }
        }

        public float Width{
            get{ return width; }
            set{ width = value; }
        }

        public float Height{
            get{ return height; }
            set{ height = value; }
        }

        public float Rotation{
            get{ return rotation; }
            set{ rotation = value; }
        }

        public float AvgLineSpacing{
            get{ return avgLineSpacing; }
            set{ avgLineSpacing = value; }
        }
    }
}