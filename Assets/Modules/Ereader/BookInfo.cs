namespace Ereader {
    
    public class BookInfo{

        public string title     { get; private set; }
        public BookFormat format  { get; private set; }
        public string origin    { get; private set; }
        public string thumbnailPath { get; private set; }

        public BookInfo(){}

        public BookInfo(string title, BookFormat format, string origin) {
            this.title = title;
            this.format = format;
            this.origin = origin;
        }
    }
}