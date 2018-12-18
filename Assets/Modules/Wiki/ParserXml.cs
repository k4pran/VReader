using System;
using System.IO;
using System.Text;
using System.Xml;

namespace WikiQueries{
    public class ParserXml{
        private XmlDocument document;

        public ParserXml(string s){
            document = new XmlDocument();
            document.PreserveWhitespace = true;
            document.Load(new StringReader(s));
        }

        /// <summary>
        /// Retrieves the wiki page id
        ///     note: negative page id means the page is not found
        /// </summary>
        /// <returns>page id</returns>
        public string PageId(){
            XmlNode node = document.SelectSingleNode("//page");
            if (node != null && node.Attributes.Count > 0){
                XmlAttribute idAtt = node.Attributes["_idx"];
                if (idAtt != null){
                    return idAtt.Value;
                }
            }
            return "-1";
        }
    }
}