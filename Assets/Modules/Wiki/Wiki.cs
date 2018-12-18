using System;
using System.Collections;
using System.IO;
using System.Net;
using UnityEngine;

namespace WikiQueries{
    
    public class Wiki : MonoBehaviour {

        private static string BASE_URI = "https://en.wikipedia.org/w/api.php";
        
        public string QueryPageName(QueryBuilder query){
            
            WWW www = new WWW(query.ToString());
            StartCoroutine(WaitForRequest(www));
            
            return "";
        }

        /// <summary>
        /// Grab the first paragraph of a page
        /// </summary>
        /// <param name="pageTitle">Title of wiki page</param>
        /// <param name="format">Format to retrieve</param>
        /// <param name="plainText">Retrieve as plain text or with html tags</param>
        /// <returns></returns>
        public string PageSummary(string pageTitle, Format format=Format.XML, bool plainText=false){
            QueryBuilder query = new QueryBuilder(BASE_URI, format);
            
            query.AppendParam("action", "query");
            query.AppendParam("titles", pageTitle);
            query.AppendParam("prop", "extracts");
            query.AppendParam("exintro");
            if (plainText){
                query.AppendParam("explaintext");
            }

            query.Format = Format.XML;
            string result = QueryPageName(query);
            return result;
        }

        /// <summary>
        /// Check a page exists
        /// </summary>
        /// <param name="pageTitle">Title of wiki page</param>
        /// <returns></returns>
        public bool DoesPageExist(string pageTitle){
            QueryBuilder query = new QueryBuilder(BASE_URI, Format.JSON);
            
            query.AppendParam("action", "query");
            query.AppendParam("titles", pageTitle);
            query.Format = Format.XML;
            string result = QueryPageName(query);
            
            ParserXml parserXml = new ParserXml(result);
            string idStr = parserXml.PageId();
            try{
                int id = Int32.Parse(idStr);
                if (id >= 0){
                    return true;
                }
            }
            catch(FormatException e){
                return false;
            }
            return false;
        }
        
        private IEnumerator WaitForRequest(WWW www) {
            yield return www;
     
            // check for errors
            if (www.error == null)
            {
                Debug.Log("WWW Success: " + www.text);
            } else {
                Debug.Log("WWW Error: " + www.error);
            }    
        }
    }
    
    
    
    class QueryResponseException : Exception{
        public QueryResponseException(string message) : base(message){
        }
    }
}