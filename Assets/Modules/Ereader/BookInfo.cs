namespace Ereader {
    
    // Contains info on books to facilitate persistence, loading etc
    public class BookInfo{

        public string title     { get; private set; }
        public BookFormat format  { get; private set; }
        public string origin    { get; private set; }
        public string thumbnailPath { get; set; }

        public BookInfo(){}

        public BookInfo(string title, BookFormat format, string origin) {
            this.title = title;
            this.format = format;
            this.origin = origin;
            this.thumbnailPath = "";
        }
    }
}