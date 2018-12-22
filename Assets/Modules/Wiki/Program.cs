using System;

namespace WikiQueries{
    internal class Program{
        public static void Main(string[] args){

            // Check if page exists
            bool doesExist = Wiki.DoesPageExist("shouldnotexist"); // false
            doesExist = Wiki.DoesPageExist("snow"); // true
            doesExist = Wiki.DoesPageExist("lion"); // true
            doesExist = Wiki.DoesPageExist("nonsensepage"); // false

            // Build custom query
            QueryBuilder queryBuilder = new QueryBuilder("https://en.wikipedia.org/w/api.php?");
            queryBuilder.AppendParam("action", "query");
            queryBuilder.AppendMultiValParam("prop", new []{"info", "description"});
            queryBuilder.AppendParam("titles", "snow");
            string result = Wiki.QueryPageName(queryBuilder);
            
            ParserXml parserXml = new ParserXml(result);

            // Convenience method for grabbing a page summary (first paragraph)
            string summary = Wiki.PageSummary("snow");
        }
    }
}