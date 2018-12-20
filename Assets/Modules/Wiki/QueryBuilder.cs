using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;

namespace WikiQueries{
        
    public class QueryBuilder{
        
        ///-------------------------------------///
        ///             Properties              ///
        ///-------------------------------------///

        private UriBuilder uriBuilder;
        private Format format;
        private List<string> queryParams;
        
        ///-------------------------------------///
        ///             Constructors            ///
        ///-------------------------------------///
        
        public QueryBuilder(string baseUri){
            Uri uri;
            queryParams = new List<string>();
            if (!Uri.TryCreate(baseUri, UriKind.Absolute, out uri)){
                throw new InvalidUriException("Base uri " + uri + " is invalid.");
            }
            
            uriBuilder = new UriBuilder(uri);
            format = Format.XML;
        }
        
        public QueryBuilder(string baseUri, Format format){
            Uri uri;
            queryParams = new List<string>();
            if (!Uri.TryCreate(baseUri, UriKind.Absolute, out uri)){
                throw new InvalidUriException("Base uri " + uri + " is invalid.");
            }
            
            uriBuilder = new UriBuilder(uri);
            this.format = format;
        }
        
        ///-------------------------------------///
        ///             Methods                 ///
        ///-------------------------------------///

        /// <summary>
        /// Append parameter with single or no value
        /// </summary>
        /// <param name="key">Parameter key</param>
        /// <param name="value">Parameter value - optional</param>
        public void AppendParam(string key, string value=""){
            StringBuilder queryPart = new StringBuilder();
            queryPart.Append(queryParams.Count == 0 ? "" : "&");
            queryPart.Append(value == "" ? Uri.EscapeDataString(key) : Uri.EscapeDataString(key) + "=" + Uri.EscapeDataString(value));
            queryParams.Add(queryPart.ToString());
        }

        /// <summary>
        /// Append parameter with one or more values
        /// </summary>
        /// <param name="key">Parameter key</param>
        /// <param name="values">Parameter values - multiple values seperated by wikis preferred '|' char</param>
        public void AppendMultiValParam(string key, string[] values) {
            StringBuilder queryPart = new StringBuilder();
            queryPart.Append(queryParams.Count == 0 ? "" : "&");
            string vals = string.Join("|", values);
            queryPart.Append(Uri.EscapeDataString(key) + "=" + Uri.EscapeDataString(vals));
            queryParams.Add(queryPart.ToString());
        }

        public void FinishQuery(){
            uriBuilder.Query = string.Join("", queryParams.ToArray());
        }

        
        ///-------------------------------------///
        ///             ACCESSORS               ///
        ///-------------------------------------///
       
         

        public Format Format{
            get{ return format; }
            set{
                format = value;
                AppendParam("format", format.ToString().ToLower());
            }
        }

        public override string ToString(){
            return uriBuilder.ToString();
        }
    }

    public class InvalidUriException : Exception {
        public InvalidUriException(string message) : base(message){}
    }
}