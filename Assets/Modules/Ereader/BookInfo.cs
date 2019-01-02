namespace Ereader {
    
    public class BookInfo{

        public string title     { get; private set; }
        public BookFormat type  { get; private set; }
        public string origin    { get; private set; }

        public BookInfo(){}

        public BookInfo(string title, BookFormat type, string origin) {
            this.title = title;
            this.type = type;
            this.origin = origin;
        }
    }
}